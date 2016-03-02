// This file is a part of MPDN Extensions.
// https://github.com/zachsaw/MPDN_Extensions
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library.

using System;
using System.Collections.Generic;
using System.Linq;
using Mpdn.Extensions.Framework.Filter;
using Mpdn.OpenCl;
using Mpdn.RenderScript;
using SharpDX;

namespace Mpdn.Extensions.Framework.RenderChain.TextureFilter
{
    using TransformFunc = Func<TextureSize, TextureSize>;
    using IBaseTextureFilter = IFilter<ITextureOutput<IBaseTexture>>;

    public class ShaderFilterSettings<T> : IShaderFilterSettings<T>
        where T : IShaderBase
    {
        public T Shader { get; private set; }
        public bool LinearSampling { get; private set; }
        public bool[] PerTextureLinearSampling { get; private set; }
        public TransformFunc Transform { get; private set; }
        public TextureFormat Format { get; private set; }
        public int SizeIndex { get; private set; }
        public float[] Args { get; private set; }

        public ShaderFilterSettings(T shader)
        {
            Shader = shader;
            PerTextureLinearSampling = new bool[0];
            Transform = (s => s);
            Format = Renderer.RenderQuality.GetTextureFormat();
            Args = new float[0];
        }

        public static implicit operator ShaderFilterSettings<T>(T shader)
        {
            return new ShaderFilterSettings<T>(shader);
        }

        public IShaderFilterSettings<T> Configure(bool? linearSampling = null,
            IEnumerable<float> arguments = null, TransformFunc transform = null, int? sizeIndex = null, 
            TextureFormat? format = null, IEnumerable<bool> perTextureLinearSampling = null)
        {
            return new ShaderFilterSettings<T>(Shader)
            {
                Transform = transform ?? Transform,
                LinearSampling = linearSampling ?? LinearSampling,
                Format = format ?? Format,
                SizeIndex = sizeIndex ?? SizeIndex,
                Args = arguments != null ? arguments.ToArray() : Args,
                PerTextureLinearSampling = perTextureLinearSampling != null ? perTextureLinearSampling.ToArray() : PerTextureLinearSampling
            };
        }
    }

    public abstract class GenericShaderFilter<T> : TextureFilter where T : IShaderBase

    {
        protected GenericShaderFilter(T shader, params IBaseTextureFilter[] inputFilters)
            : this((ShaderFilterSettings<T>) shader, inputFilters)
        { }

        protected GenericShaderFilter(IShaderFilterSettings<T> settings, params IBaseTextureFilter[] inputFilters)
            : base(inputFilters)
        {
            Shader = settings.Shader;
            LinearSampling = settings.PerTextureLinearSampling
                .Concat(Enumerable.Repeat(settings.LinearSampling, inputFilters.Length - settings.PerTextureLinearSampling.Length))
                .ToArray();

            Transform = settings.Transform;
            Format = settings.Format;
            SizeIndex = settings.SizeIndex;

            if (SizeIndex < 0 || SizeIndex >= inputFilters.Length || inputFilters[SizeIndex] == null)
            {
                throw new IndexOutOfRangeException(string.Format("No valid input filter at index {0}", SizeIndex));
            }

            var arguments = settings.Args ?? new float[0];
            Args = new float[4*((arguments.Length + 3)/4)];
            arguments.CopyTo(Args, 0);
        }

        protected T Shader { get; private set; }
        protected bool[] LinearSampling { get; private set; }
        protected TransformFunc Transform { get; private set; }
        protected TextureFormat Format { get; private set; }
        protected int SizeIndex { get; private set; }
        protected float[] Args { get; private set; }

        protected override TextureSize OutputSize
        {
            get { return Transform(InputFilters[SizeIndex].Output.Size); }
        }

        protected override TextureFormat OutputFormat
        {
            get { return Format; }
        }

        protected abstract void LoadInputs(IList<IBaseTexture> inputs);
        protected abstract void Render(T shader);

        protected override void Render(IList<ITextureOutput<IBaseTexture>> inputs)
        {
            LoadInputs(inputs.Select(x => x.Texture).ToList());
            Render(Shader);
        }
    }

    public class ShaderFilter : GenericShaderFilter<IShader>
    {
        public ShaderFilter(IShaderFilterSettings<IShader> settings, params IBaseTextureFilter[] inputFilters)
            : base(settings, inputFilters)
        {
        }

        public ShaderFilter(IShader shader, params IBaseTextureFilter[] inputFilters)
            : base(shader, inputFilters)
        {
        }

        protected int Counter { get; private set; }

        protected override void LoadInputs(IList<IBaseTexture> inputs)
        {
            var i = 0;
            foreach (var input in inputs)
            {
                if (input is ITexture2D)
                {
                    var tex = (ITexture2D) input;
                    Shader.SetTextureConstant(i, tex, LinearSampling[i], false);
                    Shader.SetConstant(string.Format("size{0}", i),
                        new Vector4(tex.Width, tex.Height, 1.0f/tex.Width, 1.0f/tex.Height), false);
                }
                else
                {
                    var tex = (ITexture3D) input;
                    Shader.SetTextureConstant(i, tex, LinearSampling[i], false);
                    Shader.SetConstant(string.Format("size3d{0}", i),
                        new Vector4(tex.Width, tex.Height, tex.Depth, 0), false);
                }
                i++;
            }

            for (i = 0; 4*i < Args.Length; i++)
            {
                Shader.SetConstant(string.Format("args{0}", i),
                    new Vector4(Args[4*i], Args[4*i + 1], Args[4*i + 2], Args[4*i + 3]), false);
            }

            // Legacy constants 
            var output = Output.Texture;
            Shader.SetConstant(0, new Vector4(output.Width, output.Height, Counter++ & 0x7fffff, Renderer.FrameTimeStampMicrosec / 1000000.0f),
                false);
            Shader.SetConstant(1, new Vector4(1.0f/output.Width, 1.0f/output.Height, 0, 0), false);
        }

        protected override void Render(IShader shader)
        {
            Renderer.Render(Target.Texture, shader);
        }
    }

    public class Shader11Filter : GenericShaderFilter<IShader11>
    {
        private readonly List<IDisposable> m_Buffers = new List<IDisposable>();

        public Shader11Filter(IShaderFilterSettings<IShader11> settings, params IBaseTextureFilter[] inputFilters)
            : base(settings, inputFilters)
        {
        }

        public Shader11Filter(IShader11 shader, params IBaseTextureFilter[] inputFilters)
            : base(shader, inputFilters)
        {
        }

        protected void LoadSizeConstants(IList<IBaseTexture> inputs, int index)
        {
            var input = inputs[index];
            if (input is ITexture2D)
            {
                var tex = (ITexture2D) input;
                var buffer =
                    Renderer.CreateConstantBuffer(new Vector4(tex.Width, tex.Height, 1.0f/tex.Width, 1.0f/tex.Height));
                m_Buffers.Add(buffer);
                Shader.SetTextureConstant(index, tex, LinearSampling[index], false);
                Shader.SetConstantBuffer(string.Format("size{0}", index), buffer, false);
            }
            else
            {
                var tex = (ITexture3D) input;
                var buffer = Renderer.CreateConstantBuffer(new Vector4(tex.Width, tex.Height, tex.Depth, 0));
                m_Buffers.Add(buffer);
                Shader.SetTextureConstant(index, tex, LinearSampling[index], false);
                Shader.SetConstantBuffer(string.Format("size3d{0}", index), buffer, false);
            }
        }

        protected override void LoadInputs(IList<IBaseTexture> inputs)
        {
            for (int i = 0; 4*i < Args.Length; i++)
            {
                var buffer =
                    Renderer.CreateConstantBuffer(new Vector4(Args[4*i], Args[4*i + 1], Args[4*i + 2], Args[4*i + 3]));
                m_Buffers.Add(buffer);
                Shader.SetConstantBuffer(string.Format("args{0}", i), buffer, false);
            }

            // Additional constants can be loaded via LoadSizeConstants()
        }

        protected override void Render(IShader11 shader)
        {
            Renderer.Render(Target.Texture, shader);

            DisposeHelper.DisposeElements(m_Buffers);
            m_Buffers.Clear();
        }
    }

    public class DirectComputeFilter : Shader11Filter
    {
        public DirectComputeFilter(IShaderFilterSettings<IShader11> settings, int threadGroupX, int threadGroupY,
            int threadGroupZ, params IBaseTextureFilter[] inputFilters)
            : base(settings, inputFilters)
        {
            ThreadGroupX = threadGroupX;
            ThreadGroupY = threadGroupY;
            ThreadGroupZ = threadGroupZ;
        }

        public DirectComputeFilter(IShader11 shader, int threadGroupX, int threadGroupY, int threadGroupZ,
            params IBaseTextureFilter[] inputFilters)
            : base(shader, inputFilters)
        {
            ThreadGroupX = threadGroupX;
            ThreadGroupY = threadGroupY;
            ThreadGroupZ = threadGroupZ;
        }

        protected override void Render(IShader11 shader)
        {
            Renderer.Compute(Target.Texture, shader, ThreadGroupX, ThreadGroupY, ThreadGroupZ);
        }

        public int ThreadGroupX { get; private set; }
        public int ThreadGroupY { get; private set; }
        public int ThreadGroupZ { get; private set; }
    }

    public class ClKernelFilter : GenericShaderFilter<IKernel>
    {
        public ClKernelFilter(IShaderFilterSettings<IKernel> settings, int[] globalWorkSizes, params IBaseTextureFilter[] inputFilters)
            : base(settings, inputFilters)
        {
            GlobalWorkSizes = globalWorkSizes;
            LocalWorkSizes = null;
        }

        public ClKernelFilter(IKernel shader, int[] globalWorkSizes, params IBaseTextureFilter[] inputFilters)
            : base(shader, inputFilters)
        {
            GlobalWorkSizes = globalWorkSizes;
            LocalWorkSizes = null;
        }

        public ClKernelFilter(IShaderFilterSettings<IKernel> settings, int[] globalWorkSizes, int[] localWorkSizes, params IBaseTextureFilter[] inputFilters)
            : base(settings, inputFilters)
        {
            GlobalWorkSizes = globalWorkSizes;
            LocalWorkSizes = localWorkSizes;
        }

        public ClKernelFilter(IKernel shader, int[] globalWorkSizes, int[] localWorkSizes, params IBaseTextureFilter[] inputFilters)
            : base(shader, inputFilters)
        {
            GlobalWorkSizes = globalWorkSizes;
            LocalWorkSizes = localWorkSizes;
        }

        protected virtual void LoadCustomInputs()
        {
            // override to load custom OpenCL inputs such as a weights buffer
        }

        protected override void LoadInputs(IList<IBaseTexture> inputs)
        {
            Shader.SetOutputTextureArg(0, Output.Texture); // Note: MPDN only supports one output texture per kernel

            var i = 1;
            foreach (var input in inputs)
            {
                if (input is ITexture2D)
                {
                    var tex = (ITexture2D) input;
                    Shader.SetInputTextureArg(i, tex, false);
                }
                else
                {
                    throw new NotSupportedException("Only 2D textures are supported in OpenCL");
                }
                i++;
            }

            foreach (var v in Args)
            {
                Shader.SetArg(i++, v, false);
            }

            LoadCustomInputs();
        }

        protected override void Render(IKernel shader)
        {
            Renderer.RunClKernel(shader, GlobalWorkSizes, LocalWorkSizes);
        }

        public int[] GlobalWorkSizes { get; private set; }
        public int[] LocalWorkSizes { get; private set; }
    }
}

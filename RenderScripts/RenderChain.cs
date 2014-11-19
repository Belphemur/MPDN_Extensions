using System;
using Mpdn.RenderScript.Scaler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using SharpDX;
using TransformFunc = System.Func<System.Drawing.Size, System.Drawing.Size>;

namespace Mpdn.RenderScript
{
    public static class StaticRenderer
    {
        private static Dictionary<String, IShader> CompiledShaders = new Dictionary<string, IShader>();
        public static IRenderer Renderer { private get; set; }

        public static IShader CompileShader(String shaderPath)
        {
            IShader shader;
            CompiledShaders.TryGetValue(shaderPath, out shader);
            
            if (shader == null)
            {
                shader = Renderer.CompileShader(shaderPath);
                CompiledShaders.Add(shaderPath, shader);
            }

            return shader;
        }

        public static void Render(ITexture texture, IShader shader)
        {
            Renderer.Render(texture, shader);
        }

        public static void Scale(ITexture renderTarget, ITexture texture, IScaler upscaler, IScaler downscaler)
        {
            Renderer.Scale(renderTarget, texture, upscaler, downscaler);
        }

        public static void ConvertToRgb(ITexture renderTarget, ITexture texture)
        {
            Renderer.ConvertToRgb(renderTarget, texture, Renderer.Colorimetric);
        }

        public static void ConvertToRgb(ITexture renderTarget, ITexture texture, YuvColorimetric yuvColorimetric)
        {
            Renderer.ConvertToRgb(renderTarget, texture, yuvColorimetric);
        }
    }

    public interface IRenderChain
    {
        IRenderer Renderer { set; }
        IFilter CreateFilter(IFilter sourceFilter);
    }

    public abstract class RenderChain : IRenderChain
    {
        public IRenderer Renderer { protected get; set; }
        public abstract IFilter CreateFilter(IFilter sourceFilter);

        #region Convenience Functions

        protected virtual string ShaderPath
        {
            get { return GetType().Name; }
        }

        protected string ShaderDataFilePath
        {
            get
            {
                var asmPath = typeof(IScriptRenderer).Assembly.Location;
                return Path.Combine(Common.GetDirectoryName(asmPath), "RenderScripts", ShaderPath);
            }
        }

        protected IShader CompileShader(string shaderFileName)
        {
            return StaticRenderer.CompileShader(Path.Combine(ShaderDataFilePath, shaderFileName));
        }

        protected IFilter CreateFilter(IShader shader, params IFilter[] inputFilters)
        {
            return CreateFilter(shader, false, inputFilters);
        }

        protected IFilter CreateFilter(IShader shader, bool linearSampling, params IFilter[] inputFilters)
        {
            return CreateFilter(shader, 0, linearSampling, inputFilters);
        }

        protected IFilter CreateFilter(IShader shader, int sizeIndex, params IFilter[] inputFilters)
        {
            return CreateFilter(shader, sizeIndex, false, inputFilters);
        }

        protected IFilter CreateFilter(IShader shader, int sizeIndex, bool linearSampling, params IFilter[] inputFilters)
        {
            return CreateFilter(shader, s => new Size(s.Width, s.Height), sizeIndex, linearSampling, inputFilters);
        }

        protected IFilter CreateFilter(IShader shader, TransformFunc transform,
            params IFilter[] inputFilters)
        {
            return CreateFilter(shader, transform, 0, false, inputFilters);
        }

        protected IFilter CreateFilter(IShader shader, TransformFunc transform, bool linearSampling,
            params IFilter[] inputFilters)
        {
            return CreateFilter(shader, transform, 0, linearSampling, inputFilters);
        }

        protected IFilter CreateFilter(IShader shader, TransformFunc transform, int sizeIndex,
            params IFilter[] inputFilters)
        {
            return CreateFilter(shader, transform, sizeIndex, false, inputFilters);
        }

        protected IFilter CreateFilter(IShader shader, TransformFunc transform, int sizeIndex,
            bool linearSampling, params IFilter[] inputFilters)
        {
            if (shader == null)
                throw new ArgumentNullException("shader");

            if (Renderer == null)
                throw new InvalidOperationException("CreateFilter is not available before Setup() is called");

            return new ShaderFilter(Renderer, shader, transform, sizeIndex, linearSampling, inputFilters);
        }

        #endregion
    }

    public class StaticChain : IRenderChain
    {
        public IRenderer Renderer { protected get; set; }
        private Func<IFilter, IFilter> Compiler;

        public StaticChain(Func<IFilter, IFilter> compiler)
        {
            Compiler = compiler;
        }

        public IFilter CreateFilter(IFilter sourceFilter)
        {
            return Compiler(sourceFilter);
        }
    }

    public class FilterChain {
        private IRenderer Renderer;
        public IFilter Filter;

        public FilterChain(IFilter sourceFilter, IRenderer renderer)
        {
            Renderer = renderer;
            Filter = sourceFilter;
        }

        public void Add(IRenderChain renderChain)
        {
            renderChain.Renderer = Renderer;
            Filter = renderChain.CreateFilter(Filter);
        }

        public Size OutputSize
        {
            get { return Filter.OutputSize; }
        }
    }

    public abstract class CombinedChain : RenderChain
    {
        protected abstract void BuildChain(FilterChain Chain);

        public override IFilter CreateFilter(IFilter sourceFilter) {
            var chain = new FilterChain(sourceFilter, Renderer);
            BuildChain(chain);
            return chain.Filter;
        }

        #region Convenience functions

        protected bool IsDownscalingFrom(Size size)
        {
            return !IsNotScalingFrom(size) && !IsUpscalingFrom(size);
        }

        protected bool IsNotScalingFrom(Size size)
        {
            return size == Renderer.TargetSize;
        }

        protected bool IsUpscalingFrom(Size size)
        {
            var targetSize = Renderer.TargetSize;
            return targetSize.Width > size.Width || targetSize.Height > size.Height;
        }

        protected bool IsDownscalingFrom(FilterChain chain)
        {
            return IsDownscalingFrom(chain.OutputSize);
        }

        protected bool IsNotScalingFrom(FilterChain chain)
        {
            return IsNotScalingFrom(chain.OutputSize);
        }

        protected bool IsUpscalingFrom(FilterChain chain)
        {
            return IsUpscalingFrom(chain.OutputSize);
        }

        #endregion
    }
}
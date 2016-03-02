﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mpdn.Extensions.Framework.Filter;
using Mpdn.Extensions.Framework.RenderChain.TextureFilter;
using SharpDX;

// ReSharper disable once CheckNamespace
namespace Mpdn.Extensions.Framework.RenderChain
{
    using TransformFunc = Func<TextureSize, TextureSize>;
    using IBaseTextureFilter = IFilter<ITextureOutput<IBaseTexture>>;

    #region Interfaces

    public interface ITextureFilter<out TTexture> : IFilter<ITextureOutput<TTexture>>
        where TTexture : class, IBaseTexture
    { }

    public interface ITextureFilter : ITextureFilter<ITexture2D>
    { }

    public interface IResizeableFilter : ITextureFilter, ITaggableFilter<ITextureOutput<ITexture2D>>
    {
        ITextureFilter SetSize(TextureSize outputSize);
    }

    public interface IOffsetFilter : ITextureFilter
    {
        void ForceOffsetCorrection();
    }

    public interface ICompositionFilter : IResizeableFilter
    {
        ITextureFilter Luma { get; }
        ITextureFilter Chroma { get; }
        TextureSize TargetSize { get; }
        Vector2 ChromaOffset { get; }

        ICompositionFilter Rebuild(IChromaScaler chromaScaler = null, TextureSize? targetSize = null, Vector2? chromaOffset = null);
    }

    public interface IShaderFilterSettings<out T>
        where T : IShaderBase
    {
        T Shader { get; }
        bool LinearSampling { get; }
        bool[] PerTextureLinearSampling { get; }
        TransformFunc Transform { get; }
        TextureFormat Format { get; }
        int SizeIndex { get; }
        float[] Args { get; }

        IShaderFilterSettings<T> Configure(
            bool? linearSampling = null,
            IEnumerable<float> arguments = null,
            TransformFunc transform = null,
            int? sizeIndex = null,
            TextureFormat? format = null,
            IEnumerable<bool> perTextureLinearSampling = null);
    }

    public interface IManagedTexture<out TTexture> : IDisposable
    where TTexture : class, IBaseTexture
    {
        ITextureOutput<TTexture> GetLease();
        void RevokeLease();
        bool Valid { get; }
    }

    public interface IChromaScaler
    {
        ITextureFilter CreateChromaFilter(ITextureFilter lumaInput, ITextureFilter chromaInput, TextureSize targetSize, Vector2 chromaOffset);
    }

    #endregion

    #region Helpers

    public static class TransformationHelper
    {
        public static ITextureFilter ConvertToRgb(this ITextureFilter filter)
        {
            return new RgbFilter(filter);
        }

        public static ITextureFilter ConvertToYuv(this ITextureFilter filter)
        {
            var sourceFilter = filter as VideoSourceFilter;
            if (sourceFilter != null)
                return sourceFilter.GetYuv();

            return new YuvFilter(filter);
        }

        public static IResizeableFilter Transform(this IResizeableFilter filter, Func<ITextureFilter, ITextureFilter> transformation)
        {
            return new TransformedResizeableFilter(transformation, filter);
        }

        public static ITextureFilter Resize(this ITextureFilter<ITexture2D> inputFilter, TextureSize outputSize, TextureChannels? channels = null, Vector2? offset = null, IScaler upscaler = null, IScaler downscaler = null, IScaler convolver = null, TextureFormat? outputFormat = null)
        {
            return new ResizeFilter(inputFilter, outputSize, channels ?? TextureChannels.All, offset ?? Vector2.Zero, upscaler, downscaler, convolver, outputFormat);
        }

        public static ITextureFilter Convolve(this ITextureFilter<ITexture2D> inputFilter, IScaler convolver, TextureChannels? channels = null, Vector2? offset = null, IScaler upscaler = null, IScaler downscaler = null, TextureFormat ? outputFormat = null)
        {
            return new ResizeFilter(inputFilter, inputFilter.Output.Size, channels ?? TextureChannels.All, offset ?? Vector2.Zero, upscaler, downscaler, convolver, outputFormat);
        }

        public static ITextureFilter AddTaggedResizer(this ITextureFilter<ITexture2D> filter)
        {
            return filter.SetSize(filter.Output.Size, true);
        }

        public static ITextureFilter SetSize(this ITextureFilter<ITexture2D> filter, TextureSize size, bool tagged = false)
        {
            var resizeable = (filter as IResizeableFilter) ?? new ResizeFilter(filter);
            if (tagged)
                resizeable.EnableTag();
            return resizeable.SetSize(size);
        }

        #region Auxilary class(es)

        private sealed class TransformedResizeableFilter : TextureFilter.TextureFilter, IResizeableFilter
        {
            private readonly IResizeableFilter m_InputFilter;
            private readonly Func<ITextureFilter, ITextureFilter> m_Transformation;

            public TransformedResizeableFilter(Func<ITextureFilter, ITextureFilter> transformation, IResizeableFilter inputFilter)
                : base(inputFilter)
            {
                m_InputFilter = inputFilter;
                m_Transformation = transformation;
            }

            protected override IFilter<ITextureOutput<ITexture2D>> Optimize()
            {
                var result = m_Transformation(m_InputFilter);

                if (m_InputFilter.Output.Size != result.Output.Size)
                    throw new InvalidOperationException("Transformation is not allowed to change the size.");

                return m_Transformation(m_InputFilter);
            }

            public void EnableTag()
            {
                m_InputFilter.EnableTag();
            }

            public ITextureFilter SetSize(TextureSize outputSize)
            {
                return new TransformedResizeableFilter(m_Transformation, m_InputFilter);
            }

            protected override TextureSize OutputSize
            {
                get { return m_InputFilter.Output.Size; }
            }

            protected override TextureFormat OutputFormat
            {
                get { return m_InputFilter.Output.Format; }
            }

            protected override void Render(IList<ITextureOutput<IBaseTexture>> textureOutputs)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    public static class ShaderFilterHelper
    {
        public static IShaderFilterSettings<T> Configure<T>(this T shader, bool? linearSampling = null,
            IEnumerable<float> arguments = null, TransformFunc transform = null, int? sizeIndex = null,
            TextureFormat? format = null, IEnumerable<bool> perTextureLinearSampling = null)
            where T : IShaderBase
        {
            return new ShaderFilterSettings<T>(shader).Configure(linearSampling, arguments, transform, sizeIndex,
                format, perTextureLinearSampling);
        }

        public static ITextureFilter ApplyTo<T>(this IShaderFilterSettings<T> settings, params IBaseTextureFilter[] inputFilters)
            where T : IShaderBase
        {
            if (settings is IShaderFilterSettings<IShader>)
                return new ShaderFilter((IShaderFilterSettings<IShader>)settings, inputFilters);
            if (settings is IShaderFilterSettings<IShader11>)
                return new Shader11Filter((IShaderFilterSettings<IShader11>)settings, inputFilters);

            throw new ArgumentException("Unsupported Shader type.");
        }

        public static ITextureFilter ApplyTo<T>(this IShaderFilterSettings<T> settings, IEnumerable<IBaseTextureFilter> inputFilters)
            where T : IShaderBase
        {
            return settings.ApplyTo(inputFilters.ToArray());
        }

        public static ITextureFilter Apply<T>(this ITextureFilter filter, IShaderFilterSettings<T> settings)
            where T : IShaderBase
        {
            return settings.ApplyTo(filter);
        }

        public static ITextureFilter ApplyTo<T>(this T shader, params IBaseTextureFilter[] inputFilters)
            where T : IShaderBase
        {
            if (shader is IShader)
                return new ShaderFilter((IShader)shader, inputFilters);
            if (shader is IShader11)
                return new Shader11Filter((IShader11)shader, inputFilters);

            throw new ArgumentException("Unsupported Shader type.");
        }

        public static ITextureFilter ApplyTo<T>(this T shader, IEnumerable<IBaseTextureFilter> inputFilters)
            where T : IShaderBase
        {
            return shader.ApplyTo(inputFilters.ToArray());
        }

        public static ITextureFilter Apply<T>(this ITextureFilter filter, T shader)
            where T : IShaderBase
        {
            return shader.ApplyTo(filter);
        }
    }

    public static class ManagedTextureHelpers
    {
        public static IManagedTexture<TTexture> GetManaged<TTexture>(this TTexture texture) where TTexture : class, IBaseTexture
        {
            return new ManagedTexture<TTexture>(texture);
        }

        public static TextureSourceFilter<TTexture> ToFilter<TTexture>(this IManagedTexture<TTexture> texture)
            where TTexture : class, IBaseTexture
        {
            return new TextureSourceFilter<TTexture>(texture.GetLease());
        }
    }

    public static class ChromaHelper
    {
        public static ITextureFilter MakeChromaFilter(this IChromaScaler chromaScaler, ITextureFilter lumaInput, ITextureFilter chromaInput, TextureSize? targetSize = null, Vector2? chromaOffset = null)
        {
            return new CompositionFilter(lumaInput, chromaInput, chromaScaler, targetSize, chromaOffset);
        }

        public static ITextureFilter MakeChromaFilter<TChromaScaler>(this TChromaScaler scaler, ITextureFilter input)
            where TChromaScaler : RenderChain, IChromaScaler
        {
            var compositionFilter = input as ICompositionFilter;
            if (compositionFilter == null)
                return input;

            return compositionFilter
                .Rebuild(scaler)
                .Tagged(new ChromaScalerTag(compositionFilter.Chroma, scaler.Status));
        }

    }

    public static class MergeHelper
    {
        public static ITextureFilter MergeWith(this ITextureFilter inputY, ITextureFilter inputUv)
        {
            return new MergeFilter(inputY, inputUv);
        }

        public static ITextureFilter MergeWith(this ITextureFilter inputY, ITextureFilter inputU, ITextureFilter inputV)
        {
            return new MergeFilter(inputY, inputU, inputV);
        }
    }

    #endregion
}

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
using Mpdn.RenderScript;
using SharpDX;

namespace Mpdn.Extensions.Framework.RenderChain
{
    public interface IChromaScaler
    {
        IFilter CreateChromaFilter(IFilter lumaInput, IFilter chromaInput, TextureSize targetSize, Vector2 chromaOffset);
    }

    public class DefaultChromaScaler : IChromaScaler
    {
        public IFilter CreateChromaFilter(IFilter lumaInput, IFilter chromaInput, TextureSize targetSize, Vector2 chromaOffset)
        {
            var fullSizeChroma = new ResizeFilter(chromaInput, targetSize, TextureChannels.ChromaOnly, 
                chromaOffset, Renderer.ChromaUpscaler, Renderer.ChromaDownscaler);

            return new MergeFilter(lumaInput.SetSize(targetSize, tagged: true), fullSizeChroma)
                .ConvertToRgb()
                .Tagged(new ChromaScalerTag(chromaInput, fullSizeChroma.Status().PrependToStatus("Chroma: ")));
        }
    }

    public class InternalChromaScaler : IChromaScaler
    {
        private readonly IFilter m_SourceFilter;

        public InternalChromaScaler(IFilter sourceFilter)
        {
            m_SourceFilter = sourceFilter;
        }

        public IFilter CreateChromaFilter(IFilter lumaInput, IFilter chromaInput, TextureSize targetSize, Vector2 chromaOffset)
        {
            if (lumaInput is YSourceFilter && chromaInput is ChromaSourceFilter && chromaOffset == Renderer.ChromaOffset)
                return m_SourceFilter.SetSize(targetSize, tagged: true);

            return null;
        }
    }

    public static class ChromaHelper
    {
        public static IFilter CreateChromaFilter(this IChromaScaler chromaScaler, IFilter lumaInput, IFilter chromaInput, Vector2 chromaOffset)
        {
            return chromaScaler.CreateChromaFilter(lumaInput, chromaInput, lumaInput.OutputSize, chromaOffset);
        }

        public static IFilter MakeChromaFilter<TChromaScaler>(this TChromaScaler scaler, IFilter input)
            where TChromaScaler : RenderChain, IChromaScaler
        {
            var chromaFilter = input as ChromaFilter;
            if (chromaFilter == null)
                return input;

            return chromaFilter.MakeNew(scaler)
                    .Tagged(new TemporaryTag("ChromaScaler"));
        }
    }

    public sealed class ChromaFilter : BaseSourceFilter, IResizeableFilter
    {
        private readonly IFilter m_Fallback;
        private IFilter<ITexture2D> m_CompilationResult;

        public readonly IFilter Luma;
        public readonly IFilter Chroma;
        public readonly IChromaScaler ChromaScaler;
        public readonly Vector2 ChromaOffset;

        public TextureSize TargetSize;

        public ChromaFilter(IFilter lumaInput, IFilter chromaInput, TextureSize? targetSize = null, Vector2? chromaOffset = null, IFilter fallback = null, IChromaScaler chromaScaler = null)
        {
            Luma = lumaInput;
            Chroma = chromaInput;
            TargetSize = targetSize ?? lumaInput.OutputSize;
            ChromaOffset = chromaOffset ?? Renderer.ChromaOffset;
            ChromaScaler = chromaScaler ?? new DefaultChromaScaler();

            m_Fallback = fallback ?? MakeNew();
            Tag = new EmptyTag();
        }

        public ChromaFilter MakeNew(IChromaScaler chromaScaler = null, TextureSize? targetSize = null, Vector2? chromaOffset = null)
        {
            return new ChromaFilter(Luma, Chroma, targetSize ?? TargetSize, chromaOffset ?? ChromaOffset, this, chromaScaler ?? ChromaScaler);
        }

        public override ITexture2D OutputTexture
        {
            get { throw new InvalidOperationException(); }
        }

        public override TextureSize OutputSize
        {
            get { return TargetSize; }
        }

        public override TextureFormat OutputFormat
        {
            get { return Renderer.RenderQuality.GetTextureFormat(); } // Not guaranteed
        }

        public void MakeTagged() { /* ChromaScaler is *always* tagged */ }

        public override IFilter<ITexture2D> Compile()
        {
            if (m_CompilationResult != null)
                return m_CompilationResult;

            var result = ChromaScaler.CreateChromaFilter(Luma, Chroma, TargetSize, ChromaOffset);

            if (result == null)
                result = m_Fallback;
            else
            {
                var chain = ChromaScaler as RenderChain;
                if (chain != null && result.Tag.IsEmpty())
                    result.AddTag(new ChromaScalerTag(Chroma, chain.Status));
            }

            Tag.AddInput(result);

            m_CompilationResult = result
                .SetSize(TargetSize, tagged: true)
                .Compile();

            return m_CompilationResult;
        }

        public void SetSize(TextureSize size)
        {
            TargetSize = size;
        }
    }
}
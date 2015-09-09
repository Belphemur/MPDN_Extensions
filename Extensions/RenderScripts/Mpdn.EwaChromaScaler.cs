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
using Mpdn.Extensions.Framework;
using Mpdn.Extensions.Framework.RenderChain;
using SharpDX;

namespace Mpdn.Extensions.RenderScripts
{
    namespace Mpdn.EwaScaler
    {
        public class EwaChromaScaler : EwaScaler, IChromaScaler
        {
            protected override string ShaderPath
            {
                get { return "EwaScaler"; }
            }

            public override IFilter CreateFilter(IFilter input)
            {
                var chromaFilter = input as ChromaFilter;
                if (chromaFilter != null)
                    return chromaFilter.MakeNew(this);

                return input;
            }

            public IFilter CreateChromaFilter(IFilter lumaInput, IFilter chromaInput, TextureSize targetSize, Vector2 chromaOffset)
            {
                DiscardTextures();

                var chromaSize = chromaInput.OutputSize;
                CreateWeights(chromaSize, targetSize);

                var offset = chromaOffset + new Vector2(0.5f, 0.5f);
                int lobes = TapCount.ToInt()/2;
                var shader = CompileShader("EwaScaler.hlsl",
                    macroDefinitions:
                        string.Format("LOBES = {0}; AR = {1}; CHROMA = 1;",
                            lobes, AntiRingingEnabled ? 1 : 0))
                    .Configure(
                        transform: size => targetSize,
                        arguments: new[] {AntiRingingStrength, offset.X, offset.Y},
                        linearSampling: true
                    );

                // Fall back to default when downscaling is needed
                if (targetSize.Width < chromaSize.Width || targetSize.Height < chromaSize.Height)
                    return new ChromaFilter(lumaInput, chromaInput, null, targetSize, chromaOffset);

                return GetEwaFilter(shader, new[] { lumaInput.SetSize(targetSize), chromaInput });
            }
        }

        public class EwaScalerChromaScalerUi : RenderChainUi<EwaChromaScaler, EwaScalerConfigDialog>
        {
            protected override string ConfigFileName
            {
                get { return "Mpdn.EwaChromaScaler"; }
            }

            public override string Category
            {
                get { return "Chroma Scaling"; }
            }

            public override ExtensionUiDescriptor Descriptor
            {
                get
                {
                    return new ExtensionUiDescriptor
                    {
                        Guid = new Guid("D93E8C6F-1A4C-40D2-913A-3773C00D1541"),
                        Name = "EWA Chroma Scaler",
                        Description = "Elliptical weighted average (EWA) chroma upscaler"
                    };
                }
            }
        }
    }
}

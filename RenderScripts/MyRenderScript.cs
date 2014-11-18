﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Mpdn.RenderScript.Mpdn.ImageProcessor;
using Mpdn.RenderScript.Mpdn.Resizer;
using Mpdn.RenderScript.Scaler;
using Mpdn.RenderScript.Shiandow.Chroma;
using Mpdn.RenderScript.Shiandow.Nedi;
using YAXLib;

namespace Mpdn.RenderScript
{
    namespace MyOwnUniqueNameSpace // e.g. Replace with your user name
    {
        public class MyRenderChain : CombinedChain {
            /*#region Settings

            private string[] PostProcess;

            public MyRenderChain()
            {
                ImageProcessorUsage = ImageProcessorUsage.Always;
            }

            [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
            public string[] ShaderFileNames
            {
                get { return PostProcess ?? (PostProcess = new string[0]); }
                set { PostProcess = value; }
            }

            [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
            public ImageProcessorUsage ImageProcessorUsage { get; set; }

            #endregion*/

            private string[] PreProcess = new[] { @"SweetFX\Bloom.hlsl", @"SweetFX\LiftGammaGain.hlsl" };
            private string[] PostProcess = new[] { @"SweetFX\LumaSharpen.hlsl" };
            private string[] Deinterlace = new[] { @"MPC-HC\Deinterlace (blend).hlsl" };
            private string[] ToLinear = new[] { @"ConvertToLinearLight.hlsl" };
            private string[] ToGamma = new[] { @"ConvertToGammaLight.hlsl" };

            protected override void BuildChain(FilterChain Chain)
            {
                // Scale chroma first (this bypasses MPDN's chroma scaler)
                Chain.Add(new BicubicChroma{ Preset = Presets.MitchellNetravali });

                if (Renderer.InterlaceFlags.HasFlag(InterlaceFlags.IsInterlaced))
                {
                    // Deinterlace using blend
                    Chain.Add(new ImageProcessor { ShaderFileNames = Deinterlace });
                }

                // Pre resize shaders, followed by NEDI image doubler
                Chain.Add(new ImageProcessor { ShaderFileNames = PreProcess });

                // Use NEDI once only.
                // Note: To use NEDI as many times as required to get the image past target size,
                //       Change the following *if* to *while*
                if (IsUpscalingFrom(Chain)) // See RenderScriptChain for other comparer methods
                {
                    Chain.Add(new Nedi { AlwaysDoubleImage = true });
                }

                if (IsDownscalingFrom(Chain))
                {
                    // Use linear light for downscaling
                    Chain.Add(new ImageProcessor { ShaderFileNames = ToLinear });
                    Chain.Add(new Resizer { ResizerOption = ResizerOption.TargetSize100Percent });
                    Chain.Add(new ImageProcessor { ShaderFileNames = ToGamma });
                }
                else
                {
                    // Otherwise, scale with gamma light
                    Chain.Add(new Resizer { ResizerOption = ResizerOption.TargetSize100Percent });
                }

                if (Renderer.VideoSize.Width >= 1920)
                {
                    // Sharpen only if video isn't full HD
                    Chain.Add(new ImageProcessor { ShaderFileNames = PostProcess });
                }
            }
        }

        public class MyRenderScript : RenderScript<MyRenderChain>
        {
            protected override RenderScriptDescriptor ScriptDescriptor
            {
                get
                {
                    return new RenderScriptDescriptor
                    {
                        Name = "Custom Render Script Chain",
                        Description = "A customized render script chain (Advanced)",
                        Guid = new Guid("B0AD7BE7-A86D-4BE4-A750-4362FEF28A55"),
                        Copyright = ""
                    };
                }
            }
        }
    }
}

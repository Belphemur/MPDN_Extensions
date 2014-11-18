﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Mpdn.RenderScript.Scaler;
using YAXLib;

namespace Mpdn.RenderScript
{
    namespace Mpdn.Resizer
    {

        #region ResizerOptions

        public enum ResizerOption
        {
            [Description("Video size")]
            VideoSize,
            [Description("Video size x2")]
            VideoSizeX2,
            [Description("Video size x4")]
            VideoSizeX4,
            [Description("Video size x8")]
            VideoSizeX8,
            [Description("Video size x16")]
            VideoSizeX16,
            [Description("The greater of target size and video size")]
            GreaterOfTargetAndVideoSize,
            [Description("The greater of target size and video size x2")]
            GreaterOfTargetAndVideoSizeX2,
            [Description("The greater of target size and video size x4")]
            GreaterOfTargetAndVideoSizeX4,
            [Description("The greater of target size and video size x8")]
            GreaterOfTargetAndVideoSizeX8,
            [Description("The greater of target size and video size x16")]
            GreaterOfTargetAndVideoSizeX16,
            [Description("Just past target using a multiple of video size")]
            PastTargetUsingVideoSize,
            [Description("Just past target using a multiple of video size except when target equals to video size")]
            PastTargetUsingVideoSizeExceptSimilar,
            [Description("Just under target using a multiple of video size")]
            UnderTargetUsingVideoSize,
            [Description("Just under target using a multiple of video size except when target equals to video size")]
            UnderTargetUsingVideoSizeExceptSimilar,
            [Description("25% of target size")]
            TargetSize025Percent,
            [Description("50% of target size")]
            TargetSize050Percent,
            [Description("75% of target size")]
            TargetSize075Percent,
            [Description("100% of target size")]
            TargetSize100Percent,
            [Description("125% of target size")]
            TargetSize125Percent,
            [Description("150% of target size")]
            TargetSize150Percent,
            [Description("175% of target size")]
            TargetSize175Percent,
            [Description("200% of target size")]
            TargetSize200Percent
        }

        #endregion

        public class Resizer : RenderChain
        {
            #region Settings

            public Resizer()
            {
                ResizerOption = ResizerOption.TargetSize100Percent;
            }

            [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
            public ResizerOption ResizerOption { get; set; }

            public IScaler Upscaler; // Not saved
            public IScaler Downscaler; // Not saved

            #endregion

            public override IFilter CreateFilter(IFilter sourceFilter)
            {
                return new ResizeFilter(Renderer, sourceFilter, GetOutputSize,
                    Upscaler ?? Renderer.LumaUpscaler, Downscaler ?? Renderer.LumaDownscaler);
            }

            #region Size Calculation

            private static readonly double s_Log2 = Math.Log10(2);

            private Size GetOutputSize()
            {
                Size size;

                var targetSize = Renderer.TargetSize;
                var videoSize = Renderer.VideoSize;
                switch (ResizerOption)
                {
                    case ResizerOption.VideoSize:
                        size = videoSize;
                        break;
                    case ResizerOption.VideoSizeX2:
                        size = new Size(videoSize.Width << 1, videoSize.Height << 1);
                        break;
                    case ResizerOption.VideoSizeX4:
                        size = new Size(videoSize.Width << 2, videoSize.Height << 2);
                        break;
                    case ResizerOption.VideoSizeX8:
                        size = new Size(videoSize.Width << 3, videoSize.Height << 3);
                        break;
                    case ResizerOption.VideoSizeX16:
                        size = new Size(videoSize.Width << 4, videoSize.Height << 4);
                        break;
                    case ResizerOption.GreaterOfTargetAndVideoSize:
                        size = GetMaxSize(targetSize, videoSize);
                        break;
                    case ResizerOption.GreaterOfTargetAndVideoSizeX2:
                        size = GetMaxSize(targetSize, new Size(videoSize.Width << 1, videoSize.Height << 1));
                        break;
                    case ResizerOption.GreaterOfTargetAndVideoSizeX4:
                        size = GetMaxSize(targetSize, new Size(videoSize.Width << 2, videoSize.Height << 2));
                        break;
                    case ResizerOption.GreaterOfTargetAndVideoSizeX8:
                        size = GetMaxSize(targetSize, new Size(videoSize.Width << 3, videoSize.Height << 3));
                        break;
                    case ResizerOption.GreaterOfTargetAndVideoSizeX16:
                        size = GetMaxSize(targetSize, new Size(videoSize.Width << 4, videoSize.Height << 4));
                        break;
                    case ResizerOption.PastTargetUsingVideoSize:
                        return GetVideoBasedSizeOver(targetSize.Width + 1, targetSize.Height + 1);
                    case ResizerOption.UnderTargetUsingVideoSize:
                        return GetVideoBasedSizeUnder(targetSize.Width - 1, targetSize.Height - 1);
                    case ResizerOption.PastTargetUsingVideoSizeExceptSimilar:
                        return GetVideoBasedSizeOver(targetSize.Width, targetSize.Height);
                    case ResizerOption.UnderTargetUsingVideoSizeExceptSimilar:
                        return GetVideoBasedSizeUnder(targetSize.Width, targetSize.Height);
                    case ResizerOption.TargetSize025Percent:
                        size = new Size(targetSize.Width * 1 / 4, targetSize.Height * 1 / 4);
                        break;
                    case ResizerOption.TargetSize050Percent:
                        size = new Size(targetSize.Width * 2 / 4, targetSize.Height * 2 / 4);
                        break;
                    case ResizerOption.TargetSize075Percent:
                        size = new Size(targetSize.Width * 3 / 4, targetSize.Height * 3 / 4);
                        break;
                    case ResizerOption.TargetSize100Percent:
                        size = new Size(targetSize.Width * 4 / 4, targetSize.Height * 4 / 4);
                        break;
                    case ResizerOption.TargetSize125Percent:
                        size = new Size(targetSize.Width * 5 / 4, targetSize.Height * 5 / 4);
                        break;
                    case ResizerOption.TargetSize150Percent:
                        size = new Size(targetSize.Width * 6 / 4, targetSize.Height * 6 / 4);
                        break;
                    case ResizerOption.TargetSize175Percent:
                        size = new Size(targetSize.Width * 7 / 4, targetSize.Height * 7 / 4);
                        break;
                    case ResizerOption.TargetSize200Percent:
                        size = new Size(targetSize.Width * 8 / 4, targetSize.Height * 8 / 4);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return size;
            }

            private static Size GetMaxSize(Size size1, Size size2)
            {
                // Use height to determine which is max
                return size1.Height > size2.Height ? size1 : size2;
            }

            private Size GetVideoBasedSizeOver(int targetWidth, int targetHeight)
            {
                int videoWidth = Renderer.VideoSize.Width;
                int videoHeight = Renderer.VideoSize.Height;
                int widthX = Math.Max(1, GetMultiplier(targetWidth, videoWidth));
                int heightX = Math.Max(1, GetMultiplier(targetHeight, videoHeight));
                var multiplier = Math.Max(widthX, heightX);
                return new Size(videoWidth * multiplier, videoHeight * multiplier);
            }

            private Size GetVideoBasedSizeUnder(int targetWidth, int targetHeight)
            {
                int videoWidth = Renderer.VideoSize.Width;
                int videoHeight = Renderer.VideoSize.Height;
                int widthX = Math.Max(1, GetMultiplier(targetWidth, videoWidth) - 1);
                int heightX = Math.Max(1, GetMultiplier(targetHeight, videoHeight) - 1);
                var multiplier = Math.Max(widthX, heightX);
                return new Size(videoWidth * multiplier, videoHeight * multiplier);
            }

            private static int GetMultiplier(int dest, int src)
            {
                return (int)Math.Ceiling((Math.Log10(dest) - Math.Log10(src)) / s_Log2) + 1;
            }

            #endregion
        }

        #region Filter Definition

        public class ResizeFilter : Filter
        {
            private readonly Func<Size> m_GetSizeFunc;
            private readonly IScaler m_Upscaler;
            private readonly IScaler m_Downscaler;

            public ResizeFilter(IRenderer renderer, IFilter inputFilter, Func<Size> getSizeFunc, 
                IScaler upscaler, IScaler downscaler)
                : base(renderer, inputFilter)
            {
                m_GetSizeFunc = getSizeFunc;
                m_Upscaler = upscaler;
                m_Downscaler = downscaler;
            }

            public override Size OutputSize
            {
                get { return m_GetSizeFunc(); }
            }

            public override void Render(IEnumerable<ITexture> inputs)
            {
                Renderer.Scale(OutputTexture, inputs.Single(), m_Upscaler, m_Downscaler);
            }
        }

        #endregion

        public class ResizerScript : ConfigurableRenderScript<Resizer, ResizerConfigDialog>
        {
            protected override string ConfigFileName
            {
                get { return "Mpdn.Resizer"; }
            }

            protected override RenderScriptDescriptor ScriptDescriptor
            {
                get
                {
                    return new RenderScriptDescriptor
                    {
                        Guid = new Guid("C5621540-C3F6-4B54-98FE-EA9ECECD0D41"),
                        Name = "Resizer",
                        Description = GetDescription()
                    };
                }
            }

            private string GetDescription()
            {
                var desc = ScriptConfig == null
                    ? "Resizes the image"
                    : string.Format("Resize to: {0}", Chain.ResizerOption.ToDescription());
                return desc;
            }
        }
    }
}
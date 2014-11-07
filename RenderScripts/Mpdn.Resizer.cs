﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YAXLib;

namespace Mpdn.RenderScript
{
    namespace Mpdn.Resizer
    {
        public class Resizer : RenderScript
        {
            private static readonly double s_Log2 = Math.Log10(2);

            private Size m_Size;
            private Size m_SavedTargetSize;
            private ResizerOption m_SavedResizerOption;
            private ResizerSettings m_Settings;

            public override ScriptDescriptor Descriptor
            {
                get
                {
                    return new ScriptDescriptor
                    {
                        Guid = new Guid("C5621540-C3F6-4B54-98FE-EA9ECECD0D41"),
                        Name = "Resizer",
                        Description = GetDescription(),
                        HasConfigDialog = true
                    };
                }
            }

            private string GetDescription()
            {
                var options = m_Settings == null
                    ? string.Empty
                    : string.Format(" to {0}", m_Settings.Config.Resizer.ToDescription());
                return string.Format("Resizes the image{0}", options);
            }

            public override ScriptInputDescriptor InputDescriptor
            {
                get
                {
                    return new ScriptInputDescriptor
                    {
                        Size = GetInputSize()
                    };
                }
            }

            public override void Initialize(int instanceId)
            {
                m_Settings = new ResizerSettings(instanceId);
                m_Settings.Load();
                m_SavedResizerOption = m_Settings.Config.Resizer;
            }

            public override void Destroy()
            {
                m_Settings.Destroy();
            }

            public override bool ShowConfigDialog()
            {
                var dialog = new ResizerConfigDialog();
                dialog.Setup(m_Settings.Config);
                if (dialog.ShowDialog() != DialogResult.OK)
                    return false;

                m_Settings.Save();
                return true;
            }

            private Size GetInputSize()
            {
                if (m_Settings.Config.Resizer == m_SavedResizerOption && 
                    m_SavedTargetSize == Renderer.TargetSize &&
                    m_Size != Size.Empty)
                    return m_Size;

                m_SavedResizerOption = m_Settings.Config.Resizer;
                m_SavedTargetSize = Renderer.TargetSize;

                var targetSize = Renderer.TargetSize;
                switch (m_SavedResizerOption)
                {
                    case ResizerOption.VideoSize:
                        m_Size = Renderer.VideoSize;
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
                        m_Size = new Size(targetSize.Width*1/4, targetSize.Height*1/4);
                        break;
                    case ResizerOption.TargetSize050Percent:
                        m_Size = new Size(targetSize.Width*2/4, targetSize.Height*2/4);
                        break;
                    case ResizerOption.TargetSize075Percent:
                        m_Size = new Size(targetSize.Width*3/4, targetSize.Height*3/4);
                        break;
                    case ResizerOption.TargetSize100Percent:
                        m_Size = new Size(targetSize.Width*4/4, targetSize.Height*4/4);
                        break;
                    case ResizerOption.TargetSize125Percent:
                        m_Size = new Size(targetSize.Width*5/4, targetSize.Height*5/4);
                        break;
                    case ResizerOption.TargetSize150Percent:
                        m_Size = new Size(targetSize.Width*6/4, targetSize.Height*6/4);
                        break;
                    case ResizerOption.TargetSize175Percent:
                        m_Size = new Size(targetSize.Width*7/4, targetSize.Height*7/4);
                        break;
                    case ResizerOption.TargetSize200Percent:
                        m_Size = new Size(targetSize.Width*8/4, targetSize.Height*8/4);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return m_Size;
            }

            private Size GetVideoBasedSizeOver(int targetWidth, int targetHeight)
            {
                int videoWidth = Renderer.VideoSize.Width;
                int videoHeight = Renderer.VideoSize.Height;
                int widthX = Math.Max(1, GetMultiplier(targetWidth, videoWidth));
                int heightX = Math.Max(1, GetMultiplier(targetHeight, videoHeight));
                var multiplier = Math.Max(widthX, heightX);
                m_Size = new Size(videoWidth * multiplier, videoHeight * multiplier);
                return m_Size;
            }

            private Size GetVideoBasedSizeUnder(int targetWidth, int targetHeight)
            {
                int videoWidth = Renderer.VideoSize.Width;
                int videoHeight = Renderer.VideoSize.Height;
                int widthX = Math.Max(1, GetMultiplier(targetWidth, videoWidth) - 1);
                int heightX = Math.Max(1, GetMultiplier(targetHeight, videoHeight) - 1);
                var multiplier = Math.Max(widthX, heightX);
                m_Size = new Size(videoWidth * multiplier, videoHeight * multiplier);
                return m_Size;
            }

            private static int GetMultiplier(int dest, int src)
            {
                return (int) Math.Ceiling((Math.Log10(dest) - Math.Log10(src))/s_Log2) + 1;
            }

            protected override ITexture GetFrame()
            {
                return InputFilter.OutputTexture;
            }
        }

        public enum ResizerOption
        {
            [Description("Video size")]
            VideoSize,
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

        #region Settings

        public class Settings
        {
            public Settings()
            {
                Resizer = ResizerOption.TargetSize100Percent;
            }

            [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
            public ResizerOption Resizer { get; set; }
        }

        public class ResizerSettings : ScriptSettings<Settings>
        {
            private readonly int m_InstanceId;

            public ResizerSettings(int instanceId)
            {
                m_InstanceId = instanceId;
            }

            protected override string ScriptConfigFileName
            {
                get { return string.Format("Mpdn.Resizer.{0}.config", m_InstanceId); }
            }
        }

        #endregion
    }
}
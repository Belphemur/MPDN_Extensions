using System;
using System.Drawing;
using System.Windows.Forms;
using YAXLib;

namespace Mpdn.RenderScript
{
    namespace Shiandow.Nedi
    {
        public class NediScaler : RenderScript
        {
            private IShader m_Nedi1Shader;
            private IShader m_Nedi2Shader;
            private IShader m_NediHInterleaveShader;
            private IFilter m_NediScaler;
            private IShader m_NediVInterleaveShader;
            private IFilter m_Scaler;

            private NediSettings m_Settings;

            protected override string ShaderPath
            {
                get { return "NEDI"; }
            }

            public override ScriptDescriptor Descriptor
            {
                get
                {
                    return new ScriptDescriptor
                    {
                        Guid = new Guid("B8E439B7-7DC2-4FC1-94E2-608A39756FB0"),
                        Name = "NEDI",
                        Description = GetDescription(),
                        Copyright = "NEDI by Shiandow",
                        HasConfigDialog = true
                    };
                }
            }

            private bool UseNedi
            {
                get { return m_Settings.Config.AlwaysDoubleImage || NeedToUpscale(); }
            }

            private string GetDescription()
            {
                var options = m_Settings == null
                    ? string.Empty
                    : string.Format("{0}", m_Settings.Config.AlwaysDoubleImage ? " (forced)" : string.Empty);
                return string.Format("NEDI image doubler{0}", options);
            }

            public override void Initialize(int instanceId)
            {
                m_Settings = new NediSettings(instanceId);
                m_Settings.Load();
            }

            public override void Destroy()
            {
                m_Settings.Destroy();
            }

            public override bool ShowConfigDialog()
            {
                var dialog = new NediConfigDialog();
                dialog.Setup(m_Settings.Config);
                if (dialog.ShowDialog() != DialogResult.OK)
                    return false;

                OnInputSizeChanged();
                m_Settings.Save();
                return true;
            }

            protected override void Dispose(bool disposing)
            {
                DiscardNediScaler();

                Common.Dispose(ref m_Nedi1Shader);
                Common.Dispose(ref m_Nedi2Shader);
                Common.Dispose(ref m_NediHInterleaveShader);
                Common.Dispose(ref m_NediVInterleaveShader);
            }

            public override void Setup(IRenderer renderer)
            {
                lock (m_Settings)
                {
                    base.Setup(renderer);
                    CompileShaders();
                    CreateNediScaler();
                }
            }

            public override void OnInputSizeChanged()
            {
                AllocateTextures();
            }

            public override void OnOutputSizeChanged()
            {
                AllocateTextures();
            }

            protected override ITexture GetFrame()
            {
                lock (m_Settings)
                {
                    return GetFrame(m_Scaler);
                }
            }

            private void AllocateTextures()
            {
                lock (m_Settings)
                {
                    if (Renderer == null)
                        return;

                    if (UseNedi)
                    {
                        m_NediScaler.AllocateTextures();
                        m_Scaler = m_NediScaler;
                    }
                    else
                    {
                        m_NediScaler.DeallocateTextures();
                        m_Scaler = InputFilter;
                    }
                }
            }

            private void CompileShaders()
            {
                m_Nedi1Shader = CompileShader("NEDI-I.hlsl");
                m_Nedi2Shader = CompileShader("NEDI-II.hlsl");
                m_NediHInterleaveShader = CompileShader("NEDI-HInterleave.hlsl");
                m_NediVInterleaveShader = CompileShader("NEDI-VInterleave.hlsl");
            }

            private void CreateNediScaler()
            {
                var nedi1 = CreateFilter(m_Nedi1Shader, InputFilter);

                var nediH =
                    CreateFilter(m_NediHInterleaveShader,
                        (w, h) => new Size(2*w, h), InputFilter, nedi1);

                var nedi2 = CreateFilter(m_Nedi2Shader, nediH);

                var nediV =
                    CreateFilter(m_NediVInterleaveShader,
                        (w, h) => new Size(w, h*2), nediH, nedi2);

                m_NediScaler = nediV;
                m_NediScaler.Initialize();
            }

            private bool NeedToUpscale()
            {
                return Renderer.TargetSize.Width > Renderer.VideoSize.Width ||
                       Renderer.TargetSize.Height > Renderer.VideoSize.Height;
            }

            private void DiscardNediScaler()
            {
                // Disposes the all ancestors too 
                Common.Dispose(ref m_NediScaler);
            }
        }

        #region Settings

        public class Settings
        {
            public Settings()
            {
                AlwaysDoubleImage = false;
            }

            [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
            public bool AlwaysDoubleImage { get; set; }
        }

        public class NediSettings : ScriptSettings<Settings>
        {
            private readonly int m_InstanceId;

            public NediSettings(int instanceId)
            {
                m_InstanceId = instanceId;
            }

            protected override string ScriptConfigFileName
            {
                get { return string.Format("Shiandow.Nedi.{0}.config", m_InstanceId); }
            }
        }

        #endregion
    }
}
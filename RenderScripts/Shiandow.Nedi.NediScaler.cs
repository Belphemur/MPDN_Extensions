using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using YAXLib;

namespace Mpdn.RenderScript
{
    namespace Shiandow.Nedi
    {
        public class MultiplyFilter : ShaderFilter
        {
            private readonly int m_WFactor;
            private readonly int m_HFactor;

            public override Size OutputSize
            {
                get
                {
                    var size = InputFilters[0].OutputSize;
                    return new Size(size.Width * m_WFactor, size.Height * m_HFactor);
                }
            }

            public MultiplyFilter(IRenderer renderer, IShader shader, int wfactor, int hfactor, bool linearSampling = false, params IFilter[] inputFilters)
                : base(renderer, shader, linearSampling, inputFilters)
            {
                m_WFactor = wfactor;
                m_HFactor = hfactor;
            }
        }

        public class NediScaler : RenderScript
        {
            private IFilter m_NediScaler;
            private IFilter m_Scaler;

            private IShader m_Nedi1Shader;
            private IShader m_Nedi2Shader;
            private IShader m_NediHInterleaveShader;
            private IShader m_NediVInterleaveShader;

            private NediSettings m_NediSettings;

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
                        Description = "NEDI image doubler",
                        Copyright = "NEDI by Shiandow",
                        HasConfigDialog = true
                    };
                }
            }

            public override void Initialize(int instanceId)
            {
                m_NediSettings = new NediSettings(instanceId);
                m_NediSettings.Load();
            }

            public override void Destroy()
            {
                m_NediSettings.Destroy();
            }

            public override bool ShowConfigDialog()
            {
                var dialog = new NediConfigDialog();
                dialog.Setup(m_NediSettings.Config);
                if (dialog.ShowDialog() != DialogResult.OK)
                    return false;

                OnInputSizeChanged();
                m_NediSettings.Save();
                return true;
            }

            protected override void Dispose(bool disposing)
            {
                Debug.WriteLine("Dispose");
                DiscardNediScaler();

                Common.Dispose(ref m_Nedi1Shader);
                Common.Dispose(ref m_Nedi2Shader);
                Common.Dispose(ref m_NediHInterleaveShader);
                Common.Dispose(ref m_NediVInterleaveShader);
            }

            public override void Setup(IRenderer renderer)
            {
                lock (m_NediSettings)
                {
                    Debug.WriteLine("Setup");
                    base.Setup(renderer);
                    CompileShaders();
                    CreateNediScaler();
                }
            }

            public override void OnOutputSizeChanged()
            {
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

            protected override ITexture GetFrame()
            {
                return GetFrame(m_Scaler);
            }

            private bool UseNedi
            {
                get { return m_NediSettings.Config.AlwaysDoubleImage || NeedToUpscale(); }
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
                var nedi1 = CreateMultiplyFilter(m_Nedi1Shader, 1, 1, InputFilter);
                var nediH = CreateMultiplyFilter(m_NediHInterleaveShader, 2, 1, InputFilter, nedi1);
                var nedi2 = CreateMultiplyFilter(m_Nedi2Shader, 1, 1, nediH);
                var nediV = CreateMultiplyFilter(m_NediVInterleaveShader, 1, 2, nediH, nedi2);

                m_NediScaler = nediV;
                m_NediScaler.Initialize();
            }

            private IFilter CreateMultiplyFilter(IShader shader, int wFactor, int hFactor, params IFilter[] filters)
            {
                return new MultiplyFilter(Renderer, shader, wFactor, hFactor, false, filters);
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
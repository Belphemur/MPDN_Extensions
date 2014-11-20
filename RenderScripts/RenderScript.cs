﻿using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using TransformFunc = System.Func<System.Drawing.Size, System.Drawing.Size>;

namespace Mpdn.RenderScript
{
    public static class ShaderCache
    {
        private static Dictionary<String, IShader> CompiledShaders = new Dictionary<string, IShader>();

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
    }

    public class RenderChainScript : IRenderScript, IDisposable
    {
        protected IRenderChain Chain;
        private TextureCache Cache;
        private IFilter m_Filter;
        private SourceFilter m_SourceFilter;

        public RenderChainScript(IRenderChain chain)
        {
            Chain = chain;
            m_SourceFilter = new SourceFilter();
            Cache = new TextureCache();
        }

        public ScriptInterfaceDescriptor Descriptor
        {
            get
            {
                return new ScriptInterfaceDescriptor
                {
                    WantYuv = false, // Not implemented yet
                    Prescale = (m_SourceFilter.LastDependentIndex > 0),
                    PrescaleSize = Size.Empty // Not implemented yet
                };
            }
        }

        public void Update()
        {
            Common.Dispose(m_Filter);

            m_Filter = Chain.CreateFilter(m_SourceFilter);
            m_Filter.Initialize();
        }

        public void Render()
        {
            Cache.PutTempTexture(Renderer.OutputRenderTarget);
            m_Filter.NewFrame();
            m_Filter.Render(Cache);
            Scale(Renderer.OutputRenderTarget, m_Filter.OutputTexture);
            m_Filter.ReleaseTexture(Cache);
            Cache.FlushTextures();
        }

        public void Dispose()
        {
            Common.Dispose(Cache);
            Common.Dispose(m_Filter);
            Common.Dispose(m_SourceFilter);
        }

        private void Scale(ITexture output, ITexture input)
        {
            Renderer.Scale(output, input, Renderer.LumaUpscaler, Renderer.LumaDownscaler);
        }
    }

    public class RenderScriptDescriptor
    {
        public Guid Guid = Guid.Empty;
        public string Name;
        public string Description;
        public string Copyright = "";
    }

    public abstract class RenderChainUi<TChain> : IRenderScriptUi
        where TChain : class, IRenderChain, new()
    {
        protected virtual TChain Chain { get { return m_Chain; } }

        protected abstract RenderScriptDescriptor ScriptDescriptor { get; }

        #region Implementation

        private TChain m_Chain;
        private IRenderScript m_RenderScript;
        public IRenderScript RenderScript
        {
            get
            {
                if (m_RenderScript == null) 
                    m_RenderScript = new RenderChainScript(Chain);

                return m_RenderScript;
            }
        }

        public virtual ScriptDescriptor Descriptor
        {
            get
            {
                return new ScriptDescriptor
                {
                    HasConfigDialog = false,
                    Guid = ScriptDescriptor.Guid,
                    Name = ScriptDescriptor.Name,
                    Description = ScriptDescriptor.Description,
                    Copyright = ScriptDescriptor.Copyright
                };
            }
        }

        public virtual void Initialize()
        {
            m_Chain = new TChain();
        }

        public virtual bool ShowConfigDialog(IWin32Window owner)
        {
            throw new NotImplementedException("Config dialog has not been implemented");
        }

        public virtual ScriptInterfaceDescriptor InterfaceDescriptor
        {
            get
            {
                return RenderScript.Descriptor;
            }
        }

        public void Destroy()
        {
            Common.Dispose(Chain);
            Common.Dispose(RenderScript);
        }

        #endregion Implementation
    }
}

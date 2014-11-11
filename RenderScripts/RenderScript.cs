﻿using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TransformFunc = System.Func<System.Drawing.Size, System.Drawing.Size>;

namespace Mpdn.RenderScript
{
    public abstract class RenderScript : IScriptRenderer, IDisposable
    {
        private IFilter m_SourceFilter;
        private IFilter m_Filter;

        protected IRenderer Renderer { get; private set; }

        protected virtual string ShaderPath
        {
            get { return GetType().FullName; }
        }

        protected string ShaderDataFilePath
        {
            get
            {
                var asmPath = typeof (IScriptRenderer).Assembly.Location;
                return Path.Combine(Common.GetDirectoryName(asmPath), "RenderScripts", ShaderPath);
            }
        }

        protected IFilter SourceFilter
        {
            get { return m_SourceFilter ?? (m_SourceFilter = new SourceFilter(Renderer)); }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract ScriptDescriptor Descriptor { get; }

        public virtual ScriptInterfaceDescriptor InterfaceDescriptor
        {
            get { return new ScriptInterfaceDescriptor(); }
        }

        public virtual void Initialize(int instanceId)
        {
        }

        public virtual void Destroy()
        {
        }

        public virtual bool ShowConfigDialog(IWin32Window owner)
        {
            throw new NotImplementedException("Config dialog has not been implemented");
        }

        public virtual void Setup(IRenderer renderer)
        {
            Renderer = renderer;
        }

        public virtual void OnInputSizeChanged()
        {
            if (TextureAllocTrigger == TextureAllocTrigger.OnInputSizeChanged ||
                TextureAllocTrigger == TextureAllocTrigger.OnInputOutputSizeChanged)
            {
                AllocateTextures();
            }
        }

        public virtual void OnOutputSizeChanged()
        {
            if (TextureAllocTrigger == TextureAllocTrigger.OnOutputSizeChanged ||
                TextureAllocTrigger == TextureAllocTrigger.OnInputOutputSizeChanged)
            {
                AllocateTextures();
            }
        }

        public virtual void Render()
        {
            Scale(Renderer.OutputRenderTarget, GetFrame(m_Filter ?? GetFilter()));
        }

        protected virtual void Dispose(bool disposing)
        {
            // Not required, but is there in case SourceFilter is changed
            Common.Dispose(ref m_SourceFilter);
        }

        ~RenderScript()
        {
            Dispose(false);
        }

        protected abstract IFilter GetFilter();

        protected abstract TextureAllocTrigger TextureAllocTrigger { get; }

        protected virtual void AllocateTextures()
        {
            if (Renderer == null)
                return;

            var filter = GetFilter();
            EnsureFilterNotNull(filter);
            if (m_Filter != filter)
            {
                if (m_Filter != null)
                {
                    m_Filter.DeallocateTextures();
                }
                m_Filter = filter;
            }
            m_Filter.AllocateTextures();
        }

        protected virtual ITexture GetFrame(IFilter filter)
        {
            EnsureFilterNotNull(filter);
            filter.NewFrame();
            filter.Render();
            return filter.OutputTexture;
        }

        protected virtual void Scale(ITexture output, ITexture input)
        {
            Renderer.Scale(output, input, Renderer.LumaUpscaler, Renderer.LumaDownscaler);
        }

        protected IShader CompileShader(string shaderFileName)
        {
            return Renderer.CompileShader(Path.Combine(ShaderDataFilePath, shaderFileName));
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

        private static void EnsureFilterNotNull(IFilter filter)
        {
            if (filter == null)
            {
                throw new NoNullAllowedException("GetFilter must not return null");
            }
        }
    }

    public enum TextureAllocTrigger
    {
        None,
        OnInputSizeChanged,
        OnOutputSizeChanged,
        OnInputOutputSizeChanged
    }
}

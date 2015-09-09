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
using System.Diagnostics;
using System.Drawing;
using Mpdn.RenderScript;

namespace Mpdn.Extensions.Framework.RenderChain
{
    public class RenderChainScript : IRenderScript, IDisposable
    {
        private SourceFilter m_SourceFilter;
        private IFilter<ITexture2D> m_Filter;

        protected readonly RenderChain Chain;

        public RenderChainScript(RenderChain chain)
        {
            Chain = chain;
            Chain.Initialize();
        }

        ~RenderChainScript()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Chain.Reset();
        }

        public ScriptInterfaceDescriptor Descriptor
        {
            get
            {
                if (m_SourceFilter == null)
                    return null;

                return m_SourceFilter.Descriptor;
            }
        }

        public void Update()
        {
            var initialFilter = MakeInitialFilter();

            m_Filter = CreateSafeFilter(Chain, initialFilter)
                .SetSize(Renderer.TargetSize)
                .Compile();
            m_Filter.Initialize();
        }

        public IResizeableFilter MakeInitialFilter()
        {
            m_SourceFilter = new SourceFilter();

            if (Renderer.InputFormat.IsRgb())
                return m_SourceFilter;

            if (Renderer.ChromaSize.Width < Renderer.LumaSize.Width || Renderer.ChromaSize.Height < Renderer.LumaSize.Height)
                return new ChromaFilter(new YSourceFilter(), new ChromaSourceFilter(), new InternalChromaScaler(m_SourceFilter));

            return m_SourceFilter;
        }

        public void Render()
        {
            TexturePool.PutTempTexture(Renderer.OutputRenderTarget);
            m_Filter.Render();
            if (Renderer.OutputRenderTarget != m_Filter.OutputTexture)
            {
                Scale(Renderer.OutputRenderTarget, m_Filter.OutputTexture);
            }
            m_Filter.Reset();
            TexturePool.FlushTextures();
        }

        private static void Scale(ITargetTexture output, ITexture2D input)
        {
            Renderer.Scale(output, input, Renderer.LumaUpscaler, Renderer.LumaDownscaler);
        }

        #region Error Handling

        private TextFilter m_TextFilter;

        public IFilter CreateSafeFilter(RenderChain chain, IFilter input)
        {
            DisposeHelper.Dispose(ref m_TextFilter);
            try
            {
                return Chain.CreateFilter(input);
            }
            catch (Exception ex)
            {
                return DisplayError(ex);
            }
        }

        private IFilter DisplayError(Exception e)
        {
            var message = ErrorMessage(e);
            Trace.WriteLine(message);
            return m_TextFilter = new TextFilter(message);
        }

        protected static Exception InnerMostException(Exception e)
        {
            while (e.InnerException != null)
            {
                e = e.InnerException;
            }

            return e;
        }

        private string ErrorMessage(Exception e)
        {
            var ex = InnerMostException(e);
            return string.Format("Error in {0}:\r\n\r\n{1}\r\n\r\n~\r\nStack Trace:\r\n{2}",
                    GetType().Name, ex.Message, ex.StackTrace);
        }

        #endregion

    }
}

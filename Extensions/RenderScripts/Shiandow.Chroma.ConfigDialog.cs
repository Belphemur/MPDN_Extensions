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
// 

using System;
using System.Linq;
using Mpdn.Extensions.Framework;
using Mpdn.Extensions.Framework.Config;

namespace Mpdn.Extensions.RenderScripts
{
    namespace Shiandow.Chroma
    {
        public partial class ChromaScalerConfigDialog : ChromaScalerConfigDialogBase
        {
            private bool m_SettingPreset;

            public ChromaScalerConfigDialog()
            {
                InitializeComponent();

                var descs = EnumHelpers.GetDescriptions<Presets>().Where(d => d != "Custom");
                foreach (var desc in descs)
                {
                    PresetBox.Items.Add(desc);
                }

                PresetBox.SelectedIndex = (int) Presets.Custom;
            }

            protected override void LoadSettings()
            {
                BSetter.Value = (decimal) Settings.B;
                CSetter.Value = (decimal) Settings.C;
                PresetBox.SelectedIndex = (int) Settings.Preset;
            }

            protected override void SaveSettings()
            {
                Settings.B = (float) BSetter.Value;
                Settings.C = (float) CSetter.Value;
                Settings.Preset = (Presets) PresetBox.SelectedIndex;
            }

            private void ValueChanged(object sender, EventArgs e)
            {
                if (m_SettingPreset)
                    return;

                PresetBox.SelectedIndex = (int) Presets.Custom;
            }

            private void SelectedIndexChanged(object sender, EventArgs e)
            {
                var index = PresetBox.SelectedIndex;
                if (index < 0)
                    return;

                m_SettingPreset = true;
                BSetter.Value = (decimal) BicubicChroma.B_CONST[index];
                CSetter.Value = (decimal) BicubicChroma.C_CONST[index];
                m_SettingPreset = false;
            }
        }

        public class ChromaScalerConfigDialogBase : ScriptConfigDialog<BicubicChroma>
        {
        }
    }
}

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

using Mpdn.Extensions.Framework.Config;

namespace Mpdn.Extensions.RenderScripts
{
    namespace Mpdn.ScriptGroup
    {
        public partial class PresetGroupDialog : PresetGroupDialogBase
        {
            public PresetGroupDialog()
            {
                InitializeComponent();
            }

            protected override void LoadSettings()
            {
                RenderChainList.PresetList = Settings.Options;
                RenderChainList.SelectedIndex = Settings.SelectedIndex;
                HotkeyBox.Text = Settings.Hotkey;
            }

            protected override void SaveSettings()
            {
                Settings.Options = RenderChainList.PresetList;
                Settings.SelectedIndex = RenderChainList.SelectedIndex;
                Settings.Hotkey = HotkeyBox.Text;
            }
        }

        public class PresetGroupDialogBase : ScriptConfigDialog<ScriptGroup>
        {
        }
    }
}

﻿// This file is a part of MPDN Extensions.
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

using System.Windows.Forms;

namespace Mpdn.Extensions.Framework.Controls
{
    public class HotkeyBox : TextBox
    {
        private Keys? m_Key;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            m_Key = e.KeyCode;
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                case Keys.Menu:
                case Keys.ShiftKey:
                    m_Key = null;
                    break;
                case Keys.Back:
                    if (!e.Control && !e.Alt && !e.Shift)
                    {
                        Text = string.Empty;
                        return;
                    }
                    break;
            }
            var hk = new Hotkey { Control = e.Control, Alt = e.Alt, Shift = e.Shift, Key = m_Key };
            Text = hk.ToString();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (m_Key == null)
            {
                Text = string.Empty;
            }
        }
    }

    public class Hotkey
    {
        public bool Control { get; set; }
        public bool Shift { get; set; }
        public bool Alt { get; set; }
        public Keys? Key { get; set; }

        public override string ToString()
        {
            return (Control ? "Ctrl+" : string.Empty) + (Shift ? "Shift+" : string.Empty) +
                   (Alt ? "Alt+" : string.Empty) + (Key != null ? Key.ToString() : string.Empty);
        }
    }
}

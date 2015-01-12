using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Mpdn.PlayerExtensions.Example.DisplayChangerNativeMethods;

namespace Mpdn.PlayerExtensions.Example
{
    public class DisplayChanger : ConfigurablePlayerExtension<DisplayChangerSettings, DisplayChangerConfigDialog>
    {
        protected override PlayerExtensionDescriptor ScriptDescriptor
        {
            get
            {
                return new PlayerExtensionDescriptor
                {
                    Guid = new Guid("9C1BBA5B-B956-43E1-9A91-58B72571EF82"),
                    Name = "Display Changer",
                    Description = "Changes display based on video",
                    Copyright = "Copyright Example © 2015. All rights reserved."
                };
            }
        }

        protected override string ConfigFileName
        {
            get { return "Example.DisplayChanger"; }
        }

        public override void Initialize(IPlayerControl playerControl)
        {
            base.Initialize(playerControl);

            PlayerControl.PlayerStateChanged += PlayerStateChanged;
        }

        public override void Destroy()
        {
            base.Destroy();

            PlayerControl.PlayerStateChanged -= PlayerStateChanged;
        }

        public override IList<Verb> Verbs
        {
            get { return new Verb[0]; }
        }

        private void PlayerStateChanged(object sender, PlayerStateEventArgs e)
        {
            if (!Settings.Activate)
                return;

            if (e.OldState != PlayerState.Closed)
                return;

            var screen = Screen.FromControl(PlayerControl.Form);

            int frequency = 0;
            int index = 0;

            // loop over all supported settings
            while (true)
            {
                var dm = NativeMethods.CreateDevmode(screen.DeviceName);
                if (GetSettings(ref dm, screen.DeviceName, index++) == 0)
                    break;

                if (dm.dmBitsPerPel != 32)
                    continue;

                if (dm.dmDisplayFlags != 0) // Only want progressive modes (1: DM_GRAYSCALE, 2: DM_INTERLACED)
                    continue;

                if (dm.dmPelsWidth != screen.Bounds.Width)
                    continue;

                if (dm.dmPelsHeight != screen.Bounds.Height)
                    continue;

                var fps = 1000000 / (int) PlayerControl.VideoInfo.AvgTimePerFrame;
                if (fps <= 30) // We use the higher refresh rates
                {
                    fps *= 2;
                }

                if (dm.dmDisplayFrequency != fps) 
                    continue;

                frequency = fps;
                break;
            }

            if (frequency != 0)
            {
                ChangeRefreshRate(screen, frequency);
            }
        }

        private void ChangeRefreshRate(Screen screen, int frequency)
        {
            var dm = NativeMethods.CreateDevmode(screen.DeviceName);
            if (GetSettings(ref dm, screen.DeviceName) == 0)
                return;

            dm.dmFields = (int) DM.DisplayFrequency;
            dm.dmDisplayFrequency = frequency;
            dm.dmDeviceName = screen.DeviceName;
            PlayerControl.StopMedia();
            ChangeSettings(dm);
            PlayerControl.PlayMedia(false);
        }

        private static void ChangeSettings(DEVMODE dm)
        {
            if (NativeMethods.ChangeDisplaySettingsEx(dm.dmDeviceName, ref dm, IntPtr.Zero, 0, IntPtr.Zero) !=
                NativeMethods.DISP_CHANGE_SUCCESSFUL)
            {
                Trace.Write("Failed to change display refresh rate");
            }
        }

        private static int GetSettings(ref DEVMODE dm, string deviceName)
        {
            // helper to obtain current settings
            return GetSettings(ref dm, deviceName, NativeMethods.ENUM_CURRENT_SETTINGS);
        }

        private static int GetSettings(ref DEVMODE dm, string deviceName, int iModeNum)
        {
            // helper to wrap EnumDisplaySettings Win32 API
            return NativeMethods.EnumDisplaySettings(deviceName, iModeNum, ref dm);
        }
    }

    public class DisplayChangerSettings
    {
        public DisplayChangerSettings()
        {
            Activate = false;
        }

        public bool Activate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace Mpdn.PlayerExtensions.Example
{
    public class MouseControl : ConfigurablePlayerExtension<MouseControlSettings, MouseControlConfigDialog>
    {
        protected override PlayerExtensionDescriptor ScriptDescriptor
        {
            get
            {
                return new PlayerExtensionDescriptor
                {
                    Guid = new Guid("DCF7797B-9D36-41F3-B28C-0A92793B94F5"),
                    Name = "Mouse Control",
                    Description =
                        string.Format("Use mouse {0}forward/back buttons to navigate chapters/playlist/folder",
                            Settings.EnableMouseWheelSeek ? "wheel to seek and " : string.Empty),
                    Copyright = "Copyright Example © 2015. All rights reserved."
                };
            }
        }

        protected override string ConfigFileName
        {
            get { return "Example.MouseControl"; }
        }

        public override void Initialize(IPlayerControl playerControl)
        {
            base.Initialize(playerControl);

            PlayerControl.MouseClick += PlayerMouseClick;
            PlayerControl.MouseWheel += PlayerMouseWheel;
            PlayerControl.MouseDoubleClick += PlayerMouseDoubleClick;
        }

        public override void Destroy()
        {
            base.Destroy();

            PlayerControl.MouseClick -= PlayerMouseClick;
            PlayerControl.MouseWheel -= PlayerMouseWheel;
            PlayerControl.MouseDoubleClick -= PlayerMouseDoubleClick;
        }

        public override IList<Verb> Verbs
        {
            get { return new Verb[0]; }
        }

        private void PlayerMouseWheel(object sender, PlayerControlEventArgs<MouseEventArgs> e)
        {
            if (!Settings.EnableMouseWheelSeek)
                return;

            var pos = PlayerControl.MediaPosition;
            pos += e.InputArgs.Delta*1000000/40;
            pos = Math.Max(pos, 0);
            PlayerControl.SeekMedia(pos);
            e.Handled = true;
        }

        private void PlayerMouseDoubleClick(object sender, PlayerControlEventArgs<MouseEventArgs> e)
        {
            if (e.InputArgs.Button != MouseButtons.Left)
            {
                e.Handled = true;
                return;
            }
        }

        private void PlayerMouseClick(object sender, PlayerControlEventArgs<MouseEventArgs> e)
        {
            if (PlayerControl.PlayerState == PlayerState.Closed)
                return;
            
            if (e.InputArgs.Button == MouseButtons.Middle)
            {
                ToggleMode();
                return;
            }

            var chapters = PlayerControl.Chapters.OrderBy(chapter => chapter.Position);
            var pos = PlayerControl.MediaPosition;
            bool next = true;

            switch (e.InputArgs.Button)
            {
                case MouseButtons.XButton2:
                    next = true;
                    break;
                case MouseButtons.XButton1:
                    next = false;
                    break;
                default:
                    return;
                    break;
            }        
            var nextChapter = next
                ? chapters.SkipWhile(chapter => chapter.Position < pos).FirstOrDefault()
                : chapters.TakeWhile(chapter => chapter.Position < Math.Max(pos - 1000000, 0)).LastOrDefault();
            if (nextChapter != null)
            {
                PlayerControl.SeekMedia(nextChapter.Position);
                return;
            }
            if (PlaylistForm.PlaylistCount <= 1)
            {
                switch (e.InputArgs.Button)
                {
                    case MouseButtons.XButton2:
                        SendKeys.Send("^{PGDN}");
                        break;
                    case MouseButtons.XButton1:
                        SendKeys.Send("^{PGUP}");
                        break;
                }
            }
            else
            {
                switch (e.InputArgs.Button)
                {
                    case MouseButtons.XButton2:
                        SendKeys.Send("^%n");
                        break;
                    case MouseButtons.XButton1:
                        SendKeys.Send("^%b");
                        break;
                }
            }
        }

        private void ToggleMode()
        {
            if (!Settings.EnableMiddleClickFsToggle)
                return;

            if (PlayerControl.InFullScreenMode)
            {
                PlayerControl.GoWindowed();
            }
            else
            {
                PlayerControl.GoFullScreen();
            }
        }
    }

    public class MouseControlSettings
    {
        public MouseControlSettings()
        {
            EnableMouseWheelSeek = false;
            EnableMiddleClickFsToggle = false;
        }

        public bool EnableMouseWheelSeek { get; set; }
        public bool EnableMiddleClickFsToggle { get; set; }
    }
}

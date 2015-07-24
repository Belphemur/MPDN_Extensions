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
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using MediaInfoDotNet;
using Mpdn.Extensions.Framework;

namespace Mpdn.Extensions.PlayerExtensions
{
    public partial class GoToForm : Form
    {
        private float fps;

        public GoToForm()
        {
            InitializeComponent();

            Load += GoToForm_Load;
            KeyDown += GoToForm_KeyDown;

            GetFps();
            SetCurrentMediaPosition();
        }

        public long Position
        {
            get
            {
                TimeSpan timespan;
                if (TimeSpan.TryParseExact(tb_time.Text, @"hh\:mm\:ss\.fff", CultureInfo.CurrentCulture, out timespan))
                    return (long) timespan.TotalMilliseconds;

                return -1;
            }
        }

        public double CurrentFrame
        {
            get
            {
                long position = Media.Position;
                var timespan = TimeSpan.FromMilliseconds(position / 1000.0);
                return Math.Round(timespan.TotalSeconds * fps);
            }
        }

        public double FrameCount
        {
            get
            {
                long duration = Media.Duration;
                var timespan = TimeSpan.FromMilliseconds(duration / 1000.0);
                return Math.Round(timespan.TotalSeconds * fps);
            }
        }

        public double Frame
        {
            get
            {
                long position;
                if (long.TryParse(nud_frame.Text, out position)) return Math.Round((position / fps) * 1000 * 1000);
                
                return -1;
            }
        }

        private void GetFps()
        {
            var media = new MediaFile(Media.FilePath);
            fps = media.Video[0].frameRate;
        }

        private void SetCurrentMediaPosition()
        {
            var timespan = TimeSpan.FromMilliseconds(Media.Position / 1000.0);
            tb_time.Text = timespan.ToString(@"hh\:mm\:ss\.fff");
            nud_frame.Maximum = (decimal)FrameCount;
            nud_frame.Value = (decimal)CurrentFrame;
        }

        private void ButtonTimeOkClick(object sender, EventArgs e)
        {
            long pos = Position * 1000;
            if (pos > Media.Duration) pos = Media.Duration;
            Media.Seek(pos);
        }

        private void ButtonFrameOkClick(object sender, EventArgs e)
        {
            long frame = Convert.ToInt64(Frame);
            if (frame > Media.Duration) frame = Media.Duration;
            Media.Seek(frame);
        }

        private void tb_time_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) btn_timeOk.PerformClick();
        }

        private void nud_frame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) btn_frameOk.PerformClick();
        }

        private void nud_frame_Enter(object sender, EventArgs e)
        {
            nud_frame.Select(0, nud_frame.Text.Length);
        }

        private void GoToForm_Load(object sender, EventArgs e)
        {
            var pos = new Point(Cursor.Position.X - Width / 2, Cursor.Position.Y - Height / 2);
            Location = pos;
        }

        private void GoToForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape) Close();
        }
    }
}

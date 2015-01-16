﻿using System;
using System.Collections.Generic;
using MediaInfoDotNet;

namespace Mpdn.PlayerExtensions.GitHub
{
    public class ViewMediaInfo : PlayerExtension
    {
        public override ExtensionDescriptor Descriptor
        {
            get
            {
                return new ExtensionDescriptor
                {
                    Guid = new Guid("6FD61379-FF5D-4143-8A7B-97516BB7822F"),
                    Name = "Media Info",
                    Description = "View media info of current file"
                };
            }
        }

        public override IList<Verb> Verbs
        {
            get
            {
                return new[]
                {
                    new Verb(Category.View, string.Empty, "Media Info...", "Ctrl+Shift+I", string.Empty, ShowMediaInfoDialog)
                };
            }
        }

        private void ShowMediaInfoDialog()
        {
            if (PlayerControl.PlayerState == PlayerState.Closed)
                return;

            using (var form = new ViewMediaInfoForm(PlayerControl.MediaFilePath))
            {
                form.ShowDialog(PlayerControl.VideoPanel);
            }
        }
    }
}
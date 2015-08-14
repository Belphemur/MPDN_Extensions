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
// 

using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Mpdn.Extensions.Framework;

namespace Mpdn.Extensions.PlayerExtensions.UpdateChecker
{
    public partial class SimpleUpdateForm : Form
    {
        public enum UpdateType
        {
            Player,
            Extensions,
            Both
        }

        private readonly UpdateCheckerSettings m_Settings;
        private readonly UpdateType m_Type;
        private WebFile m_DownloadingWebFile;

        public SimpleUpdateForm(UpdateType type, UpdateCheckerSettings settings)
        {
            InitializeComponent();
            Icon = Gui.Icon;
            m_Type = type;
            m_Settings = settings;
            extensionVersionLinkLabel.LinkClicked +=ExtensionVersionLinkLabelOnLinkClicked;
            playerVersionLinkLabel.LinkClicked +=PlayerVersionLinkLabelOnLinkClicked;
            switch (m_Type)
            {
                case UpdateType.Both:
                    EnableExtensionInfo();
                    EnablePlayerInfo();
                    break;
                case UpdateType.Extensions:
                    EnableExtensionInfo();
                    break;
                case UpdateType.Player:
                    EnablePlayerInfo();
                    break;
            }
        }

        private void EnablePlayerInfo()
        {
            playerVersionLinkLabel.Text = m_Settings.MpdnVersionOnServer.ToString();
            playerLabel.Visible = true;
            playerVersionLinkLabel.Visible = true;
        }

        private void EnableExtensionInfo()
        {
            extensionVersionLinkLabel.Text = m_Settings.ExtensionVersionOnServer.ToString();
            extensionLabel.Visible = true;
            extensionVersionLinkLabel.Visible = true;
        }

        private void PlayerVersionLinkLabelOnLinkClicked(object sender, LinkLabelLinkClickedEventArgs linkLabelLinkClickedEventArgs)
        {
            new ChangelogForm(UpdateType.Player, m_Settings.MpdnVersionOnServer).ShowDialog(this);
        }

        private void ExtensionVersionLinkLabelOnLinkClicked(object sender, LinkLabelLinkClickedEventArgs linkLabelLinkClickedEventArgs)
        {
            new ChangelogForm(UpdateType.Extensions, m_Settings.ExtensionVersionOnServer).ShowDialog(this);
        }

        private void ForgetUpdateButtonClick(object sender, EventArgs e)
        {
            switch (m_Type)
            {
                case UpdateType.Both:
                    m_Settings.ForgetExtensionVersion = true;
                    m_Settings.ForgetMpdnVersion = true;
                    break;
                case UpdateType.Extensions:
                    m_Settings.ForgetExtensionVersion = true;
                    break;
                case UpdateType.Player:
                    m_Settings.ForgetMpdnVersion = true;
                    break;
            }
            StopDownload();
        }

        private void InstallButtonClick(object sender, EventArgs e)
        {
            WebFile installer = null;
            downloadProgressBar.Visible = true;
            installButton.Enabled = false;
            switch (m_Type)
            {
                case UpdateType.Both:
                    UpdateBoth();
                    break;
                case UpdateType.Extensions:
                    installer = UpdateExtensions();
                    break;
                case UpdateType.Player:
                    installer = UpdatePlayer();
                    break;
            }

            if (installer == null) return;

            m_DownloadingWebFile = installer;
            m_DownloadingWebFile.Downloaded += InstallerOnDownloaded;
            m_DownloadingWebFile.DownloadFailed += InstallerOnDownloadFailed;
            m_DownloadingWebFile.DownloadProgressChanged += InstallerOnDownloadProgressChanged;
            m_DownloadingWebFile.DownloadFile();
        }

        private void InstallerOnDownloadProgressChanged(object sender,
            DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            GuiThread.DoAsync(() => { downloadProgressBar.Value = downloadProgressChangedEventArgs.ProgressPercentage; });
        }

        private void InstallerOnDownloadFailed(object sender, Exception error)
        {
            var file = ((WebFile) sender);
            GuiThread.DoAsync(() =>
            {
                downloadProgressBar.Visible = false;
                installButton.Enabled = true;
                MessageBox.Show(Gui.VideoBox,
                    string.Format("Problem while downloading: {0}\n{1}", file.FileUri, error.Message),
                    "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }

        private void InstallerOnDownloaded(object sender)
        {
            ((WebFile) sender).Start();
            GuiThread.Do((Application.Exit));
        }

        private WebFile UpdatePlayer()
        {
            var arch = ArchitectureHelper.GetPlayerArtchitecture().ToString();
            var installer =
                m_Settings.MpdnVersionOnServer.GenerateSplitButtonItemList()
                    .First(file => file.Name.Contains(arch) && file.IsFile && file.Name.Contains("Installer"));
            downloadProgressBar.CustomText = installer.Name;
            return new TemporaryWebFile(new Uri(installer.Url));
        }

        private WebFile UpdateExtensions()
        {
            var installer =
                m_Settings.ExtensionVersionOnServer.Files.First(file => file.name.Contains(".exe"));
            downloadProgressBar.CustomText = installer.name;
            return new TemporaryWebFile(new Uri(installer.browser_download_url));
        }

        private void UpdateBoth()
        {
            m_DownloadingWebFile = UpdateExtensions();
            m_DownloadingWebFile.DownloadFailed += InstallerOnDownloadFailed;
            m_DownloadingWebFile.DownloadProgressChanged += InstallerOnDownloadProgressChanged;
            m_DownloadingWebFile.Downloaded += (o =>
            {
                var downloadedExtensionInstaller = (WebFile) o;
                downloadedExtensionInstaller.Start(string.Format("/ARCH={0} /MPDN_VERSION=\"{1}\"",
                    ArchitectureHelper.GetPlayerArtchitecture(), m_Settings.MpdnVersionOnServer));
                GuiThread.Do((Application.Exit));
            });
            m_DownloadingWebFile.DownloadFile();
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            StopDownload();
        }

        /// <summary>
        ///     Stop the download and close the Form
        /// </summary>
        private void StopDownload()
        {
            if (m_DownloadingWebFile != null) m_DownloadingWebFile.CancelDownload();
            Close();
        }
    }
}
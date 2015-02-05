﻿namespace Mpdn.PlayerExtensions.Playlist
{
    partial class PlaylistConfigDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_addFileToPlaylistOnOpen = new System.Windows.Forms.CheckBox();
            this.cb_rememberLastPlayedFile = new System.Windows.Forms.CheckBox();
            this.cb_rememberWindowBounds = new System.Windows.Forms.CheckBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_addFileToPlaylistOnOpen);
            this.groupBox1.Controls.Add(this.cb_rememberLastPlayedFile);
            this.groupBox1.Controls.Add(this.cb_rememberWindowBounds);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 92);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // cb_addFileToPlaylistOnOpen
            // 
            this.cb_addFileToPlaylistOnOpen.AutoSize = true;
            this.cb_addFileToPlaylistOnOpen.Location = new System.Drawing.Point(6, 65);
            this.cb_addFileToPlaylistOnOpen.Name = "cb_addFileToPlaylistOnOpen";
            this.cb_addFileToPlaylistOnOpen.Size = new System.Drawing.Size(149, 17);
            this.cb_addFileToPlaylistOnOpen.TabIndex = 2;
            this.cb_addFileToPlaylistOnOpen.Text = "Add to playlist on file open";
            this.cb_addFileToPlaylistOnOpen.UseVisualStyleBackColor = true;
            // 
            // cb_rememberLastPlayedFile
            // 
            this.cb_rememberLastPlayedFile.AutoSize = true;
            this.cb_rememberLastPlayedFile.Location = new System.Drawing.Point(6, 42);
            this.cb_rememberLastPlayedFile.Name = "cb_rememberLastPlayedFile";
            this.cb_rememberLastPlayedFile.Size = new System.Drawing.Size(170, 17);
            this.cb_rememberLastPlayedFile.TabIndex = 1;
            this.cb_rememberLastPlayedFile.Text = "Remember previous played file";
            this.cb_rememberLastPlayedFile.UseVisualStyleBackColor = true;
            // 
            // cb_rememberWindowBounds
            // 
            this.cb_rememberWindowBounds.AutoSize = true;
            this.cb_rememberWindowBounds.Location = new System.Drawing.Point(6, 19);
            this.cb_rememberWindowBounds.Name = "cb_rememberWindowBounds";
            this.cb_rememberWindowBounds.Size = new System.Drawing.Size(192, 17);
            this.cb_rememberWindowBounds.TabIndex = 0;
            this.cb_rememberWindowBounds.Text = "Remember playlist size and position";
            this.cb_rememberWindowBounds.UseVisualStyleBackColor = true;
            // 
            // btn_save
            // 
            this.btn_save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_save.Location = new System.Drawing.Point(61, 110);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 1;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(142, 110);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            // 
            // PlaylistConfigDialog
            // 
            this.AcceptButton = this.btn_save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 138);
            this.ControlBox = false;
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PlaylistConfigDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Playlist Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_rememberWindowBounds;
        private System.Windows.Forms.CheckBox cb_addFileToPlaylistOnOpen;
        private System.Windows.Forms.CheckBox cb_rememberLastPlayedFile;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_cancel;
    }
}
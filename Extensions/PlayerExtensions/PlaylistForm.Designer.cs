﻿namespace Mpdn.PlayerExtensions.Playlist
{
    partial class PlaylistForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PlaylistForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.buttonAdd = new Mpdn.PlayerExtensions.Playlist.ButtonStripItem();
            this.buttonDel = new Mpdn.PlayerExtensions.Playlist.ButtonStripItem();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonLeft = new Mpdn.PlayerExtensions.Playlist.ButtonStripItem();
            this.buttonRight = new Mpdn.PlayerExtensions.Playlist.ButtonStripItem();
            this.buttonSortAscending = new Mpdn.PlayerExtensions.Playlist.ButtonStripItem();
            this.buttonSortDescending = new Mpdn.PlayerExtensions.Playlist.ButtonStripItem();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonNew = new Mpdn.PlayerExtensions.Playlist.ButtonStripItem();
            this.buttonOpen = new Mpdn.PlayerExtensions.Playlist.ButtonStripItem();
            this.buttonSave = new Mpdn.PlayerExtensions.Playlist.ButtonStripItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openPlaylistDialog = new System.Windows.Forms.OpenFileDialog();
            this.savePlaylistDialog = new System.Windows.Forms.SaveFileDialog();
            this.dgv_PlayList = new System.Windows.Forms.DataGridView();
            this.Playing = new System.Windows.Forms.DataGridViewImageColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SkipChapters = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndChapter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PlayList)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonAdd,
            this.buttonDel,
            this.toolStripStatusLabel2,
            this.buttonLeft,
            this.buttonRight,
            this.buttonSortAscending,
            this.buttonSortDescending,
            this.toolStripStatusLabel1,
            this.buttonNew,
            this.buttonOpen,
            this.buttonSave});
            this.statusStrip1.Location = new System.Drawing.Point(0, 202);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(795, 27);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.TabStop = true;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // buttonAdd
            // 
            this.buttonAdd.AutoSize = false;
            this.buttonAdd.BackgroundImage = (System.Drawing.Bitmap)resources.GetObject("add-icon");
            this.buttonAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(25, 25);
            this.buttonAdd.ToolTipText = "Add files";
            this.buttonAdd.Click += new System.EventHandler(this.ButtonAddClick);
            // 
            // buttonDel
            // 
            this.buttonDel.AutoSize = false;
            this.buttonDel.BackgroundImage = (System.Drawing.Bitmap)resources.GetObject("delete-icon");
            this.buttonDel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(25, 25);
            this.buttonDel.ToolTipText = "Remove files";
            this.buttonDel.Click += new System.EventHandler(this.ButtonDelClick);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 22);
            this.toolStripStatusLabel2.Text = " ";
            // 
            // buttonLeft
            // 
            this.buttonLeft.AutoSize = false;
            this.buttonLeft.BackgroundImage = (System.Drawing.Bitmap)resources.GetObject("arrow-left-icon");
            this.buttonLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(25, 25);
            this.buttonLeft.ToolTipText = "Previous";
            this.buttonLeft.Click += new System.EventHandler(this.ButtonLeftClick);
            // 
            // buttonRight
            // 
            this.buttonRight.AutoSize = false;
            this.buttonRight.BackgroundImage = (System.Drawing.Bitmap)resources.GetObject("arrow-right-icon");
            this.buttonRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(25, 25);
            this.buttonRight.ToolTipText = "Next";
            this.buttonRight.Click += new System.EventHandler(this.ButtonRightClick);
            // 
            // buttonSortAscending
            // 
            this.buttonSortAscending.BackgroundImage = (System.Drawing.Bitmap)resources.GetObject("sort-ascending-icon");
            this.buttonSortAscending.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonSortAscending.Margin = new System.Windows.Forms.Padding(10, 2, 0, 0);
            this.buttonSortAscending.Name = "buttonSortAscending";
            this.buttonSortAscending.Size = new System.Drawing.Size(25, 25);
            this.buttonSortAscending.ToolTipText = "Sort playlist (ascending)";
            this.buttonSortAscending.Click += new System.EventHandler(this.ButtonSortAscendingClick);
            // 
            // buttonSortDescending
            // 
            this.buttonSortDescending.BackgroundImage = (System.Drawing.Bitmap)resources.GetObject("sort-descending-icon");
            this.buttonSortDescending.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonSortDescending.Name = "buttonSortDescending";
            this.buttonSortDescending.Size = new System.Drawing.Size(25, 25);
            this.buttonSortDescending.ToolTipText = "Sort playlist (descending)";
            this.buttonSortDescending.Click += new System.EventHandler(this.ButtonSortDescendingClick);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(502, 22);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = " ";
            // 
            // buttonNew
            // 
            this.buttonNew.AutoSize = false;
            this.buttonNew.BackgroundImage = (System.Drawing.Bitmap)resources.GetObject("new-icon");
            this.buttonNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(25, 25);
            this.buttonNew.ToolTipText = "New playlist";
            this.buttonNew.Click += new System.EventHandler(this.ButtonNewClick);
            // 
            // buttonOpen
            // 
            this.buttonOpen.AutoSize = false;
            this.buttonOpen.BackgroundImage = (System.Drawing.Bitmap)resources.GetObject("open-icon");
            this.buttonOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonOpen.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(25, 25);
            this.buttonOpen.ToolTipText = "Open playlist";
            this.buttonOpen.Click += new System.EventHandler(this.ButtonOpenClick);
            // 
            // buttonSave
            // 
            this.buttonSave.AutoSize = false;
            this.buttonSave.BackgroundImage = (System.Drawing.Bitmap)resources.GetObject("save-icon");
            this.buttonSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(25, 25);
            this.buttonSave.ToolTipText = "Save playlist";
            this.buttonSave.Click += new System.EventHandler(this.ButtonSaveClick);
            // 
            // timer
            // 
            this.timer.Interval = 30;
            this.timer.Tick += new System.EventHandler(this.TimerTick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = resources.GetString("openFileDialog.Filter");
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.SupportMultiDottedExtensions = true;
            this.openFileDialog.Title = "Add file...";
            // 
            // openPlaylistDialog
            // 
            this.openPlaylistDialog.DefaultExt = "mpl";
            this.openPlaylistDialog.Filter = resources.GetString("playlistDialog.Filter");
            this.openPlaylistDialog.RestoreDirectory = true;
            this.openPlaylistDialog.SupportMultiDottedExtensions = true;
            this.openPlaylistDialog.Title = "Open playlist...";
            // 
            // savePlaylistDialog
            // 
            this.savePlaylistDialog.DefaultExt = "mpl";
            this.savePlaylistDialog.Filter = resources.GetString("playlistDialog.Filter");
            this.savePlaylistDialog.SupportMultiDottedExtensions = true;
            this.savePlaylistDialog.Title = "Save playlist...";
            // 
            // dgv_PlayList
            // 
            this.dgv_PlayList.AllowUserToAddRows = false;
            this.dgv_PlayList.AllowUserToDeleteRows = false;
            this.dgv_PlayList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.dgv_PlayList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_PlayList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv_PlayList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_PlayList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgv_PlayList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_PlayList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Playing,
            this.Title,
            this.SkipChapters,
            this.EndChapter});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.SkyBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_PlayList.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_PlayList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_PlayList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgv_PlayList.GridColor = System.Drawing.SystemColors.Window;
            this.dgv_PlayList.Location = new System.Drawing.Point(0, 0);
            this.dgv_PlayList.Name = "dgv_PlayList";
            this.dgv_PlayList.RowHeadersVisible = false;
            this.dgv_PlayList.RowTemplate.Height = 29;
            this.dgv_PlayList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_PlayList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_PlayList.ShowCellErrors = false;
            this.dgv_PlayList.ShowCellToolTips = false;
            this.dgv_PlayList.ShowEditingIcon = false;
            this.dgv_PlayList.ShowRowErrors = false;
            this.dgv_PlayList.Size = new System.Drawing.Size(795, 202);
            this.dgv_PlayList.TabIndex = 1;
            // 
            // Playing
            // 
            this.Playing.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "null";
            this.Playing.DefaultCellStyle = dataGridViewCellStyle2;
            this.Playing.FillWeight = 16F;
            this.Playing.HeaderText = "";
            this.Playing.MinimumWidth = 16;
            this.Playing.Name = "Playing";
            this.Playing.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Playing.Width = 16;
            // 
            // Title
            // 
            this.Title.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Title.FillWeight = 187.2093F;
            this.Title.HeaderText = "Title";
            this.Title.MinimumWidth = 125;
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SkipChapters
            // 
            this.SkipChapters.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.NullValue = null;
            this.SkipChapters.DefaultCellStyle = dataGridViewCellStyle3;
            this.SkipChapters.FillWeight = 50.62795F;
            this.SkipChapters.HeaderText = "Skip Chapters";
            this.SkipChapters.Name = "SkipChapters";
            this.SkipChapters.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // EndChapter
            // 
            this.EndChapter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.NullValue = null;
            this.EndChapter.DefaultCellStyle = dataGridViewCellStyle4;
            this.EndChapter.FillWeight = 36.16279F;
            this.EndChapter.HeaderText = "End Chapter";
            this.EndChapter.MaxInputLength = 2;
            this.EndChapter.MinimumWidth = 10;
            this.EndChapter.Name = "EndChapter";
            this.EndChapter.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PlaylistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(795, 229);
            this.Controls.Add(this.dgv_PlayList);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(278, 260);
            this.Name = "PlaylistForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Playlist";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlaylistFormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormKeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PlayList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private ButtonStripItem buttonAdd;
        private ButtonStripItem buttonDel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private ButtonStripItem buttonNew;
        private ButtonStripItem buttonOpen;
        private ButtonStripItem buttonSave;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private ButtonStripItem buttonLeft;
        private ButtonStripItem buttonRight;
        private System.Windows.Forms.OpenFileDialog openPlaylistDialog;
        private System.Windows.Forms.SaveFileDialog savePlaylistDialog;
        private System.Windows.Forms.DataGridView dgv_PlayList;
        private System.Windows.Forms.DataGridViewImageColumn Playing;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn SkipChapters;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndChapter;
        private ButtonStripItem buttonSortAscending;
        private ButtonStripItem buttonSortDescending;
    }
}
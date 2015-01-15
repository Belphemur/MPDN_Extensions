﻿namespace Mpdn.RenderScript
{
    namespace Shiandow.Deband
    {
        partial class DebandConfigDialog
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
            this.StrengthSetter = new System.Windows.Forms.NumericUpDown();
            this.MarginSetter = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.PassesSetter = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MaxErrorLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.AdvancedBox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.StrengthSetter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MarginSetter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PassesSetter)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StrengthSetter
            // 
            this.StrengthSetter.DecimalPlaces = 1;
            this.StrengthSetter.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.StrengthSetter.Location = new System.Drawing.Point(133, 3);
            this.StrengthSetter.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.StrengthSetter.Name = "StrengthSetter";
            this.StrengthSetter.Size = new System.Drawing.Size(44, 20);
            this.StrengthSetter.TabIndex = 1;
            this.StrengthSetter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.StrengthSetter.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.StrengthSetter.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // MarginSetter
            // 
            this.MarginSetter.DecimalPlaces = 1;
            this.MarginSetter.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.MarginSetter.Location = new System.Drawing.Point(133, 29);
            this.MarginSetter.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.MarginSetter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.MarginSetter.Name = "MarginSetter";
            this.MarginSetter.Size = new System.Drawing.Size(44, 20);
            this.MarginSetter.TabIndex = 2;
            this.MarginSetter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.MarginSetter.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.MarginSetter.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Threshold";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Margin";
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.Cursor = System.Windows.Forms.Cursors.Default;
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Location = new System.Drawing.Point(61, 123);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 3;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(142, 123);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 4;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Disable when bitdepth >";
            // 
            // PassesSetter
            // 
            this.PassesSetter.Location = new System.Drawing.Point(145, 9);
            this.PassesSetter.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.PassesSetter.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.PassesSetter.Name = "PassesSetter";
            this.PassesSetter.Size = new System.Drawing.Size(44, 20);
            this.PassesSetter.TabIndex = 0;
            this.PassesSetter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.PassesSetter.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "bits";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(183, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "bit";
            // 
            // MaxErrorLabel
            // 
            this.MaxErrorLabel.AutoSize = true;
            this.MaxErrorLabel.Location = new System.Drawing.Point(6, 44);
            this.MaxErrorLabel.Name = "MaxErrorLabel";
            this.MaxErrorLabel.Size = new System.Drawing.Size(121, 13);
            this.MaxErrorLabel.TabIndex = 14;
            this.MaxErrorLabel.Text = " (maximum error 0.17 bit)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(183, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "bit";
            // 
            // AdvancedBox
            // 
            this.AdvancedBox.AutoSize = true;
            this.AdvancedBox.Location = new System.Drawing.Point(12, 101);
            this.AdvancedBox.Name = "AdvancedBox";
            this.AdvancedBox.Size = new System.Drawing.Size(149, 17);
            this.AdvancedBox.TabIndex = 16;
            this.AdvancedBox.Text = "Enable advanced settings";
            this.AdvancedBox.UseVisualStyleBackColor = true;
            this.AdvancedBox.CheckedChanged += new System.EventHandler(this.AdvancedBox_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.StrengthSetter);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.MarginSetter);
            this.panel1.Controls.Add(this.MaxErrorLabel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(12, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(206, 60);
            this.panel1.TabIndex = 17;
            // 
            // DebandConfigDialog
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(229, 158);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AdvancedBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PassesSetter);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DebandConfigDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Deband Settings";
            ((System.ComponentModel.ISupportInitialize)(this.StrengthSetter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MarginSetter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PassesSetter)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.NumericUpDown StrengthSetter;
            private System.Windows.Forms.NumericUpDown MarginSetter;
            private System.Windows.Forms.Label label1;
            private System.Windows.Forms.Label label2;
            private System.Windows.Forms.Button ButtonOK;
            private System.Windows.Forms.Button ButtonCancel;
            private System.Windows.Forms.Label label5;
            private System.Windows.Forms.NumericUpDown PassesSetter;
            private System.Windows.Forms.Label label3;
            private System.Windows.Forms.Label label4;
            private System.Windows.Forms.Label MaxErrorLabel;
            private System.Windows.Forms.Label label6;
            private System.Windows.Forms.CheckBox AdvancedBox;
            private System.Windows.Forms.Panel panel1;

        }
    }
}
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
namespace Mpdn.RenderScript
{
    namespace Shiandow.SuperRes
    {
        partial class SuperResConfigDialog
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
            this.SharpnessSetter = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.AntiAliasingSetter = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.AntiRingingSetter = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.PassesSetter = new System.Windows.Forms.NumericUpDown();
            this.PrescalerBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.FastBox = new System.Windows.Forms.CheckBox();
            this.ConfigButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.StrengthSetter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SharpnessSetter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AntiAliasingSetter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AntiRingingSetter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PassesSetter)).BeginInit();
            this.SuspendLayout();
            // 
            // StrengthSetter
            // 
            this.StrengthSetter.DecimalPlaces = 2;
            this.StrengthSetter.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.StrengthSetter.Location = new System.Drawing.Point(81, 33);
            this.StrengthSetter.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StrengthSetter.Name = "StrengthSetter";
            this.StrengthSetter.Size = new System.Drawing.Size(44, 20);
            this.StrengthSetter.TabIndex = 0;
            this.StrengthSetter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.StrengthSetter.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            this.StrengthSetter.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // SharpnessSetter
            // 
            this.SharpnessSetter.DecimalPlaces = 2;
            this.SharpnessSetter.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.SharpnessSetter.Location = new System.Drawing.Point(81, 59);
            this.SharpnessSetter.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.SharpnessSetter.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.SharpnessSetter.Name = "SharpnessSetter";
            this.SharpnessSetter.Size = new System.Drawing.Size(44, 20);
            this.SharpnessSetter.TabIndex = 1;
            this.SharpnessSetter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SharpnessSetter.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.SharpnessSetter.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Strength";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sharpness";
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.Cursor = System.Windows.Forms.Cursors.Default;
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Location = new System.Drawing.Point(141, 132);
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
            this.ButtonCancel.Location = new System.Drawing.Point(222, 132);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 4;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(152, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Anti Aliasing";
            // 
            // AntiAliasingSetter
            // 
            this.AntiAliasingSetter.DecimalPlaces = 2;
            this.AntiAliasingSetter.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.AntiAliasingSetter.Location = new System.Drawing.Point(221, 33);
            this.AntiAliasingSetter.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.AntiAliasingSetter.Name = "AntiAliasingSetter";
            this.AntiAliasingSetter.Size = new System.Drawing.Size(44, 20);
            this.AntiAliasingSetter.TabIndex = 5;
            this.AntiAliasingSetter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AntiAliasingSetter.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(152, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Anti Ringing";
            // 
            // AntiRingingSetter
            // 
            this.AntiRingingSetter.DecimalPlaces = 2;
            this.AntiRingingSetter.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.AntiRingingSetter.Location = new System.Drawing.Point(221, 59);
            this.AntiRingingSetter.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AntiRingingSetter.Name = "AntiRingingSetter";
            this.AntiRingingSetter.Size = new System.Drawing.Size(44, 20);
            this.AntiRingingSetter.TabIndex = 7;
            this.AntiRingingSetter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AntiRingingSetter.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Passes";
            // 
            // PassesSetter
            // 
            this.PassesSetter.Location = new System.Drawing.Point(81, 7);
            this.PassesSetter.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.PassesSetter.Name = "PassesSetter";
            this.PassesSetter.Size = new System.Drawing.Size(44, 20);
            this.PassesSetter.TabIndex = 10;
            this.PassesSetter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.PassesSetter.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // PrescalerBox
            // 
            this.PrescalerBox.FormattingEnabled = true;
            this.PrescalerBox.Location = new System.Drawing.Point(81, 85);
            this.PrescalerBox.Name = "PrescalerBox";
            this.PrescalerBox.Size = new System.Drawing.Size(135, 21);
            this.PrescalerBox.TabIndex = 12;
            this.PrescalerBox.SelectedIndexChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Prescaler";
            // 
            // FastBox
            // 
            this.FastBox.AutoSize = true;
            this.FastBox.Location = new System.Drawing.Point(15, 112);
            this.FastBox.Name = "FastBox";
            this.FastBox.Size = new System.Drawing.Size(103, 17);
            this.FastBox.TabIndex = 14;
            this.FastBox.Text = "Use fast method";
            this.FastBox.UseVisualStyleBackColor = true;
            this.FastBox.CheckedChanged += new System.EventHandler(this.ValueChanged);
            this.FastBox.CheckStateChanged += new System.EventHandler(this.ValueChanged);
            // 
            // ConfigButton
            // 
            this.ConfigButton.Location = new System.Drawing.Point(222, 83);
            this.ConfigButton.Name = "ConfigButton";
            this.ConfigButton.Size = new System.Drawing.Size(75, 23);
            this.ConfigButton.TabIndex = 15;
            this.ConfigButton.Text = "Configure";
            this.ConfigButton.UseVisualStyleBackColor = true;
            this.ConfigButton.Click += new System.EventHandler(this.ConfigButton_Click);
            // 
            // SuperResConfigDialog
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(309, 167);
            this.Controls.Add(this.ConfigButton);
            this.Controls.Add(this.FastBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.PrescalerBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PassesSetter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AntiRingingSetter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AntiAliasingSetter);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SharpnessSetter);
            this.Controls.Add(this.StrengthSetter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SuperResConfigDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SuperRes Settings";
            ((System.ComponentModel.ISupportInitialize)(this.StrengthSetter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SharpnessSetter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AntiAliasingSetter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AntiRingingSetter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PassesSetter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.NumericUpDown StrengthSetter;
            private System.Windows.Forms.NumericUpDown SharpnessSetter;
            private System.Windows.Forms.Label label1;
            private System.Windows.Forms.Label label2;
            private System.Windows.Forms.Button ButtonOK;
            private System.Windows.Forms.Button ButtonCancel;
            private System.Windows.Forms.Label label3;
            private System.Windows.Forms.NumericUpDown AntiAliasingSetter;
            private System.Windows.Forms.Label label4;
            private System.Windows.Forms.NumericUpDown AntiRingingSetter;
            private System.Windows.Forms.Label label5;
            private System.Windows.Forms.NumericUpDown PassesSetter;
            private System.Windows.Forms.ComboBox PrescalerBox;
            private System.Windows.Forms.Label label6;
            private System.Windows.Forms.CheckBox FastBox;
            private System.Windows.Forms.Button ConfigButton;

        }
    }
}

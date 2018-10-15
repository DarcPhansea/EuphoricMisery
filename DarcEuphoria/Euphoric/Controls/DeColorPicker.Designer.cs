namespace DarcEuphoria.Euphoric.Controls
{
    partial class DeColorPicker
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.HexValue = new System.Windows.Forms.Label();
            this.panel_ColorPreview = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_opacity = new DarcEuphoria.Euphoric.Controls.DeDBPanel();
            this.panel_RedSlider = new DarcEuphoria.Euphoric.Controls.DeDBPanel();
            this.panel_ColorPicker = new DarcEuphoria.Euphoric.Controls.DeDBPanel();
            this.panel1 = new DarcEuphoria.Euphoric.Controls.DeDBPanel();
            this.SuspendLayout();
            // 
            // HexValue
            // 
            this.HexValue.AutoSize = true;
            this.HexValue.BackColor = System.Drawing.Color.Transparent;
            this.HexValue.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HexValue.ForeColor = System.Drawing.Color.White;
            this.HexValue.Location = new System.Drawing.Point(208, 259);
            this.HexValue.Name = "HexValue";
            this.HexValue.Size = new System.Drawing.Size(47, 13);
            this.HexValue.TabIndex = 0;
            this.HexValue.Text = "#FF0000";
            // 
            // panel_ColorPreview
            // 
            this.panel_ColorPreview.BackColor = System.Drawing.Color.Transparent;
            this.panel_ColorPreview.Location = new System.Drawing.Point(265, 259);
            this.panel_ColorPreview.Name = "panel_ColorPreview";
            this.panel_ColorPreview.Size = new System.Drawing.Size(50, 13);
            this.panel_ColorPreview.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(11, 255);
            this.textBox1.MaxLength = 3;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(30, 21);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "255";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(54, 255);
            this.textBox2.MaxLength = 3;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(30, 21);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "255";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(97, 255);
            this.textBox3.MaxLength = 3;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(30, 21);
            this.textBox3.TabIndex = 9;
            this.textBox3.Text = "255";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox4.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.ForeColor = System.Drawing.Color.White;
            this.textBox4.Location = new System.Drawing.Point(139, 255);
            this.textBox4.MaxLength = 3;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(30, 21);
            this.textBox4.TabIndex = 10;
            this.textBox4.Text = "255";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Candara", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 258);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "A                  R                  G                  B";
            // 
            // panel_opacity
            // 
            this.panel_opacity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(1)))), ((int)(((byte)(1)))));
            this.panel_opacity.Location = new System.Drawing.Point(295, 0);
            this.panel_opacity.Name = "panel_opacity";
            this.panel_opacity.Size = new System.Drawing.Size(20, 255);
            this.panel_opacity.TabIndex = 5;
            // 
            // panel_RedSlider
            // 
            this.panel_RedSlider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(1)))), ((int)(((byte)(1)))));
            this.panel_RedSlider.Location = new System.Drawing.Point(265, 0);
            this.panel_RedSlider.Name = "panel_RedSlider";
            this.panel_RedSlider.Size = new System.Drawing.Size(20, 255);
            this.panel_RedSlider.TabIndex = 0;
            // 
            // panel_ColorPicker
            // 
            this.panel_ColorPicker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(1)))), ((int)(((byte)(1)))));
            this.panel_ColorPicker.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.panel_ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.panel_ColorPicker.Name = "panel_ColorPicker";
            this.panel_ColorPicker.Size = new System.Drawing.Size(255, 255);
            this.panel_ColorPicker.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(255, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(63, 255);
            this.panel1.TabIndex = 6;
            // 
            // DeColorPicker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.HexValue);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.panel_opacity);
            this.Controls.Add(this.panel_RedSlider);
            this.Controls.Add(this.panel_ColorPicker);
            this.Controls.Add(this.panel_ColorPreview);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "DeColorPicker";
            this.Size = new System.Drawing.Size(318, 276);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label HexValue;
        private System.Windows.Forms.Panel panel_ColorPreview;
        private DeDBPanel panel_ColorPicker;
        private DeDBPanel panel_RedSlider;
        private DeDBPanel panel1;
        private DeDBPanel panel_opacity;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label1;
    }
}
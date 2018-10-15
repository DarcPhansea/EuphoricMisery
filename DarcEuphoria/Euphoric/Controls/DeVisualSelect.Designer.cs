namespace DarcEuphoria.Euphoric.Controls
{
    partial class DeVisualSelect
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
            this.dePanel1 = new DarcEuphoria.Euphoric.Controls.DePanel();
            this.tab2 = new DarcEuphoria.Euphoric.Controls.DeRadioButton();
            this.tab1 = new DarcEuphoria.Euphoric.Controls.DeRadioButton();
            this.tab0 = new DarcEuphoria.Euphoric.Controls.DeRadioButton();
            this.dePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dePanel1
            // 
            this.dePanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.dePanel1.Controls.Add(this.tab2);
            this.dePanel1.Controls.Add(this.tab1);
            this.dePanel1.Controls.Add(this.tab0);
            this.dePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dePanel1.Location = new System.Drawing.Point(0, 0);
            this.dePanel1.Name = "dePanel1";
            this.dePanel1.Size = new System.Drawing.Size(270, 25);
            this.dePanel1.TabIndex = 0;
            // 
            // tab2
            // 
            this.tab2.Appearance = System.Windows.Forms.Appearance.Button;
            this.tab2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.tab2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tab2.FlatAppearance.BorderSize = 0;
            this.tab2.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(62)))), ((int)(((byte)(66)))));
            this.tab2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tab2.ForeColor = System.Drawing.Color.White;
            this.tab2.Location = new System.Drawing.Point(180, 0);
            this.tab2.Name = "tab2";
            this.tab2.Size = new System.Drawing.Size(90, 25);
            this.tab2.TabIndex = 8;
            this.tab2.Text = "Yourself";
            this.tab2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tab2.UseVisualStyleBackColor = false;
            // 
            // tab1
            // 
            this.tab1.Appearance = System.Windows.Forms.Appearance.Button;
            this.tab1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.tab1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tab1.FlatAppearance.BorderSize = 0;
            this.tab1.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(62)))), ((int)(((byte)(66)))));
            this.tab1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tab1.ForeColor = System.Drawing.Color.White;
            this.tab1.Location = new System.Drawing.Point(90, 0);
            this.tab1.Name = "tab1";
            this.tab1.Size = new System.Drawing.Size(90, 25);
            this.tab1.TabIndex = 7;
            this.tab1.Text = "Team";
            this.tab1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tab1.UseVisualStyleBackColor = false;
            // 
            // tab0
            // 
            this.tab0.Appearance = System.Windows.Forms.Appearance.Button;
            this.tab0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.tab0.Checked = true;
            this.tab0.Dock = System.Windows.Forms.DockStyle.Left;
            this.tab0.FlatAppearance.BorderSize = 0;
            this.tab0.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(62)))), ((int)(((byte)(66)))));
            this.tab0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tab0.ForeColor = System.Drawing.Color.White;
            this.tab0.Location = new System.Drawing.Point(0, 0);
            this.tab0.Name = "tab0";
            this.tab0.Size = new System.Drawing.Size(90, 25);
            this.tab0.TabIndex = 6;
            this.tab0.TabStop = true;
            this.tab0.Text = "Enemy";
            this.tab0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tab0.UseVisualStyleBackColor = false;
            // 
            // DeVisualSelect
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.dePanel1);
            this.Font = new System.Drawing.Font("Candara", 11.25F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DeVisualSelect";
            this.Size = new System.Drawing.Size(270, 25);
            this.dePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DePanel dePanel1;
        private DeRadioButton tab2;
        private DeRadioButton tab1;
        private DeRadioButton tab0;
    }
}

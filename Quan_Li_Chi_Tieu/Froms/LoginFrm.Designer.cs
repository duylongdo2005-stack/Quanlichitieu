namespace Quan_Li_Chi_Tieu.Froms
{
    partial class LoginFrm
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
            label1 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            panelLeft = new Panel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.HotTrack;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(12, 92);
            label1.Name = "label1";
            label1.Size = new Size(292, 82);
            label1.TabIndex = 0;
            label1.Text = "QUẢN LÝ \r\nCHI TIÊU CÁ NHÂN";
            label1.Click += label1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(412, 136);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(285, 27);
            textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(412, 212);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(285, 27);
            textBox2.TabIndex = 2;
            // 
            // panelLeft
            // 
            panelLeft.BackColor = SystemColors.HotTrack;
            panelLeft.ForeColor = SystemColors.HighlightText;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(383, 452);
            panelLeft.TabIndex = 3;
            // 
            // LoginFrm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(932, 450);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(panelLeft);
            ForeColor = SystemColors.ActiveBorder;
            Name = "LoginFrm";
            Text = "LoginFrm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Panel panelLeft;
    }
}
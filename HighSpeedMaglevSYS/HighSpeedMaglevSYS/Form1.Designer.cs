namespace HighSpeedMaglevSYS
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.A = new System.Windows.Forms.Panel();
            this.B = new System.Windows.Forms.Panel();
            this.C = new System.Windows.Forms.Panel();
            this.D = new System.Windows.Forms.Panel();
            this.F = new System.Windows.Forms.Panel();
            this.E = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // A
            // 
            this.A.Location = new System.Drawing.Point(0, 0);
            this.A.Margin = new System.Windows.Forms.Padding(0);
            this.A.Name = "A";
            this.A.Size = new System.Drawing.Size(54, 300);
            this.A.TabIndex = 0;
            this.A.Paint += new System.Windows.Forms.PaintEventHandler(this.A_Paint);
            // 
            // B
            // 
            this.B.BackColor = System.Drawing.Color.Transparent;
            this.B.Location = new System.Drawing.Point(54, 0);
            this.B.Margin = new System.Windows.Forms.Padding(0);
            this.B.Name = "B";
            this.B.Size = new System.Drawing.Size(280, 300);
            this.B.TabIndex = 1;
            this.B.Paint += new System.Windows.Forms.PaintEventHandler(this.B_Paint);
            this.B.MouseHover += new System.EventHandler(this.B_MouseHover);
            this.B.MouseMove += new System.Windows.Forms.MouseEventHandler(this.B_MouseMove);
            // 
            // C
            // 
            this.C.Location = new System.Drawing.Point(0, 300);
            this.C.Margin = new System.Windows.Forms.Padding(0);
            this.C.Name = "C";
            this.C.Size = new System.Drawing.Size(334, 54);
            this.C.TabIndex = 2;
            this.C.Paint += new System.Windows.Forms.PaintEventHandler(this.C_Paint);
            // 
            // D
            // 
            this.D.Location = new System.Drawing.Point(334, 0);
            this.D.Margin = new System.Windows.Forms.Padding(0);
            this.D.Name = "D";
            this.D.Size = new System.Drawing.Size(244, 300);
            this.D.TabIndex = 3;
            this.D.Paint += new System.Windows.Forms.PaintEventHandler(this.D_Paint);
            // 
            // F
            // 
            this.F.Location = new System.Drawing.Point(578, 0);
            this.F.Margin = new System.Windows.Forms.Padding(0);
            this.F.Name = "F";
            this.F.Size = new System.Drawing.Size(62, 480);
            this.F.TabIndex = 0;
            this.F.Paint += new System.Windows.Forms.PaintEventHandler(this.F_Paint);
            // 
            // E
            // 
            this.E.Location = new System.Drawing.Point(0, 300);
            this.E.Margin = new System.Windows.Forms.Padding(0);
            this.E.Name = "E";
            this.E.Size = new System.Drawing.Size(578, 180);
            this.E.TabIndex = 4;
            this.E.Paint += new System.Windows.Forms.PaintEventHandler(this.E_Paint);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 25;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(641, 479);
            this.Controls.Add(this.F);
            this.Controls.Add(this.D);
            this.Controls.Add(this.C);
            this.Controls.Add(this.B);
            this.Controls.Add(this.A);
            this.Controls.Add(this.E);
            this.Name = "Form1";
            this.Text = "Form1";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel A;
        private System.Windows.Forms.Panel B;
        private System.Windows.Forms.Panel C;
        private System.Windows.Forms.Panel D;
        private System.Windows.Forms.Panel E;
        private System.Windows.Forms.Panel F;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}


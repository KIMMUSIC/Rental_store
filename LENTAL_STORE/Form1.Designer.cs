﻿
namespace LENTAL_STORE
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.admin1 = new LENTAL_STORE.LS.Admin();
            this.detail1 = new LENTAL_STORE.LS.detail();
            this.usermain1 = new LENTAL_STORE.LS.usermain();
            this.home1 = new LENTAL_STORE.home();
            this.signin1 = new LENTAL_STORE.signin();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LavenderBlush;
            this.panel1.Controls.Add(this.admin1);
            this.panel1.Controls.Add(this.detail1);
            this.panel1.Controls.Add(this.usermain1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.home1);
            this.panel1.Controls.Add(this.signin1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 600);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(726, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(37, 37);
            this.button2.TabIndex = 7;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // admin1
            // 
            this.admin1.Location = new System.Drawing.Point(3, 3);
            this.admin1.Name = "admin1";
            this.admin1.Size = new System.Drawing.Size(794, 594);
            this.admin1.TabIndex = 13;
            // 
            // detail1
            // 
            this.detail1.BackColor = System.Drawing.Color.LavenderBlush;
            this.detail1.Location = new System.Drawing.Point(155, 169);
            this.detail1.Name = "detail1";
            this.detail1.Size = new System.Drawing.Size(608, 431);
            this.detail1.TabIndex = 12;
            // 
            // usermain1
            // 
            this.usermain1.BackColor = System.Drawing.Color.LavenderBlush;
            this.usermain1.Location = new System.Drawing.Point(0, 0);
            this.usermain1.Name = "usermain1";
            this.usermain1.Size = new System.Drawing.Size(800, 600);
            this.usermain1.TabIndex = 11;
            // 
            // home1
            // 
            this.home1.BackColor = System.Drawing.Color.LavenderBlush;
            this.home1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.home1.Location = new System.Drawing.Point(0, 0);
            this.home1.Name = "home1";
            this.home1.Size = new System.Drawing.Size(800, 600);
            this.home1.TabIndex = 10;
            this.home1.Load += new System.EventHandler(this.home1_Load);
            // 
            // signin1
            // 
            this.signin1.BackColor = System.Drawing.Color.LavenderBlush;
            this.signin1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.signin1.Location = new System.Drawing.Point(0, 0);
            this.signin1.Name = "signin1";
            this.signin1.Size = new System.Drawing.Size(800, 600);
            this.signin1.TabIndex = 9;
            this.signin1.Visible = false;
            this.signin1.Load += new System.EventHandler(this.signin1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private signin signin1;
        private home home1;
        private LS.usermain usermain1;
        private LS.detail detail1;
        private LS.Admin admin1;
    }
}


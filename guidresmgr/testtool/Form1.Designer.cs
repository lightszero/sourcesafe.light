﻿namespace WindowsFormsApplication1
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
            this.textUrl = new System.Windows.Forms.TextBox();
            this.textCall1 = new System.Windows.Forms.TextBox();
            this.textCall2 = new System.Windows.Forms.TextBox();
            this.textCall3 = new System.Windows.Forms.TextBox();
            this.textFile = new System.Windows.Forms.TextBox();
            this.buttonSetFile = new System.Windows.Forms.Button();
            this.GetFileName = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textUser = new System.Windows.Forms.TextBox();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textUserID = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textTag = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textUrl
            // 
            this.textUrl.Location = new System.Drawing.Point(13, 13);
            this.textUrl.Name = "textUrl";
            this.textUrl.Size = new System.Drawing.Size(531, 21);
            this.textUrl.TabIndex = 0;
            this.textUrl.Text = "http://file.cltri.com/hash_upload/";
            // 
            // textCall1
            // 
            this.textCall1.Location = new System.Drawing.Point(13, 40);
            this.textCall1.Name = "textCall1";
            this.textCall1.Size = new System.Drawing.Size(531, 21);
            this.textCall1.TabIndex = 1;
            this.textCall1.Text = "getFilename.php";
            // 
            // textCall2
            // 
            this.textCall2.Location = new System.Drawing.Point(13, 67);
            this.textCall2.Name = "textCall2";
            this.textCall2.Size = new System.Drawing.Size(531, 21);
            this.textCall2.TabIndex = 2;
            this.textCall2.Text = "getFileinfo.php";
            this.textCall2.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textCall3
            // 
            this.textCall3.Location = new System.Drawing.Point(13, 94);
            this.textCall3.Name = "textCall3";
            this.textCall3.Size = new System.Drawing.Size(531, 21);
            this.textCall3.TabIndex = 3;
            this.textCall3.Text = "upload.php";
            // 
            // textFile
            // 
            this.textFile.Location = new System.Drawing.Point(13, 138);
            this.textFile.Name = "textFile";
            this.textFile.Size = new System.Drawing.Size(393, 21);
            this.textFile.TabIndex = 4;
            // 
            // buttonSetFile
            // 
            this.buttonSetFile.Location = new System.Drawing.Point(413, 138);
            this.buttonSetFile.Name = "buttonSetFile";
            this.buttonSetFile.Size = new System.Drawing.Size(75, 23);
            this.buttonSetFile.TabIndex = 5;
            this.buttonSetFile.Text = "...";
            this.buttonSetFile.UseVisualStyleBackColor = true;
            this.buttonSetFile.Click += new System.EventHandler(this.buttonSetFile_Click);
            // 
            // GetFileName
            // 
            this.GetFileName.Location = new System.Drawing.Point(13, 274);
            this.GetFileName.Name = "GetFileName";
            this.GetFileName.Size = new System.Drawing.Size(94, 23);
            this.GetFileName.TabIndex = 6;
            this.GetFileName.Text = "GetFileName";
            this.GetFileName.UseVisualStyleBackColor = true;
            this.GetFileName.Click += new System.EventHandler(this.GetFileName_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(113, 274);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "GetFileInfo";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(213, 274);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "Upload";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(13, 303);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(475, 71);
            this.textBox6.TabIndex = 9;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(13, 166);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(393, 64);
            this.listBox1.TabIndex = 10;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 247);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(475, 21);
            this.textBox1.TabIndex = 11;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(13, 381);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(475, 21);
            this.textBox2.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(725, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Reg/Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textUser
            // 
            this.textUser.Location = new System.Drawing.Point(700, 12);
            this.textUser.Name = "textUser";
            this.textUser.Size = new System.Drawing.Size(100, 21);
            this.textUser.TabIndex = 14;
            this.textUser.Text = "3";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(700, 39);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(100, 21);
            this.textPassword.TabIndex = 15;
            this.textPassword.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(641, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(641, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(641, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "userid";
            // 
            // textUserID
            // 
            this.textUserID.Location = new System.Drawing.Point(700, 115);
            this.textUserID.Name = "textUserID";
            this.textUserID.Size = new System.Drawing.Size(100, 21);
            this.textUserID.TabIndex = 18;
            this.textUserID.Text = "2";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(725, 142);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "GetInfo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(641, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "tag";
            // 
            // textTag
            // 
            this.textTag.Location = new System.Drawing.Point(700, 171);
            this.textTag.Name = "textTag";
            this.textTag.Size = new System.Drawing.Size(100, 21);
            this.textTag.TabIndex = 21;
            this.textTag.Text = "2";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(725, 198);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 23;
            this.button5.Text = "PushTag";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 452);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textTag);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textUserID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textPassword);
            this.Controls.Add(this.textUser);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.GetFileName);
            this.Controls.Add(this.buttonSetFile);
            this.Controls.Add(this.textFile);
            this.Controls.Add(this.textCall3);
            this.Controls.Add(this.textCall2);
            this.Controls.Add(this.textCall1);
            this.Controls.Add(this.textUrl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textUrl;
        private System.Windows.Forms.TextBox textCall1;
        private System.Windows.Forms.TextBox textCall2;
        private System.Windows.Forms.TextBox textCall3;
        private System.Windows.Forms.TextBox textFile;
        private System.Windows.Forms.Button buttonSetFile;
        private System.Windows.Forms.Button GetFileName;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textUser;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textUserID;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textTag;
        private System.Windows.Forms.Button button5;
    }
}


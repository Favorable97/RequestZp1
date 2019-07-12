namespace RequestZp1 {
    partial class SignInProfile {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignInProfile));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.HeadLine = new System.Windows.Forms.Label();
            this.NameP = new System.Windows.Forms.TextBox();
            this.lPassword = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.TextBox();
            this.lName = new System.Windows.Forms.Label();
            this.lSignIn = new System.Windows.Forms.Label();
            this.SignInPanel = new System.Windows.Forms.Panel();
            this.SignIn = new System.Windows.Forms.Button();
            this.RememberMe = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SignInPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Teal;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.HeadLine);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 441);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(21, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(348, 247);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // HeadLine
            // 
            this.HeadLine.AutoSize = true;
            this.HeadLine.Font = new System.Drawing.Font("Century Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HeadLine.ForeColor = System.Drawing.Color.White;
            this.HeadLine.Location = new System.Drawing.Point(135, 318);
            this.HeadLine.Name = "HeadLine";
            this.HeadLine.Size = new System.Drawing.Size(135, 38);
            this.HeadLine.TabIndex = 0;
            this.HeadLine.Text = "ТФОМС";
            // 
            // NameP
            // 
            this.NameP.Font = new System.Drawing.Font("Century", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NameP.Location = new System.Drawing.Point(21, 117);
            this.NameP.Name = "NameP";
            this.NameP.Size = new System.Drawing.Size(286, 30);
            this.NameP.TabIndex = 3;
            // 
            // lPassword
            // 
            this.lPassword.AutoSize = true;
            this.lPassword.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lPassword.Location = new System.Drawing.Point(21, 170);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new System.Drawing.Size(85, 22);
            this.lPassword.TabIndex = 2;
            this.lPassword.Text = "Пароль:";
            // 
            // Password
            // 
            this.Password.Font = new System.Drawing.Font("Century", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Password.Location = new System.Drawing.Point(21, 205);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(286, 30);
            this.Password.TabIndex = 3;
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lName.Location = new System.Drawing.Point(21, 82);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(54, 22);
            this.lName.TabIndex = 2;
            this.lName.Text = "Имя:";
            // 
            // lSignIn
            // 
            this.lSignIn.AutoSize = true;
            this.lSignIn.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lSignIn.Location = new System.Drawing.Point(15, 30);
            this.lSignIn.Name = "lSignIn";
            this.lSignIn.Size = new System.Drawing.Size(83, 36);
            this.lSignIn.TabIndex = 1;
            this.lSignIn.Text = "Вход";
            // 
            // SignInPanel
            // 
            this.SignInPanel.Controls.Add(this.SignIn);
            this.SignInPanel.Controls.Add(this.lSignIn);
            this.SignInPanel.Controls.Add(this.lName);
            this.SignInPanel.Controls.Add(this.Password);
            this.SignInPanel.Controls.Add(this.lPassword);
            this.SignInPanel.Controls.Add(this.NameP);
            this.SignInPanel.Controls.Add(this.RememberMe);
            this.SignInPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.SignInPanel.Location = new System.Drawing.Point(427, 0);
            this.SignInPanel.Name = "SignInPanel";
            this.SignInPanel.Size = new System.Drawing.Size(400, 441);
            this.SignInPanel.TabIndex = 5;
            // 
            // SignIn
            // 
            this.SignIn.BackColor = System.Drawing.Color.LightSteelBlue;
            this.SignIn.Font = new System.Drawing.Font("Century", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SignIn.Location = new System.Drawing.Point(21, 288);
            this.SignIn.Name = "SignIn";
            this.SignIn.Size = new System.Drawing.Size(146, 42);
            this.SignIn.TabIndex = 7;
            this.SignIn.Text = "Войти";
            this.SignIn.UseVisualStyleBackColor = false;
            this.SignIn.Click += new System.EventHandler(this.SignIn_Click);
            // 
            // RememberMe
            // 
            this.RememberMe.AutoSize = true;
            this.RememberMe.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RememberMe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.RememberMe.Location = new System.Drawing.Point(21, 254);
            this.RememberMe.Name = "RememberMe";
            this.RememberMe.Size = new System.Drawing.Size(169, 24);
            this.RememberMe.TabIndex = 11;
            this.RememberMe.Text = "Запомнить меня";
            this.RememberMe.UseVisualStyleBackColor = true;
            // 
            // SignInProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SignInPanel);
            this.Controls.Add(this.panel1);
            this.Name = "SignInProfile";
            this.Size = new System.Drawing.Size(827, 441);
            this.Load += new System.EventHandler(this.Registration_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.SignInPanel.ResumeLayout(false);
            this.SignInPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label HeadLine;
        private System.Windows.Forms.TextBox NameP;
        private System.Windows.Forms.Label lPassword;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label lSignIn;
        private System.Windows.Forms.Panel SignInPanel;
        private System.Windows.Forms.Button SignIn;
        private System.Windows.Forms.CheckBox RememberMe;
    }
}

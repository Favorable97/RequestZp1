namespace RequestZp1 {
    partial class Form1 {
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tSurname = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tName = new System.Windows.Forms.TextBox();
            this.tFatherName = new System.Windows.Forms.TextBox();
            this.tBirthday = new System.Windows.Forms.TextBox();
            this.AddPeople = new System.Windows.Forms.Button();
            this.CreateXmlFile = new System.Windows.Forms.Button();
            this.RequestTable = new System.Windows.Forms.DataGridView();
            this.Column7 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.historyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myHistoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allHistoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.namePerson2 = new System.Windows.Forms.ToolStripMenuItem();
            this.addPersonMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelWithHistory = new System.Windows.Forms.Panel();
            this.HistoryTable = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tNumber = new System.Windows.Forms.TextBox();
            this.tSeries = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.DownLoadFileCSV = new System.Windows.Forms.Button();
            this.registration1 = new RequestZp1.Registration();
            this.signInProfile1 = new RequestZp1.SignInProfile();
            ((System.ComponentModel.ISupportInitialize)(this.RequestTable)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panelWithHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HistoryTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(71, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Фамилия";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(96, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 22);
            this.label2.TabIndex = 0;
            this.label2.Text = "Имя";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(73, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 22);
            this.label3.TabIndex = 0;
            this.label3.Text = "Отчество";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(42, 287);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 22);
            this.label4.TabIndex = 10;
            this.label4.Text = "Дата рождения";
            // 
            // tSurname
            // 
            this.tSurname.Font = new System.Drawing.Font("Century", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tSurname.Location = new System.Drawing.Point(7, 75);
            this.tSurname.Name = "tSurname";
            this.tSurname.Size = new System.Drawing.Size(227, 30);
            this.tSurname.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(70, 157);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(73, 201);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 22);
            this.label5.TabIndex = 11;
            this.label5.Text = "Отчество";
            // 
            // tName
            // 
            this.tName.Font = new System.Drawing.Font("Century", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tName.Location = new System.Drawing.Point(7, 157);
            this.tName.Name = "tName";
            this.tName.Size = new System.Drawing.Size(227, 30);
            this.tName.TabIndex = 2;
            // 
            // tFatherName
            // 
            this.tFatherName.Font = new System.Drawing.Font("Century", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tFatherName.Location = new System.Drawing.Point(7, 239);
            this.tFatherName.Name = "tFatherName";
            this.tFatherName.Size = new System.Drawing.Size(227, 30);
            this.tFatherName.TabIndex = 3;
            // 
            // tBirthday
            // 
            this.tBirthday.Font = new System.Drawing.Font("Century", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tBirthday.Location = new System.Drawing.Point(7, 324);
            this.tBirthday.Name = "tBirthday";
            this.tBirthday.Size = new System.Drawing.Size(227, 30);
            this.tBirthday.TabIndex = 4;
            // 
            // AddPeople
            // 
            this.AddPeople.BackColor = System.Drawing.Color.LightGray;
            this.AddPeople.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddPeople.Location = new System.Drawing.Point(523, 324);
            this.AddPeople.Name = "AddPeople";
            this.AddPeople.Size = new System.Drawing.Size(129, 51);
            this.AddPeople.TabIndex = 5;
            this.AddPeople.Text = "Добавить";
            this.AddPeople.UseVisualStyleBackColor = false;
            this.AddPeople.Click += new System.EventHandler(this.AddPeople_Click);
            // 
            // CreateXmlFile
            // 
            this.CreateXmlFile.BackColor = System.Drawing.Color.LightSteelBlue;
            this.CreateXmlFile.Enabled = false;
            this.CreateXmlFile.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CreateXmlFile.Location = new System.Drawing.Point(1048, 324);
            this.CreateXmlFile.Name = "CreateXmlFile";
            this.CreateXmlFile.Size = new System.Drawing.Size(141, 60);
            this.CreateXmlFile.TabIndex = 7;
            this.CreateXmlFile.Text = "Проверить в ЦС";
            this.CreateXmlFile.UseVisualStyleBackColor = false;
            this.CreateXmlFile.Click += new System.EventHandler(this.CreateXmlFile_Click);
            // 
            // RequestTable
            // 
            this.RequestTable.AllowUserToAddRows = false;
            this.RequestTable.AllowUserToDeleteRows = false;
            this.RequestTable.BackgroundColor = System.Drawing.Color.PaleTurquoise;
            this.RequestTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RequestTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RequestTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11});
            this.RequestTable.Location = new System.Drawing.Point(678, 75);
            this.RequestTable.Name = "RequestTable";
            this.RequestTable.RowHeadersVisible = false;
            this.RequestTable.Size = new System.Drawing.Size(524, 240);
            this.RequestTable.TabIndex = 6;
            this.RequestTable.CurrentCellDirtyStateChanged += new System.EventHandler(this.RequestTable_CurrentCellDirtyStateChanged);
            // 
            // Column7
            // 
            this.Column7.HeaderText = "";
            this.Column7.Name = "Column7";
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column7.Width = 30;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "ФИО";
            this.Column1.Name = "Column1";
            this.Column1.Width = 110;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Дата рождения";
            this.Column2.Name = "Column2";
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Статус";
            this.Column3.Name = "Column3";
            this.Column3.Width = 70;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Поиск в РС";
            this.Column8.Name = "Column8";
            this.Column8.Width = 60;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Отправка запроса в ЦС";
            this.Column9.Name = "Column9";
            this.Column9.Width = 70;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Ответ1 (ЦС)";
            this.Column10.Name = "Column10";
            this.Column10.Width = 50;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Ответ2 (ЦС)";
            this.Column11.Name = "Column11";
            this.Column11.Width = 50;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.LightBlue;
            this.menuStrip1.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.historyMenuItem,
            this.namePerson2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip1.Size = new System.Drawing.Size(1312, 28);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // historyMenuItem
            // 
            this.historyMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.myHistoryMenuItem,
            this.allHistoryMenuItem});
            this.historyMenuItem.Name = "historyMenuItem";
            this.historyMenuItem.Size = new System.Drawing.Size(86, 24);
            this.historyMenuItem.Text = "История";
            // 
            // myHistoryMenuItem
            // 
            this.myHistoryMenuItem.Name = "myHistoryMenuItem";
            this.myHistoryMenuItem.Size = new System.Drawing.Size(175, 24);
            this.myHistoryMenuItem.Text = "Моя история";
            this.myHistoryMenuItem.Click += new System.EventHandler(this.MyHistoryMenuItem_Click);
            // 
            // allHistoryMenuItem
            // 
            this.allHistoryMenuItem.Name = "allHistoryMenuItem";
            this.allHistoryMenuItem.Size = new System.Drawing.Size(175, 24);
            this.allHistoryMenuItem.Text = "Вся история";
            this.allHistoryMenuItem.Click += new System.EventHandler(this.AllHistoryMenuItem_Click);
            // 
            // namePerson2
            // 
            this.namePerson2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPersonMenuItem,
            this.exitMenuItem});
            this.namePerson2.Name = "namePerson2";
            this.namePerson2.Size = new System.Drawing.Size(62, 24);
            this.namePerson2.Text = "name";
            // 
            // addPersonMenuItem
            // 
            this.addPersonMenuItem.Name = "addPersonMenuItem";
            this.addPersonMenuItem.Size = new System.Drawing.Size(225, 24);
            this.addPersonMenuItem.Text = "Добавить человека";
            this.addPersonMenuItem.Click += new System.EventHandler(this.AddPersonMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(225, 24);
            this.exitMenuItem.Text = "Выход";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // panelWithHistory
            // 
            this.panelWithHistory.Controls.Add(this.HistoryTable);
            this.panelWithHistory.Location = new System.Drawing.Point(7, 533);
            this.panelWithHistory.Name = "panelWithHistory";
            this.panelWithHistory.Size = new System.Drawing.Size(918, 488);
            this.panelWithHistory.TabIndex = 11;
            this.panelWithHistory.Visible = false;
            // 
            // HistoryTable
            // 
            this.HistoryTable.AllowUserToDeleteRows = false;
            this.HistoryTable.BackgroundColor = System.Drawing.SystemColors.Control;
            this.HistoryTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.HistoryTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HistoryTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column5,
            this.Column6});
            this.HistoryTable.Location = new System.Drawing.Point(57, 20);
            this.HistoryTable.Name = "HistoryTable";
            this.HistoryTable.RowHeadersVisible = false;
            this.HistoryTable.Size = new System.Drawing.Size(404, 461);
            this.HistoryTable.TabIndex = 0;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "ID";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Фамилия";
            this.Column5.Name = "Column5";
            this.Column5.Width = 150;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Дата действия";
            this.Column6.Name = "Column6";
            this.Column6.Width = 150;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tNumber);
            this.groupBox1.Controls.Add(this.tSeries);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(240, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 240);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выберите тип документа и введите необходимые данные";
            // 
            // tNumber
            // 
            this.tNumber.Location = new System.Drawing.Point(85, 159);
            this.tNumber.Name = "tNumber";
            this.tNumber.Size = new System.Drawing.Size(116, 27);
            this.tNumber.TabIndex = 4;
            // 
            // tSeries
            // 
            this.tSeries.Location = new System.Drawing.Point(85, 111);
            this.tSeries.Name = "tSeries";
            this.tSeries.Size = new System.Drawing.Size(116, 27);
            this.tSeries.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 20);
            this.label7.TabIndex = 2;
            this.label7.Text = "Номер:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 20);
            this.label6.TabIndex = 1;
            this.label6.Text = "Серия:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 59);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(385, 28);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.Text = "Тип документа";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // DownLoadFileCSV
            // 
            this.DownLoadFileCSV.BackColor = System.Drawing.Color.LightGray;
            this.DownLoadFileCSV.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DownLoadFileCSV.Location = new System.Drawing.Point(246, 324);
            this.DownLoadFileCSV.Name = "DownLoadFileCSV";
            this.DownLoadFileCSV.Size = new System.Drawing.Size(199, 51);
            this.DownLoadFileCSV.TabIndex = 5;
            this.DownLoadFileCSV.Text = "Загрузить файл";
            this.DownLoadFileCSV.UseVisualStyleBackColor = false;
            this.DownLoadFileCSV.Click += new System.EventHandler(this.DownLoadFileCSV_Click);
            // 
            // registration1
            // 
            this.registration1.Location = new System.Drawing.Point(-1, 483);
            this.registration1.Name = "registration1";
            this.registration1.Size = new System.Drawing.Size(1314, 546);
            this.registration1.TabIndex = 13;
            // 
            // signInProfile1
            // 
            this.signInProfile1.Location = new System.Drawing.Point(-1, 476);
            this.signInProfile1.Name = "signInProfile1";
            this.signInProfile1.Size = new System.Drawing.Size(1306, 538);
            this.signInProfile1.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(1312, 539);
            this.Controls.Add(this.signInProfile1);
            this.Controls.Add(this.registration1);
            this.Controls.Add(this.panelWithHistory);
            this.Controls.Add(this.RequestTable);
            this.Controls.Add(this.CreateXmlFile);
            this.Controls.Add(this.DownLoadFileCSV);
            this.Controls.Add(this.AddPeople);
            this.Controls.Add(this.tBirthday);
            this.Controls.Add(this.tFatherName);
            this.Controls.Add(this.tName);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.tSurname);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "RequestZp1";
            ((System.ComponentModel.ISupportInitialize)(this.RequestTable)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelWithHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HistoryTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tSurname;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tName;
        private System.Windows.Forms.TextBox tFatherName;
        private System.Windows.Forms.TextBox tBirthday;
        private System.Windows.Forms.Button AddPeople;
        private System.Windows.Forms.Button CreateXmlFile;
        private System.Windows.Forms.DataGridView RequestTable;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem namePerson2;
        private System.Windows.Forms.ToolStripMenuItem addPersonMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myHistoryMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allHistoryMenuItem;
        private System.Windows.Forms.Panel panelWithHistory;
        private System.Windows.Forms.DataGridView HistoryTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox tNumber;
        private System.Windows.Forms.TextBox tSeries;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button DownLoadFileCSV;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private Registration registration1;
        private SignInProfile signInProfile1;
        //private Registration registration1;
    }
}


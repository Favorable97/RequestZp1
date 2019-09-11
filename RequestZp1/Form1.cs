using System;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Security.Cryptography;
using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Threading;

namespace RequestZp1 {
    public partial class Form1 : Form {
        public string nameUser, rights, encPas;
        public bool flag = false;
        private bool isFile = false;
        string fileName;
        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=ident;Persist Security Info=True;User ID=user;Password=гыук";
        public Form1() {
            InitializeComponent();
            openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.Title = "Обзор";
            //folderBrowserDialog1.ShowNewFolderButton = false;
            signInProfile1.Location = this.Location;
            menuStrip1.Visible = false;
            this.Size = signInProfile1.Size;
            registration1.Location = this.Location;
            //panelWithHistory.Location = new System.Drawing.Point(0, 31);
            registration1.Hide();
        }
        
        public void VisibleProfile() {
            menuStrip1.Visible = true;
            namePerson2.Text = nameUser;
            menuStrip1.Visible = true;
            splitContainer1.Visible = true;
        }

        // заполнение таблицы
        public void ToFillTable() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                SqlCommand com = new SqlCommand("SELECT Peoples.Surname, Peoples.Name, Peoples.FatherName, Peoples.DateBirthday, Peoples.Pol, Peoples.CodeDocument, Peoples.SeriesDoc, Peoples.NumbDoc, Peoples.Uprak1, Peoples.Uprak2 " +
                    "FROM Users join ListOperator on Users.ID = " + GetID() + " join Peoples on ListOperator.IDPeople = Peoples.ID", con);
                con.Open();
                //com.Parameters.AddWithValue("@ListOperator.IDUser", GetID());

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read()) {
                    DataGridViewRow newRow = new DataGridViewRow();

                    DataGridViewCell checkoxCell = new DataGridViewCheckBoxCell {
                        Value = 0
                    };
                    DataGridViewCell Surname = new DataGridViewTextBoxCell {
                        Value = reader.GetString(0)
                    };

                    DataGridViewCell Name = new DataGridViewTextBoxCell {
                        Value = reader.GetString(1)
                    };

                    DataGridViewCell FatherName = new DataGridViewTextBoxCell {
                        Value = reader.GetString(2)
                    };

                    DataGridViewCell Birthday = new DataGridViewTextBoxCell {
                        Value = reader.GetDateTime(3).ToShortDateString()
                    };

                    DataGridViewCell Pol = new DataGridViewTextBoxCell {
                        Value = Convert.ToInt32(reader.GetInt32(4))
                    };

                    DataGridViewCell CodeDocument = new DataGridViewTextBoxCell {
                        Value = reader.GetInt32(5)
                    };

                    DataGridViewCell SeriesDoc = new DataGridViewTextBoxCell {
                        Value = reader.GetString(6)
                    };

                    DataGridViewCell NumbDoc = new DataGridViewTextBoxCell {
                        Value = reader.GetString(7)
                    };

                    DataGridViewCell RS = new DataGridViewTextBoxCell {
                        Value = ""
                    };

                    DataGridViewCell CS = new DataGridViewTextBoxCell {
                        Value = ""
                    };

                    DataGridViewCell Uprak1 = new DataGridViewTextBoxCell {
                        Value = reader.IsDBNull(8) ? "" : reader.GetDateTime(8).ToString()
                    };

                    DataGridViewCell Uprak2 = new DataGridViewTextBoxCell {
                        Value = reader.IsDBNull(9) ? "" : reader.GetDateTime(8).ToString()
                    };

                    newRow.Cells.Add(checkoxCell);
                    newRow.Cells.Add(Surname);
                    newRow.Cells.Add(Name);
                    newRow.Cells.Add(FatherName);
                    newRow.Cells.Add(Birthday);
                    newRow.Cells.Add(Pol);
                    newRow.Cells.Add(CodeDocument);
                    newRow.Cells.Add(SeriesDoc);
                    newRow.Cells.Add(NumbDoc);
                    newRow.Cells.Add(RS);
                    newRow.Cells.Add(CS);
                    newRow.Cells.Add(Uprak1);
                    newRow.Cells.Add(Uprak2);

                    RequestTable.Rows.Add(newRow);
                }
                reader.Close();
                flag = true;
            }
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("SELECT Distinct FilesCSV.FileName " +
                                            "FROM Users join ListOperator on Users.ID = " + GetID() + " join FilesCSV on ListOperator.IDCSV = FilesCSV.ID", con)) {
                    con.Open();
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        while (reader.Read()) {
                            DataGridViewRow newRow = new DataGridViewRow();

                            DataGridViewCell Name = new DataGridViewTextBoxCell {
                                Value = reader.GetString(0)
                            };

                            newRow.Cells.Add(Name);
                            TableWithFilesCSV.Rows.Add(newRow);
                        }
                    }
                }
            }
        }


        // кнопка для добавления людей
        private void AddPeople_Click(object sender, EventArgs e) {
            if (tName.Text == "" || tSurname.Text == "" || tBirthday.Text == "" || tBirthday.Text == "" || tSeries.Text == "" || tNumber.Text == "" || (!radioButton1.Checked && !radioButton2.Checked)) {
                MessageBox.Show("Введены не все данные");
            } else OperationWithPerson();
        }

        // Методы с добавление людей
        private void OperationWithPerson() {
            /*if (isFile) {
                if (IsExistFile()) {
                    RecordFileCSV();
                    RecordDBListOperatorWithFile();
                    FillTableWithFileCSV();
                } else {
                    if (IsExistFileOperator()) {
                        RecordDBListOperatorWithFile();
                        FillTableWithFileCSV();
                    }
                }
            }*/
            try {
                if (CheckPeople()) {
                    RecordDataBasePeoples();
                    RecordDataBaseListOperationAddPeoples();

                    if (!isFile) {
                        RecordDBListOperator();
                        AddPeopleTable();
                    } else {
                        RecordDBListOperatorWithFile();
                        AddPeopleMarkTable();
                    }
                } else {
                    if (CheckOperator()) {
                        RecordDataBaseListOperationAddPeoples();
                        if (!isFile) {
                            RecordDBListOperator();
                            AddPeopleTable();
                        } else {
                            AddPeopleMarkTable();
                            RecordDBListOperatorWithFile();
                        }
                    } else return;
                }
                DelInform();
            } catch (Exception e) {
                MessageBox.Show("Произошла ошибка: " + e.Message + "\nОбратитесь к специалистам" );
            }
            
        }

        // проверка людей на наличие уже существующего человека
        private bool CheckPeople() {
            SqlConnection con = null;
            SqlCommand com;
            try {
                con = new SqlConnection(connectionString);
                con.Open();
                com = new SqlCommand("Select Count(*) From Peoples Where Name = @Name and Surname = @Surname and FatherName = @FatherName and DateBirthday = @DateBirthday", con);
                com.Parameters.AddWithValue("@Name", tName.Text);
                com.Parameters.AddWithValue("@Surname", tSurname.Text);
                com.Parameters.AddWithValue("@FatherName", tFatherName.Text);//
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(tBirthday.Text), "yyyyMdd", null));
                int count = (int)com.ExecuteScalar();

                if (count > 0)
                    return false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
            return true;
        }

        // проверка: имеется ли в списке у этого пользователя человек
        private bool CheckOperator() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select Count(*) From ListOperator Where IDUser = @IDUser and IDPeople = @IDPeople", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@IDUser", GetID());
                    com.Parameters.AddWithValue("@IDPeople", GetIDPeople());
                    int count = (int)com.ExecuteScalar();
                    if (count > 0)
                        return false;
                }
            }
            return true;
        }


        // запись в таблицу Peoples добавленных людей
        private void RecordDataBasePeoples() {
            SqlConnection con = null;
            try {
                con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand com = new SqlCommand("INSERT INTO Peoples(Surname, Name, FatherName, DateBirthday, Pol, CodeDocument, SeriesDoc, NumbDoc) VALUES (@Surname, @Name, @FatherName, @DateBirthday, @Pol, @CodeDocument, @SeriesDoc, @NumbDoc)", con);
                com.Parameters.AddWithValue("@Surname", tSurname.Text);
                com.Parameters.AddWithValue("@Name", tName.Text);
                com.Parameters.AddWithValue("@FatherName", tFatherName.Text);//
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(tBirthday.Text), "yyyyMdd", null));
                com.Parameters.AddWithValue("@Pol", radioButton1.Checked ? 1 : 2);
                com.Parameters.AddWithValue("@CodeDocument", GetCode());
                com.Parameters.AddWithValue("@SeriesDoc", tSeries.Text);
                com.Parameters.AddWithValue("@NumbDoc", tNumber.Text);
                com.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
        }

        // запись в таблицу ListOperation информации о том, что был добавлен человек
        private void RecordDataBaseListOperationAddPeoples() {
            SqlConnection con = null;
            try {
                con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand com = new SqlCommand("INSERT INTO ListOperation(IP, DateTime, ID, Operation, IDPerson) VALUES (@IP, @DateTime, @ID, @Operation, @IDPerson)", con); // что за IDPerson???
                com.Parameters.AddWithValue("@IP", System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString());
                com.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("s"));
                com.Parameters.AddWithValue("@ID", GetID());
                com.Parameters.AddWithValue("@Operation", "Добавление данных");
                com.Parameters.AddWithValue("@IDPerson", GetIDPeople());
                com.ExecuteNonQuery();
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так!"); }
            finally { con.Close(); }
        }

        // запись в таблицу ListOperator

        private void RecordDBListOperator() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("INSERT INTO ListOperator(IDUser, IDPeople) VALUES(@IDUser, @IDPeople)", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@IDUser", GetID());
                    com.Parameters.AddWithValue("@IDPeople", GetIDPeople());
                    com.ExecuteNonQuery();
                }
            }
        }

        // запись в ListOperator людей вместе с файлом, к которому они относятся
        private void RecordDBListOperatorWithFile() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("INSERT INTO ListOperator(IDUser, IDPeople, IDCSV) VALUES (@IDUser, @IDPeople, @IDCSV)", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@IDUser", GetID());
                    com.Parameters.AddWithValue("@IDPeople", GetIDPeople());
                    com.Parameters.AddWithValue("@IDCSV", GetIDFileCSV());
                    com.ExecuteNonQuery();
                }
            }
        }

        // добавление человека
        private void AddPeopleTable() {
            DataGridViewRow newRow = new DataGridViewRow();

            DataGridViewCell checkoxCell = new DataGridViewCheckBoxCell {
                Value = 0
            };
            DataGridViewCell Surname = new DataGridViewTextBoxCell {
                Value = tSurname.Text //
            };

            DataGridViewCell Name = new DataGridViewTextBoxCell {
                Value = tName.Text
            };

            DataGridViewCell FatherName = new DataGridViewTextBoxCell {
                Value = tFatherName.Text
            };

            DataGridViewCell Birthday = new DataGridViewTextBoxCell {
                Value = tBirthday.Text
            };

            DataGridViewCell Pol = new DataGridViewTextBoxCell {
                Value = radioButton1.Checked ? "1" : "2"
            };

            DataGridViewCell CodeDoc = new DataGridViewTextBoxCell {
                Value = code
            };

            DataGridViewCell SeriesDoc = new DataGridViewTextBoxCell {
                Value = tSeries.Text
            };

            DataGridViewCell NumbDoc = new DataGridViewTextBoxCell {
                Value = tNumber.Text
            };

            DataGridViewCell RS = new DataGridViewTextBoxCell {
                Value = ""
            };

            DataGridViewCell CSRequest = new DataGridViewTextBoxCell {
                Value = ""
            };

            DataGridViewCell CS1 = new DataGridViewTextBoxCell {
                Value = ""
            };

            DataGridViewCell CS2 = new DataGridViewTextBoxCell {
                Value = ""
            };
            newRow.Cells.Add(checkoxCell);
            newRow.Cells.Add(Surname);
            newRow.Cells.Add(Name);
            newRow.Cells.Add(FatherName);
            newRow.Cells.Add(Birthday);
            newRow.Cells.Add(Pol);
            newRow.Cells.Add(CodeDoc);
            newRow.Cells.Add(SeriesDoc);
            newRow.Cells.Add(NumbDoc);
            newRow.Cells.Add(RS);
            newRow.Cells.Add(CSRequest);
            newRow.Cells.Add(CS1);
            newRow.Cells.Add(CS2);

            RequestTable.Rows.Add(newRow);

            
        }

        // добавляем людей с активными чекбоксами
        private void AddPeopleMarkTable() {
            DataGridViewRow newRow = new DataGridViewRow();

            DataGridViewCell checkoxCell = new DataGridViewCheckBoxCell {
                Value = 1
            };
            DataGridViewCell Surname = new DataGridViewTextBoxCell {
                Value = tSurname.Text //
            };

            DataGridViewCell Name = new DataGridViewTextBoxCell {
                Value = tName.Text
            };

            DataGridViewCell FatherName = new DataGridViewTextBoxCell {
                Value = tFatherName.Text
            };

            DataGridViewCell Birthday = new DataGridViewTextBoxCell {
                Value = tBirthday.Text
            };

            DataGridViewCell Pol = new DataGridViewTextBoxCell {
                Value = radioButton1.Checked ? "1" : "2"
            };

            DataGridViewCell CodeDoc = new DataGridViewTextBoxCell {
                Value = code
            };

            DataGridViewCell SeriesDoc = new DataGridViewTextBoxCell {
                Value = tSeries.Text
            };

            DataGridViewCell NumbDoc = new DataGridViewTextBoxCell {
                Value = tNumber.Text
            };

            DataGridViewCell RS = new DataGridViewTextBoxCell {
                Value = ""
            };

            DataGridViewCell CSRequest = new DataGridViewTextBoxCell {
                Value = ""
            };

            DataGridViewCell CS1 = new DataGridViewTextBoxCell {
                Value = ""
            };

            DataGridViewCell CS2 = new DataGridViewTextBoxCell {
                Value = ""
            };
            newRow.Cells.Add(checkoxCell);
            newRow.Cells.Add(Surname);
            newRow.Cells.Add(Name);
            newRow.Cells.Add(FatherName);
            newRow.Cells.Add(Birthday);
            newRow.Cells.Add(Pol);
            newRow.Cells.Add(CodeDoc);
            newRow.Cells.Add(SeriesDoc);
            newRow.Cells.Add(NumbDoc);
            newRow.Cells.Add(RS);
            newRow.Cells.Add(CSRequest);
            newRow.Cells.Add(CS1);
            newRow.Cells.Add(CS2);

            RequestTable.Rows.Add(newRow);

            
        }

        private void DelInform() {
            tSurname.Clear();
            tName.Clear();
            tFatherName.Clear();
            tBirthday.Clear();
            comboBox1.Text = "Тип документа";
            tSeries.Clear();
            tNumber.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        // Кнопка отправки в ЦС
        private void CheckCS_Click(object sender, EventArgs e) {
            try {
                RecordListOperationCreateFile();
                RecordTempDB();
            } catch (Exception ex) {
                MessageBox.Show("Произошла ошибка: " + ex.Message + "\nОбратитесь к специалистам");
            }
               
        }

        // запись в третью БД
        private void RecordTempDB() {
            for (int i = 0; i < RequestTable.Rows.Count; i++) {
                if (Convert.ToBoolean(RequestTable.Rows[i].Cells[0].Value)) {
                    RequestTable.Rows[i].Cells[10].Value = "OK";
                    using (SqlConnection con = new SqlConnection(connectionString)) {
                        using (SqlCommand com = new SqlCommand("INSERT INTO Temp(Surname, Name, FatherName, DateBirthday, Pol, CodeDocument, Series, Number) VALUES" +
                            "(@Surname, @Name, @FatherName, @DateBirthday, @Pol, @CodeDocument, @Series, @Number)", con)) {
                            con.Open();
                            com.Parameters.AddWithValue("@Surname", RequestTable.Rows[i].Cells[1].Value.ToString());
                            com.Parameters.AddWithValue("@Name", RequestTable.Rows[i].Cells[2].Value.ToString());
                            com.Parameters.AddWithValue("@FatherName", RequestTable.Rows[i].Cells[3].Value.ToString());//
                            com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(RequestTable.Rows[i].Cells[4].Value.ToString()), "yyyyMdd", null));
                            com.Parameters.AddWithValue("@Pol", RequestTable.Rows[i].Cells[5].Value.ToString());
                            com.Parameters.AddWithValue("@CodeDocument", RequestTable.Rows[i].Cells[6].Value);
                            com.Parameters.AddWithValue("@Series", RequestTable.Rows[i].Cells[7].Value.ToString());
                            com.Parameters.AddWithValue("@Number", RequestTable.Rows[i].Cells[8].Value.ToString());
                            com.ExecuteNonQuery();
                        }
                    }

                }
            }
        }

        public void ToFillDropDownList() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                SqlCommand com = new SqlCommand("SELECT DocumentName FROM DocumentsType", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read()) {
                    comboBox1.Items.Add(reader.GetString(0));
                }
                reader.Close();
            }
            
        }

        
        private void AddPersonMenuItem_Click(object sender, EventArgs e) {
            this.AutoSize = false;
            this.Size = registration1.Size;
            registration1.Show();
        }
        // Моя история
        private void MyHistoryMenuItem_Click(object sender, EventArgs e) {
            panelWithHistory.Visible = true;
            HistoryTable.Rows.Clear();
            
            SqlDataAdapter adapter;
            DataSet ds;

            try {
                HistoryTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                HistoryTable.AllowUserToAddRows = false;

                using (SqlConnection con = new SqlConnection(connectionString)) {
                    con.Open();
                    adapter = new SqlDataAdapter("Select * From Peoples Where id = " + GetID(), con);
                    ds = new DataSet();
                    adapter.Fill(ds);
                    HistoryTable.DataSource = ds.Tables[0];
                }
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }
        // Вся история
        private void AllHistoryMenuItem_Click(object sender, EventArgs e) {
            panelWithHistory.Visible = true;
            HistoryTable.Rows.Clear();

            SqlDataAdapter adapter;
            DataSet ds;

            try {
                HistoryTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                HistoryTable.AllowUserToAddRows = false;

                using (SqlConnection con = new SqlConnection(connectionString)) {
                    con.Open();
                    adapter = new SqlDataAdapter("Select * From Request", con);
                    ds = new DataSet();
                    adapter.Fill(ds);
                    HistoryTable.DataSource = ds.Tables[0];
                }
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e) {
            signInProfile1.Show();
            this.AutoSize = false;
            this.Size = signInProfile1.Size;
            signInProfile1.ClearTextBox();
            using (FileStream stream = new FileStream("date.txt", FileMode.Truncate)) {
                StreamWriter write = new StreamWriter(stream);
                write.Write("");
                write.Close();
            }
        }
        // получение ID пользователя
        public object GetID() {
            SqlConnection con = null;
            SqlCommand com;
            try {
                con = new SqlConnection(connectionString);
                con.Open();
                com = new SqlCommand("Select ID From Users Where Name = @Name and Password = @Password", con);
                com.Parameters.AddWithValue("@Name", nameUser);
                com.Parameters.AddWithValue("@Password", encPas);
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                object id = reader.GetValue(0);
                reader.Close();
                return id;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
            return 0;
        }

        // получение ID человека
        private object GetIDPeople() {
            SqlConnection con = null;
            SqlCommand com;
            try {
                con = new SqlConnection(connectionString);
                con.Open();
                com = new SqlCommand("Select ID From Peoples Where Name = @Name and Surname = @Surname and FatherName = @FatherName and DateBirthday = @DateBirthday", con);
                com.Parameters.AddWithValue("@Name", tName.Text);
                com.Parameters.AddWithValue("@Surname", tSurname.Text);
                com.Parameters.AddWithValue("@FatherName", tFatherName.Text);
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(tBirthday.Text), "yyyyMdd", null));
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                object id = reader.GetValue(0);
                reader.Close();
                return id;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
            return 0;
        }

        private object GetIDFileCSV() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select ID From FilesCSV Where FileName = @FileName", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@FileName", fileName);
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        reader.Read();
                        return reader.GetValue(0);
                    }
                }
            }
        }

        private object GetIDPeople(string name, string surname, string fatherName, string birthday) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select ID From Peoples Where Surname = @Surname and Name = @Name and FatherName = @FatherName and DateBirthday = @DateBirthday", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@Surname", surname);
                    com.Parameters.AddWithValue("@Name", name);
                    com.Parameters.AddWithValue("@FatherName", fatherName);
                    com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(birthday), "yyyyMdd", null));
                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    object id = reader.GetValue(0);
                    reader.Close();
                    return id;
                }
            }
        }

        // запись в таблицу ListOperation информации о том, что был создан файл
        private void RecordListOperationCreateFile() {
            SqlConnection con = null;

            try {
                con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand com = new SqlCommand("INSERT INTO ListOperation(IP, DateTime, ID, Operation) VALUES (@IP, @DateTime, @ID, @Operation)", con); // что за IDPerson???
                com.Parameters.AddWithValue("@IP", System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString());
                com.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("s"));
                com.Parameters.AddWithValue("@ID", GetID());
                com.Parameters.AddWithValue("@Operation", "Поиск в ЦС");
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так!"); }
            finally { con.Close(); }
        }

        
        int k;
        int code;
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            string selectedState = comboBox1.SelectedItem.ToString();
            if (selectedState.Equals("Свидетельство о рождении")) {
                
                tSeries.Text = GetSeriesSB();
                tNumber.Text = GetNumberSB();
                code = 3;
            } else {
                tSeries.Mask = "99 99";
                tSeries.Text = GetSeriesP();
                tNumber.Text = GetNumberP();
                code = 14;
            }
        }
      
        #region Свидетельство о рождении
        private string GetNumberSB() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                con.Open();
                SqlCommand com = new SqlCommand("Select Number From DocumentsType Where DocumentName = @DocumentName", con);
                com.Parameters.AddWithValue("@DocumentName", comboBox1.SelectedItem.ToString());
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                string number = reader.GetString(0);
                reader.Close();
                return number;
            }
        }

        private string GetSeriesSB() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                con.Open();
                SqlCommand com = new SqlCommand("Select Series From DocumentsType Where DocumentName = @DocumentName", con);
                com.Parameters.AddWithValue("@DocumentName", comboBox1.SelectedItem.ToString());
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                string series = reader.GetString(0);
                reader.Close();
                return series;
            }
        }
        #endregion
        #region Паспорт
        private string GetNumberP() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                con.Open();
                SqlCommand com = new SqlCommand("Select Number From DocumentsType Where DocumentName = @DocumentName", con);
                com.Parameters.AddWithValue("@DocumentName", comboBox1.SelectedItem.ToString());
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                string number = reader.GetString(0);
                reader.Close();
                return number;
            }
        }

        private string GetSeriesP() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                con.Open();
                SqlCommand com = new SqlCommand("Select Series From DocumentsType Where DocumentName = @DocumentName", con);
                com.Parameters.AddWithValue("@DocumentName", comboBox1.SelectedItem.ToString());
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                string series = reader.GetString(0);
                reader.Close();
                return series;
            }
        }
        #endregion

        // получение кода документа
        private int GetCode() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                con.Open();
                SqlCommand com = new SqlCommand("Select Code From DocumentsType Where DocumentName = @DocumentName", con);
                com.Parameters.AddWithValue("@DocumentName", comboBox1.SelectedItem.ToString());
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                int code = Convert.ToInt32(reader.GetValue(0));
                reader.Close();
                return code;
            }
        }
        
        private void DownLoadFileCSV_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            //fileName = openFileDialog1.SafeFileName.Substring(0, openFileDialog1.SafeFileName.Length - 4);
            isFile = true;


            fileName = openFileDialog1.FileName;
            string[] person = null;     
            string[] peoples = File.ReadAllLines(fileName);

            if (IsExistFile()) {
                RecordFileCSV();
                FillTableWithFileCSV();
            } else if (IsExistFileOperator()) {
                FillTableWithFileCSV();
            } else return;
            for (int i = 1; i < peoples.Length; i++) {
                person = peoples[i].Split(';');
                tSurname.Text = person[0];
                tName.Text = person[1];
                tFatherName.Text = person[2];
                tBirthday.Text = person[3];
                if (Convert.ToInt32(person[4]) == 1) {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                } else {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
                comboBox1.SelectedIndex = Convert.ToInt32(person[5]) == 14 ? 0 : 1;
                tSeries.Text = person[6];
                tNumber.Text = person[7];
                isFile = true;
                OperationWithPerson();
            }

            isFile = false;
            DialogResult result = MessageBox.Show(
                "Обработать запрос в ЦС?",
                "Сообщение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );
            if (result == DialogResult.Yes) {
                RecordTempDB();
                RecordListOperationCreateFile();
            } else {
                for (int i = 0; i < RequestTable.Rows.Count; i++)
                    RequestTable.Rows[i].Cells[0].Value = 0;
            }
        } 

        private bool IsExistFile() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select Count(*) From FilesCSV Where FileName = @FileName", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@FileName", fileName);
                    
                    int count = (int)com.ExecuteScalar();

                    if (count > 0)
                        return false;

                    return true;
                }
            }

        }

        private bool IsExistFileOperator() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select Count(*) From ListOperator Where IDUser = @IDUser and IDCSV = @IDCSV", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@IDUser", GetID());
                    com.Parameters.AddWithValue("@IDCSV", GetIDFileCSV());
                    int count = (int)com.ExecuteScalar();
                    if (count > 0)
                        return false;
                    return true;
                }
            }
        }







        // запись в таблицу FileCSV файла .csv
        private void RecordFileCSV() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("INSERT INTO FilesCSV(FileName) VALUES (@FileName)", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@FileName", fileName);
                    com.ExecuteNonQuery();
                }
            }
        }

        private void FillTableWithFileCSV() {
            DataGridViewRow newRow = new DataGridViewRow();

            DataGridViewCell Name = new DataGridViewTextBoxCell {
                Value = fileName
            };

            newRow.Cells.Add(Name);
            TableWithFilesCSV.Rows.Add(newRow);
        }

        // удаление человека из таблицы и БД
        private void DeletePeople_Click(object sender, EventArgs e) {
            for (int i = 0; i < RequestTable.Rows.Count; i++) {
                if (Convert.ToBoolean(RequestTable.Rows[i].Cells[0].Value)) {
                    using (SqlConnection con = new SqlConnection(connectionString)) {
                        using (SqlCommand com = new SqlCommand("Delete From ListOperator Where IDUser = @IDUser and IDPeople = @IDPeople", con)) {
                            con.Open();
                            com.Parameters.AddWithValue("@IDUser", GetID());
                            string surname = RequestTable.Rows[i].Cells[1].Value.ToString();
                            string name = RequestTable.Rows[i].Cells[2].Value.ToString();
                            string fatherName = RequestTable.Rows[i].Cells[3].Value.ToString();
                            string datebirthday = RequestTable.Rows[i].Cells[4].Value.ToString();
                            com.Parameters.AddWithValue("@IDPeople", GetIDPeople(name, surname, fatherName, datebirthday));
                            com.ExecuteNonQuery();
                        }
                    }
                    RequestTable.Rows.Remove(RequestTable.Rows[i]);
                    i--;
                }
            }   
        }

        private void TSurname_KeyPress(object sender, KeyPressEventArgs e) {
            if ((!Char.IsDigit(e.KeyChar)) && 
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я' || e.KeyChar >= 'а' 
                    && e.KeyChar <= 'я' || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '-') || (e.KeyChar == (char)Keys.Space)) 
                ) return;
            e.Handled = true;
            return;
        }

        private void TName_KeyPress(object sender, KeyPressEventArgs e) {
            if ((!Char.IsDigit(e.KeyChar)) &&
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я' || e.KeyChar >= 'а'
                    && e.KeyChar <= 'я' || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '-') || (e.KeyChar == (char)Keys.Space))
                ) return;
            e.Handled = true;
            return;
        }

        private void TFatherName_KeyPress(object sender, KeyPressEventArgs e) {
            if ((!Char.IsDigit(e.KeyChar)) &&
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я' || e.KeyChar >= 'а'
                    && e.KeyChar <= 'я' || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '-') || (e.KeyChar == (char)Keys.Space))
                ) return;
            e.Handled = true;
            return;
        }

        private void TSurname_Click(object sender, EventArgs e) {
            //InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("ru-RU"));
        }

        private void TName_Click(object sender, EventArgs e) {
            //InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("ru-RU"));
        }

        private void TFatherName_Click(object sender, EventArgs e) {
            //InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("ru-RU"));
        }

        private void RequestTable_CurrentCellDirtyStateChanged(object sender, EventArgs e) {
            if (Convert.ToInt16(RequestTable.SelectedCells[0].ColumnIndex) != 0)
                return;
            k = 0;
            RequestTable.EndEdit();
            if (k == 1)
                return;
            else {
                k++;
                bool flag = false;
                for (int i = 0; i < RequestTable.Rows.Count; i++) {
                    if (Convert.ToBoolean(RequestTable.Rows[i].Cells[0].Value) == true) {
                        flag = true;
                        break;
                    }
                }

                if (flag) {
                    CheckCS.Enabled = true;
                    DeletePeople.Enabled = true;
                    CreateFileAnwer.Enabled = true;
                    ToRepayPolis.Enabled = true;
                } else {
                    CheckCS.Enabled = false;
                    DeletePeople.Enabled = false;
                    CreateFileAnwer.Enabled = false;
                    ToRepayPolis.Enabled = false;
                }
                return;
            }
        }

        private void RequestTable_SelectionChanged(object sender, EventArgs e) {
            if (!flag) return;
            if (Convert.ToInt16(RequestTable.CurrentCell.ColumnIndex) == 0)
                return;
            //RequestTable.EndEdit();
            TableWithInformation.Rows.Clear();
            int index = RequestTable.CurrentRow.Index;
            SearchInformCS(index);
            EditButton.Enabled = true;
        }

        // поиск информации в ЦС
        private void SearchInformCS(int index) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select ID From Peoples " +
                            "Where Surname = @Surname and Name = @Name and FatherName = @FatherName and DateBirthday = @DateBirthday", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@Surname", RequestTable.Rows[index].Cells[1].Value.ToString());
                    com.Parameters.AddWithValue("@Name", RequestTable.Rows[index].Cells[2].Value.ToString());
                    com.Parameters.AddWithValue("@FatherName", RequestTable.Rows[index].Cells[3].Value.ToString());//
                    com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(RequestTable.Rows[index].Cells[4].Value.ToString()), "yyyyMdd", null));
                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows) {
                        GetInformationCS(Convert.ToInt32(reader.GetInt32(0)));
                    }
                    reader.Close();
                }
            }
        }
        // выдача информации о выделенном человеке
        private void GetInformationCS(int id) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select * From Results Where PID = @ID", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@ID", id);
                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows) {
                        AddInformation(reader);
                    }
                    reader.Close();
                }
            }
        }

        private void AddInformation(SqlDataReader reader) {
            TableWithInformation.Rows.Add();
            TableWithInformation.Rows[0].Cells[0].Value = reader.IsDBNull(0) ? "NULL" : Convert.ToInt32(reader.GetInt32(0)).ToString();
            TableWithInformation.Rows[0].Cells[1].Value = reader.IsDBNull(1) ? "NULL" : Convert.ToInt32(reader.GetInt32(1)).ToString();
            TableWithInformation.Rows[0].Cells[2].Value = reader.IsDBNull(2) ? "NULL" : reader.GetString(2);
            TableWithInformation.Rows[0].Cells[3].Value = reader.IsDBNull(3) ? "NULL" : reader.GetDateTime(3).ToShortDateString();
            TableWithInformation.Rows[0].Cells[4].Value = reader.IsDBNull(4) ? "NULL" : reader.GetDateTime(4).ToShortDateString();
            TableWithInformation.Rows[0].Cells[5].Value = reader.IsDBNull(5) ? "NULL" : reader.GetString(5);
            TableWithInformation.Rows[0].Cells[6].Value = reader.IsDBNull(6) ? "NULL" : reader.GetString(6);
            TableWithInformation.Rows[0].Cells[7].Value = reader.IsDBNull(7) ? "NULL" : reader.GetString(7);
            TableWithInformation.Rows[0].Cells[8].Value = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
            TableWithInformation.Rows[0].Cells[9].Value = reader.IsDBNull(9) ? "NULL" : reader.GetString(9);
            TableWithInformation.Rows[0].Cells[10].Value = reader.IsDBNull(10) ? "NULL" : reader.GetString(10);
            TableWithInformation.Rows[0].Cells[11].Value = reader.IsDBNull(11) ? "NULL" : Convert.ToInt32(reader.GetInt32(11)).ToString();
            TableWithInformation.Rows[0].Cells[12].Value = reader.IsDBNull(12) ? "NULL" : reader.GetString(12);
            TableWithInformation.Rows[0].Cells[13].Value = reader.IsDBNull(13) ? "NULL" : Convert.ToInt32(reader.GetInt32(13)).ToString();
            TableWithInformation.Rows[0].Cells[14].Value = reader.IsDBNull(14) ? "NULL" : reader.GetDateTime(14).ToShortDateString();
            TableWithInformation.Rows[0].Cells[15].Value = reader.IsDBNull(15) ? "NULL" : Convert.ToInt32(reader.GetInt32(15)).ToString();
            TableWithInformation.Rows[0].Cells[16].Value = reader.IsDBNull(16) ? "NULL" : reader.GetString(16);
        }

        private void TakeOff_Click(object sender, EventArgs e) {
            for (int i = 0; i < RequestTable.Rows.Count; i++)
                RequestTable.Rows[i].Cells[0].Value = 0;
            CheckCS.Enabled = false;
            DeletePeople.Enabled = false;
            CreateFileAnwer.Enabled = false;
        }

        private void Highlight_Click(object sender, EventArgs e) {
            for (int i = 0; i < RequestTable.Rows.Count; i++)
                RequestTable.Rows[i].Cells[0].Value = 1;
            CreateFileAnwer.Enabled = true;
            CheckCS.Enabled = true;
            DeletePeople.Enabled = true;
        }

        private void RefreshButton_Click(object sender, EventArgs e) {
            flag = false;
            CheckCS.Enabled = false;
            DeletePeople.Enabled = false;
            CreateFileAnwer.Enabled = false;
            RequestTable.Rows.Clear();
            TableWithFilesCSV.Rows.Clear();
            ToFillTable();
        }
        /*изменить работу
         */
        private void RefreshInfo_Click(object sender, EventArgs e) {
            try {
                int index = Convert.ToInt32(TableWithInformation.Rows[0].Cells[1].Value);
                TableWithInformation.Rows.Clear();
                GetInformationCS(index);
            } catch (ArgumentOutOfRangeException) {
                return;
            }
            
        }

        private void CreateFileAnswer_Click(object sender, EventArgs e) {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string path = folderBrowserDialog1.SelectedPath;
            List<string> persons = new List<string>();
            for (int i = 0; i < RequestTable.Rows.Count; i++) {
                if (Convert.ToBoolean(RequestTable.Rows[i].Cells[0].Value)) {
                    persons.Add(RequestTable.Rows[i].Cells[1].Value + "," + RequestTable.Rows[i].Cells[2].Value + "," + RequestTable.Rows[i].Cells[3].Value + "," + RequestTable.Rows[i].Cells[4].Value);
                }
            }
            CreateAnswer createAnswer = new CreateAnswer(persons, path);
            createAnswer.SearchPeoples();
        }

        private void TableWithFilesCSV_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == 1) {
                CheckCS.Enabled = true;
                DeletePeople.Enabled = true;
                CreateFileAnwer.Enabled = true;
                fileName = TableWithFilesCSV.Rows[e.RowIndex].Cells[0].Value.ToString();
                SearchPeoples();
            } else if (e.ColumnIndex == 2) { 
                using (SqlConnection con = new SqlConnection(connectionString)) {
                    using (SqlCommand com = new SqlCommand("Delete From ListOperator Where IDUser = @IDUser and IDCSV = @IDCSV", con)) {
                        con.Open();
                        com.Parameters.AddWithValue("@IDUser", GetID());
                        fileName = TableWithFilesCSV.Rows[e.RowIndex].Cells[0].Value.ToString();
                        com.Parameters.AddWithValue("@IDCSV", GetIDFileCSV());
                        com.ExecuteNonQuery();
                    }
                    TableWithFilesCSV.Rows.Remove(TableWithFilesCSV.Rows[e.RowIndex]);
                    flag = false;
                    RequestTable.Rows.Clear();
                    ToFillTable();
                }
            }
        }

        private void SearchPeoples() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select IDPeople From ListOperator Where IDCSV = @IDCSV", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@IDCSV", GetIDFileCSV());
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        while (reader.Read()) {
                            GetInformPeople(Convert.ToInt32(reader.GetInt32(0)));
                        }
                    }
                }
            }
        }

        private void GetInformPeople(int id) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select Surname, Name, FatherName, DateBirthday From Peoples Where ID = @ID", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        reader.Read();
                        MarkPeoples(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3).ToShortDateString());
                    }
                }
            }
        }

        private void MarkPeoples(string surname, string name, string fatherName, string dr) {
            for (int i = 0; i < RequestTable.Rows.Count; i++) {
                if (RequestTable.Rows[i].Cells[1].Value.ToString() == surname && RequestTable.Rows[i].Cells[2].Value.ToString() == name && RequestTable.Rows[i].Cells[3].Value.ToString() == fatherName && RequestTable.Rows[i].Cells[4].Value.ToString() == dr) {
                    RequestTable.Rows[i].Cells[0].Value = 1;
                }
            }
        }
        bool flagEdit = true;
        string nameEd, surnameEd, fatherNameEd, drEd;

        private void ToRepayPolis_Click(object sender, EventArgs e) {
            List<string> persons = new List<string>();
            for (int i = 0; i < RequestTable.Rows.Count; i++) {
                if (Convert.ToBoolean(RequestTable.Rows[i].Cells[0].Value)) {
                    persons.Add(RequestTable.Rows[i].Cells[1].Value + "," + RequestTable.Rows[i].Cells[2].Value + "," + RequestTable.Rows[i].Cells[3].Value + "," + RequestTable.Rows[i].Cells[4].Value);
                }
            }

            RepayPolises repay = new RepayPolises(persons);
            repay.ToRepay();
        }

        private void RequestTable_Leave(object sender, EventArgs e) {
            RequestTable.ClearSelection();
            RequestTable.EndEdit();
            EditButton.Enabled = false;
            //CreateFileAnwer.Enabled = false;
        }

        int indexRow;
        private void EditButton_Click(object sender, EventArgs e) {
            try {
                if (flagEdit) {
                    indexRow = RequestTable.CurrentRow.Index;
                    surnameEd = RequestTable.Rows[indexRow].Cells[1].Value.ToString();
                    nameEd = RequestTable.Rows[indexRow].Cells[2].Value.ToString();
                    fatherNameEd = RequestTable.Rows[indexRow].Cells[3].Value.ToString();
                    drEd = RequestTable.Rows[indexRow].Cells[4].Value.ToString();
                    for (int i = 0; i < RequestTable.CurrentRow.Cells.Count; i++) {
                        RequestTable.Rows[indexRow].Cells[i].ReadOnly = false;
                    }
                    flagEdit = false;
                    flag = false;
                    EditButton.Text = "Сохранить";
                } else {
                    using (SqlConnection con = new SqlConnection(connectionString)) {
                        using (SqlCommand com = new SqlCommand("Update Peoples " +
                                            "Set Surname = @Surname, Name = @Name, FatherName = @FatherName, DateBirthday = @DR, " +
                                            "Pol = @Pol, CodeDocument = @Code, SeriesDoc = @Series, NumbDoc = @NumbDoc " +
                                            "Where Surname = @SurnameEd and Name = @NameEd and FatherName = @FatherEd and DateBirthday = @DrED", con)) {
                            con.Open();
                            com.Parameters.AddWithValue("@Surname", RequestTable.Rows[indexRow].Cells[1].Value.ToString());
                            com.Parameters.AddWithValue("@Name", RequestTable.Rows[indexRow].Cells[2].Value.ToString());
                            com.Parameters.AddWithValue("@FatherName", RequestTable.Rows[indexRow].Cells[3].Value.ToString());
                            com.Parameters.AddWithValue("@DR", DateTime.ParseExact(GetBirthday(RequestTable.Rows[indexRow].Cells[4].Value.ToString()), "yyyyMdd", null));
                            com.Parameters.AddWithValue("@Pol", Convert.ToInt32(RequestTable.Rows[indexRow].Cells[5].Value.ToString()));
                            com.Parameters.AddWithValue("@Code", Convert.ToInt32(RequestTable.Rows[indexRow].Cells[6].Value.ToString()));
                            com.Parameters.AddWithValue("@Series", RequestTable.Rows[indexRow].Cells[7].Value.ToString());
                            com.Parameters.AddWithValue("@NumbDoc", RequestTable.Rows[indexRow].Cells[8].Value.ToString());
                            com.Parameters.AddWithValue("@SurnameEd", surnameEd);
                            com.Parameters.AddWithValue("@NameEd", nameEd);
                            com.Parameters.AddWithValue("@FatherEd", fatherNameEd);
                            com.Parameters.AddWithValue("@DrED", DateTime.ParseExact(GetBirthday(drEd), "yyyyMdd", null));
                            com.ExecuteNonQuery();
                        }
                    }
                    for (int i = 0; i < RequestTable.CurrentRow.Cells.Count; i++) {
                        RequestTable.Rows[indexRow].Cells[i].ReadOnly = true;
                    }
                    flagEdit = true;
                    EditButton.Enabled = false;
                    EditButton.Text = "Изменить";
                    flag = true;
                }
            } catch (Exception ex) {
                MessageBox.Show("Произошла ошибка: " + ex.Message + "\nОбратитесь к специалистам");
            }
            
            
        }

        // получение отредактированного дня рождения
        private string GetBirthday(string birthday) {
            string year = birthday.Split('.')[2];
            string month = birthday.Split('.')[1];
            string day = birthday.Split('.')[0];
            string date = year  + month + day;
            return date;
        }

        public void IsAdmin() {
            if (rights != "admin")
                addPersonMenuItem.Enabled = false;
        }
    }
}

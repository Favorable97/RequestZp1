using System;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Security.Cryptography;
using CsvHelper;
using System.Collections;
using System.Xml.Linq;
using System.Threading;

namespace RequestZp1 {
    public partial class Form1 : Form {
        public string nameUser, rights, encPas, surnames = "";

        public Form1() {
            InitializeComponent();
            signInProfile1.Location = this.Location;
            registration1.Location = this.Location;
            menuStrip1.Visible = false;
            this.Size = signInProfile1.Size;
            //panelWithHistory.Location = new System.Drawing.Point(0, 31);
            registration1.Hide();
        }
        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=ident;Persist Security Info=True;User ID=user;Password=гыук";

        public void VisibleProfile() {
            menuStrip1.Visible = true;
            namePerson2.Text = nameUser;
            menuStrip1.Visible = true;
        }

        // кнопка для добавления людей
        private void AddPeople_Click(object sender, EventArgs e) {
            DataGridViewRow newRow = new DataGridViewRow();

            DataGridViewCell checkoxCell = new DataGridViewCheckBoxCell {
                Value = 0
            };
            DataGridViewCell FIO = new DataGridViewTextBoxCell {
                Value = tSurname.Text + " " + tName.Text + " " + tFatherName.Text
            };

            DataGridViewCell Birthday = new DataGridViewTextBoxCell {
                Value = tBirthday.Text
            };

            DataGridViewCell Status = new DataGridViewTextBoxCell {
                Value = "Active"
            };
            newRow.Cells.Add(checkoxCell);
            newRow.Cells.Add(FIO);
            newRow.Cells.Add(Birthday);
            newRow.Cells.Add(Status);

            RequestTable.Rows.Add(newRow);
            surnames += tSurname;

            OperationWithPerson();

            tSurname.Clear();
            tName.Clear();
            tFatherName.Clear();
            tBirthday.Clear();
            comboBox1.SelectedItem = -1;
            tSeries.Clear();
            tNumber.Clear();
        }
        // Кнопка отправки в ЦС
        private void CheckCS_Click(object sender, EventArgs e) {
            RecordListOperationCreateFile();
            RecordTempDB();
        }

        // заполнение таблицы
        public void ToFillTable() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                SqlCommand com = new SqlCommand("SELECT Surname, Name, FatherName, DateBirthday, CodeDocument, SeriesDoc, NumbDoc FROM Peoples WHERE Status = @Status", con);
                con.Open();
                com.Parameters.AddWithValue("@Status", "Active");
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read()) {
                    DataGridViewRow newRow = new DataGridViewRow();

                    DataGridViewCell checkoxCell = new DataGridViewCheckBoxCell {
                        Value = 0
                    };
                    DataGridViewCell FIO = new DataGridViewTextBoxCell {
                        Value = reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2)
                    };

                    DataGridViewCell Birthday = new DataGridViewTextBoxCell {
                        Value = reader.GetDateTime(3)
                    };

                    DataGridViewCell CodeDocument = new DataGridViewTextBoxCell {
                        Value = reader.GetInt32(4)
                    };

                    DataGridViewCell SeriesDoc = new DataGridViewTextBoxCell {
                        Value = reader.GetString(5)
                    };

                    DataGridViewCell NumbDoc = new DataGridViewTextBoxCell {
                        Value = reader.GetString(6)
                    };

                    newRow.Cells.Add(checkoxCell);
                    newRow.Cells.Add(FIO);
                    newRow.Cells.Add(Birthday);
                    newRow.Cells.Add(CodeDocument);
                    newRow.Cells.Add(SeriesDoc);
                    newRow.Cells.Add(NumbDoc);

                    RequestTable.Rows.Add(newRow);
                }
                reader.Close();
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

        // Методы с добавление людей
        private void OperationWithPerson() {
            if (CheckPeople()) {
                RecordDataBasePeoples();
                RecordDataBaseListOperationAddPeoples();
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
                com.Parameters.AddWithValue("@FatherName", tFatherName.Text);
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(tBirthday.Text), "yyyyMdd", null));
                int count = (int)com.ExecuteScalar();

                if (count > 0) {
                    MessageBox.Show("Данный человек уже имеется в БД!");
                    tName.Clear();
                    tSurname.Clear();
                    tFatherName.Clear();
                    tBirthday.Clear();
                    return false;
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
            return true;
        }

        private void AddPersonMenuItem_Click(object sender, EventArgs e) {
            signInProfile1.Hide();
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

        // запись в таблицу Peoples добавленных людей
        private void RecordDataBasePeoples() {
            SqlConnection con = null;
            try {
                con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand com = new SqlCommand("INSERT INTO Peoples(Surname, Name, FatherName, DateBirthday, Status, IDUser, CodeDocument, SeriesDoc, NumbDoc) VALUES (@Surname, @Name, @FatherName, @DateBirthday, @Status, @IDUser, @CodeDocument, @SeriesDoc, @NumbDoc)", con);
                com.Parameters.AddWithValue("@Surname", tSurname.Text);
                com.Parameters.AddWithValue("@Name", tName.Text);
                com.Parameters.AddWithValue("@FatherName", tFatherName.Text);
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(tBirthday.Text), "yyyyMdd", null));
                com.Parameters.AddWithValue("@Status", "Active");
                com.Parameters.AddWithValue("@IDUser", GetID ());
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

        // запись в третью БД
        private void RecordTempDB() {
            for (int i = 0; i < RequestTable.Rows.Count; i++) {
                if (Convert.ToBoolean(RequestTable.Rows[i].Cells[0].Value)) {
                    using (SqlConnection con = new SqlConnection(connectionString)) {
                        using (SqlCommand com = new SqlCommand("INSERT INTO Temp(Surname, Name, FatherName, DateBirthday, CodeDocument, Series, Number) VALUES" +
                            "(@Surname, @Name, @FatherName, @DateBirhday, @CodeDocument, @Series, @Number)", con)) {
                            con.Open();
                            com.Parameters.AddWithValue("@Surname", RequestTable.Rows[i].Cells[1].Value.ToString().Split(' ')[0]);
                            com.Parameters.AddWithValue("@Name", RequestTable.Rows[i].Cells[1].Value.ToString().Split(' ')[1]);
                            com.Parameters.AddWithValue("@FatherName", RequestTable.Rows[i].Cells[1].Value.ToString().Split(' ')[2]);
                            com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(RequestTable.Rows[i].Cells[2].Value.ToString()), "yyyyMdd", null));
                            com.Parameters.AddWithValue("@CodeDocument", RequestTable.Rows[i].Cells[3].Value);
                            com.Parameters.AddWithValue("@Series", RequestTable.Rows[i].Cells[4].Value.ToString());
                            com.Parameters.AddWithValue("@Number", RequestTable.Rows[i].Cells[5].Value.ToString());
                        }
                    }
                    
                }
            }
        }
        int k;
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            string selectedState = comboBox1.SelectedItem.ToString();
            if (selectedState.Equals("Свидетельство о рождении")) {
                tSeries.Text = GetSeriesSB();
                tNumber.Text = GetNumberSB();
            } else {
                tSeries.Text = GetSeriesP();
                tNumber.Text = GetNumberP();
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
            string path = "csvFile.csv";
            
            using (StreamReader read = new StreamReader(path)) {
                using (CsvReader csvReader = new CsvReader(read)) {
                    csvReader.Configuration.Delimiter = ",";
                    IEnumerable peoples = csvReader.GetRecords<Peoples>();
                    MessageBox.Show(peoples.GetEnumerator().Current.ToString());
                }
            }
            
        }

        // удаление человека из таблицы и изменение его статуса в БД
        private void UpdateStatus_Click(object sender, EventArgs e) {
            int indexRow = RequestTable.CurrentRow.Index;
            // Изменение статуса в таблице БД
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("UPDATE Peoples SET Status = @Status WHERE Surname = @Surname and Name = @Name and FatherName = @FatherName and DateBirthday = @DateBirthday", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@Status", "Archive");
                    com.Parameters.AddWithValue("@Surname", RequestTable.Rows[indexRow].Cells[1].Value.ToString().Split(' ')[0]);
                    com.Parameters.AddWithValue("@Name", RequestTable.Rows[indexRow].Cells[1].Value.ToString().Split(' ')[1]);
                    com.Parameters.AddWithValue("@FatherName", RequestTable.Rows[indexRow].Cells[1].Value.ToString().Split(' ')[2]);
                    com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(RequestTable.Rows[indexRow].Cells[2].Value.ToString()), "yyyyMdd", null));
                    com.ExecuteNonQuery();
                }
            }
            // удаление из DataGridView
            RequestTable.Rows.Remove(RequestTable.Rows[indexRow]);
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
                    UpdateStatus.Enabled = true;
                } else {
                    CheckCS.Enabled = false;
                    UpdateStatus.Enabled = false;
                }
                return;
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

    public class Peoples {
        public string FIO { get; set; }
        public string DateBirthday { get; set; }
    }
}

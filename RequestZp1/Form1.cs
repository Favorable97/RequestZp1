using System;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Security.Cryptography;

namespace RequestZp1 {
    public partial class Form1 : Form {
        public string nameUser, rights, encPas, surnames = "";
        public Form1() {
            InitializeComponent();
            signInProfile1.Location = this.Location;
            registration1.Location = this.Location;
            
            panelWithHistory.Location = new System.Drawing.Point(0, 31);
            registration1.Hide();
        }
        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";
        public void VisibleProfile() {
            namePerson2.Text = nameUser;
        }

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
            newRow.Cells.Add(checkoxCell);
            newRow.Cells.Add(FIO);
            newRow.Cells.Add(Birthday);

            RequestTable.Rows.Add(newRow);
            surnames += tSurname;

            OperationWithPerson();

            tSurname.Clear();
            tName.Clear();
            tFatherName.Clear();
            tBirthday.Clear();
        }
        UprmesClass uprmesFile;
        private void CreateXmlFile_Click(object sender, EventArgs e) {
            if (RequestTable.Rows.Count > 0) {
                uprmesFile = new UprmesClass(RequestTable);
                uprmesFile.CreateXmlFile();
                RecordListOperationCreateFile();
            } else MessageBox.Show("Таблица пуста");
                
        }

        public void ToFillTable() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                SqlCommand com = new SqlCommand("SELECT Surname, Name, FatherName, DateBirthday, Status FROM Peoples WHERE Status = @Status", con);
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

                    DataGridViewCell Status = new DataGridViewTextBoxCell {
                        Value = reader.GetString(4)
                    };

                    newRow.Cells.Add(checkoxCell);
                    newRow.Cells.Add(FIO);
                    newRow.Cells.Add(Birthday);
                    newRow.Cells.Add(Status);

                    RequestTable.Rows.Add(newRow);
                }
            }
        }

        // Request
        private void OperationWithPerson() {
            RecordDataBasePeoples();
            RecordDataBaseListOperationAddPeoples();
        }

        private void AddPersonMenuItem_Click(object sender, EventArgs e) {
            signInProfile1.Hide();
            registration1.Show();
        }

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
            signInProfile1.ClearTextBox();
            using (FileStream stream = new FileStream("date.txt", FileMode.Truncate)) {
                StreamWriter write = new StreamWriter(stream);
                write.Write("");
                write.Close();
            }
        }

        private void RecordDataBasePeoples() {
            SqlConnection con = null;
            
            try {
                con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand com = new SqlCommand("INSERT INTO Peoples(Surname, Name, FatherName, DateBirthday, Status) VALUES (@Surname, @Name, @FatherName, @DateBirthday, @Status)", con);
                com.Parameters.AddWithValue("@Surname", tSurname.Text);
                com.Parameters.AddWithValue("@Name", tName.Text);
                com.Parameters.AddWithValue("@FatherName", tFatherName.Text);
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(tBirthday.Text), "yyyyMdd", null));
                com.Parameters.AddWithValue("@Status", "Active");
                com.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
        }

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

        private void RecordListOperationCreateFile() {
            SqlConnection con = null;

            try {
                con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand com = new SqlCommand("INSERT INTO ListOperation(IP, DateTime, ID, Operation) VALUES (@IP, @DateTime, @ID, @Operation)", con); // что за IDPerson???
                com.Parameters.AddWithValue("@IP", System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString());
                com.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("s"));
                com.Parameters.AddWithValue("@ID", GetID());
                com.Parameters.AddWithValue("@Operation", "Создание документа");
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так!"); }
            finally { con.Close(); }
        }

        int k;
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

                if (flag)
                    CreateXmlFile.Enabled = true;
                else
                    CreateXmlFile.Enabled = false;
                return;
            }
        }

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

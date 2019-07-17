using System;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Windows.Forms;

namespace RequestZp1 {
    public partial class Form1 : Form {
        public string nameUser, rights, encPas, surnames = "";
        public Form1() {
            InitializeComponent();
            registration1.Location = this.Location;
            signInProfile1.Location = this.Location;
            panelWithHistory.Location = new System.Drawing.Point(0, 31);
            registration1.Hide();
        }
        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";
        public void VisibleProfile() {
            namePerson2.Text = nameUser;
        }

        private void AddPeople_Click(object sender, EventArgs e) {
            DataGridViewRow newRow = new DataGridViewRow();

            DataGridViewCell FIO = new DataGridViewTextBoxCell {
                Value = tSurname.Text + " " + tName.Text + " " + tFatherName.Text
            };

            DataGridViewCell Birthday = new DataGridViewTextBoxCell {
                Value = tBirthday.Text
            };

            newRow.Cells.Add(FIO);
            newRow.Cells.Add(Birthday);

            RequestTable.Rows.Add(newRow);
            surnames += tSurname;
            tSurname.Clear();
            tName.Clear();
            tFatherName.Clear();
            tBirthday.Clear();
        }

        private void CreateXmlFile_Click(object sender, EventArgs e) {
            UprmesClass uprmesFile = new UprmesClass(RequestTable);
            uprmesFile.CreateXmlFile();
        }

        // Request
        private void OperationWithPerson() {
            object id = GetID();
            RecordDataBase(id);
        }

        private object GetID() {
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
                object id = reader.GetInt32(0);
                reader.Close();
                return id;
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так!"); }
            finally { con.Close(); }
            return 0;
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
                    adapter = new SqlDataAdapter("Select * From Request Where id = " + GetID(), con);
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

        private void RecordDataBase(object id) {
            SqlConnection con = null;
            SqlCommand com;
            try {
                con = new SqlConnection(connectionString);
                con.Open();
                com = new SqlCommand("INSERT INTO Request(IDUser, SurnamePersons, DateTimeCreateRequest, DateTimeGetAnswew) VALUES (@IDUser, @SurnamePersons, @DateTimeCreateRequest, @DateTimeGetAnswew", con);
                com.Parameters.AddWithValue("@IDUser", id);
                com.Parameters.AddWithValue("@SurnamePersons", surnames);
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так!"); }
            finally { con.Close(); }
        }

        public void IsAdmin() {
            if (rights != "admin")
                addPersonMenuItem.Enabled = false;
        }

        
    }
}

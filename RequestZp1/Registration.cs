using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;

namespace RequestZp1 {
    public partial class Registration : UserControl {
        public Registration() {
            InitializeComponent();
        }
        string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";
        private void SignUp_Click(object sender, EventArgs e) {
            AddUsers();
            RecordListOperationAddUser();

            this.Hide();
        }

        private void AddUsers() {
            SqlConnection con = null;

            try {
                string encPassword = GetEncodingPassword(Password.Text);

                con = new SqlConnection(connectionString);
                SqlCommand com = new SqlCommand("INSERT INTO Users(Surname, Name, FatherName, Department, Rights, Password) VALUES (@Surname, @Name, @FatherName, @Department, @Rights, @Password)", con);
                con.Open();
                com.Parameters.AddWithValue("@Surname", Surname.Text);
                com.Parameters.AddWithValue("@Name", NameR.Text);
                com.Parameters.AddWithValue("@FatherName", FatherName.Text);
                com.Parameters.AddWithValue("@Department", Department.Text);
                com.Parameters.AddWithValue("@Rights", Rights.Text);
                com.Parameters.AddWithValue("@Password", encPassword);
                com.ExecuteNonQuery();
                RecordListOperationAddUser();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
        }

        private void ToWriteFile() {
            FileStream stream = null;
            StreamWriter write = null;

            try {
                stream = new FileStream("date.txt", FileMode.Truncate);
                write = new StreamWriter(stream, System.Text.Encoding.Default);

                byte[] nameByte = System.Text.Encoding.Default.GetBytes(NameR.Text);
                string strName = "";
                for (int i = 0; i < nameByte.Length; i++) {
                    if (i == nameByte.Length - 1)
                        strName += nameByte[i].ToString();
                    else
                        strName += nameByte[i].ToString() + ",";
                }

                byte[] pasByte = System.Text.Encoding.Default.GetBytes(Password.Text);
                string strPas = "";
                for (int i = 0; i < pasByte.Length; i++) {
                    if (i == pasByte.Length - 1)
                        strPas += pasByte[i].ToString();
                    else
                        strPas += pasByte[i].ToString() + ",";
                }

                /*(Application.OpenForms[0] as Form1).nameUser = NameR.Text;
                (Application.OpenForms[0] as Form1).encPas = GetEncodingPassword(Password.Text);
                (Application.OpenForms[0] as Form1).VisibleProfile();
                (Application.OpenForms[0] as Form1).rights = Rights.Text;
                (Application.OpenForms[0] as Form1).IsAdmin();*/

                write.WriteLine(strName);
                write.WriteLine(strPas);
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так!"); }
            finally { write.Close(); stream.Close(); }
        }

        private void RecordListOperationAddUser() {
            SqlConnection con = null;
            SqlCommand com;

            try {
                con = new SqlConnection(connectionString);
                com = new SqlCommand("INSERT INTO ListOperation(IP, DateTime, ID, Operation) VALUES (@IP, @DateTimeLogIn, @ID, @Operation)", con);
                con.Open();
                com.Parameters.AddWithValue("@IP", System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString());
                com.Parameters.AddWithValue("@DateTimeLogIn", DateTime.Now.ToString("s"));
                com.Parameters.AddWithValue("@ID", (Application.OpenForms[0] as Form1).GetID());
                com.Parameters.AddWithValue("@Operation", "Добавление пользователя");
                com.ExecuteNonQuery();
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
            finally { con.Close(); }
        }

        private string GetEncodingPassword(string password) {
            // переводим строку в байт-массив
            byte[] bytes = Encoding.Unicode.GetBytes(password);

            // создаём объект для получения средств шифрования
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            // вычисляем хэш-представление в байтах
            byte[] byteHash = CSP.ComputeHash(bytes);

            string encPassword = string.Empty;

            // формируем одну целую строку из массива

            foreach (byte b in byteHash)
                encPassword += string.Format("{0:x2}", b);

            return encPassword;

        }
    }
}

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
using System.IO;
using System.Security.Cryptography;

namespace RequestZp1 {
    public partial class SignInProfile : UserControl {
        public SignInProfile() {
            InitializeComponent();
        }

        private void SignIn_Click(object sender, EventArgs e) {
            ToSignIn();
        }

        public void ClearTextBox() {
            NameP.Clear();
            Password.Clear();
            RememberMe.Checked = false;
        }        
        private void ToSignIn() {
            SqlConnection con = null;
            SqlCommand com;
            string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";
            try {
                con = new SqlConnection(connectionString);
                string encPassword = GetEncodingPassword(Password.Text);
                con.Open();
                com = new SqlCommand("Select Count(*) From Users Where Name = @Name and Password = @Password", con);
                com.Parameters.AddWithValue("@Name", NameP.Text);
                com.Parameters.AddWithValue("@Password", encPassword);
                int count = (int)com.ExecuteScalar();
                byte countTry = 0;
                do {
                    if (count == 0) {
                        MessageBox.Show("Введеные неверные данные");
                        NameP.Clear();
                        Password.Clear();
                        countTry++;
                    } else {
                        MessageBox.Show("Вход выполнен");
                        com = new SqlCommand("Select Rights From Users Where Name = @Name and Password = @Password", con);
                        com.Parameters.AddWithValue("@Name", NameP.Text);
                        com.Parameters.AddWithValue("@Password", encPassword);
                        SqlDataReader reader = com.ExecuteReader();
                        reader.Read();

                        object rights = reader.GetString(0);
                        ToWriteFile(rights);
                    }
                } while (countTry != 3);
                if (countTry == 3) {
                    MessageBox.Show("Мсье, вы не знаете пароля");
                    Application.Exit();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close();  }
        }

        private void ToWriteFile(object rights) {
            FileStream stream = null;
            StreamWriter write = null;
            try {
                stream = new FileStream("date.txt", FileMode.Truncate);
                write = new StreamWriter(stream, System.Text.Encoding.Default);
                this.Hide();
                (Application.OpenForms[0] as Form1).nameUser = NameP.Text;
                (Application.OpenForms[0] as Form1).VisibleProfile();

                (Application.OpenForms[0] as Form1).rights = (string)rights;
                (Application.OpenForms[0] as Form1).IsAdmin();
                if (!RememberMe.Checked) {
                    write.Write("");
                    NameP.Clear();
                    Password.Clear();
                } else {
                    byte[] nameByte = System.Text.Encoding.Default.GetBytes(NameP.Text);
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

                    write.WriteLine(strName);
                    write.WriteLine(strPas);
                }
                
            } catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
            finally { write.Close(); stream.Close(); }
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

        private void Registration_Load(object sender, EventArgs e) {
            string text = "";
            using (FileStream stream = new FileStream("date.txt", FileMode.Open)) {
                StreamReader reader = new StreamReader(stream);
                string txt = "";
                while ((txt = reader.ReadLine()) != null)
                    text += txt + '\n';
                reader.Close();
            }
            if (text != null && text != "") {
                string[] masStr = text.Split('\n');
                string[] masName = masStr[0].Split(',');
                string[] masPassword = masStr[1].Split(',');

                byte[] byteName = new byte[masName.Length];
                for (int i = 0; i < byteName.Length; i++)
                    byteName[i] = Convert.ToByte(masName[i]);

                byte[] bytePassword = new byte[masPassword.Length];
                for (int i = 0; i < bytePassword.Length; i++)
                    bytePassword[i] = Convert.ToByte(masPassword[i]);

                NameP.Text = Encoding.Default.GetString(byteName);
                Password.Text = Encoding.Default.GetString(bytePassword);
                RememberMe.Checked = true;
            }



        }
    }
}

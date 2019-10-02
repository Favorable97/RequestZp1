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
        private object id;
        readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";
        byte countTry = 1;
        private void ToSignIn() {
            SqlConnection con = null;
            SqlCommand com;
            SqlDataReader reader = null;


            try {
                con = new SqlConnection(connectionString);
                string encPassword = GetEncodingPassword(Password.Text);
                con.Open();
                com = new SqlCommand("Select Count(*) From Users Where Name = @Name and Password = @Password", con);
                com.Parameters.AddWithValue("@Name", NameP.Text);
                com.Parameters.AddWithValue("@Password", encPassword);
                int count = (int)com.ExecuteScalar();
                byte countTry = 1;

                if (count == 0) {
                    MessageBox.Show("Введеные неверные данные");
                    NameP.Clear();
                    Password.Clear();
                    countTry++;
                    return;
                } else {
                    MessageBox.Show("Вход выполнен");
                    ToWriteFile();
                    countTry = 1;
                    com = new SqlCommand("Select ID, Rights From Users Where Name = @Name and Password = @Password", con);
                    com.Parameters.AddWithValue("@Name", NameP.Text);
                    com.Parameters.AddWithValue("@Password", encPassword);
                    reader = com.ExecuteReader();
                    reader.Read();

                    object rights = reader.GetString(1);
                    id = reader.GetValue(0);
                    EditForm(rights);
                    ToWriteDataBaseSuccessful(countTry);
                    reader.Close();
                }

                if (countTry == 3) {
                    ToWriteDataBaseNotSuccessful(countTry);
                    MessageBox.Show("Мсье, вы не знаете пароля");
                    Application.Exit();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { con.Close(); }
        }

        private void EditForm(object rights) {
            this.Hide();
            (Application.OpenForms[0] as Form1).AutoSize = true;
            (Application.OpenForms[0] as Form1).nameUser = NameP.Text;
            (Application.OpenForms[0] as Form1).VisibleProfile();
            (Application.OpenForms[0] as Form1).encPas = GetEncodingPassword(Password.Text);
            (Application.OpenForms[0] as Form1).rights = (string)rights;
            (Application.OpenForms[0] as Form1).IsAdmin();
            (Application.OpenForms[0] as Form1).ToFillTable();
            (Application.OpenForms[0] as Form1).ToFillDropDownList();
            (Application.OpenForms[0] as Form1).flag = true;
        }

        private void ToWriteFile() {
            FileStream stream = null;
            StreamWriter write = null;
            try {
                stream = new FileStream("date.txt", FileMode.Truncate);
                write = new StreamWriter(stream, System.Text.Encoding.Default);
                
                if (!RememberMe.Checked) {
                    write.Write("");
                    //NameP.Clear();
                    //Password.Clear();
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

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { write.Close(); stream.Close(); }
        }

        #region ListOperation
        private void ToWriteDataBaseSuccessful(byte countTry) {
            SqlConnection con = null;
            SqlCommand com;

            try {
                con = new SqlConnection(connectionString);
                com = new SqlCommand("INSERT INTO ListOperation(IP, DateTime, IsLogIn, Try, ID, Operation) VALUES (@IP, @DateTimeLogIn, @IsLogIn, @Try, @ID, @Operation)", con);
                con.Open();
                com.Parameters.AddWithValue("@IP", System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString());
                com.Parameters.AddWithValue("@DateTimeLogIn", DateTime.Now.ToString("s"));
                com.Parameters.AddWithValue("@IsLogIn", "Да");
                com.Parameters.AddWithValue("@Try", countTry);
                com.Parameters.AddWithValue("@ID", id);
                com.Parameters.AddWithValue("@Operation", "Выполнен вход");
                com.ExecuteNonQuery();
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
            finally { con.Close(); }
        }

        private void ToWriteDataBaseNotSuccessful(byte countTry) {
            SqlConnection con = null;
            SqlCommand com;

            try {
                con = new SqlConnection(connectionString);
                com = new SqlCommand("INSERT INTO ListOperation(IP, DateTime, IsLogIn, Try, Operation) VALUES (@IP, @DateTimeLogIn, @IsLogIn, @Try, @Operation)", con);
                con.Open();
                com.Parameters.AddWithValue("@IP", System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString());
                com.Parameters.AddWithValue("@DateTimeLogIn", DateTime.Now.ToString("s"));
                com.Parameters.AddWithValue("@IsLogIn", "Нет");
                com.Parameters.AddWithValue("@Try", countTry);
                com.Parameters.AddWithValue("@Operation", "Неудачная попытка входа");
                com.ExecuteNonQuery();
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
            finally { con.Close(); }
        }
        #endregion

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

        private void SignInProfile_Load(object sender, EventArgs e) {
            string text = "";
            //using (FileStream stream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "date.txt", FileMode.OpenOrCreate)) {

            using (var reader = new System.IO.StreamReader("date.txt")) {
                string txt = "";
                while ((txt = reader.ReadLine()) != null)
                    text += txt + '\n';
                //reader.Close();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

/*
 * CreateAnswer
 * Предназночение:
 * Создание файла с выгруженными данными людей.
 * Поиск выбранных людей в БД по ФИО и дате рождения. Берём ID этих людей.
 * По найденным ID ищем результаты в БД, в таблице Results. 
 * Запись в файл данных из таблицы Results
 */

namespace RequestZp1 {
    class CreateAnswer {
        private List<string> MasWithPeoples { get; set; }
        private string Path { get; set; }
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["con"].ConnectionString; //@"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";


        public CreateAnswer(List<string> persons, string path) {
            MasWithPeoples = persons;
            Path = path;
        }
        string fileName = "";
        /*
            using (var sw = new StreamWriter(@"C:\1.csv", false, Encoding.Default))
            {
                sw.WriteLine("Ячейка1;Ячейка2;Ячейка3");
                sw.WriteLine("Ячейка1-1;Ячейка1-2;Ячейка1-3");
                sw.WriteLine("Ячейка2-1;Ячейка2-2;Ячейка2-3");
            }
         */
        private string surname, name, fatherName, dr;
        
        // Поиск людей, информацию по которым необходимо выгрузить в файл
        public void SearchPeoples() {
            fileName = Path + @"\Result - " + DateTime.Now.ToString("yy.MM.dd HH.mm.ss") + ".csv";
            WriteFileHeader();
            for (int i = 0; i < MasWithPeoples.Count; i++) {
                string[] person = MasWithPeoples[i].Split(',');
                surname = person[0];
                name = person[1];
                fatherName = person[2];
                dr = person[3];
                using (SqlConnection con = new SqlConnection(connectionString)) {
                    using (SqlCommand com = new SqlCommand("Select ID From Peoples Where " +
                                                    "Surname = @Surname and Name = @Name and FatherName = @FatherName and DateBirthday = @DateBirthday", con)) {
                        con.Open();
                        com.Parameters.AddWithValue("@Surname", surname);
                        com.Parameters.AddWithValue("@Name", name);
                        com.Parameters.AddWithValue("@FatherName", fatherName);
                        com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(GetBirthday(dr), "yyyyMdd", null));
                        using (SqlDataReader reader = com.ExecuteReader()) {
                            reader.Read();
                            SearchResult(Convert.ToInt32(reader.GetInt32(0)));
                        }
                    }
                }
            }
        }

        // Поиск результатов
        private void SearchResult(int id) {
            using (StreamWriter writer = new StreamWriter(fileName, true, Encoding.Default)) {
                using (SqlConnection con = new SqlConnection(connectionString)) {
                    using (SqlCommand com = new SqlCommand("Select ENP, DBEG, DEND, OKATO, QOGRN, POLIS, MAINENP, DR From Results Where PID = @ID", con)) {
                        con.Open();
                        com.Parameters.AddWithValue("@ID", id);
                        using (SqlDataReader reader = com.ExecuteReader()) {
                            while (reader.Read()) {
                                string enp = reader.IsDBNull(0) ? "" : reader.GetString(0);
                                string dbeg = reader.IsDBNull(1) ? "" : reader.GetDateTime(1).ToShortDateString();
                                string dend = reader.IsDBNull(2) ? "" : reader.GetDateTime(2).ToShortDateString();
                                string okato = reader.IsDBNull(3) ? "" : reader.GetString(3);
                                string ogrn = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                string polis = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                string mainenp = reader.IsDBNull(6) ? "" : reader.GetString(6);
                                string dr = reader.IsDBNull(7) ? "" : reader.GetDateTime(7).ToShortDateString();
                                writer.WriteLine(surname + ";" + name + ";" + fatherName + ";" + enp + ";" + dbeg + ";" +
                                    dend + ";" + okato + ";" + ogrn + ";" + polis + ";" + mainenp + ";" + dr);
                            }
                        }
                    }
                }
            }
        }

        // Запись в файл заголовков столбцов
        private void WriteFileHeader() {
            using (StreamWriter writer = new StreamWriter(fileName, true, Encoding.Default)) {
                writer.WriteLine("Фамилия;Имя;Отчество;ENP;Дата начала;Дата окончания;OKATO;QOGRN;Серия и номер полиса;MAINEMNP;Дата рождения");
            }
        }

        private string GetBirthday(string birthday) {
            string year = birthday.Split('.')[2];
            string month = birthday.Split('.')[1];
            string day = birthday.Split('.')[0];
            string date = year + month + day;
            return date;
        }
    }
}

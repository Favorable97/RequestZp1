using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

/*
 * RecordDBResultFileSearch
 * Предназночение: 
 * Поиск в БД, в таблице FileXML файлов, у которых в ячейке Uprak1 и Uprak2 значения равны null,
 * После нахождения таких файлов, берутся имена, спереди приписывается путь к каталогу и в конце добавляется расширение .uprak1.
 * Проверяем, существет ли такой файл в каталоге. Если да, то в БД, для соответствующего файла, в ячейку Uprak1 записывается OK.
 * А также записываем в таблицу Peoples состояние Uprak1 о каждом человеке в записи.
 * Далее, в БД ищем файлы, у которых в ячейке Uprak1 значение Ok, а в Uprak2 - null.
 * После нахождения таких файлов, берутся имена, спереди приписывается путь к каталогу и в конце добавляется расширение .uprak2.
 * Проверяем, существет ли такой файл в каталоге. Если да, то в БД, для соответствующего файла, в ячейку Uprak2 записывается OK.
 * А также записываем в таблицу Peoples состояние Uprak2 о каждом человеке в записи.
 */

namespace CreateRequest {
    class RecordDBResultFileSearch {
        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";
        private readonly static string path = @"\\192.168.2.205\Ident\tfoms";
        
        // Метод, который ищет файлы в БД, у которых ячейка Uprak1 = null 
        public void SearchFileWithoutUprak1() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select FileName From FileXML Where Uprak1 IS NULL", con)) {
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read()) {
                        SearchUprak1(reader.GetString(0));
                    }
                    reader.Close();
                }
            }
        }

        // Проверка на существование файла. На входе имя файла, который будем проверять
        private void SearchUprak1(string fileName) {
            string fullFileName = path + @"\" + fileName + ".uprak1";
            if (File.Exists(fullFileName)) {
                WriteToOkUprak1(fileName);
            }
        }

        // Запись в БД соответсвующего результата
        private void WriteToOkUprak1(string fileName) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("UPDATE FileXML SET Uprak1 = @Otv WHERE FileName = @FileName", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@Otv", "OK");
                    com.Parameters.AddWithValue("@FileName", fileName);
                    com.ExecuteNonQuery();
                }
            }
            SearchPeoples1(fileName);
        }
        
        // поиск файла с нужным именем
        private int GetIDFile(string fileName) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select ID FROM FileXML WHERE FileName = @FileName", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@FileName", fileName);
                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    return Convert.ToInt32(reader.GetInt32(0));
                }
            }
        }

        private void SearchPeoples1(string fileName) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select Peoples.ID" +
                                                       " From RecordsXMLFile join Peoples on RecordsXMLFile.IDPerson = Peoples.ID" +
                                                       " Where RecordsXMLFile.IDFile = " + GetIDFile(fileName), con)) {
                    con.Open();
                    //com.Parameters.AddWithValue("@IDFile", GetIDFile(fileName));
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read()) {
                        UpdateTableUprak1(Convert.ToInt32(reader.GetInt32(0)));
                    }
                    
                }
            }
        }

        private void UpdateTableUprak1(int id) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Update Peoples Set Uprak1 = @Uprak1 Where ID = @ID", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@Uprak1", DateTime.Now);
                    com.Parameters.AddWithValue("@ID", id);
                    com.ExecuteNonQuery();
                }
            }
        }

        // Метод, который ищет файлы в БД, у которых ячейка Uprak2 = null 
        public void SearchFileWithoutUprak2() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select FileName From FileXML Where Uprak1 IS NOT NULL and Uprak2 IS NULL", con)) {
                    con.Open();
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        while (reader.Read()) {
                            SearchUprak2(reader.GetString(0));
                        }
                    }
                }
            }
        }

        // Проверка на существование файла. На входе имя файла, который будем проверять
        private void SearchUprak2(string fileName) {
            string fullFileName = path + @"\" + fileName + ".uprak2";
            if (File.Exists(fullFileName)) {
                WriteToOkUprak2(fileName);
                ParsingXMLFileAndRecordDataToDB par = new ParsingXMLFileAndRecordDataToDB(fileName);
                par.ParsingXMLFile();
            }
        }
        
        // Запись в БД соответсвующего результата
        private void WriteToOkUprak2(string fileName) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("UPDATE FileXML SET Uprak2 = @Otv WHERE FileName = @FileName", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@Otv", "OK");
                    com.Parameters.AddWithValue("@FileName", fileName);
                    com.ExecuteNonQuery();
                }
            }
            SearchPeoples2(fileName);
        }

        private void SearchPeoples2(string fileName) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select Peoples.ID From " +
                                                       " RecordsXMLFile join Peoples on RecordsXMLFile.IDPerson = Peoples.ID" +
                                                       " Where RecordsXMLFile.IDFile = " + GetIDFile(fileName), con)) {
                    con.Open();
                    //com.Parameters.AddWithValue("@IDFile", GetIDFile(fileName));
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read()) {
                        UpdateTableUprak2(Convert.ToInt32(reader.GetInt32(0)));
                    }

                }
            }
        }

        private void UpdateTableUprak2(int id) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Update Peoples Set Uprak2 = @Uprak2 Where ID = @ID", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@Uprak2", DateTime.Now);
                    com.Parameters.AddWithValue("@ID", id);
                    com.ExecuteNonQuery();
                }
            }
        }

    }
}

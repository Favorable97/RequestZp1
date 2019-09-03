using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

/*
 * CreateRequest
 * Предназночение:
 * Формирование ответов на файлы.
 * Поиск в таблице Files файлов, у которых поле Flag = Null, т.е. те файлы, на которые не были сформированы ответы
 * Получение ID файла из таблицы FileXML, по имени файла
 * Поиск людей, которые должны находиться в нужном файле
 */

namespace CreateRequest {
    class CreateAnswer {
        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";
        /*private string FileName { get; set; }

        public CreateAnswer(string fileName) {
            FileName = fileName;
        }*/

        public void SearchFilesWithoutFlag() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select FileName From Files Where Flag = @Flag", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@Flag", DBNull.Value);
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        while (reader.Read()) {
                            SearchPersons(reader.GetString(0));
                        }
                    }
                }
            }
        }

        private void SearchPersons(string fileName) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select IDPerson From RecordsXMLFile Where IDFile = @IDFile", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@IDFile", GetIDFile(fileName));
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        while (reader.Read()) {
                            GetResults(Convert.ToInt32(reader.GetInt32(0)));
                        }
                    }
                }
            }
        }

        private int GetIDFile(string fileName) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select ID From FileXML Where FileName = @FileName", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@FileName", fileName);
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        reader.Read();
                        return Convert.ToInt32(reader.GetInt32(0));
                    }
                }
            }
        }

        private void GetResults(int idPerson) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select * From Results Where PID = @PID", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@PID", idPerson);
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        while (reader.Read()) {

                        }
                    }
                }
            }
        }


        
    }
}

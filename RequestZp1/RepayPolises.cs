using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * RepayPolis
 * Предназначение:
 * Погашение полисов
 */

namespace RequestZp1 {
    class RepayPolises {
        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=srz3_00;User ID=user;Password=гыук";
        private List<string> MasWithPeoples { get; set; }

        public RepayPolises(List<string> persons) {
            MasWithPeoples = persons;
        }

        private string name, surname, fatherName, dr;
        int pid;
        string polis;

        public void ToRepay() {
            for (int i = 0; i < MasWithPeoples.Count; i++) {
                string[] person = MasWithPeoples[i].Split(',');
                surname = person[0];
                name = person[1];
                fatherName = person[2];
                dr = person[3];
                pid = GetPID();
                if (pid != -1) {
                    polis = GetPolis();
                    string res = GetResult();
                    
                    RepayPeople();
                    RepayPolis();
                    InsertInform();
                }
            }
        }

        // выставление даты погашения человека
        private void RepayPeople() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Update PEOPLE " +
                                                       "Set DSTOP = @stop, " +
                                                       "RSTOP = " + 3 + " " +
                                                       "Where ID = @pid", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@stop", DateTime.Now);
                    com.Parameters.AddWithValue("@pid", pid);
                    com.ExecuteNonQuery();
                }
            }
        }
        // выставление даты погашения полиса
        private void RepayPolis() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Update POLIS " +
                                                       "Set DSTOP = @stop, " +
                                                       "RSTOP = " + 3 + " " +
                                                       "Where PID = @pid and NPOL = @pol", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@stop", DateTime.Now);
                    com.Parameters.AddWithValue("@pid", pid);
                    com.Parameters.AddWithValue("@pol", polis);
                    com.ExecuteNonQuery();
                }
            }
        }

        // получаем ЕНП
        private int GetPID() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select ID " +
                                                       "From PEOPLE " +
                                                       "Where FAM = @surname and IM = @name and OT = @fatherName and DR = @dr", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@surname", surname);
                    com.Parameters.AddWithValue("@name", name);
                    com.Parameters.AddWithValue("@fatherName", fatherName);
                    com.Parameters.AddWithValue("@dr", DateTime.ParseExact(GetBirthday(dr), "yyyyMdd", null));
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                            return Convert.ToInt32(reader.GetInt32(0));
                    }
                    return -1;
                }
                
            }
            return 0;
        }
        // получаем полис
        private string GetPolis() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select NPOL " +
                                                       "From PEOPLE " +
                                                       "Where FAM = @surname and IM = @name and OT = @fatherName and DR = @dr", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@surname", surname);
                    com.Parameters.AddWithValue("@name", name);
                    com.Parameters.AddWithValue("@fatherName", fatherName);
                    com.Parameters.AddWithValue("@dr", DateTime.ParseExact(GetBirthday(dr), "yyyyMdd", null));
                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    return reader.GetString(0);
                }
            }
        }

        private void InsertInform() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Insert Into ERPMSG(TP, PID, DT, REASON, PARM) Values (@tp, @pid, @dt, @reas, isnull(@parm, @tmp))", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@tp", "А08");
                    com.Parameters.AddWithValue("@pid", pid);
                    com.Parameters.AddWithValue("@dt", DateTime.Now);
                    com.Parameters.AddWithValue("@reas", "п02");
                    com.Parameters.AddWithValue("@parm", GetResult());
                    com.Parameters.AddWithValue("@tmp", "");
                    com.ExecuteNonQuery();
                }
            }
        }

        // строка результатов
        private string GetResult() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                FileInfo file = new FileInfo("GetString.sql");
                string script = file.OpenText().ReadToEnd();
                using (SqlCommand com = new SqlCommand(script, con)) {
                    con.Open();
                    //com.Parameters.Clear();
                    //com.Parameters["@id1"].Value = pid;
                    com.Parameters.AddWithValue("@id1", pid);
                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    return reader.GetString(0);
                }
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

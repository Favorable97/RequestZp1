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
        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=srz3_00;User ID=expert;Password=123";
        private List<string> MasWithPeoples { get; set; }

        public RepayPolises(List<string> persons) {
            MasWithPeoples = persons;
        }

        private string name, surname, fatherName, dr;
        int pid;
        string polis, okato, res;

        public void ToRepay() {
            for (int i = 0; i < MasWithPeoples.Count; i++) {
                string[] person = MasWithPeoples[i].Split(',');
                surname = person[0];
                name = person[1];
                fatherName = person[2];
                dr = person[3];
                pid = GetPID();
                res = GetResult();
                okato = GetOkato();
                if (pid != -1 && okato == "90000" && res != "0") {
                    polis = GetPolis();
                    string res = GetResult();
                    RepayPeople();
                    RepayPolis();
                    InsertInform();
                } else {
                    if (pid == -1) {
                        WriteInformWithStatus1();
                    } else  if (GetOkato() != "90000"){
                        WriteInformWithStatus2();
                    } else if (GetResult() == "0") {
                        WriteInformWithStatus3();
                    }
                }
            }
        }

        // запись в БД тех людей, которые не прошли проверку по okato
        private void WriteInformWithStatus1() {
            using (SqlConnection con = new SqlConnection(@"Data Source=SRZ\SRZ;Initial Catalog=ident;Persist Security Info=True;User ID=user;Password=гыук")) {
                using (SqlCommand com = new SqlCommand("Insert Into ResultRepay(IDPerson, Status) Values(@id, @status)", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@id", GetID());
                    com.Parameters.AddWithValue("@status", 1);
                    com.ExecuteNonQuery();
                }
            }
        }

        // запись в БД тех людей, которые не нашлись в ЦС
        private void WriteInformWithStatus2() {
            using (SqlConnection con = new SqlConnection(@"Data Source=SRZ\SRZ;Initial Catalog=ident;Persist Security Info=True;User ID=user;Password=гыук")) {
                using (SqlCommand com = new SqlCommand("Insert Into ResultRepay(IDPerson, Status) Values(@id, @status)", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@id", GetID());
                    com.Parameters.AddWithValue("@status", 2);
                    com.ExecuteNonQuery();
                }
            }
        }

        private void WriteInformWithStatus3() {
            using (SqlConnection con = new SqlConnection(@"Data Source=SRZ\SRZ;Initial Catalog=ident;Persist Security Info=True;User ID=user;Password=гыук")) {
                using (SqlCommand com = new SqlCommand("Insert Into ResultRepay(IDPerson, Status) Values(@id, @status)", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@id", GetID());
                    com.Parameters.AddWithValue("@status", 3);
                    com.ExecuteNonQuery();
                }
            }
        }

        private int GetID() {
            using (SqlConnection con = new SqlConnection(@"Data Source=SRZ\SRZ;Initial Catalog=ident;Persist Security Info=True;User ID=user;Password=гыук")) {
                using (SqlCommand com = new SqlCommand("Select ID From Peoples Where Surname = @fam and Name = @im and FatherName = @ot and DateBirthday = @dr", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@fam", surname);
                    com.Parameters.AddWithValue("@im", name);
                    com.Parameters.AddWithValue("@ot", fatherName);
                    com.Parameters.AddWithValue("@dr", DateTime.ParseExact(GetBirthday(dr), "yyyyMdd", null));
                    using (SqlDataReader reader = com.ExecuteReader()) {
                        reader.Read();
                        return Convert.ToInt32(reader.GetInt32(0));
                    }
                }
            }
        }

        // выставление даты погашения человека
        private void RepayPeople() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Update PEOPLE " +
                                                       "Set DSTOP = @stop, " +
                                                       "RSTOP = " + 7 + " " +
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
                                                       "RSTOP = " + 7 + " " +
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
                        if (reader.HasRows) {
                            //if (!reader.IsDBNull(0))
                                return Convert.ToInt32(reader.GetInt32(0));
                        }
                        
                    }
                    return -1;
                }
                
            }
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
                using (SqlCommand com = new SqlCommand("Insert Into ERPMSG(PID, DT, TP, REASON, PARM) Values (@pid, @dt, @tp, @reas, isnull(@parm, @tmp))", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@pid", pid);
                    com.Parameters.AddWithValue("@dt", DateTime.Now);
                    com.Parameters.AddWithValue("@tp", "A08");
                    com.Parameters.AddWithValue("@reas", "П02");
                    com.Parameters.AddWithValue("@parm", GetResult());
                    com.Parameters.AddWithValue("@tmp", "");
                    com.ExecuteNonQuery();
                }
            }
        }


        private string GetOkato() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select OKATO From PEOPLE" +
                    " Where FAM = @surname and IM = @name and OT = @fatherName and DR = @dr" , con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@surname", surname);
                    com.Parameters.AddWithValue("@name", name);
                    com.Parameters.AddWithValue("@fatherName", fatherName);
                    com.Parameters.AddWithValue("@dr", DateTime.ParseExact(GetBirthday(dr), "yyyyMdd", null));
                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows) {
                        if (!reader.IsDBNull(0)) {
                            string okato = reader.GetString(0);
                            return okato;
                        }
                    }
                    
                    return "0";
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
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "0";
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

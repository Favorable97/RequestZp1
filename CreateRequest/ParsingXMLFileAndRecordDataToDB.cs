using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data.SqlClient;
//using System.Data.SqlTypes;

/*
 * ParsingXMLFileAndRecordDataToDB
 * Предназночение:
 * Парсинг uprak2 файла.
 * Получение данных из uprak2 и запись в БД, в таблицу Results.
 */

namespace CreateRequest {
    class ParsingXMLFileAndRecordDataToDB {
        private readonly static string path = @"\\192.168.2.205\Ident\tfoms";

        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";
        
        // порядковый номер, для определения ID человека
        int serialNumber = 0;
        // Имя файла передаётся в конструкторе
        private string FileName { get; set; }
        readonly XNamespace xNamespace = XNamespace.Get("urn:hl7-org:v2xml");
        public ParsingXMLFileAndRecordDataToDB(string fileName) {
            FileName = fileName;
        }

        public void ParsingXMLFile() {
            string fullName = path + @"\" + FileName + ".uprak2";
            XDocument uprak2 = XDocument.Load(fullName);
            XNamespace xNamespace = XNamespace.Get("urn:hl7-org:v2xml");
            uprak2.Declaration = new XDeclaration("1.0", "Windows-1251", null);

            foreach (XElement rsp in uprak2.Element(xNamespace + "UPRMessageBatch").Elements(xNamespace + "RSP_ZK1")) {
                serialNumber++;
                if (rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE") != null) {
                    RecordDBInformation(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                } else {
                    RecordDBInformation();
                }
            }
        }

        // Записываем информацию в случае наличия информации в ЦС
        private void RecordDBInformation(XElement element) {
            if (IsExistPeople()) {
                RecordResult(element);
            } else {
                DeletePerson();
                RecordResult(element);
            }
            
        }

        // Проверка, проверялся ли этот человек
        private bool IsExistPeople() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select Count(*) From Results Where PID = @PID", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@PID", GetPID());
                    int count = (int)com.ExecuteScalar();
                    if (count > 0)
                        return false;
                    else
                        return true;
                }
            }
        }

        /*// запись в таблицу Results данных о человеке
        private void RecordResult(XElement element) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("INSERT INTO Results(PID, ENP, DBEG, DEND, OKATO, OPDOC, QOGRN, MAIN, POLIS, DUP, NR, MAINENP, DS, DR, W, POLVID) " +
                    "VALUES(@PID, @ENP, @DBEG, @DEND, @OKATO, @OPDOC, @QOGRN, @MAIN, @POLIS, @DUP, @NR, @MAINENP, @DS, @DR, @W, @POLVID)", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@PID", GetPID());
                    if (GetENP(element) != "")
                        com.Parameters.AddWithValue("@ENP", GetENP(element));
                    else {
                        com.Parameters.AddWithValue("@ENP", DBNull.Value);
                    }
                    if (GetDBEG(element) != "")
                        com.Parameters.AddWithValue("@DBEG", DateTime.ParseExact(GetDBEG(element), "yyyy-MM-dd", null));
                    else {
                        com.Parameters.AddWithValue("@DBEG", DBNull.Value);
                    }
                    if (GetDEND(element) != "")
                        com.Parameters.AddWithValue("@DEND", DateTime.ParseExact(GetDEND(element), "yyyy-MM-dd", null));
                    else {
                        com.Parameters.AddWithValue("@DEND", DBNull.Value);
                    }
                    if (GetOkato(element) != "")
                        com.Parameters.AddWithValue("@OKATO", GetOkato(element));
                    else {
                        com.Parameters.AddWithValue("@OKATO", DBNull.Value);
                    }
                    if (GetOPDOC(element) != "")
                        com.Parameters.AddWithValue("@OPDOC", GetOPDOC(element));
                    else {
                        com.Parameters.AddWithValue("@OPDOC", DBNull.Value);
                    }
                    if (GetQOGRN(element) != "")
                        com.Parameters.AddWithValue("@QOGRN", GetQOGRN(element));
                    else {
                        com.Parameters.AddWithValue("@QOGRN", DBNull.Value);
                    }
                    com.Parameters.AddWithValue("@MAIN", 1);
                    if (GetPOLIS(element) != "")
                        com.Parameters.AddWithValue("@POLIS", GetPOLIS(element));
                    else {
                        com.Parameters.AddWithValue("@POLIS", DBNull.Value);
                    }
                    if (GetDUP(element) != "")
                        com.Parameters.AddWithValue("@DUP", GetDUP(element));
                    else {
                        com.Parameters.AddWithValue("@DUP", DBNull.Value);
                    }
                    com.Parameters.AddWithValue("@NR", 1);
                    if (GetMainENP(element) != "")
                        com.Parameters.AddWithValue("@MAINENP", GetMainENP(element));
                    else {
                        com.Parameters.AddWithValue("@MAINENP", DBNull.Value);
                    }
                    if (GetDS(element) != -1)
                        com.Parameters.AddWithValue("@DS", GetDS(element));
                    else {
                        com.Parameters.AddWithValue("@DS", DBNull.Value);
                    }
                    if (GetDR(element) != "")
                        com.Parameters.AddWithValue("@DR", DateTime.ParseExact(GetDR(element), "yyyy-MM-dd", null));
                    else {
                        com.Parameters.AddWithValue("@DR", DBNull.Value);
                    }
                    if (GetW(element) != 0)
                        com.Parameters.AddWithValue("@W", GetW(element));
                    else {
                        com.Parameters.AddWithValue("@W", DBNull.Value);
                    }
                    if (GetPOLVID(element) != "")
                        com.Parameters.AddWithValue("@POLVID", GetPOLVID(element));
                    else {
                        com.Parameters.AddWithValue("@POLVID", DBNull.Value);
                    }
                    com.ExecuteNonQuery();
                }
            }
        }*/

        // запись в таблицу Results данных о человеке
        private void RecordResult(XElement element) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("INSERT INTO Results(PID, ENP, DBEG, DEND, OKATO, OPDOC, QOGRN, MAIN, POLIS, DUP, NR, MAINENP, DS, DR, W, POLVID) " +
                    "VALUES(@PID, @ENP, @DBEG, @DEND, @OKATO, @OPDOC, @QOGRN, @MAIN, @POLIS, @DUP, @NR, @MAINENP, @DS, @DR, @W, @POLVID)", con)) {
                    con.Open();
                    foreach (XElement el in element.Elements(xNamespace + "PID"))
                    com.Parameters.AddWithValue("@PID", GetPID());
                    if (GetENP(element) != "")
                        com.Parameters.AddWithValue("@ENP", GetENP(element));
                    else {
                        com.Parameters.AddWithValue("@ENP", DBNull.Value);
                    }
                    if (GetDBEG(element) != "")
                        com.Parameters.AddWithValue("@DBEG", DateTime.ParseExact(GetDBEG(element), "yyyy-MM-dd", null));
                    else {
                        com.Parameters.AddWithValue("@DBEG", DBNull.Value);
                    }
                    if (GetDEND(element) != "")
                        com.Parameters.AddWithValue("@DEND", DateTime.ParseExact(GetDEND(element), "yyyy-MM-dd", null));
                    else {
                        com.Parameters.AddWithValue("@DEND", DBNull.Value);
                    }
                    if (GetOkato(element) != "")
                        com.Parameters.AddWithValue("@OKATO", GetOkato(element));
                    else {
                        com.Parameters.AddWithValue("@OKATO", DBNull.Value);
                    }
                    if (GetOPDOC(element) != "")
                        com.Parameters.AddWithValue("@OPDOC", GetOPDOC(element));
                    else {
                        com.Parameters.AddWithValue("@OPDOC", DBNull.Value);
                    }
                    if (GetQOGRN(element) != "")
                        com.Parameters.AddWithValue("@QOGRN", GetQOGRN(element));
                    else {
                        com.Parameters.AddWithValue("@QOGRN", DBNull.Value);
                    }
                    com.Parameters.AddWithValue("@MAIN", 1);
                    if (GetPOLIS(element) != "")
                        com.Parameters.AddWithValue("@POLIS", GetPOLIS(element));
                    else {
                        com.Parameters.AddWithValue("@POLIS", DBNull.Value);
                    }
                    if (GetDUP(element) != "")
                        com.Parameters.AddWithValue("@DUP", GetDUP(element));
                    else {
                        com.Parameters.AddWithValue("@DUP", DBNull.Value);
                    }
                    com.Parameters.AddWithValue("@NR", 1);
                    if (GetMainENP(element) != "")
                        com.Parameters.AddWithValue("@MAINENP", GetMainENP(element));
                    else {
                        com.Parameters.AddWithValue("@MAINENP", DBNull.Value);
                    }
                    if (GetDS(element) != -1)
                        com.Parameters.AddWithValue("@DS", GetDS(element));
                    else {
                        com.Parameters.AddWithValue("@DS", DBNull.Value);
                    }
                    if (GetDR(element) != "")
                        com.Parameters.AddWithValue("@DR", DateTime.ParseExact(GetDR(element), "yyyy-MM-dd", null));
                    else {
                        com.Parameters.AddWithValue("@DR", DBNull.Value);
                    }
                    if (GetW(element) != 0)
                        com.Parameters.AddWithValue("@W", GetW(element));
                    else {
                        com.Parameters.AddWithValue("@W", DBNull.Value);
                    }
                    if (GetPOLVID(element) != "")
                        com.Parameters.AddWithValue("@POLVID", GetPOLVID(element));
                    else {
                        com.Parameters.AddWithValue("@POLVID", DBNull.Value);
                    }
                    com.ExecuteNonQuery();
                }
            }
        }


        // удаление данных о человеке в таблице Results
        private void DeletePerson() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com  = new SqlCommand("Delete From Results Where PID = @PID", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@PID", GetPID());
                    com.ExecuteNonQuery();
                }
            }
        }

        // Записываем информацию в случае отсутсвия информации в ЦС
        private void RecordDBInformation() {
            if (IsExistPeople()) {
                using (SqlConnection con = new SqlConnection(connectionString)) {
                    using (SqlCommand com = new SqlCommand("INSERT INTO Results(PID, ENP) VALUES(@PID, @ENP)", con)) {
                        con.Open();
                        com.Parameters.AddWithValue("@PID", GetPID());
                        com.Parameters.AddWithValue("@ENP", "Нет страхования в ЦС");
                        com.ExecuteNonQuery();
                    }
                }
            } else {
                DeletePerson();
                using (SqlConnection con = new SqlConnection(connectionString)) {
                    using (SqlCommand com = new SqlCommand("INSERT INTO Results(PID, ENP) VALUES(@PID, @ENP)", con)) {
                        con.Open();
                        com.Parameters.AddWithValue("@PID", GetPID());
                        com.Parameters.AddWithValue("@ENP", "Нет страхования в ЦС");
                        com.ExecuteNonQuery();
                    }
                }
            }
            
        }

        // ID человека
        #region PID
        // Выясняем ID человека
        private int GetPID() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select IDPerson From RecordsXMLFile Where IDFile = @IDFile and SerialNumber = @SerialNumber", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@IDFile", GetIDFile());
                    com.Parameters.AddWithValue("@SerialNumber", serialNumber);
                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
        }
        
        // Получаем ID файла
        private int GetIDFile() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("Select ID From FileXML Where FileName = @FileName", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@FileName", FileName);
                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
        }

        #endregion

        // ENP
        #region ENP
        // ENP человека
        private string GetENP(XElement element) {
            byte count = 1;
            foreach (XElement el in element.Element(xNamespace + "PID").Elements(xNamespace + "PID.3")) {
                if (count == 1) {
                    count++;
                    continue;
                } 
                else
                    return el.Element(xNamespace + "CX.1").Value.ToString();
            }
            return null;
        }
        #endregion
        // com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(RefBirthday(date), "yyyyMdd", null));

        // Дата начала
        #region DBEG
        // Дата начала
        private string GetDBEG(XElement element) {
            if (element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.12").Value != null)
                return element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.12").Value;
            else return null;
        }
        #endregion

        // Дата окончания
        #region DEND
        private string GetDEND(XElement element) {
            if (element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.13").Value != null)
                return element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.13").Value;
            else return null;
        }
        #endregion

        // OKATO
        #region OKATO
        private string GetOkato(XElement element) {
            if (element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.15").Value != null)
                return element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.15").Value;
            else return null;
        }
        #endregion

        // Тип полиса
        #region OPDOC
        private string GetOPDOC(XElement element) {
            switch (element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.35").Value) {
                case "С":
                    return "1";
                case "В":
                    return "2";
                case "П":
                    return "3";
                case "Х":
                    return "4";
                case "Э":
                    return "5";
                default:
                    return null;
            }
        }
        #endregion

        // ОГРН
        #region QOGRN
        private string GetQOGRN(XElement element) {
            if (element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.3").Element(xNamespace + "CX.1").Value != null)
                return element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.3").Element(xNamespace + "CX.1").Value;
            else return null;
        }
        #endregion

        // серия и номер полиса
        #region POLIS
        private string GetPOLIS(XElement element) {
            if (element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.36").Value != null)
                return element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.36").Value;
            else return null;
        }
        #endregion

        // ключи
        #region DUP
        private string GetDUP(XElement element) {
            string dup = "";
            foreach (var el in element.Element(xNamespace + "QRI").Descendants()) {
                dup += el.Value + ",";
            }
            if (dup == null)
                return null;
            else return dup.TrimEnd(',');
        }
        #endregion

        // MainENP
        #region MainENP
        // ENP человека
        private string GetMainENP(XElement element) {
            //byte count = 1;
            foreach (XElement el in element.Element(xNamespace + "PID").Elements(xNamespace + "PID.3")) {
                return el.Element(xNamespace + "CX.1").Value.ToString();
            }
            return null;
        }
        #endregion

        // Признак смерти
        #region DS
        private int GetDS(XElement element) {
            switch (element.Element(xNamespace + "PID").Element(xNamespace + "PID.30").Value) {
                case "Y":
                    return 1;
                case "N":
                    return 0;
                default:
                    return -1;
            }
        }
        #endregion

        // День рождения
        #region DR
        private string GetDR(XElement element) {
            if (element.Element(xNamespace + "PID").Element(xNamespace + "PID.7").Value != null)
                return element.Element(xNamespace + "PID").Element(xNamespace + "PID.7").Value;
            else return null;
        }
        #endregion
        
        // Пол
        #region W
        private int GetW(XElement element) {
            if (element.Element(xNamespace + "PID").Element(xNamespace + "PID.8").Value != null) {
                return Convert.ToInt32(element.Element(xNamespace + "PID").Element(xNamespace + "PID.8").Value);
            } else return 0;
            
        }
        #endregion
        
        // Вид полиса
        #region POLVID
        private string GetPOLVID(XElement element) {
            if (element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.35").Value != null)
                return element.Element(xNamespace + "IN1").Element(xNamespace + "IN1.35").Value;
            else return null;
        }
        #endregion
    }
}

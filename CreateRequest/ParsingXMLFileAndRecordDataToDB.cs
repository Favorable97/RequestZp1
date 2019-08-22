using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data.SqlClient;

/*
 * ParsingXMLFileAndRecordDataToDB
 * Предназночение:
 * Парсинг uprak2 файла.
 * Получение данных из uprak2 и запись в БД, в таблицу Results.
 */

namespace CreateRequest {
    class ParsingXMLFileAndRecordDataToDB {
        private readonly static string path = @"\\192.168.2.205\Ident";

        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";
        
        // порядковый номер, для определения ID человека
        int serialNumber = 0;
        // Имя файла передаётся в конструкторе
        private string FileName { get; set; }
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
                    int a = GetPID();
                    string b = GetENP(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    string d = GetDBEG(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    string c = GetDEND(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    string e = GetOkato(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    string f = GetOPDOC(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    string g = GetQOGRN(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    string u = GetPOLIS(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    string y = GetDUP(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    string k = GetMainENP(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    int s = GetDS(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    string dr = GetDR(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    int w = GetW(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));
                    string pol = GetPOLVID(rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE"));

                } else {
                    // гововорим о том, что информации нет
                }
            }
        }
        XNamespace xNamespace = XNamespace.Get("urn:hl7-org:v2xml");
        
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

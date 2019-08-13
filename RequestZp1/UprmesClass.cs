using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RequestZp1 {
    class UprmesClass {
        private readonly string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        private readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True;User ID=user;Password=гыук";

        private DataGridView RequestTable { get; set; }
        public string FileName { get; set; }

        private string name, surname, fatherName;
        string date;
        public UprmesClass(DataGridView table) {
            RequestTable = table;
        }

        public void CreateXmlFile() {
            Random rnd = new Random();
            int rndNumb = rnd.Next(0, 33);
            string hash = "q" + GetHash(alphabet[rndNumb].ToString()).ToString();
            hash = hash.Replace("=", "");
            hash = hash.Replace("+", "");
            FileName = ("90000-" + hash.ToString().Replace(@"/", "") + ".uprmes").ToString();
            RecordFileXML();

            #region Комментарий

            #endregion

            #region XML
            XDocument xdoc = new XDocument();

            XNamespace xNamespace = XNamespace.Get("urn:hl7-org:v2xml");
            xdoc.Declaration = new XDeclaration("1.0", "Windows-1251", null);
            XElement upprmes = new XElement(xNamespace + "UPRMessageBatch");

            XAttribute xmlnss = new XAttribute("xmlns", "urn:hl7-org:v2xml");

            XAttribute xmlnsrtc = new XAttribute(XNamespace.Xmlns + "rtc", "http://www.rintech.ru");
            XAttribute xmlnsxsd = new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema");
            XAttribute xmlnsxsi = new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance");
            upprmes.Add(xmlnss, xmlnsrtc, xmlnsxsd, xmlnsxsi);

            //QPD_ZP1
            #region BHS
            // BHS
            XElement bhs = new XElement(xNamespace + "BHS");

            XElement bhs1 = new XElement(xNamespace + "BHS.1", "|");
            XNamespace amp = "&";
            XElement bhs2 = new XElement(xNamespace + "BHS.2", "^" + (char)126 + "" + (char)92 + "" + amp);

            XElement bhs3 = new XElement(xNamespace + "BHS.3");
            XElement hd_1 = new XElement(xNamespace + "HD.1", "СРЗ 15"); // bhs3
            bhs3.Add(hd_1);

            XElement bhs4 = new XElement(xNamespace + "BHS.4");
            XElement hd_1__ = new XElement(xNamespace + "HD.1", "15"); // bhs4
            XElement hd2__ = new XElement(xNamespace + "HD.2", "1.2.643.2.40.3.3.1.0");
            XElement hd3__ = new XElement(xNamespace + "HD.3", "ISO");
            bhs4.Add(hd_1__, hd2__, hd3__);

            XElement bhs5 = new XElement(xNamespace + "BHS.5");
            XElement hd_1_ = new XElement(xNamespace + "HD.1", "ЦК ЕРП"); // bhs5
            bhs5.Add(hd_1_);

            XElement bhs6 = new XElement(xNamespace + "BHS.6");
            XElement hd1___ = new XElement(xNamespace + "HD.1", "00"); // bhs6
            XElement hd2___ = new XElement(xNamespace + "HD.2", "1.2.643.2.40.3.3.1.0");
            XElement hd3___ = new XElement(xNamespace + "HD.3", "ISO");
            bhs6.Add(hd1___, hd2___, hd3___);

            DateTime time1 = DateTime.Now;
            string strTime = time1.ToString("s") + "Z+03:00";
            XElement bhs7 = new XElement(xNamespace + "BHS.7", strTime);

            XElement bhs11 = new XElement(xNamespace + "BHS.11", hash.ToString().Replace(@"/", ""));

            bhs.Add(bhs1, bhs2, bhs3, bhs4, bhs5, bhs6, bhs7, bhs11);
            #endregion
            #region MSH
            //MSH

            XElement msh1 = new XElement(xNamespace + "MSH.1", "|");

            XElement msh2 = new XElement(xNamespace + "MSH.2", "^" + "" + (char)126 + "" + (char)92 + "" + amp);

            XElement msh3 = new XElement(xNamespace + "MSH.3");
            XElement hd1 = new XElement(xNamespace + "HD.1", "СРЗ 15"); // msh3
            msh3.Add(hd1);

            XElement msh4 = new XElement(xNamespace + "MSH.4");
            XElement hd1_ = new XElement(xNamespace + "HD.1", "15"); // msh4
            XElement hd2 = new XElement(xNamespace + "HD.2", "1.2.643.2.40.3.3.1.0");
            XElement hd3 = new XElement(xNamespace + "HD.3", "ISO");
            msh4.Add(hd1_, hd2, hd3);

            XElement msh5 = new XElement(xNamespace + "MSH.5");
            XElement _hd1 = new XElement(xNamespace + "HD.1", "ЦК ЕРП"); // msh5
            msh5.Add(_hd1);

            XElement msh6 = new XElement(xNamespace + "MSH.6");
            XElement hd1__ = new XElement(xNamespace + "HD.1", "00"); // msh6
            XElement hd2_ = new XElement(xNamespace + "HD.2", "1.2.643.2.40.3.3.1.0");
            XElement hd3_ = new XElement(xNamespace + "HD.3", "ISO");
            msh6.Add(hd1__, hd2_, hd3_);

            XElement msh9 = new XElement(xNamespace + "MSH.9");
            XElement msg1 = new XElement(xNamespace + "MSG.1", "QBP"); // msh9
            XElement msg2 = new XElement(xNamespace + "MSG.2", "ZP1");
            XElement msg3 = new XElement(xNamespace + "MSG.3", "QBP_ZP1");
            msh9.Add(msg1, msg2, msg3);

            XElement msh11 = new XElement(xNamespace + "MSH.11");
            XElement pt1 = new XElement(xNamespace + "PT.1", "P"); // msh11
            msh11.Add(pt1);

            XElement msh12 = new XElement(xNamespace + "MSH.12");
            XElement vid1 = new XElement(xNamespace + "VID.1", "2.6");
            msh12.Add(vid1);

            XElement msh15 = new XElement(xNamespace + "MSH.15", "AL");
            XElement msh16 = new XElement(xNamespace + "MSH.16", "AL");
            #endregion
            #region QPD
            //QPD
            XElement qpd1 = new XElement(xNamespace + "QPD.1");
            XElement cwe1 = new XElement(xNamespace + "CWE.1", "СП");
            XElement cwe3 = new XElement(xNamespace + "CWE.3", "1.2.643.2.40.1.9");
            qpd1.Add(cwe1, cwe3);
            XElement qpd3 = new XElement(xNamespace + "QPD.3", "У");
            XElement qpd4 = new XElement(xNamespace + "QPD.4", GetBirthday(DateTime.Now.ToShortDateString()));
            string strWithDate = msh1.Value.ToString() + msh2.Value.ToString() + hd1.Value.ToString() + hd1_.Value.ToString() + hd2.Value.ToString() +
                hd3.Value.ToString() + _hd1.Value.ToString() + hd1__.Value.ToString() + hd2_.Value.ToString() + hd3_.Value.ToString() +
                msg1.Value.ToString() + msg2.Value.ToString() + msg3.Value.ToString() + pt1.Value.ToString() + vid1.Value.ToString() + msh15.Value.ToString() +
                msh16.Value.ToString() + cwe1.Value.ToString() + cwe3.Value.ToString() + qpd3.Value.ToString() + qpd4.Value.ToString();

            

            upprmes.Add(bhs);
            int count = 0;
            for (int i = 0; i < RequestTable.Rows.Count; i++) {
                if (Convert.ToBoolean(RequestTable.Rows[i].Cells[0].Value)) {
                    count++;
                    surname = RequestTable.Rows[i].Cells[1].Value.ToString().Split(' ')[0];
                    name = RequestTable.Rows[i].Cells[1].Value.ToString().Split(' ')[1];
                    fatherName = RequestTable.Rows[i].Cells[1].Value.ToString().Split(' ')[2];
                    date = GetBirthday(RequestTable.Rows[i].Cells[2].Value.ToString());
                    RecordDataBaseRec();
                    UpdatePeople();
                    XElement qbp_zp1 = new XElement(xNamespace + "QBP_ZP1");
                    XElement qpd = new XElement(xNamespace + "QPD");
                    XElement msh = new XElement(xNamespace + "MSH");
                    #region MSH
                    DateTime time = DateTime.Now;
                    string timeStr = time.ToString("s") + "Z+03:00";
                    XElement msh7 = new XElement(xNamespace + "MSH.7", timeStr);
                    XElement msh10;
                    int j = 0;
                    XElement qpd5 = new XElement(xNamespace + "QPD.5");
                    XElement cx1 = null;
                    XElement cx5 = null;
                    if (GetCode(surname, name, fatherName, date) == 14) {
                        cx1 = new XElement(xNamespace + "CX.1", GetDocInfoPassport(surname, name, fatherName, date));
                        cx5 = new XElement(xNamespace + "CX.5", GetCode(surname, name, fatherName, date));
                    } else {
                        cx1 = new XElement(xNamespace + "CX.1", GetDocInfoSR(surname, name, fatherName, date));
                        cx5 = new XElement(xNamespace + "CX.5", GetCode(surname, name, fatherName, date));
                    }

                    qpd5.Add(cx1, cx5);
                    if (i < 33) {
                        string tempStr = alphabet[i].ToString();
                        tempStr = GetHash(tempStr).ToString().Replace("=", "");
                        tempStr = tempStr.Replace(@"/", "");
                        tempStr = tempStr.Replace("+", "");
                        msh10 = new XElement(xNamespace + "MSH.10", tempStr);

                    } else {
                        string tempStr = (alphabet[i] + alphabet[j]).ToString();
                        tempStr = GetHash(tempStr).ToString().Replace("=", "");
                        tempStr = tempStr.Replace(@"/", "");
                        tempStr = tempStr.Replace("+", "");
                        msh10 = new XElement(xNamespace + "MSH.10", tempStr);
                        j++;
                    }
                    #endregion

                    XElement qpd6 = new XElement(xNamespace + "QPD.6");
                    XElement xpn1 = new XElement(xNamespace + "XPN.1");
                    XElement fn1 = new XElement(xNamespace + "FN.1", surname);
                    XElement xpn2 = new XElement(xNamespace + "XPN.2", name);
                    XElement xpn3 = new XElement(xNamespace + "XPN.3", fatherName);
                    XElement xpn7 = new XElement(xNamespace + "XPN.7", "L");
                    XElement qpd7 = new XElement(xNamespace + "QPD.7", date);
                    XElement qpd8;
                    XElement qpd9 = new XElement(xNamespace + "QPD.9", "В");
                    int index = fatherName.Length - 1;

                    if (fatherName[index] == 'ч') {
                        qpd8 = new XElement(xNamespace + "QPD.8", "1");
                    } else {
                        qpd8 = new XElement(xNamespace + "QPD.8", "2");
                    }
                    xpn1.Add(fn1);
                    qpd6.Add(xpn1, xpn2, xpn3, xpn7);

                    qpd.Add(qpd1, qpd3, qpd4, qpd5, qpd6, qpd7, qpd8, qpd9);
                    msh.Add(msh1, msh2, msh3, msh4, msh5, msh6, msh7, msh9, msh10, msh11, msh12, msh15, msh16);
                    qbp_zp1.Add(msh, qpd);
                    upprmes.Add(qbp_zp1);
                    strWithDate += msh7.Value.ToString() + cx1.Value.ToString() + cx5.Value.ToString() + msh10.Value.ToString() + fn1.Value.ToString() +
                    xpn2.Value.ToString() + xpn3.Value.ToString() + qpd7.Value.ToString() + qpd8.Value.ToString() + qpd9.Value.ToString();
                }
                #endregion
            }
            #region BTS
            XElement bts = new XElement(xNamespace + "BTS");
            XElement bts1 = new XElement(xNamespace + "BTS.1", count);

            Crc32 crc32 = new Crc32();
            var arrayOfBytes = Encoding.ASCII.GetBytes(strWithDate);
            XElement bts3 = new XElement(xNamespace + "BTS.3", crc32.Get(arrayOfBytes).ToString("X"));
            bts.Add(bts1, bts3);

            #endregion
            upprmes.Add(bts);
            xdoc.Add(upprmes);
            xdoc.Save(@"\\192.168.2.205\Ident\" + ("90000-" + hash.ToString().Replace(@"/", "") + ".uprmes").ToString());

            #endregion
        }

        private string GetDocInfoPassport(string surname, string name, string fatherName, string birthday) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                con.Open();
                SqlCommand com = new SqlCommand("Select SeriesDoc, NumbDoc From Peoples Where Surname = @Surname and Name = @Name and FatherName = @FatherName and DateBirthday = @DateBirthday", con);
                com.Parameters.AddWithValue("@Surname", surname);
                com.Parameters.AddWithValue("@Name", name);
                com.Parameters.AddWithValue("@FatherName", fatherName);
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(RefBirthday(birthday), "yyyyMdd", null));
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                string series = reader.GetString(0);
                //string correctSeries = series[0] + series[1] + " " + series[2] + series[3];
                string number = reader.GetString(1);
                reader.Close();
                string docInfo = series + " № " + number;
                return docInfo;
            }
        }

        private string GetDocInfoSR(string surname, string name, string fatherName, string birthday) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                con.Open();
                SqlCommand com = new SqlCommand("Select SeriesDoc, NumbDoc From Peoples Where Surname = @Surname and Name = @Name and FatherName = @FatherName and DateBirthday = @DateBirthday", con);
                com.Parameters.AddWithValue("@Surname", surname);
                com.Parameters.AddWithValue("@Name", name);
                com.Parameters.AddWithValue("@FatherName", fatherName);
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(RefBirthday(birthday), "yyyyMdd", null));
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                string series = reader.GetString(0);
                string number = reader.GetString(1);
                reader.Close();
                string docInfo = series + " " + number;
                return docInfo;
            }
        }

        private int GetCode(string surname, string name, string fatherName, string birthday) {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                con.Open();
                SqlCommand com = new SqlCommand("Select CodeDocument From Peoples Where Surname = @Surname and Name = @Name and FatherName = @FatherName and DateBirthday = @DateBirthday", con);
                com.Parameters.AddWithValue("@Surname", surname);
                com.Parameters.AddWithValue("@Name", name);
                com.Parameters.AddWithValue("@FatherName", fatherName);
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(RefBirthday(birthday), "yyyyMdd", null));
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                int code = Convert.ToInt32(reader.GetValue(0));
                reader.Close();
                return code;
            }
        }

        private void UpdatePeople() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                SqlCommand com = new SqlCommand("UPDATE Peoples SET Status = @Status WHERE Surname = @Surname and Name = @Name and FatherName = @FatherName and DateBirthday = @DateBirthday", con);
                con.Open();
                com.Parameters.AddWithValue("@Status", "Archive");
                com.Parameters.AddWithValue("@Surname", surname);
                com.Parameters.AddWithValue("@Name", name);
                com.Parameters.AddWithValue("@FatherName", fatherName);
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(RefBirthday(date), "yyyyMdd", null));
                com.ExecuteNonQuery();
            }
        }

        

        private void RecordFileXML() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                SqlCommand com = new SqlCommand("INSERT INTO FileXML(FileName, DateCreate) VALUES (@FileName, @DateCreate)", con);
                con.Open();
                com.Parameters.AddWithValue("@FileName", FileName);
                com.Parameters.AddWithValue("@DateCreate", DateTime.Now.ToString("s"));
                com.ExecuteNonQuery();
            }
        }

        private void RecordDataBaseRec() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                SqlCommand com = new SqlCommand("INSERT INTO RecordsXMLFile(IDFile, IDPerson, DateAdd) VALUES (@IDFile, @IDPerson, @DateAdd)", con);
                con.Open();
                com.Parameters.AddWithValue("@IDFile", SelectIDFile());
                com.Parameters.AddWithValue("@IDPerson", SelectIDPerson());
                com.Parameters.AddWithValue("@DateAdd", DateTime.Now.ToString("s"));
                com.ExecuteNonQuery();
            }
        }

        private int SelectIDPerson() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                SqlCommand com = new SqlCommand("Select ID From Peoples WHERE Surname = @Surname and Name = @Name and FatherName = @FatherName and DateBirthday = @DateBirthday", con);
                con.Open();
                com.Parameters.AddWithValue("@Surname", surname);
                com.Parameters.AddWithValue("@Name", name);
                com.Parameters.AddWithValue("@FatherName", fatherName);
                com.Parameters.AddWithValue("@DateBirthday", DateTime.ParseExact(RefBirthday(date), "yyyyMdd", null));
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                int id = Convert.ToInt32(reader.GetValue(0));
                reader.Close();
                return id;
            }
        }

        private int SelectIDFile() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                SqlCommand com = new SqlCommand("Select ID From FileXML WHERE FileName = @FileName", con);
                con.Open();
                com.Parameters.AddWithValue("@FileName", FileName);
                SqlDataReader reader = com.ExecuteReader();
                reader.Read();
                int id = Convert.ToInt32(reader.GetValue(0));
                reader.Close();
                return id;
            }
        }

        private string GetBirthday(string birthday) {
            string year = birthday.Split('.')[2];
            string month = birthday.Split('.')[1];
            string day = birthday.Split('.')[0];
            string date = year + '-' + month + '-' + day;
            return date;
        }

        private string RefBirthday(string birthday) {
            string year = birthday.Split('-')[0];
            string month = birthday.Split('-')[1];
            string day = birthday.Split('-')[2];
            string date = year + month + day;
            return date;
        }

        private string GetHash(string input) {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }
    }
}

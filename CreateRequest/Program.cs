using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Xml.Linq;

namespace CreateRequest {
    class Program {

        static FileSystemWatcher watcher1;
        static FileSystemWatcher watcher2;
        
        static string fileName;
        readonly static string path = @"\\192.168.2.205\Ident";
        static void Main(string[] args) {
            while (true) {
                UprmesClass uprmesFile = new UprmesClass();
                uprmesFile.WorkingProgram();
                
                RecordDBResultFileSearch rfs = new RecordDBResultFileSearch();
                rfs.SearchFileWithoutUprak1();
                rfs.SearchFileWithoutUprak2();
                Thread.Sleep(30000);
            }
        }

        // Ожидание *.uprak1 файла
        private static void WaitUpkak1() {
            watcher1 = new FileSystemWatcher {
                Path = @"\\192.168.2.205\Ident",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = fileName.Remove(fileName.Length - 6, 6) + "uprak1"
            };
            watcher1.Created += Watcher1_Created;
            watcher1.EnableRaisingEvents = true;

        }

        // ожидание *.uprak2 файла
        private static void WaitUprak2() {
            watcher2 = new FileSystemWatcher {
                Path = @"\\192.168.2.205\Ident",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = fileName.Remove(fileName.Length - 6, 6) + "uprak2"
            };
            watcher2.Created += Watcher2_Created;
            watcher2.EnableRaisingEvents = true;

        }

        // Парсинг *.uprak1 файла
        private static void Watcher1_Created(object sender, FileSystemEventArgs e) {
            //Thread.Sleep(100);
            //WriteToOkUprak1();
            watcher1.EnableRaisingEvents = false;
        }

        // запись в БД FileXml иформации о том, что uprak1 был получен
        /*private static void WriteToOkUprak1() {
            using (SqlConnection con = new SqlConnection(uprmesFile.connectionString)) {
                using (SqlCommand com = new SqlCommand("UPDATE FileXML SET Uprak1 = @Otv WHERE FileName = @FileName", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@Otv", "OK");
                    com.Parameters.AddWithValue("@FileName", fileName.Remove(fileName.Length - 7, 7));
                    com.ExecuteNonQuery();
                }
            }
        }*/

        

        // парсинг *.uprak2 файла
        private static void Watcher2_Created(object sender, FileSystemEventArgs e) {
            //Thread.Sleep(100);
            //WriteToOkUprak2();
            string filePath = @"\\192.168.2.205\Ident\" + fileName.Remove(fileName.Length - 6, 6) + "uprak2";
            XDocument uprak2 = XDocument.Load(filePath);
            XNamespace xNamespace = XNamespace.Get("urn:hl7-org:v2xml");
            uprak2.Declaration = new XDeclaration("1.0", "Windows-1251", null);
            foreach (XElement rsp in uprak2.Element(xNamespace + "UPRMessageBatch").Elements(xNamespace + "RSP_ZK1")) {
                if (rsp.Element(xNamespace + "RSP_ZK1.QUERY_RESPONSE") != null) {
                    // парсим RSP_ZK1.QUERY_RESPONSE

                } else {
                    // гововорим о том, что информации нет
                }
            }
            //string code = uprak2.Element(xNamespace + "UPRMessageBatch").Element(xNamespace + "RSP_ZK1").Element(xNamespace + "MSA").Element(xNamespace + "MSA.1").Value;
            watcher2.EnableRaisingEvents = false;
        }

        /*private static void WriteToOkUprak2() {
            using (SqlConnection con = new SqlConnection(connectionString)) {
                using (SqlCommand com = new SqlCommand("UPDATE FileXML SET Uprak2 = @Otv WHERE FileName = @FileName", con)) {
                    con.Open();
                    com.Parameters.AddWithValue("@Otv", "OK");
                    com.Parameters.AddWithValue("@FileName", fileName.Remove(fileName.Length - 7, 7));
                    com.ExecuteNonQuery();
                }
            }
        }*/


    }
}

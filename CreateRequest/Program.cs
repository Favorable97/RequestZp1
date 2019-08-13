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

        static void Main(string[] args) {
            WaitUpkak1();
            WaitUprak2();
        }

        // Ожидание *.uprak1 файла
        private static void WaitUpkak1() {
            watcher1 = new FileSystemWatcher {
                Path = @"\\192.168.2.205\Ident",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = uprmesFile.FileName.Remove(uprmesFile.FileName.Length - 6, 6) + "uprak1"
            };
            watcher1.Created += Watcher1_Created;
            watcher1.EnableRaisingEvents = true;

        }

        // ожидание *.uprak2 файла
        private static void WaitUprak2() {
            watcher2 = new FileSystemWatcher {
                Path = @"\\192.168.2.205\Ident",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = uprmesFile.FileName.Remove(uprmesFile.FileName.Length - 6, 6) + "uprak2"
            };
            watcher2.Created += Watcher2_Created;
            watcher2.EnableRaisingEvents = true;

        }

        // Парсинг *.uprak1 файла
        private static void Watcher1_Created(object sender, FileSystemEventArgs e) {
            Thread.Sleep(100);
            string filePath = @"\\192.168.2.205\Ident\" + uprmesFile.FileName.Remove(uprmesFile.FileName.Length - 6, 6) + "uprak1";
            XDocument uprak1 = XDocument.Load(filePath);
            XNamespace xNamespace = XNamespace.Get("urn:hl7-org:v2xml");
            uprak1.Declaration = new XDeclaration("1.0", "Windows-1251", null);
            //XElement upprmes = new XElement(xNamespace + "UPRMessageBatch");
            string code = uprak1.Element(xNamespace + "UPRMessageBatch").Element(xNamespace + "RSP_ZK1").Element(xNamespace + "MSA").Element(xNamespace + "MSA.1").Value;
            watcher1.EnableRaisingEvents = false;
        }

        // парсинг *.uprak2 файла
        private static void Watcher2_Created(object sender, FileSystemEventArgs e) {
            Thread.Sleep(100);
            string filePath = @"\\192.168.2.205\Ident\" + uprmesFile.FileName.Remove(uprmesFile.FileName.Length - 6, 6) + "uprak2";
            XDocument uprak2 = XDocument.Load(filePath);
            XNamespace xNamespace = XNamespace.Get("urn:hl7-org:v2xml");
            uprak2.Declaration = new XDeclaration("1.0", "Windows-1251", null);
            string code = uprak2.Element(xNamespace + "UPRMessageBatch").Element(xNamespace + "RSP_ZK1").Element(xNamespace + "MSA").Element(xNamespace + "MSA.1").Value;

            watcher2.EnableRaisingEvents = false;
            
            
        }
    }
}

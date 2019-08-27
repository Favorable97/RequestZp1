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
    }
}

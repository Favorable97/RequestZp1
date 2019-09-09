using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RequestZp1 {
    static class Program {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        
        static void Main() {
            int prC = 0;
            foreach (Process pr in Process.GetProcesses())
                if (pr.ProcessName == "RequestZp1") prC++;
            if (prC > 1) {
                MessageBox.Show("Приложение уже запущено");
                Process.GetCurrentProcess().Kill();
            } else {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            
        }
    }
}

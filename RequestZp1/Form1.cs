using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RequestZp1 {
    public partial class Form1 : Form {
        public string nameUser, rights;
        public Form1() {
            InitializeComponent();
        }

        public void VisibleProfile() {
            UserName.Text = nameUser;
        }

        private void AddPeople_Click(object sender, EventArgs e) {
            DataGridViewRow newRow = new DataGridViewRow();

            DataGridViewCell FIO = new DataGridViewTextBoxCell();
            FIO.Value = tSurname.Text + " " + tName.Text + " " + tFatherName.Text;

            DataGridViewCell Birthday = new DataGridViewTextBoxCell();
            Birthday.Value = tBirthday.Text;

            newRow.Cells.Add(FIO);
            newRow.Cells.Add(Birthday);

            RequestTable.Rows.Add(newRow);

            tSurname.Clear();
            tName.Clear();
            tFatherName.Clear();
            tBirthday.Clear();
        }

        private void CreateXmlFile_Click(object sender, EventArgs e) {
            UprmesClass uprmesFile = new UprmesClass(RequestTable);
            uprmesFile.CreateXmlFile();
        }

        private void SignOut_Click(object sender, EventArgs e) {
            signInProfile1.Show();
        }

        public void IsAdmin() {
            if (rights != "admin")
                AddUsers.Enabled = false;
        }

        private void AddUsers_Click(object sender, EventArgs e) {
             
        }
    }
}

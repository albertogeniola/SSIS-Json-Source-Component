using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SSISInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            // Initialize main graphic elements
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            versionList.Rows.Clear();
            
            // For each Version of SQL (100,110,120...) enumerated into SQLVersion, get its installation Path
            foreach (int i in Enum.GetValues(typeof(SQLVersion)))
            {
                // Retrive the decription of the instance given its version. This will be printed on the UI.
                string fullDescr = Program.GetSQLServerDescription((SQLVersion)i);
                
                // Check if the machine has this version (x32) installed. 
                string path = null;
                if (Program.GetDTSInstallationPath((SQLVersion)i, out path,Architecture.x32) && path != null)
                {
                    // If so, print it
                    DataGridViewRow row = new DataGridViewRow();
                    row.Tag = (SQLVersion)i;
                    DataGridViewCheckBoxCell cb = new DataGridViewCheckBoxCell();
                    cb.Value = true;
                    cb.ReadOnly = false;
                    DataGridViewTextBoxCell opath = new DataGridViewTextBoxCell();
                    opath.Value = fullDescr + " " + Program.GetArchDescription(Architecture.x32);
                    opath.Tag = path;
                    opath.ToolTipText = path;
                    row.Cells.AddRange(cb, opath);
                    versionList.Rows.Add(row);
                }

                // Check if the machine has this version (x32) installed. 
                if (Program.GetDTSInstallationPath((SQLVersion)i, out path, Architecture.x64) && path != null)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Tag = (SQLVersion)i;
                    DataGridViewCheckBoxCell cb = new DataGridViewCheckBoxCell();
                    cb.Value = true;
                    cb.ReadOnly = false;
                    DataGridViewTextBoxCell opath = new DataGridViewTextBoxCell();
                    opath.Value = fullDescr + " " + Program.GetArchDescription(Architecture.x64);
                    opath.Tag = path;
                    opath.ToolTipText = path;
                    row.Cells.AddRange(cb, opath);
                    versionList.Rows.Add(row);
                }
                
            }
        }

        private void abortBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            Dictionary<SQLVersion, List<string>> toInstall = new Dictionary<SQLVersion, List<string>>();
            foreach (DataGridViewRow row in versionList.Rows)
            {
                if ((bool)row.Cells[0].Value == true)
                {
                    SQLVersion key = (SQLVersion)row.Tag;
                    string val = (string)row.Cells[1].Tag;
                    if (toInstall.ContainsKey(key))
                    { 
                        toInstall[key].Add(val);
                    }
                    else
                    {
                        List<string> list = new List<string>();
                        list.Add(val);
                        toInstall.Add(key,list);
                    }
                }
            }

            if (toInstall.Count == 0)
            {
                MessageBox.Show("No version selected. Please select a version to install first. If no version is available, it means the installer found no SQL server compatible version.");
                return;
            }
            // Esegue l'installazione.
            Program.PerformInstallation(toInstall);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

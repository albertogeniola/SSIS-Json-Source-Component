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
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            // Pulisci la listView da eventuali altri oggetti
            versionList.Rows.Clear();
            // Cicla su tutti gli elementi dell'enumerativo ed aggiungi i rispettivi valori alla lista.
            // Inserisci i soli elementi che possono essere installati
            foreach (int i in Enum.GetValues(typeof(SQLVersion)))
            {
                string txt = Program.GetSQLServerDescription((SQLVersion)i);
                
                // Controlla se trovo il path adatto, in tal caso aggiungi l'elemento alla lista di quelli installabili.
                string path = null;
                if (Program.GetDTSInstallationPath((SQLVersion)i, out path,Architecture.x32) && path != null)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Tag = (SQLVersion)i;
                    DataGridViewCheckBoxCell cb = new DataGridViewCheckBoxCell();
                    cb.Value = true;
                    cb.ReadOnly = false;
                    DataGridViewTextBoxCell opath = new DataGridViewTextBoxCell();
                    opath.Value = txt + " " + Program.GetArchDescription(Architecture.x32);
                    opath.Tag = path;
                    opath.ToolTipText = path;
                    row.Cells.AddRange(cb, opath);
                    versionList.Rows.Add(row);
                }

                if (Program.GetDTSInstallationPath((SQLVersion)i, out path, Architecture.x64) && path != null)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Tag = (SQLVersion)i;
                    DataGridViewCheckBoxCell cb = new DataGridViewCheckBoxCell();
                    cb.Value = true;
                    cb.ReadOnly = false;
                    DataGridViewTextBoxCell opath = new DataGridViewTextBoxCell();
                    opath.Value = txt + " " + Program.GetArchDescription(Architecture.x64);
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

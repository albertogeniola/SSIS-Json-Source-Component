using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace com.webkingsoft.JSONSource_120.CustomControls
{
    public partial class AdvancedView : UserControl
    {
        public AdvancedView()
        {
            InitializeComponent();
        }

        private void tmpBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog b = new FolderBrowserDialog();
            var r = b.ShowDialog();
            if (r == System.Windows.Forms.DialogResult.OK)
            {
                uiTempDir.Text = b.SelectedPath;
            }
        }
    }
}

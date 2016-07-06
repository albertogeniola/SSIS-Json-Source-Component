using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
#if LINQ_SUPPORTED
using System.Linq;
#endif


namespace com.webkingsoft.JSONSource_Common
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

        public void LoadModel(JSONAdvancedSettingsModel advancedSettings)
        {
            if (advancedSettings.CustomLocalTempDir != null)
            {
                uiTempDir.Text = advancedSettings.CustomLocalTempDir;
                cbParseJsonDate.Checked = advancedSettings.ParseDates;
            }
        }

        public JSONAdvancedSettingsModel SaveToModel()
        {
            JSONAdvancedSettingsModel result = new JSONAdvancedSettingsModel();

            if (!string.IsNullOrEmpty(uiTempDir.Text))
                result.CustomLocalTempDir = uiTempDir.Text;
            else
                result.CustomLocalTempDir = null;

            result.ParseDates = cbParseJsonDate.Checked;

            return result;
        }
    }
}

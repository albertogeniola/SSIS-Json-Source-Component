using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#if LINQ_SUPPORTED
using System.Linq;
#endif


namespace com.webkingsoft.JSONSource_Common
{
    public partial class SourceAdvancedUI : Form
    {
        public static readonly string DONATE_URL = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=6HPAB89UYSZF2";
        public static readonly string SUPPORT_URL = "https://jsonsource.codeplex.com/";
        public static readonly string LI_PROFILE = "https://www.linkedin.com/in/albertogeniola";

        private SourceView _sourceView;
        private Variables _vars;
        private IServiceProvider _sp;
        private ColumnView _columnView;
        private AdvancedView _advancedView;
        private JSONSourceComponentModel _savedModel;
        private IDTSVirtualInputColumnCollection100 _inputs;
        private int _metadataVersion;
        private int _attributeVersion;

        public SourceAdvancedUI(Variables vars,IServiceProvider sp, IDTSVirtualInputColumnCollection100 virtualInputs, int metadataVersion=0, int attributeVersion = 0)
        {
            _vars = vars;
            _sp = sp;
            _inputs = virtualInputs;

            _sourceView = new SourceView(_vars,_sp, _inputs);
            _sourceView.Visible = false;

            _columnView = new ColumnView(_inputs);
            _columnView.Visible = false;

            _advancedView = new AdvancedView();
            _advancedView.Visible = false;

            InitializeComponent();
            mainPanel.Controls.Add(_sourceView);
            _sourceView.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(_columnView);
            _columnView.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(_advancedView);
            _advancedView.Dock = DockStyle.Fill;
            menulist.SelectedIndex = 0;

            
            _metadataVersion = metadataVersion;
            _attributeVersion = attributeVersion;

            labelComponentVersion.Text = ""+_attributeVersion;
            labelMetadataVersion.Text = ""+_metadataVersion;
        }

        public void LoadModel(JSONSourceComponentModel m)
        {
            // Given a model, load it into the whole UI
            _sourceView.LoadModel(m.DataSource);
            _columnView.LoadModel(m.DataMapping);
            _advancedView.LoadModel(m.AdvancedSettings);
        }

        private void menulist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (menulist.SelectedIndex == -1)
                return;
            
            string sel = menulist.SelectedItem.ToString();

            _sourceView.Visible = sel == "Settings";
            _columnView.Visible = sel == "Columns";
            _advancedView.Visible = sel == "Advanced";
        }

        public JSONSourceComponentModel SavedModel {
            get { return _savedModel; }
        }

        /// <summary>
        /// This method will collect all the info of the view and will save it into a variable that can be publicly accessed.
        /// </summary>
        private void SaveModel() {
            // Rely on each specified view to do so.
            JSONSourceComponentModel result = new JSONSourceComponentModel();
            result.AdvancedSettings = _advancedView.SaveToModel();
            result.DataMapping = _columnView.SaveToModel();
            result.DataSource = _sourceView.SaveToModel();

            _savedModel = result;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            try
            {
                // Save and return success.
                SaveModel();
                DialogResult = DialogResult.OK;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + ex.Message);
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void testButton_Click(object sender, EventArgs e)
        {
            SaveModel();
            var tester = new ComponentTester(_savedModel);
            tester.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(DONATE_URL);
        }

        private void help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(SUPPORT_URL);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(DONATE_URL);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(LI_PROFILE);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(LI_PROFILE);
        }
    }
}

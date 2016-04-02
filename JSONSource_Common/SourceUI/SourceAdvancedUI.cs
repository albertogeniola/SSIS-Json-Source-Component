using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
#if LINQ_SUPPORTED
using System.Linq;
#endif


namespace com.webkingsoft.JSONSource_Common
{
    
    public partial class SourceAdvancedUI : Form
    {
        private SourceView _sourceView;
        private Variables _vars;
        private IServiceProvider _sp;
        private ColumnView _columnView;
        private AdvancedView _advancedView;
        private JSONSourceComponentModel _savedModel;
        private IDTSComponentMetaData100 _md;

        public SourceAdvancedUI(Variables vars,IServiceProvider sp, IDTSComponentMetaData100 md)
        {
            _vars = vars;
            _sp = sp;
            _md = md;
            
            _sourceView = new SourceView(_vars,_sp,_md);
            _sourceView.Visible = false;

            _columnView = new ColumnView();
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
    }
}

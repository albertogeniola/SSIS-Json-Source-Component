using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Design;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
#if LINQ_SUPPORTED
using System.Linq;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    public partial class SourceView : UserControl
    {
        private Variables _vars;
        private IServiceProvider _sp;
        private JSONSourceComponentModel _model;
        private IDTSComponentMetaData100 _md;

        public SourceView(Variables vars, IServiceProvider sp, IDTSComponentMetaData100 md)
        {
            _sp = sp;
            _vars = vars;
            _md = md;

            _tmpParams = new List<HTTPParameter>();
            InitializeComponent();
        }
        
        public string GetHTTPMethod() {
            if (getRadio.Checked)
                return "GET";
            else if (postRadio.Checked)
                return "POST";
            else if (putRadio.Checked)
                return "PUT";
            else if (delRadio.Checked)
                return "DELETE";
            else
                throw new ArgumentException("Invalid method selection!");
        }

        public SourceType GetSourceType() {
            if (directInputR.Checked)
                return SourceType.WebUrlPath;
            else if (variableR.Checked)
                return SourceType.WebUrlVariable;
            else throw new ApplicationException("INVALID SOURCE TYPE");
        }

        private void direct_variable_check_change(object sender, EventArgs e)
        {
            addButton.Visible = variableR.Checked;
            addButton.Enabled = variableR.Checked;
            uiWebURL.ReadOnly = variableR.Checked;
            uiWebURL.Text = "";
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            // If variable is selected, open the variable browser, otherwise open the file browser
            if (variableR.Checked)
            {
                VariableChooser vc = new VariableChooser(_vars, new TypeCode[] { TypeCode.String }, null);
                DialogResult dr = vc.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Microsoft.SqlServer.Dts.Runtime.Variable v = vc.GetResult();
                    uiWebURL.Text = v.QualifiedName;
                }
            }
            else {
                var res = openFileDialog1.ShowDialog(this);
                if (res == DialogResult.OK) {
                    // Update the textedit with the file path
                    uiWebURL.Text = openFileDialog1.FileName;
                }
            }
        }

        /// <summary>
        /// Returns the current view configuration into a model object
        /// </summary>
        /// <returns></returns>
        public JSONDataSourceModel SaveToModel()
        {
            JSONDataSourceModel result = new JSONDataSourceModel();

            if (variableR.Checked)
            {
                result.FromVariable = true;
                result.VariableName = uiWebURL.Text;
            }
            else {
                result.FromVariable = false;
                result.SourceUri = new Uri(uiWebURL.Text);
            }
            
            if (getRadio.Checked)
                result.WebMethod = "GET";
            else if (postRadio.Checked)
                result.WebMethod = "POST";
            else if (putRadio.Checked)
                result.WebMethod = "PUT";
            else if (delRadio.Checked)
                result.WebMethod = "DELETE";

            result.HttpParameters = GetHttpParameters();
            result.CookieVariable = String.IsNullOrEmpty(cookieVarTb.Text) ? null : cookieVarTb.Text;

            return result;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            VariableChooser vc = new VariableChooser(_vars, new TypeCode[] { TypeCode.Object }, null);
            DialogResult dr = vc.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Microsoft.SqlServer.Dts.Runtime.Variable v = vc.GetResult();
                if (v.DataType != TypeCode.Object)
                {
                    MessageBox.Show("The cookie variable MUST be of type \"Object\".");
                    return;
                }
                cookieVarTb.Text = v.QualifiedName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IDtsVariableService vservice = (IDtsVariableService)_sp.GetService(typeof(IDtsVariableService));
            Microsoft.SqlServer.Dts.Runtime.Variable vv = vservice.PromptAndCreateVariable(this, null, "COOKIES_" + this.Name, "User", typeof(object));
            if (vv != null)
            {
                cookieVarTb.Text = vv.QualifiedName;
            }
        }

        private IEnumerable<HTTPParameter> _tmpParams = null;
        private void httpparams_Click(object sender, EventArgs e)
        {
            // Collect the variables currently available to the component alongside the input colums available at the moment
            HttpInputBinding[] inputs = null;
            var inputlane = _md.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS];
            if (inputlane.IsAttached) {
                inputs = new HttpInputBinding[inputlane.GetVirtualInput().VirtualInputColumnCollection.Count];
                int count = 0;
                foreach (IDTSVirtualInputColumn100 vcol in inputlane.GetVirtualInput().VirtualInputColumnCollection)
                {
                    inputs[count] = new HttpInputBinding();
                    inputs[count].Name = vcol.Name;
                    inputs[count].LineageID = vcol.LineageID;
                    count++;
                }
            }

            Parameters p = new Parameters(_vars, inputs);
            p.SetModel(_tmpParams);
            var res = p.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                _tmpParams = p.GetModel();
            }
        }

        public IEnumerable<HTTPParameter> GetHttpParameters() {
            return _tmpParams;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (variableR.Checked)
            {
                IDtsVariableService vservice = (IDtsVariableService)_sp.GetService(typeof(IDtsVariableService));
                Microsoft.SqlServer.Dts.Runtime.Variable vv = vservice.PromptAndCreateVariable(this, null, null, "User", typeof(string));
                if (vv != null)
                    uiWebURL.Text = vv.QualifiedName;
            }
        }

        public void LoadModel(JSONDataSourceModel m)
        {
            // Keep all the http parameters locally, we will need them in order to provide the custom HTTP parameter dialog
            _tmpParams = m.HttpParameters;

            // Fill in the rest of the view using model data
            variableR.Checked = m.FromVariable; // TODO: Make sure this puts the edit text as readonly
            if (m.FromVariable)
            {
                uiWebURL.Text = m.VariableName;
            }
            else {
                if (m.SourceUri != null)
                    uiWebURL.Text = m.SourceUri.ToString();
                else
                    uiWebURL.Text = "";
            }
            switch (m.WebMethod) {
                case "GET":
                    getRadio.Checked = true;
                    break;
                case "POST":
                    postRadio.Checked = true;
                    break;
                case "PUT":
                    putRadio.Checked = true;
                    break;
                case "DELETE":
                    delRadio.Checked = true;
                    break;
                default:
                    getRadio.Checked = true;
                    break;
            }

            if (String.IsNullOrEmpty(m.CookieVariable))
                cookieVarTb.Text = "";
            else
                cookieVarTb.Text = m.CookieVariable;

        }
    }
}

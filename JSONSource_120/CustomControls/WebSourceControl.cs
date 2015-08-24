using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Design;
using JSONSource.webkingsoft.JSONSource_120;

namespace com.webkingsoft.JSONSource_120.CustomControls
{
    public partial class WebSourceControl : UserControl
    {
        private Variables _vars;
        private IServiceProvider _sp;
        private SourceModel _model;

        public WebSourceControl(Variables vars, IServiceProvider sp)
        {
            _sp = sp;
            _vars = vars;
            _tmpParams = new List<HTTPParameter>();

            InitializeComponent();
        }

        private void directInputR_CheckedChanged(object sender, EventArgs e)
        {
            browseButton.Visible = variableR.Checked;
            browseButton.Enabled = variableR.Checked;
            addButton.Visible = variableR.Checked;
            addButton.Enabled = variableR.Checked;
            uiWebURL.ReadOnly = variableR.Checked;
            uiWebURL.Text = "";
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

        private void variableR_CheckedChanged(object sender, EventArgs e)
        {
            browseButton.Visible = variableR.Checked;
            browseButton.Enabled = variableR.Checked;
            addButton.Visible = variableR.Checked;
            addButton.Enabled = variableR.Checked;
            uiWebURL.ReadOnly = variableR.Checked;
            uiWebURL.Text = "";
            
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            VariableChooser vc = new VariableChooser(_vars,new TypeCode[]{TypeCode.String},null);
            DialogResult dr = vc.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Microsoft.SqlServer.Dts.Runtime.Variable v = vc.GetResult();
                uiWebURL.Text = v.QualifiedName;
            }
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
            Parameters p = new Parameters(_vars);
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
            IDtsVariableService vservice = (IDtsVariableService)_sp.GetService(typeof(IDtsVariableService));
            Microsoft.SqlServer.Dts.Runtime.Variable vv = vservice.PromptAndCreateVariable(this, null, null, "User", typeof(string));
            if (vv != null)
                uiWebURL.Text = vv.QualifiedName;
        }

        public void LoadModel(SourceModel m)
        {
            _model = m;
            _tmpParams = m.HttpParameters;

            // Web URL
            if (m.SourceType == SourceType.WebUrlVariable)
            {
                variableR.Checked = true;
                uiWebURL.Text = m.WebUrlVariable;
            }
            if (m.SourceType == SourceType.WebUrlPath)
            {
                directInputR.Checked = true;   
                uiWebURL.Text = m.WebUrl == null ? "" : m.WebUrl.ToString();
            }

            if (String.IsNullOrEmpty(m.CookieVariable))
                cookieVarTb.Text = "";
            else
                cookieVarTb.Text = m.CookieVariable;

        }
    }
}

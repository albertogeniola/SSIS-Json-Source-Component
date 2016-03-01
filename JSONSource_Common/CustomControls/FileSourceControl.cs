using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Runtime;
#if LINQ_SUPPORTED
using System.Linq;
#endif


namespace com.webkingsoft.JSONSource_Common
{
    public partial class FileSourceControl : UserControl
    {
        private Variables _vars;

        public FileSourceControl(Variables vars)
        {
            _vars = vars;
            InitializeComponent();
            directInputR.Checked = true;
        }

        public SourceType GetSourceType()
        {
            if (directInputR.Checked)
                return SourceType.FilePath;
            else if (variableR.Checked)
                return SourceType.FilePathVariable;
            else throw new ApplicationException("INVALID SOURCE TYPE");
        }

        private void directInputR_CheckedChanged(object sender, EventArgs e)
        {
            uiBrowseFilePath.Visible = directInputR.Checked;
            uiBrowseFilePath.Enabled = directInputR.Checked;
            uiBrowseFilePathVariable.Visible = variableR.Checked;
            uiBrowseFilePathVariable.Enabled = variableR.Checked;
            addVarByutton.Enabled = variableR.Checked;
            addVarByutton.Visible = variableR.Checked;
            jsonFilePath.Text = "";
            jsonFilePath.ReadOnly = variableR.Checked;
        }

        private void variableR_CheckedChanged(object sender, EventArgs e)
        {
            uiBrowseFilePath.Visible = directInputR.Checked;
            uiBrowseFilePath.Enabled = directInputR.Checked;
            uiBrowseFilePathVariable.Visible = variableR.Checked;
            uiBrowseFilePathVariable.Enabled = variableR.Checked;
            addVarByutton.Enabled = variableR.Checked;
            addVarByutton.Visible = variableR.Checked;
            jsonFilePath.Text = "";
            jsonFilePath.ReadOnly = variableR.Checked;
        }

        private void uiBrowseFilePathVariable_Click(object sender, EventArgs e)
        {
            VariableChooser vc = new VariableChooser(_vars, new TypeCode[]{TypeCode.String},null);
            DialogResult dr = vc.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Microsoft.SqlServer.Dts.Runtime.Variable v = vc.GetResult();
                jsonFilePath.Text = v.QualifiedName;
            }
        }

        public void LoadModel(SourceModel m)
        {
            // File Path
            if (m.SourceType == SourceType.FilePath)
            {
                directInputR.Checked = true;
                jsonFilePath.Text = m.FilePath;
            }
            else if (m.SourceType == SourceType.FilePathVariable)
            {
                variableR.Checked = true;
                jsonFilePath.Text = m.FilePathVar;
            }
        }

        private void uiBrowseFilePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog b = new FolderBrowserDialog();
            var r = b.ShowDialog();
            if (r == System.Windows.Forms.DialogResult.OK)
            {
                jsonFilePath.Text = b.SelectedPath;
            }
        }
    }
}

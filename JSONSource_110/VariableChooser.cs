using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.webkingsoft.JSONSource_110
{
    public partial class VariableChooser : Form
    {
        private Variables _vars;
        private Variable _result;


        public VariableChooser(Variables vars)
        {
            _vars = vars;
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            varList.Items.Clear();

            foreach (Variable v in _vars)
            {
                if (v.DataType == TypeCode.String)
                {
                    varList.Items.Add(v.QualifiedName);
                }
            }

            if (varList.Items.Count > 0)
                varList.SelectedIndex = 0;
        }

        private void VariableChooser_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _result = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (varList.SelectedIndex != -1)
            {
                string cur = varList.Items[varList.SelectedIndex].ToString();
                foreach (Variable v in _vars)
                    if (v.QualifiedName == cur)
                    {
                        _result = v;
                        DialogResult = DialogResult.OK;
                        return;
                    }
            }

            DialogResult = DialogResult.Cancel;
            _result = null;

        }

        public Variable GetResult()
        {
            return _result;
        }
    }
}

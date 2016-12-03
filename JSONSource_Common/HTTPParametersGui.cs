using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
#if LINQ_SUPPORTED
using System.Linq;
#endif


namespace com.webkingsoft.JSONSource_Common
{
    public partial class HTTPParametersGui : Form
    {
        private List<HTTPParameter> _model = new List<HTTPParameter>();
        private Microsoft.SqlServer.Dts.Runtime.Variables _vars;
        private string[] _input_options;

        public HTTPParametersGui(Microsoft.SqlServer.Dts.Runtime.Variables vars, string[] inputHttpCols = null)
        {
            _vars = vars;
            InitializeComponent();
            bindingType.DataSource = Enum.GetNames(typeof(ParamBinding));
            dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.RowValidating += dataGridView1_RowValidating;
            _input_options = inputHttpCols; // May be null and it's ok.
            _model = new List<HTTPParameter>();
            
        }

        void dataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Convalida la riga.
            DataGridView d = (DataGridView)sender;
            DataGridViewCell name = d.Rows[e.RowIndex].Cells[0];
            DataGridViewCell binding = d.Rows[e.RowIndex].Cells[1];
            DataGridViewCell value = d.Rows[e.RowIndex].Cells[2];
            DataGridViewCell encode = d.Rows[e.RowIndex].Cells[3];

            // 1. Nome del parametro non nullo
            if (name.Value==null || String.IsNullOrEmpty(name.Value.ToString().Trim()))
            {
                d.Rows[e.RowIndex].Cells[0].ErrorText= "Parameter name cannot be null or empty.";
                e.Cancel = true;
                return;
            }

            // Controlla che il nome del parametro sia univoco
            foreach (DataGridViewRow r in d.Rows) {
                if (r.Cells[0].Value == null)
                    // E' una riga appena creata, skip!
                    continue;
                if (r.Cells[0].Value.ToString().Trim() == name.Value.ToString().Trim() && !Object.ReferenceEquals(r,d.Rows[e.RowIndex])) { 
                    // Duplicato!
                    d.Rows[e.RowIndex].Cells[0].ErrorText = "Duplicate Parameter name detected.";
                    e.Cancel = true;
                    return;
                }
            }

            // 2. Tipo di binding
            ParamBinding bin;
            if (!Enum.TryParse<ParamBinding>(binding.Value.ToString(), out bin)) {
                d.Rows[e.RowIndex].Cells[1].ErrorText = "Invalid binding option specified.";
                e.Cancel = true;
                return;
            }

            // 3. Valore: se è di tipo bound, controlla che la variabile specificata esista. Se no, accetta tutto. Per noi un parametro HTTP può anche essere nullo.
            if (bin == ParamBinding.Variable)
            {
                string var = value.Value.ToString().Trim();
                bool valid = false;
                foreach (Variable v in _vars)
                {
                    if (v.QualifiedName == var)
                    {
                        valid = true;
                        break;
                    }
                }
                if (!valid)
                {
                    d.Rows[e.RowIndex].Cells[2].ErrorText = "Invalid variable choosen";
                    e.Cancel = true;
                    return;
                }
            }
            else if (bin == ParamBinding.InputField) {
                if (value.Value == null)
                {
                    d.Rows[e.RowIndex].Cells[2].ErrorText = "Invalid input choosen";
                    e.Cancel = true;
                    return;
                }

                string colname = (string)value.Value;
                if (!_input_options.Contains(colname))
                {
                    d.Rows[e.RowIndex].Cells[2].ErrorText = "Invalid input choosen";
                    e.Cancel = true;
                    return;
                }
            }
        }
        
        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // If the binding type has changed, change the column relative to its possible value.
            // If binding type is either variable or custom, just leave it as textbox, otherwise keep it as combobox
            if (e.ColumnIndex == 1) {
                DataGridView d = (DataGridView)sender;
                ParamBinding b = (ParamBinding)Enum.Parse(typeof(ParamBinding), d.Rows[e.RowIndex].Cells[1].Value.ToString());

                if (b == ParamBinding.Variable || b == ParamBinding.CustomValue)
                {
                    d.Rows[e.RowIndex].Cells[2].Value = null;
                    d.Rows[e.RowIndex].Cells[2] = new DataGridViewTextBoxCell();

                } else if (b == ParamBinding.InputField) {
                    d.Rows[e.RowIndex].Cells[2].ValueType = typeof(ParamBinding);
                    var cbox = new DataGridViewComboBoxCell();
                    cbox.DataSource = _input_options;
                    d.Rows[e.RowIndex].Cells[2] = cbox;

                } else {
                    MessageBox.Show("Invalid or inconsistent HTTP binding type");
                    return;
                }
            }
        }

        void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView d = (DataGridView)sender;
            // Se si sta per modificare il valore del parametro...
            if (e.ColumnIndex == 2) {
                ParamBinding b = (ParamBinding)Enum.Parse(typeof(ParamBinding), d.Rows[e.RowIndex].Cells[1].Value.ToString());
                // Se la riga corrente si riferisce ad un parametro variable-bound, fai in modo che si apra il popup della scelta delle variabili.
                if (b == ParamBinding.Variable)
                {
                    // Mostra il dialog di scelta delle variabili
                    VariableChooser vc = new VariableChooser(_vars);
                    DialogResult dr = vc.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        Microsoft.SqlServer.Dts.Runtime.Variable v = vc.GetResult();
                        d.Rows[e.RowIndex].Cells[2].Value = v.QualifiedName;
                        e.Cancel = true;
                        //d.EndEdit();
                    }
                }
                else if (b == ParamBinding.InputField) {
                    if (_input_options == null || _input_options.Length < 1)
                    {
                        MessageBox.Show("There is no input attached to this lane. First attach an input, then you'll be able to select a vale from this box.");
                        e.Cancel = true;
                    }
                }
            }
        }

        public IEnumerable<HTTPParameter> GetModel() {
            return _model;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int r = dataGridView1.Rows.Add();
            dataGridView1.Rows[r].Cells[0].Value = null;
            dataGridView1.Rows[r].Cells[1].Value = Enum.GetName(typeof(ParamBinding),ParamBinding.CustomValue);
            dataGridView1.Rows[r].Cells[2].Value = null;
            dataGridView1.Rows[r].Cells[3].Value = false;
            dataGridView1.CurrentCell = dataGridView1.Rows[r].Cells[0];
            dataGridView1.BeginEdit(true);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Prepare model
            _model.Clear();
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                HTTPParameter p = new HTTPParameter();
                p.Name = row.Cells[0].Value.ToString().Trim();
                p.Binding = (ParamBinding) Enum.Parse(typeof(ParamBinding), row.Cells[1].Value.ToString().Trim());

                if (p.Binding == ParamBinding.InputField) {
                    p.BindingValue = (string)row.Cells[2].Value;
                } else
                {
                    p.BindingValue= row.Cells[2].Value.ToString().Trim();
                }
                
                p.Encode = (bool)row.Cells[3].Value;
                _model.Add(p);
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public void SetModel(IEnumerable<HTTPParameter> pars) {
            _model.Clear();
            dataGridView1.Rows.Clear();
            if (pars!=null)
                foreach (HTTPParameter p in pars) {
                    if (p.Binding == ParamBinding.InputField)
                    {
                        string bind = null;
                        // Check if the input column is available
                        if (_input_options == null)
                            // Column might have been deleted
                            bind = null;
                        else {
                            if (_input_options.Contains(p.BindingValue))
                                bind = p.BindingValue;
                        }

                        if (bind == null)
                        {
                            //TODO: firewarning?
                        }

                        int index = dataGridView1.Rows.Add();
                        var cbox = new DataGridViewComboBoxCell();
                        cbox.DataSource = _input_options;
                                                
                        dataGridView1.Rows[index].Cells[2] = cbox;

                        dataGridView1.Rows[index].Cells[0].Value = p.Name;
                        dataGridView1.Rows[index].Cells[1].Value = Enum.GetName(typeof(ParamBinding), p.Binding);
                        if (bind != null)
                            dataGridView1.Rows[index].Cells[2].Value = bind;
                        else
                            dataGridView1.Rows[index].Cells[2].ErrorText = "Inputs have changed. Please update this mapping.";

                        dataGridView1.Rows[index].Cells[3].Value = p.Encode;
                    }
                    else {
                        dataGridView1.Rows.Add(new object[] { p.Name, Enum.GetName(typeof(ParamBinding), p.Binding), p.BindingValue, p.Encode });
                    }
                    _model.Add(p);
                }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView v = (DataGridView)sender;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //dataGridView1.EndEdit()
            foreach (DataGridViewRow r in dataGridView1.SelectedRows) {
                dataGridView1.Rows.Remove(r);
            }
            /*
            int r = dataGridView1.Rows.Add();
            dataGridView1.Rows[r].Cells[0].Value = null;
            dataGridView1.Rows[r].Cells[1].Value = Enum.GetName(typeof(HTTPParamBinding), HTTPParamBinding.CustomValue);
            dataGridView1.Rows[r].Cells[2].Value = null;
            dataGridView1.Rows[r].Cells[3].Value = false;
            dataGridView1.CurrentCell = dataGridView1.Rows[r].Cells[0];
            dataGridView1.BeginEdit(true);
            */
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}

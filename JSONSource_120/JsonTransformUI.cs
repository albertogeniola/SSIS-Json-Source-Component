using JSONSource.webkingsoft.JSONSource_120;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Design;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.webkingsoft.JSONSource_120
{
    public partial class JsonTransformUI : Form
    {
        private Variables _vars;
        private TransformationModel _model;
        private IServiceProvider _sp;
        private System.Windows.Forms.IWin32Window _parent;
        public JsonTransformUI(System.Windows.Forms.IWin32Window parent, Variables vars, TransformationModel model, IServiceProvider sp)
        {
            // Salva i riferimenti in locale
            _parent = parent;
            _vars = vars;
            _model = model;
            _sp = sp;
            
            // Inizializza la UI
            InitializeComponent();

            // Imposta i vari Enumerativi previsti come tipi di dato.
            (uiIOGrid.Columns["OutColumnType"] as DataGridViewComboBoxColumn).DataSource = Enum.GetNames(typeof(JsonTypes));

            // Registra l'handler per il settaggio dei valori di default
            uiIOGrid.DefaultValuesNeeded += uiIOGrid_DefaultValuesNeeded;

            // Carico il model
            LoadModel(_model);
        }

        private void uiIOGrid_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["OutColumnType"].Value = Enum.GetName(typeof(JsonTypes), JsonTypes.String);
        }

        /*
         * Carico il modello all'interno della view
         */
        private void LoadModel(TransformationModel m)
        {
            inutColumnCb.Items.Clear();
            foreach (var el in m.Inputs) {
                inutColumnCb.Items.Add(el.Key);
            }

            if (!String.IsNullOrEmpty(m.InputColumnName))
                inutColumnCb.SelectedItem = m.InputColumnName;

            if (m.JsonObjectRelativePath == null)
                uiPathToArray.Text = "";
            else
                uiPathToArray.Text = m.JsonObjectRelativePath;
            if (m.CustomLocalTempDir == null)
                uiTempDir.Text = "";
            else
                uiTempDir.Text = m.CustomLocalTempDir;

            // Tab IO
            if (m.IoMap != null)
                LoadIO(m.IoMap);
            else
                uiIOGrid.Rows.Clear();
        }

        /*
         * Carica la configurazione di IO nel datagrid della view.
         */
        private void LoadIO(IEnumerable<IOMapEntry> ios)
        {
            uiIOGrid.Rows.Clear();
            foreach(IOMapEntry e in ios)
            {
                int index = uiIOGrid.Rows.Add();
                uiIOGrid.Rows[index].Cells[0].Value = e.InputFieldPath;
                uiIOGrid.Rows[index].Cells[1].Value = e.InputFieldLen;
                uiIOGrid.Rows[index].Cells[2].Value = e.OutputColName;
                uiIOGrid.Rows[index].Cells[3].Value = Enum.GetName(typeof(JsonTypes),e.OutputJsonColumnType);
            }
        }


        private void ok_Click(object sender, EventArgs e)
        {
            try {
                // Salva tutti i dettagli nel model

                if (inutColumnCb.SelectedIndex == -1)
                {
                    MessageBox.Show("No input column has been selected. If none is available, please attach an input with textual columns to this component.");
                    return;
                }
                else {
                    _model.InputColumnName = inutColumnCb.SelectedItem.ToString();
                }

                _model.ClearMapping();
                // - Salva le impostazioni di IO
                if (uiIOGrid.IsCurrentCellDirty || uiIOGrid.IsCurrentRowDirty)
                {
                    uiIOGrid.CurrentRow.DataGridView.EndEdit();
                    uiIOGrid.EndEdit();
                    CurrencyManager cm = (CurrencyManager)uiIOGrid.BindingContext[uiIOGrid.DataSource, uiIOGrid.DataMember];
                    cm.EndCurrentEdit();
                }

                int row = 1;
                foreach (DataGridViewRow r in uiIOGrid.Rows)
                {
                    if (r.IsNewRow)
                        continue;
                    string inputName = null;
                    try {
                        inputName = (string)r.Cells[0].Value;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("JSON Field Name on row "+row);
                        return;
                    }
                    int maxLen = -1;
                    try
                    {
                        maxLen = int.Parse((string)r.Cells[1].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Maximum length is invalid on row " + row);
                        return;
                    }

                    string outName = null;
                    try
                    {
                        outName = (string)r.Cells[2].Value;
                        if (string.IsNullOrEmpty(outName))
                            throw new ArgumentException();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Output Column name is invalid on row " + row);
                        return;
                    }

                    JsonTypes dataType = 0;
                    try
                    {
                        dataType = (JsonTypes)Enum.Parse(typeof(JsonTypes), (string)r.Cells[3].Value);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Column type is invalid on row " + row);
                        return;
                    }

                    IOMapEntry map = new IOMapEntry();
                    map.InputFieldPath = inputName;
                    map.OutputColName = outName;
                    map.OutputJsonColumnType = dataType;
                    map.InputFieldLen = maxLen;

                    _model.AddMapping(map);
                    row++;
                }

                // - Salava le impostazioni avanzate
                if (!string.IsNullOrEmpty(uiTempDir.Text))
                    _model.CustomLocalTempDir = uiTempDir.Text;
                else
                    _model.CustomLocalTempDir = null;
                if (!string.IsNullOrEmpty(uiPathToArray.Text))
                    _model.JsonObjectRelativePath = uiPathToArray.Text;
                else
                    _model.JsonObjectRelativePath = null;

                DialogResult = DialogResult.OK;

                Close();

            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid number (max length) specified. Please fix it.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + ex.Message);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        
        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void uiIOGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Adatta il campo lunghezza in base alla tipologia di dato scelta.
            if (e.ColumnIndex == 3)
            {
                var row = uiIOGrid.Rows[e.RowIndex];
                var type = (JsonTypes)Enum.Parse(typeof(JsonTypes), row.Cells[3].Value.ToString());
                switch (type)
                {
                    case JsonTypes.Boolean:
                        row.Cells[1].Value = 0;
                        row.Cells[1].ReadOnly = true;
                        break;
                    case JsonTypes.Number:
                        row.Cells[1].Value = 0;
                        row.Cells[1].ReadOnly = true;
                        break;
                    case JsonTypes.String:
                        row.Cells[1].Value = 255;
                        row.Cells[1].ReadOnly = false;
                        break;
                    case JsonTypes.RawJson:
                        row.Cells[1].Value = 1024;
                        row.Cells[1].ReadOnly = false;
                        break;
                }
            }
        }
    }
}

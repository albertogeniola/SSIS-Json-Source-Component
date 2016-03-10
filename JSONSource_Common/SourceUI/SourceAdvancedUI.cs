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
    
    public partial class SourceAdvancedUI : Form
    {
        private SourceControl _sourceView;
        private Variables _vars;
        private IServiceProvider _sp;
        private SourceModel _model;
        private ColumnView _columnView;
        private AdvancedView _advancedView;

        public SourceAdvancedUI(Variables vars,IServiceProvider sp)
        {
            _vars = vars;
            _sp = sp;
            
            _sourceView = new SourceControl(_vars,_sp);
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

            _model = new SourceModel();
        }

        public void LoadModel(SourceModel m)
        {
            _model = m;

            // Tipo di sorgente
            _sourceView.LoadModel(m);
            _columnView.LoadModel(m);

            // Advanced Tab
            if (m.CustomLocalTempDir == null)
                _advancedView.uiTempDir.Text = "";
            else
                _advancedView.uiTempDir.Text = m.CustomLocalTempDir;
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

        private void ok_Click(object sender, EventArgs e)
        {
            try
            {
                // Salva tutti i dettagli nel model
                // setta il model nelle properties del componente.
                // Evito la validazione dalla view, verrà fatta dal component direttamente.

                // - Salva le informazioni riguardanti la sorgente dei dati
                _model.SourceType = _sourceView.GetSourceType();
                _model.WebUrl = _model.SourceType == SourceType.WebUrlPath ? new Uri(_sourceView._webUriView.uiWebURL.Text) : null;
                _model.WebUrlVariable = _model.SourceType == SourceType.WebUrlVariable ? _sourceView._webUriView.uiWebURL.Text : null;
                _model.FilePath = _model.SourceType == SourceType.FilePath ? _sourceView._filePathView.jsonFilePath.Text : null;
                _model.FilePathVar = _model.SourceType == SourceType.FilePathVariable ? _sourceView._filePathView.jsonFilePath.Text : null;


                if (_sourceView._webUriView.getRadio.Checked)
                    _model.WebMethod = "GET";
                else if (_sourceView._webUriView.postRadio.Checked)
                    _model.WebMethod = "POST";
                else if (_sourceView._webUriView.putRadio.Checked)
                    _model.WebMethod = "PUT";
                else if (_sourceView._webUriView.delRadio.Checked)
                    _model.WebMethod = "DELETE";

                _model.HttpParameters = _sourceView._webUriView.GetHttpParameters();

                _model.CookieVariable = String.IsNullOrEmpty(_sourceView._webUriView.cookieVarTb.Text) ? null : _sourceView._webUriView.cookieVarTb.Text;

                _model.ClearMapping();
                // - Salva le impostazioni di IO
                if (_columnView.uiIOGrid.IsCurrentCellDirty || _columnView.uiIOGrid.IsCurrentRowDirty)
                {
                    _columnView.uiIOGrid.CurrentRow.DataGridView.EndEdit();
                    _columnView.uiIOGrid.EndEdit();
                    CurrencyManager cm = (CurrencyManager)_columnView.uiIOGrid.BindingContext[_columnView.uiIOGrid.DataSource, _columnView.uiIOGrid.DataMember];
                    cm.EndCurrentEdit();
                }

                RootType root;
                Enum.TryParse<RootType>(_columnView.uiRootType.SelectedValue.ToString(), out root);

                _model.RootType = root;

                int row = 1;
                foreach (DataGridViewRow r in _columnView.uiIOGrid.Rows)
                {
                    if (r.IsNewRow)
                        continue;
                    string inputName = null;
                    try
                    {
                        inputName = (string)r.Cells[0].Value;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("JSON Field Name on row " + row);
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
                if (!string.IsNullOrEmpty(_advancedView.uiTempDir.Text))
                    _model.CustomLocalTempDir = _advancedView.uiTempDir.Text;
                else
                    _model.CustomLocalTempDir = null;
                if (!string.IsNullOrEmpty(_columnView.uiPathToArray.Text))
                    _model.JsonObjectRelativePath = _columnView.uiPathToArray.Text;
                else
                    _model.JsonObjectRelativePath = null;

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

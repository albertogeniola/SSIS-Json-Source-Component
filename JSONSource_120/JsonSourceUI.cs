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
    public partial class JsonSourceUI : Form
    {
        private Variables _vars;
        private SourceModel _model;
        private IServiceProvider _sp;
        private System.Windows.Forms.IWin32Window _parent;
        public JsonSourceUI(System.Windows.Forms.IWin32Window parent, Variables vars, SourceModel model,IServiceProvider sp)
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
            uiSourceType.DataSource = Enum.GetNames(typeof(SourceType));

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
        private void LoadModel(SourceModel m)
        {
            
            sourceTabPage.Enabled = true;
            sourceTabPage.Visible = true;
            // Tipo di sorgente
            uiSourceType.SelectedItem = Enum.GetName(typeof(SourceType), m.SourceType);

            // File Path Custom
            if (m.FilePath == null)
                uiFilePathCustom.Text = "";
            else
                uiFilePathCustom.Text = m.FilePath;

            // File Path variable
            if (m.FilePathVar == null)
                uiFilePathVariable.Text = "";
            else
                uiFilePathVariable.Text = m.FilePathVar;

            //  Web Url custom
            if (m.WebUrl == null)
                uiWebURLCustom.Text = "";
            else
                uiWebURLCustom.Text = m.WebUrl;

            if (m.WebUrlVariable == null)
                uiURLVariable.Text = "";
            else
                uiURLVariable.Text = m.WebUrlVariable;

            // Configura la UI in modo opportuno
            uiVariableFilePathGroup.Enabled = m.SourceType == SourceType.FilePathVariable;
            uiFilePathGroup.Enabled = m.SourceType == SourceType.FilePath;
            uiCustomUrlGroup.Enabled = m.SourceType == SourceType.WebUrlPath;
            uiVariableUrlGroup.Enabled = m.SourceType == SourceType.WebUrlVariable;
            
            // Advanced Tab
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
                // setta il model nelle properties del componente.
                // Evito la validazione dalla view, verrà fatta dal component direttamente.

                
                // - Salva le informazioni riguardanti la sorgente dei dati
                _model.SourceType = (SourceType)Enum.Parse(typeof(SourceType), uiSourceType.SelectedItem.ToString());
                _model.FilePath = uiSourceType.Text == Enum.GetName(typeof(SourceType), SourceType.FilePath) ? uiFilePathCustom.Text : null;
                _model.WebUrl = uiSourceType.Text == Enum.GetName(typeof(SourceType), SourceType.WebUrlPath) ? uiWebURLCustom.Text : null;

                if (uiSourceType.Text == Enum.GetName(typeof(SourceType), SourceType.FilePathVariable))
                    _model.FilePathVar = uiFilePathVariable.Text;
                else
                    _model.FilePathVar = null;
                if (uiSourceType.Text == Enum.GetName(typeof(SourceType), SourceType.WebUrlVariable))
                    _model.WebUrlVariable = uiURLVariable.Text;
                else
                    _model.WebUrlVariable = null;

                _model.WebMethod = postRadio.Checked ? "POST" : "GET";

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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SourceType newVal = (SourceType)Enum.Parse(typeof(SourceType), (sender as ComboBox).SelectedItem.ToString());
            // Aggiorna la UI in modo opportuno
            uiFilePathGroup.Enabled = newVal == SourceType.FilePath;
            uiVariableFilePathGroup.Enabled = newVal == SourceType.FilePathVariable;
            uiCustomUrlGroup.Enabled = newVal == SourceType.WebUrlPath;
            uiVariableUrlGroup.Enabled = newVal == SourceType.WebUrlVariable;
        }

        private void FilePathFromVariable_Click(object sender, EventArgs e)
        {
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Json Text Files (*.json)|*.json|Text Files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 1;
            DialogResult rd = openFileDialog.ShowDialog();
            if (rd == System.Windows.Forms.DialogResult.OK)
            {
                uiFilePathCustom.Text = openFileDialog.FileName;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        /**
         * Controlla che il website sia raggiungibile.
         */
        private void uiTestWebURL_Click(object sender, EventArgs e)
        {
            try
            {
                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(uiWebURLCustom.Text);
                rq.Credentials = CredentialCache.DefaultCredentials;
                rq.Method = postRadio.Checked ? "POST" : "GET";
                HttpWebResponse resp = (HttpWebResponse)rq.GetResponse();
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("Warning: status code received was " + resp.StatusCode+", i.e. "+resp.StatusDescription);
                }
                // Se l'utente vuole avere una previsione del file che scaricherà, lo scarichiamo
                DialogResult dr = MessageBox.Show("Do you want to download and preview the JSON data?","Preview",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    string tmp = Path.GetTempFileName()+".json";
                    StreamWriter sw = new StreamWriter(new FileStream(tmp,FileMode.OpenOrCreate));
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    char[] buff = new char[4096];
                    int read = 0;
                    while ((read = sr.ReadBlock(buff, 0, buff.Length)) > 0)
                        sw.Write(buff, 0, read);
                    sw.Close();
                    Process p = new Process();
                    p.StartInfo.FileName = "notepad";
                    p.StartInfo.Arguments = tmp;
                    p.Start();
                }
                resp.Close();
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show("Error: invalid URL format. " + ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Error: URL cannot be null. " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }




        private void uiSelectURLVariable_Click(object sender, EventArgs e)
        {
            VariableChooser vc = new VariableChooser(_vars);
            DialogResult dr = vc.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Microsoft.SqlServer.Dts.Runtime.Variable v = vc.GetResult();
                uiURLVariable.Text = v.QualifiedName;
            }
        }

        private void uiBrowseFilePathVariable_Click(object sender, EventArgs e)
        {
            VariableChooser vc = new VariableChooser(_vars);
            DialogResult dr = vc.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Microsoft.SqlServer.Dts.Runtime.Variable v = vc.GetResult();
                uiFilePathVariable.Text = v.QualifiedName;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IDtsVariableService vservice = (IDtsVariableService)_sp.GetService(typeof(IDtsVariableService));
            Microsoft.SqlServer.Dts.Runtime.Variable vv = vservice.PromptAndCreateVariable(_parent, null, null, "User", typeof(string));
            if (vv != null)
                uiWebURLCustom.Text = vv.QualifiedName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IDtsVariableService vservice = (IDtsVariableService)_sp.GetService(typeof(IDtsVariableService));
            Microsoft.SqlServer.Dts.Runtime.Variable vv = vservice.PromptAndCreateVariable(_parent, null,null,"User",typeof(string));
            if (vv != null)
                uiFilePathVariable.Text = vv.QualifiedName;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void uiIOGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void uiIOGrid_Validating(object sender, CancelEventArgs e)
        {
            
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

        private void JsonSourceUI_Load(object sender, EventArgs e)
        {

        }
    }
}

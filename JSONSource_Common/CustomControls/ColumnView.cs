using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
#if LINQ_SUPPORTED
using System.Linq;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    public partial class ColumnView : UserControl
    {
        public ColumnView()
        {
            InitializeComponent();
            (uiIOGrid.Columns["OutColumnType"] as DataGridViewComboBoxColumn).DataSource = Enum.GetNames(typeof(JsonTypes));
            uiRootType.DataSource = Enum.GetNames(typeof(RootType));
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

        public void LoadModel(JSONDataMappingModel m)
        {
            uiIOGrid.Rows.Clear();

            if (!string.IsNullOrEmpty(m.JsonRootPath))
                 uiPathToArray.Text=uiPathToArray.Text;

            uiRootType.SelectedText = Enum.GetName(typeof(RootType), m.RootType);

            if (m != null)
            {
                foreach (IOMapEntry e in m.IoMap)
                {
                    int index = uiIOGrid.Rows.Add();
                    uiIOGrid.Rows[index].Cells[0].Value = e.InputFieldPath;
                    uiIOGrid.Rows[index].Cells[1].Value = e.InputFieldLen;
                    uiIOGrid.Rows[index].Cells[2].Value = e.OutputColName;
                    uiIOGrid.Rows[index].Cells[3].Value = Enum.GetName(typeof(JsonTypes), e.OutputJsonColumnType);
                }
            }
        }

        public JSONDataMappingModel SaveToModel()
        {
            JSONDataMappingModel result = new JSONDataMappingModel();

            // Json root and type
            RootType root;
            Enum.TryParse<RootType>(uiRootType.SelectedValue.ToString(), out root);
            result.RootType = root;

            if (!string.IsNullOrEmpty(uiPathToArray.Text))
                result.JsonRootPath = uiPathToArray.Text;
            else
                result.JsonRootPath = null;

            // IO columns mapping
            result.ClearMapping();
            if (uiIOGrid.IsCurrentCellDirty || uiIOGrid.IsCurrentRowDirty)
            {
                uiIOGrid.CurrentRow.DataGridView.EndEdit();
                uiIOGrid.EndEdit();
                CurrencyManager cm = (CurrencyManager)uiIOGrid.BindingContext[uiIOGrid.DataSource, uiIOGrid.DataMember];
                cm.EndCurrentEdit();
            }

            // In case of error, rewrite the exception for user friendliness
            int row = 1;
            foreach (DataGridViewRow r in uiIOGrid.Rows)
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
                    throw new Exception("JSON Field Name on row " + row);
                }
                int maxLen = -1;
                try
                {
                    maxLen = int.Parse((string)r.Cells[1].Value.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception("Maximum length is invalid on row " + row);
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
                    throw new Exception("Output Column name is invalid on row " + row);
                }

                JsonTypes dataType = 0;
                try
                {
                    dataType = (JsonTypes)Enum.Parse(typeof(JsonTypes), (string)r.Cells[3].Value);
                }
                catch (Exception ex)
                {
                    throw new Exception("Column type is invalid on row " + row);
                }

                IOMapEntry map = new IOMapEntry();
                map.InputFieldPath = inputName;
                map.OutputColName = outName;
                map.OutputJsonColumnType = dataType;
                map.InputFieldLen = maxLen;

                result.AddMapping(map);
                row++;
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
#if LINQ_SUPPORTED
using System.Linq;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    public partial class ColumnView : UserControl
    {
        private class InputColumnWrapper
        {
            public InputColumnWrapper(IDTSVirtualInputColumn100 col) {
                Column = col;
            }
            public IDTSVirtualInputColumn100 Column { get; }
            public override string ToString()
            {
                return Column.Name;
            }
        }

        private IDTSVirtualInputColumnCollection100 _inputs;

        public ColumnView(IDTSVirtualInputColumnCollection100 inputs)
        {
            _inputs = inputs;

            InitializeComponent();

            // Add input columns as selectable output. As default, all of them will be selected.
            foreach (IDTSVirtualInputColumn100 inputCol in _inputs) {
                int index = inputsCb.Items.Add(new InputColumnWrapper(inputCol), CheckState.Checked);

            }

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

            // Rootpath + json response type
            if (!string.IsNullOrEmpty(m.JsonRootPath))
                 uiPathToArray.Text= m.JsonRootPath;

            uiRootType.SelectedItem = Enum.GetName(typeof(RootType), m.RootType);

            // IoMap for json derived columns
            foreach (IOMapEntry e in m.IoMap)
            {
                int index = uiIOGrid.Rows.Add();
                uiIOGrid.Rows[index].Cells[0].Value = e.InputFieldPath;
                uiIOGrid.Rows[index].Cells[1].Value = e.InputFieldLen;
                uiIOGrid.Rows[index].Cells[2].Value = e.OutputColName;
                uiIOGrid.Rows[index].Cells[3].Value = Enum.GetName(typeof(JsonTypes), e.OutputJsonColumnType);
            }

            // Set all items check status to false
            for (var j = 0; j < inputsCb.Items.Count; j++)
                inputsCb.SetItemChecked(j, false);

            if (m.CopyColumnsIOIDs != null)
                // Input columns to be copied as output
                foreach (var i in m.CopyColumnsIOIDs) {
                    // Note that the input might have changed here. 
                    // If we do not find the given lineage id among ours inputs, we simply skip them.
                    for (var j=0;j<inputsCb.Items.Count;j++) {
                        InputColumnWrapper col = inputsCb.Items[j] as InputColumnWrapper;
                        if (i.Key == col.Column.LineageID) {
                            // Ok this column is available. Check it.
                            inputsCb.SetItemChecked(j, true);
                        }
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

            // Save the InputColumns to be copied as output
            var ids = new Dictionary<int, int>();
            for (var i=0;i<inputsCb.CheckedItems.Count;i++) {
                // Setup a placemark. The UI will feed this after outputcolumns are added.
                ids[(inputsCb.CheckedItems[i] as InputColumnWrapper).Column.LineageID]=-1;
            }

            result.CopyColumnsIOIDs = ids;

            return result;
        }

        private void uiIOGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

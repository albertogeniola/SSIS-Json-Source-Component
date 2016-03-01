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
        private SourceModel _model;
        public ColumnView()
        {
            InitializeComponent();
            _model = new SourceModel();
            (uiIOGrid.Columns["OutColumnType"] as DataGridViewComboBoxColumn).DataSource = Enum.GetNames(typeof(JsonTypes));
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

        public void LoadModel(SourceModel m)
        {
            uiIOGrid.Rows.Clear();
            _model = m;

            if (!string.IsNullOrEmpty(uiPathToArray.Text))
                _model.JsonObjectRelativePath = uiPathToArray.Text;
            else
                _model.JsonObjectRelativePath = null;

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
    }
}

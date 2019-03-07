using com.webkingsoft.JsonSuite.Component.Model;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static com.webkingsoft.JsonSuite.Component.Model.ObjectParserProperties;

namespace com.webkingsoft.JsonSuite.UI
{
    public partial class ObjectParserGUI : Form
    {
        private IDTSComponentMetaData100 _metadata;
        private IServiceProvider _serviceProvider;
        private IDTSDesigntimeComponent100 _desingTimeComponent;

        public ObjectParserGUI(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            _metadata = dtsComponentMetadata;
            _serviceProvider = serviceProvider;
            _desingTimeComponent = _metadata.Instantiate();

            InitializeComponent();

            // Add event listener to the grid
            mappingGrid.CellValidating += CellValidation;
            mappingGrid.CellValueChanged += CellValueChanged;

            // Setup the Json Data Type comboboxes
            JsonDataType.DataSource = Enum.GetValues(typeof(JsonAttributeType));
            JsonDataType.ValueType = typeof(JsonAttributeType);

            // Check which column, if any, was selected
            JsonSuite.Component.Model.ObjectParserProperties props = new Component.Model.ObjectParserProperties();
            props.LoadFromComponent(_metadata.CustomPropertyCollection);

            // Load data into UI
            PopulateInputColumn(props);
            PopulateMappingGrid(props);
        }

        private void PopulateMappingGrid(ObjectParserProperties props)
        {
            mappingGrid.Rows.Clear();
            foreach (KeyValuePair<string, AttributeMapping> conf in props.AttributeMappings) {
                int index = mappingGrid.Rows.Add();
                mappingGrid[OutpucColumnName.Index, index].Value = conf.Key;
                mappingGrid[AttributeExpression.Index, index].Value = conf.Value.ObjectAttributeSelectionExpression;
                mappingGrid[JsonDataType.Index, index].Value = conf.Value.AttributeType;
                mappingGrid[Precision.Index, index].Value = conf.Value.SSISPrecision;
                mappingGrid[Scale.Index, index].Value = conf.Value.SSISScale;
            }
        }

        private void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Handles value change for every cell.
            // If the use selected a specific JsonDataType, we need to align the row.
            if (e.ColumnIndex == mappingGrid.Columns.IndexOf(JsonDataType)) {
                JsonAttributeType? jType = mappingGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as JsonAttributeType?;

                var precisionCell = mappingGrid.Rows[e.RowIndex].Cells[Precision.Name];
                var scaleCell = mappingGrid.Rows[e.RowIndex].Cells[Scale.Name];

                switch (jType) {
                    // We only discriminate numeric, booleans and all the rest
                    case JsonAttributeType.Number:
                        // Allow user to type in precision and scale
                        precisionCell.Value = 8;
                        precisionCell.ReadOnly = false;

                        scaleCell.Value = 2;
                        scaleCell.ReadOnly = false;
                        
                        break;

                    default:
                        // Just set precision and scale to 0
                        precisionCell.Value = 0;
                        precisionCell.ReadOnly = true;

                        scaleCell.Value = 0;
                        scaleCell.ReadOnly = true;

                        break;
                }
            }
        }

        private void CellValidation(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var cell = mappingGrid[e.ColumnIndex, e.RowIndex];
            var value = cell.Value;
            cell.ErrorText = null;

            // Validate single cells one by one. 
            if (e.ColumnIndex == OutpucColumnName.Index) {
                // Validate output column name
                // TODO.
            }

            // Only allow non-nagetive integer values for precision, scale
            if (e.ColumnIndex == Precision.Index) {
                // Precision validation
                // Should be an integer value from 0 to 38
                int v;
                if (!int.TryParse(value?.ToString(), out v)) {
                    cell.ErrorText = "Invalid value";
                    return;
                }

                if (v < 0 || v > 38) {
                    cell.ErrorText = "Invalid precision value. Must be between 0 and 38";
                    return;
                }
            }

            if (e.ColumnIndex == Scale.Index) {
                // Scale validation
                int v;
                if (!int.TryParse(value?.ToString(), out v))
                {
                    cell.ErrorText = "Invalid value";
                    return;
                }

                if (v < 0 || v > 38)
                {
                    cell.ErrorText = "Invalid scale value. Must be between 0 and 38";
                    return;
                }
            }
        }

        private void PopulateInputColumn(ObjectParserProperties props)
        {
            // Retrieve the input lane and get the virutal input columns
            var input = _metadata.InputCollection[JsonSuite.Component.JsonObjectParser.INPUT_LANE_NAME];
            IDTSVirtualInputColumnCollection100 vCols = input.GetVirtualInput().VirtualInputColumnCollection;
            int columnCount = vCols.Count;
            foreach (IDTSVirtualInputColumn100 vCol in vCols)
            {
                // Only add columns that are supported: TEXTUAL ones.
                if (JsonSuite.Component.JsonObjectParser.SUPPORTED_JSON_DATA_TYPE.Contains(vCol.DataType)) {
                    var vColumnWrapper = new ColumnWrapper(vCol);
                    var selectedIndex = inputColumn.Items.Add(vColumnWrapper);

                    // If the column was the one previously selected into the metadata, preselect it.
                    if (props.RawJsonInputColumnName == vCol.Name) {
                        inputColumn.SelectedIndex = selectedIndex;
                    }
                }
            }
       }
        
        private void okBtn_Click(object sender, EventArgs e)
        {
            // We do not apply any strong validation in here. 
            // Hopefully there will be no need since most of it already happens at 
            // Validate() on the Component implementation. Anyways, we should ensure is that
            // there is at least one item selected in our combo-box
            if (inputColumn.SelectedItem == null) {
                errorProvider.SetError(inputColumn, "Please select the input column which will contain the JSON array.");
                return;
            } else
                errorProvider.SetError(inputColumn, null);
            
            // What we do in here, instead, is to set the custom property on the component.
            var props = new ObjectParserProperties();

            // Set the input column name
            props.RawJsonInputColumnName = (inputColumn.SelectedItem as ColumnWrapper).Column.Name;

            // Set-up the column mappings
            SaveColumnMappings(props);

            // Save the configuration to the component
            props.CopyToComponent(_metadata.CustomPropertyCollection);

            DialogResult = DialogResult.OK;
        }

        private void SaveColumnMappings(ObjectParserProperties props)
        {
            props.AttributeMappings.Clear();
            foreach (DataGridViewRow row in mappingGrid.Rows) {
                if (row.IsNewRow)
                    continue;

                var outputColumnName = (string)row.Cells[OutpucColumnName.Index].Value;
                var attributeExpression = (string)row.Cells[AttributeExpression.Index].Value;
                var type = (JsonAttributeType)row.Cells[JsonDataType.Index].Value;
                var scale = (int)row.Cells[Scale.Index].Value;
                var precision = (int)row.Cells[Precision.Index].Value;
                var mapping = new AttributeMapping(attributeExpression, type, scale, precision);

                props.AttributeMappings.Add(outputColumnName, mapping);
            }
        }

        private class ColumnWrapper
        {
            public IDTSVirtualInputColumn100 Column { get; }
            public ColumnWrapper(IDTSVirtualInputColumn100 col)
            {
                Column = col;
            }

            public override string ToString()
            {
                return Column.Name;
            }
        }

        private void mappingGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

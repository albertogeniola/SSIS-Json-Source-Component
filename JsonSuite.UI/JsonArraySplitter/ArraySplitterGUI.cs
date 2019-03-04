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

namespace com.webkingsoft.JsonSuite.UI
{
    public partial class ArraySplitterGUI : Form
    {
        private IDTSComponentMetaData100 _metadata;
        private IServiceProvider _serviceProvider;
        private IDTSDesigntimeComponent100 _desingTimeComponent;

        public ArraySplitterGUI(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            _metadata = dtsComponentMetadata;
            _serviceProvider = serviceProvider;
            _desingTimeComponent = _metadata.Instantiate();

            InitializeComponent();
            PopulateInputColumns();
        }

        private void PopulateInputColumns()
        {
            // Check which column, if any, was selected
            JsonSuite.Component.Model.ArraySplitterProperties props = new Component.Model.ArraySplitterProperties();
            props.LoadFromComponent(_metadata.CustomPropertyCollection);

            // Retrieve the input lane and get the virutal input columns
            var input = _metadata.InputCollection[JsonSuite.Component.JsonArraySplitter.INPUT_LANE_NAME];
            IDTSVirtualInputColumnCollection100 vCols = input.GetVirtualInput().VirtualInputColumnCollection;
            int columnCount = vCols.Count;
            foreach (IDTSVirtualInputColumn100 vCol in vCols)
            {
                // Only add columns that are supported: TEXTUAL ones.
                if (JsonSuite.Component.JsonArraySplitter.SUPPORTED_JSON_DATA_TYPE.Contains(vCol.DataType)) {
                    var vColumnWrapper = new ColumnWrapper(vCol);
                    var selectedIndex = inputColumns.Items.Add(vColumnWrapper);

                    // If the column was the one previously selected into the metadata, preselect it.
                    if (props.ArrayInputColumnName == vCol.Name) {
                        inputColumns.SelectedIndex = selectedIndex;
                    }
                }
            }
       }
        
        private void okBtn_Click(object sender, EventArgs e)
        {
            // We do not apply any strong validation in here. 
            // Hopefully there will be no need since most of it already happens at 
            // Validate() on the Component implementation. The only thing we should ensure is that
            // there is at least one item selected in our combo-box
            if (inputColumns.SelectedItem == null) {
                errorProvider.SetError(inputColumns, "Please select the input column which will contain the JSON array.");
                return;
            } else
                errorProvider.SetError(inputColumns, null);

            // What we do in here, instead, is to set the custom property on the component.
            JsonSuite.Component.Model.ArraySplitterProperties props = new Component.Model.ArraySplitterProperties();
            props.ArrayInputColumnName = (inputColumns.SelectedItem as ColumnWrapper).Column.Name;
            props.CopyToComponent(_metadata.CustomPropertyCollection);

            DialogResult = DialogResult.OK;
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
    }
}

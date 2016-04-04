using System;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Design;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Design;
using Microsoft.SqlServer.Dts.Runtime.Design;
using System.Collections.Generic;

namespace com.webkingsoft.JSONSource_Common
{

    /*
     * Costituisce il controller della view.
     */
    public class JSONSourceComponentUI : IDtsComponentUI
    {
        private IDTSComponentMetaData100 _md;
        private IServiceProvider _sp;
        private JSONSourceComponentModel _model;
        private IDTSVirtualInputColumnCollection100 _virtualInputs;

        public void Help(System.Windows.Forms.IWin32Window parentWindow)
        {
        }

        public void New(System.Windows.Forms.IWin32Window parentWindow)
        {
            
        }
        public void Delete(System.Windows.Forms.IWin32Window parentWindow)
        {
        }

        /// <summary>
        /// This method is invoked by the Design Time IDE when an user wants to edit the component setup.
        /// We here build up the UI and pass parameters from one way to the other.
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <param name="vars"></param>
        /// <param name="cons"></param>
        /// <returns></returns>
        public bool Edit(System.Windows.Forms.IWin32Window parentWindow, Variables vars, Connections cons)
        {
            _virtualInputs = _md.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].GetVirtualInput().VirtualInputColumnCollection;
            SourceAdvancedUI componentEditor = new SourceAdvancedUI(vars,_sp, _virtualInputs);
            componentEditor.LoadModel(_model);

            DialogResult result = componentEditor.ShowDialog(parentWindow);

            if (result == DialogResult.OK)
            {
                _model = componentEditor.SavedModel;

                // Setup the column output accordingly
                AddInputColumns(_model.DataMapping.InputColumnsToCopy);
                AddOutputColumns(componentEditor.SavedModel.DataMapping.IoMap, _model.DataMapping.InputColumnsToCopy);

                // Serialize the configuration.
                // TODO: use a standard way to do that
                _md.CustomPropertyCollection[ComponentConstants.PROPERTY_KEY_MODEL].Value = componentEditor.SavedModel.ToJsonConfig();

                return true;
            }
            return false;
        }

        private void AddInputColumns(IEnumerable<string> inputColNames)
        {
            var input = _md.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS];
            
            // Clear inputs
            input.InputColumnCollection.RemoveAll();

            // For each virtual input selected, add a physical input
            foreach (var colname in inputColNames) {
                var incol = input.InputColumnCollection.New();
                incol.LineageID = _virtualInputs[colname].LineageID;
            }
        }

        private void AddOutputColumns(IEnumerable<IOMapEntry> IoMap, IEnumerable<string> copyColNames)
        {
            // Reconfigure outputs: 
            _md.OutputCollection[0].Name = "JSON Source Output";
            _md.OutputCollection[0].OutputColumnCollection.RemoveAll();

            // Add simple copy columns
            foreach (var colname in copyColNames) {                
                // Copy that column
                var input = _md.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].InputColumnCollection[colname];
                IDTSOutputColumn100 output = _md.OutputCollection[0].OutputColumnCollection.New();
                output.SetDataTypeProperties(input.DataType, input.Length, input.Precision, input.Scale, input.CodePage);
                output.MappedColumnID = input.LineageID;
                output.Name = input.Name;
            }

            // For each espected outputcolumn json derived, add the equivalent.
            foreach (IOMapEntry e in IoMap)
            {
                if (e.InputFieldLen < 0)
                {
                    // FIXME TODO: this must be done directly within the UI
                    _md.FireWarning(0, _md.Name, "A row of the IO configuration presents a negative value, which is forbidden.", null, 0);
                }

                IDTSOutputColumn100 col = _md.OutputCollection[0].OutputColumnCollection.New();
                col.Name = e.OutputColName;

                // There might be some possible errors regarding data lenght. We try to correct them here.
                if (e.OutputColumnType == Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_WSTR && e.InputFieldLen > 4000)
                {
                    // FIXME TODO: this must be done directly within the UI
                    // e.OutputColumnType = Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_NTEXT;
                    _md.FireWarning(0, _md.Name, string.Format("Column {0} is supposed to be longer than 4000 chars, so DT_WSTR is not a suitable column type. Instead, DT_NTEXT has been selected.", e.OutputColName), null, 0);
                    col.SetDataTypeProperties(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_NTEXT, e.InputFieldLen, 0, 0, 0);
                }
                else {
                    col.SetDataTypeProperties(e.OutputColumnType, e.InputFieldLen, 0, 0, 0);
                }
            }
        }


        /// <summary>
        /// This method is ivoked once, when the user double clicks on it at design time.
        /// </summary>
        /// <param name="dtsComponentMetadata"></param>
        /// <param name="serviceProvider"></param>
        public void Initialize(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            // Save a reference to the components metadata and service provider
            _sp = serviceProvider;
            _md = dtsComponentMetadata;

            // Check model: if no model was specified, add it one now.
            IDTSCustomProperty100 model = null;
            try
            {
                model = dtsComponentMetadata.CustomPropertyCollection[ComponentConstants.PROPERTY_KEY_MODEL];
                _model = JSONSourceComponentModel.LoadFromJson(model.Value.ToString());
            }
            catch (Exception e) {
                // No model found. Add a new now.
                _model = new JSONSourceComponentModel();
                model = dtsComponentMetadata.CustomPropertyCollection.New();
                model.Name = ComponentConstants.PROPERTY_KEY_MODEL;
                model.Value = _model.ToJsonConfig();
            }
                
            if (_md == null)
                _md = (IDTSComponentMetaData100)_md.Instantiate();

        }
    }
}
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
            SourceAdvancedUI componentEditor = new SourceAdvancedUI(vars,_sp);
            componentEditor.LoadModel(_model);

            DialogResult result = componentEditor.ShowDialog(parentWindow);

            if (result == DialogResult.OK)
            {
                // Serialize the configuration.
                // TODO: use a standard way to do that
                _md.CustomPropertyCollection[ComponentConstants.PROPERTY_KEY_MODEL].Value = _model.ToJsonConfig();
                
                // Setup the column output accordingly
                // TODO: Is the right place where to do that?
                AddOutputColumns(_model.DataMapping.IoMap);
                return true;
            }
            return false;
        }

        private void AddOutputColumns(IEnumerable<IOMapEntry> IoMap)
        {
            // Reconfigure outputs: 
            _md.OutputCollection[0].Name = "JSON Source Output";
            _md.OutputCollection[0].OutputColumnCollection.RemoveAll();

            // For each espected outputcolumn, add the equivalent.
            foreach (IOMapEntry e in IoMap)
            {
                if (e.InputFieldLen < 0)
                {
                    // FIXME TODO
                    _md.FireWarning(0, _md.Name, "A row of the IO configuration presents a negative value, which is forbidden.", null, 0);
                }

                IDTSOutputColumn100 col = _md.OutputCollection[0].OutputColumnCollection.New();
                col.Name = e.OutputColName;
                col.SetDataTypeProperties(e.OutputColumnType, e.InputFieldLen, 0, 0, 0);
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
            IDTSCustomProperty100 model = _md.CustomPropertyCollection[ComponentConstants.PROPERTY_KEY_MODEL];
            if (model.Value == null)
            {
                _model = new JSONSourceComponentModel();
                model.Value = _model.ToJsonConfig();
            }
            else
                _model = JSONSourceComponentModel.LoadFromJson(model.Value.ToString());

            if (_md == null)
                _md = (IDTSComponentMetaData100)_md.Instantiate();

        }
    }
}
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
    public class JSONTransformComponentUI : IDtsComponentUI
    {
        private IDTSComponentMetaData100 _md;
        private IServiceProvider _sp;
        private TransformationModel _model;
        public void Help(System.Windows.Forms.IWin32Window parentWindow)
        {
        }

        public void New(System.Windows.Forms.IWin32Window parentWindow)
        {
            
        }
        public void Delete(System.Windows.Forms.IWin32Window parentWindow)
        {
        }

        public bool Edit(System.Windows.Forms.IWin32Window parentWindow, Variables vars, Connections cons)
        {
            // Create and display the form for the user interface.
            _model.Inputs.Clear();
            IDTSInput100 input = _md.InputCollection[0];
            foreach (IDTSVirtualInputColumn100 vcol in input.GetVirtualInput().VirtualInputColumnCollection) {
                // Only add the textual columns as valid input.
                if (vcol.DataType == Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_NTEXT || vcol.DataType == Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_TEXT || vcol.DataType == Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR || vcol.DataType == Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_WSTR)
                    _model.Inputs.Add(vcol.Name, vcol.LineageID);
            }

            JsonTransformUI componentEditor = new JsonTransformUI(parentWindow, _model, _sp);


            DialogResult result = componentEditor.ShowDialog(parentWindow);

            if (result == DialogResult.OK)
            {
                _md.CustomPropertyCollection[ComponentConstants.PROPERTY_KEY_MODEL].Value = _model.ToJsonConfig();
                AddInputColumn(_model.InputColumnName);
                AddOutputColumns(_model.IoMap);
                return true;
            }
            return false;
        }

        private void AddInputColumn(string vcolInputName)
        {
            var input = _md.InputCollection[0];
            var virtualInputs = _md.InputCollection[0].GetVirtualInput();
            IDTSVirtualInputColumn100 vcol = null;

            // Aggancia tutti i virtual input agli input veri e propri
            input.InputColumnCollection.RemoveAll();
            foreach (IDTSVirtualInputColumn100 vc in virtualInputs.VirtualInputColumnCollection)
            {
                var col = input.InputColumnCollection.New();
                col.Name = vc.Name;
                col.LineageID = vc.LineageID;
            }

            /*
            CManagedComponentWrapper destDesignTime = _md.Instantiate();
            // Il metodo seguente effettua il mapping tra una VirtualInputColumn (che di fatto corrisponde all'output del componente in gerarchia) ed una colonna fisica
            // di questo componente. Nel nostro caso abbiamo una sola colonna, quindi ci basta eseguire questo metodo una sola volta.
            destDesignTime.SetUsageType(input.ID, virtualInputs, vcol.LineageID, DTSUsageType.UT_READONLY);
             * */
        }

        private void AddOutputColumns(IEnumerable<IOMapEntry> IoMap)
        {
            _md.OutputCollection[0].OutputColumnCollection.RemoveAll();

            var input = _md.InputCollection[0];

            // Aggiungi tante colonne di output quante sono le colonne di input
            foreach (IDTSInputColumn100 i in input.InputColumnCollection) {
                IDTSOutputColumn100 col = _md.OutputCollection[0].OutputColumnCollection.New();
                col.Name = i.Name;
                col.SetDataTypeProperties(i.DataType, i.Length, i.Precision, i.Scale, i.CodePage);
            }

            // Aggiungi le colonne di output derivanti dall'interpretazione di JSON
            foreach (IOMapEntry e in IoMap)
            {
                if (e.InputFieldLen < 0)
                {
                    // FIXME TODO
                    _md.FireWarning(0, _md.Name, "A row of the IO configuration presents a negative value, which is forbidden.", null, 0);
                }

                // Creo la nuova colonna descritta dalla riga e la configuro in base ai dettagli specificati
                IDTSOutputColumn100 col = _md.OutputCollection[0].OutputColumnCollection.New();
                col.Name = e.OutputColName;

                // There might be some possible errors regarding data lenght. We try to correct them here.
                // If length > 4000 and type is string, put TEXT datatype.
                if (e.OutputColumnType == Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_WSTR && (e.InputFieldLen > 4000 || e.InputFieldLen == 0))
                {
                    // FIXME TODO: this must be done directly within the UI
                    // e.OutputColumnType = Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_TEXT;
                    _md.FireWarning(0, _md.Name, string.Format("Column {0} is supposed to be longer than 4000 chars, so DT_WSTR is not a suitable column type. Instead, DT_TEXT has been selected.", e.OutputColName), null, 0);
                    // TODO: parametrize the codepage
                    col.SetDataTypeProperties(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_TEXT, 0, 0, 0, 1252);
                }
                else
                {
                    col.SetDataTypeProperties(e.OutputColumnType, e.InputFieldLen, 0, 0, 0);
                }
            }
        }


        /*
         * Metodo invocato quando il componente UI viene caricato per la prima volta, generalmente in seguito al doppio click sul componente.
         */
        public void Initialize(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            // Salva un link ai metadati del runtime editor ed al serviceProvider
            _sp = serviceProvider;
            _md = dtsComponentMetadata;

            // Controlla se l'oggetto contiene il model serializzato nelle proprietà. In caso negativo, creane uno nuovo ed attribuisciglielo.
            IDTSCustomProperty100 model = _md.CustomPropertyCollection[ComponentConstants.PROPERTY_KEY_MODEL];
            if (model.Value == null)
            {
                _model = new TransformationModel();
                model.Value = _model.ToJsonConfig();
            }
            else
                _model = TransformationModel.LoadFromJson(model.Value.ToString());

            if (_md == null)
                _md = (IDTSComponentMetaData100)_md.Instantiate();

        }
    }
}
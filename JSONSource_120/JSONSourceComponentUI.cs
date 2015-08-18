using System;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Design;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using JSONSource;
using Microsoft.SqlServer.Dts.Design;
using Microsoft.SqlServer.Dts.Runtime.Design;
using System.Collections.Generic;
using com.webkingsoft.JSONSource_120.SourceUI;

namespace com.webkingsoft.JSONSource_120
{

    /*
     * Costituisce il controller della view.
     */
    public class JSONSourceComponentUI : IDtsComponentUI
    {
        private IDTSComponentMetaData100 _md;
        private IServiceProvider _sp;
        private SourceModel _model;
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
            //JsonSourceUI componentEditor = new JsonSourceUI(parentWindow,vars,_model,_sp);
            SourceAdvancedUI componentEditor = new SourceAdvancedUI(vars,_sp);
            componentEditor.LoadModel(_model);

            DialogResult result = componentEditor.ShowDialog(parentWindow);

            if (result == DialogResult.OK)
            {
                _md.CustomPropertyCollection[JSONSourceComponent.PROPERTY_KEY_MODEL].Value = _model.ToJsonConfig();
                AddOutputColumns(_model.IoMap);
                return true;
            }
            return false;
        }

        private void AddOutputColumns(IEnumerable<IOMapEntry> IoMap)
        {
            // Tutto andato a buonfine: aggiorna le colonne di output:
            _md.OutputCollection[0].Name = "JSON Source Output";
            _md.OutputCollection[0].OutputColumnCollection.RemoveAll();
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
                col.SetDataTypeProperties(e.OutputColumnType, e.InputFieldLen, 0, 0, 0);
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
            IDTSCustomProperty100 model = _md.CustomPropertyCollection[JSONSourceComponent.PROPERTY_KEY_MODEL];
            if (model.Value == null)
            {
                _model = new SourceModel();
                model.Value = _model.ToJsonConfig();
            }
            else
                _model = SourceModel.LoadFromJson(model.Value.ToString());

            if (_md == null)
                _md = (IDTSComponentMetaData100)_md.Instantiate();

        }
    }
}
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using System.Linq;
#if LINQ_SUPPORTED
using System.Threading.Tasks;
#endif

namespace com.webkingsoft.JSONSource_Common
{
#if DTS120
    [DtsPipelineComponent(DisplayName = "JSON Source Component", Description = "Downloads and parses a JSON file from the web.", ComponentType = ComponentType.SourceAdapter, UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_Common,Version=1.0.120.0,Culture=neutral", IconResource = "com.webkingsoft.JSONSource_Common.jsource.ico")]
#elif DTS110
    [DtsPipelineComponent(DisplayName = "JSON Source Component", Description = "Downloads and parses a JSON file from the web.", ComponentType = ComponentType.SourceAdapter, UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_Common,Version=1.0.110.0,Culture=neutral", IconResource = "com.webkingsoft.JSONSource_Common.jsource.ico")]
#endif
    public class JSONSourceComponent : PipelineComponent
    {
        public override void ProvideComponentProperties()
        {
            // Pulisco gli input, output e le custom properties
            base.RemoveAllInputsOutputsAndCustomProperties();
            // Configuro gli output (per default nessun output presente)
            var output = ComponentMetaData.OutputCollection.New();
            output.Name = "Parsed Json lines";            

            SourceModel m = null;
            try
            {
                m = GetModel();
            }
            catch (ModelNotFoundException e)
            {
                // Non l'ho trovato. Aggiungi la proprietà, salvando l'oggetto MODEL serializzato
                // in XML. 
                var model = ComponentMetaData.CustomPropertyCollection.New();
                model.Description = "Contains information about the confiuguration of the item.";
                model.Name = ComponentConstants.PROPERTY_KEY_MODEL;
                model.Value = new SourceModel().ToJsonConfig();
            }
        }

        /**
         * Questo metodo è invocato diverse volte durante il designtime. Al suo interno verifico che i metadati siano 
         * coerenti e consistenti. In caso di ambiguità o lacune, segnalo al designer le situazioni di inconsistenza,
         * generando opportunamente Warning o Errors.
        **/
        public override Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus Validate()
        {
            bool fireAgain = false;

            // Validazione di base
            // - Una sola linea di output.
            // - Nessuna linea di input.
            if (ComponentMetaData.InputCollection.Count > 0)
            {
                ComponentMetaData.FireError(ComponentConstants.ERROR_NO_INPUT_SUPPORTED, ComponentMetaData.Name, "This component doesn't support any input lane. Please detach or remove those inputs.", null, 0, out fireAgain);
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }
            if (ComponentMetaData.OutputCollection.Count != 1)
            {
                ComponentMetaData.FireError(ComponentConstants.ERROR_SINGLE_OUTPUT_SUPPORTED, ComponentMetaData.Name, "This component only supports a single output lane.", null, 0, out fireAgain);
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }

            SourceModel m = null;
            try
            {
                m = GetModel();
            }
            catch (Exception e)
            {
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }

            // Controlla la validità di ogni elemento del MODEL
            // Sorgente: controlla la validità di tutti i campi in base al tipo di sorgente specificato.
            switch (m.SourceType)
            { 
                case SourceType.FilePath:
                    // l'URL deve essere corretto. Se non lo è, lancia un warning. Non faccio fallire il componente,
                    // pochè potrebbe essere interessante scaricare o posizionare il file a runtime.
                    if (string.IsNullOrEmpty(m.FilePath))
                    {
                        ComponentMetaData.FireError(ComponentConstants.ERROR_FILE_PATH_MISSING, ComponentMetaData.Name, "The filepath has not been set.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    if (!File.Exists(m.FilePath))
                        ComponentMetaData.FireWarning(ComponentConstants.WARNING_FILE_MISSING, ComponentMetaData.Name, "The file " + m.FilePath + " doesn't exist. Make sure it will at runtime.", null, 0);
                    break;
                case SourceType.FilePathVariable:
                    // La variabile deve esistere. Se non esiste, produci un errore
                    if (string.IsNullOrEmpty(m.FilePathVar) || !VariableDispenser.Contains(m.FilePathVar))
                    {
                        ComponentMetaData.FireError(ComponentConstants.ERROR_FILE_VARIABLE_WRONG, ComponentMetaData.Name, "The variable " + m.FilePathVar + " doesn't exist.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    break;
                case SourceType.WebUrlPath:
                    if (m.WebUrl==null)
                    {
                        ComponentMetaData.FireError(ComponentConstants.ERROR_WEB_URL_MISSING, ComponentMetaData.Name, "Web URL has not been set.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    break;
                case SourceType.WebUrlVariable:
                    if (string.IsNullOrEmpty(m.WebUrlVariable))
                    {
                        ComponentMetaData.FireError(ComponentConstants.ERROR_WEB_URL_VARIABLE_MISSING, ComponentMetaData.Name, "Variable value can't be empty.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    if (!VariableDispenser.Contains(m.WebUrlVariable))
                    {
                        ComponentMetaData.FireError(ComponentConstants.ERROR_WEB_URL_VARIABLE_MISSING, ComponentMetaData.Name, "Variable " + m.WebUrlVariable + " isn't valid.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    break;
                
                default:
                    return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }

            // Controlla la tabella di IO
            // Il modello è vuoto?
            if (m.IoMap == null || m.IoMap.Count() == 0)
            {
                ComponentMetaData.FireError(ComponentConstants.ERROR_IOMAP_EMPTY, ComponentMetaData.Name, "This component must at least have one output column.", null, 0, out fireAgain);
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }

            // Assicurati di avere tutte le informazioni per ogni colonna
            foreach (IOMapEntry e in m.IoMap)
            {
                // FieldName and outputFiledName cannot be null, empty and must be unique.
                if (string.IsNullOrEmpty(e.InputFieldPath))
                {
                    ComponentMetaData.FireError(ComponentConstants.ERROR_IOMAP_ENTRY_ERROR, ComponentMetaData.Name, "One row of the Input-Output mapping is invalid: null or empty input field name. Please review IO configuration.", null, 0, out fireAgain);
                    return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                }
                if (string.IsNullOrEmpty(e.OutputColName))
                {
                    ComponentMetaData.FireError(ComponentConstants.ERROR_IOMAP_ENTRY_ERROR, ComponentMetaData.Name, "One row of the Input-Output mapping is invalid: null or empty output field name. Please review IO configuration.", null, 0, out fireAgain);
                    return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                }
                // Checks for unique cols
                foreach (IOMapEntry e1 in m.IoMap)
                {
                    if (!ReferenceEquals(e, e1) && e.InputFieldPath == e1.InputFieldPath)
                    {
                        // Not unique!
                        ComponentMetaData.FireError(ComponentConstants.ERROR_IOMAP_ENTRY_ERROR, ComponentMetaData.Name, "There are two or more rows with same InputFieldName. This is not allowed.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    if (!ReferenceEquals(e, e1) && e.OutputColName == e1.OutputColName)
                    {
                        // Not unique!
                        ComponentMetaData.FireError(ComponentConstants.ERROR_IOMAP_ENTRY_ERROR, ComponentMetaData.Name, "There are two or more rows with same OutputColName. This is not allowed.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                }
            }

            // Controllo i parametri avanzati
            if (!string.IsNullOrEmpty(m.CustomLocalTempDir))
            { 
                // Give warning only if the user specified a custom value and that one is invalid
                if (!Directory.Exists(m.CustomLocalTempDir))
                {
                    ComponentMetaData.FireWarning(ComponentConstants.WARNING_CUSTOM_TEMP_DIR_INVALID, ComponentMetaData.Name, "The path to " + m.CustomLocalTempDir + " doesn't exists on this FS. If you're going to deploy the package on another server, make sure the path is correct and the service has write permission on it.", null, 0);
                    return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                }
            }

            return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISVALID;
        }

        private SourceModel GetModel()
        {
            SourceModel m = null;
            bool found = false;
            // Recupera i metadati. Se non sono presenti, ritorna uno stato di invalido
            foreach (dynamic prop in ComponentMetaData.CustomPropertyCollection)
                if (prop.Name == ComponentConstants.PROPERTY_KEY_MODEL)
                {
                    // Trovato!
                    found = true;
                    m = SourceModel.LoadFromJson((string)prop.Value);    
                    break;
                }

            if (!found)
                throw new ModelNotFoundException();
            else
                return m;
        }


        // Le seguenti variabili contengono gli oggetti da usare a runtime, instanziati dal metodo seguente,
        // invocato appena prima di processare l'input.
        private StreamReader _sr = null;
        private IOMapEntry[] _iomap;
        private Dictionary<string, int> _outColsMaps;
        private string _pathToArray = null;
        ParallelOptions _opt;

        public override void PreExecute()
        {

            _opt = new ParallelOptions();
            _opt.MaxDegreeOfParallelism = 4;

            bool cancel = false;
            // Carico i dettagli dal model
            SourceModel m = null;
            bool found = false;
            foreach (dynamic prop in ComponentMetaData.CustomPropertyCollection)
                if (prop.Name == ComponentConstants.PROPERTY_KEY_MODEL)
                {
                    // Trovato!
                    found = true;
                    try
                    {
                        m = SourceModel.LoadFromJson((string)prop.Value);
                    }
                    catch (Exception e)
                    {
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Invalid Metadata for this component.", null, 0, out cancel);
                        return;
                    }
                    break;
                }
            if (!found)
            {
                ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Invalid Metadata for this component.", null, 0, out cancel);
                return;
            }

            // Ottenimento della sorgente: scaricarla da web oppure leggerla da file
            // Essendo passato per il validate, non effettuo nuovamente i controlli a questo livello.
            // Un utente folle potrebbe aprire il pacchetto ssis e modificare a mano l'XML serializzato
            // cui attinge il model, rendendolo inutilizzabile. Però se l'è andata a cercare!
            switch (m.SourceType)
            { 
                case SourceType.FilePath:
                    // Provo ad aprire il file
                    if (!File.Exists(m.FilePath))
                    {
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "File " + m.FilePath + " doesn't exist.", null, 0, out cancel);
                        return;
                    }
                    try
                    {
                        _sr = new StreamReader(new FileStream(m.FilePath, FileMode.Open));
                    }
                    catch (Exception e)
                    {
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Cannot open file stream to " + m.FilePath + ". Check if the file exists and you have permission to read it.", null, 0, out cancel);
                        return;
                    }
                    break;
                case SourceType.FilePathVariable:
                    // Assumo che la variabile esista e non sia nulla. Me lo conferma la corretta esecuzione del metodo VALIDATE.
                    // Provo ad aprire il file
                    IDTSVariables100 vars = null;
                    VariableDispenser.LockOneForRead(m.FilePathVar, ref vars);
                    string filePath = vars[m.FilePathVar].Value.ToString();
                    vars.Unlock();

                    if (!File.Exists(filePath))
                    {
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "File " + filePath + " doesn't exist.", null, 0, out cancel);
                        return;
                    }
                    try
                    {
                        _sr = new StreamReader(new FileStream(filePath, FileMode.Open));
                    }
                    catch (Exception e)
                    {
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Cannot open file stream to " + filePath + ". Check if the file exists and you have permission to read it.", null, 0, out cancel);
                        return;
                    }
                    break;
                case SourceType.WebUrlPath:
                    // Tento di scaricare il file. Se è stato specificato un percorso temporaneo dove appoggiarsi,
                    // utilizzo quel path.
                    string fName = null;
                    try
                    {
                        fName = Utils.DownloadJson(VariableDispenser, m.WebUrl,m.WebMethod,m.HttpParameters,m.CookieVariable,m.CustomLocalTempDir);
                        _sr = new StreamReader(new FileStream(fName, FileMode.Open));
                    }
                    catch(Exception ex)
                    {
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, ex.Message, null, 0, out cancel);
                        return;
                    }
                    
                    break;
                case SourceType.WebUrlVariable:

                    Uri r = null;
                    vars = null;
                    try
                    {
                        VariableDispenser.LockOneForRead(m.WebUrlVariable, ref vars);
                        string url = vars[m.WebUrlVariable].Value.ToString();
                        r = new Uri(url, UriKind.Absolute);
                    }
                    catch (Exception e)
                    {
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Invalid URI into variable \"" + m.WebUrlVariable + "\" or invalid variable.", null, 0, out cancel);
                        throw new Exception("Invalid URL");
                    }
                    finally { 
                        if (vars!=null)
                            vars.Unlock(); 
                    }

                    filePath = Utils.DownloadJson(VariableDispenser, r, m.WebMethod, m.HttpParameters,m.CookieVariable, m.CustomLocalTempDir);

                    if (!File.Exists(filePath))
                    {
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "File " + filePath + " doesn't exist.", null, 0, out cancel);
                        return;
                    }
                    try
                    {
                        _sr = new StreamReader(new FileStream(filePath, FileMode.Open));
                    }
                    catch (Exception e)
                    {
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Cannot open file stream to " + filePath + ". Check if the file exists and you have permission to read it.", null, 0, out cancel);
                        return;
                    }
                    
                    break;
            }

            // Salva il mapping in un array locale
            _iomap = m.IoMap.ToArray<IOMapEntry>();
            // Genera un dizionario ad accesso veloce per il nome della colonna: mappo nome colonna - Indice della colonna nella riga
            _outColsMaps = new Dictionary<string, int>();
            foreach (IOMapEntry e in _iomap)
            {
                found = false;
                foreach(IDTSOutputColumn100 col in base.ComponentMetaData.OutputCollection[0].OutputColumnCollection)
                {
                    
                    if (col.Name == e.OutputColName)
                    {
                        found =true;
                        int colIndex = BufferManager.FindColumnByLineageID(ComponentMetaData.OutputCollection[0].Buffer, col.LineageID);
                        _outColsMaps.Add(e.OutputColName,colIndex);
                        break;
                    }
                }
                if (!found)
                {
                    // Una colonna del model non ha trovato il corrispettivo nel componente attuale
                    ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "The component is unable to locate the column named " + e.OutputColName + " inside the component metadata. Please review the component.", null, 0, out cancel);
                    return;
                }
            }

            // Salva una copia locale del percorso cui attingere l'array
            _pathToArray = m.JsonObjectRelativePath;
        }

        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
            PipelineBuffer buffer = buffers[0];
            
            try
            {
                ProcessInMemory(_sr, buffer);
                buffer.SetEndOfRowset();
            }
            catch (Exception e)
            {
                bool fireAgain = false;
                ComponentMetaData.FireError(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, "An error has occurred: " + e.Message + ". \n" + e.StackTrace, null, 0, out fireAgain);
                return;
            }
        }

        /**
         * Executes the navigation+parsing operation for the given json, putting results into the buffer.
         */
        private void ProcessInMemory(StreamReader _sr, PipelineBuffer buffer)
        {
            using (_sr)
            {
                bool cancel = false;
                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Loading whole model into memory and deserializing...", null, 0, ref cancel);

                // Navigate to the relative Root.
                try
                {
                    // Load all the Array so we can navigate it quickly.
                    JObject o = JObject.Load(new JsonTextReader(_sr));
                    ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Object loaded.", null, 0, ref cancel);

                    // Get all the tokens returned by the XPath string specified
                    if (_pathToArray == null)
                        _pathToArray = "";

                    IEnumerable<JToken> els =  o.SelectTokens(_pathToArray);
                    int rootEls = els.Count();
                    ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Array: loaded " + rootEls + " tokens.", null, 0, ref cancel);

                    int count = 0;
                    // For each root element we got...
                    foreach (JToken t in els) {
                        if (t.Type == JTokenType.Array) {
                            count+=ProcessArray(t as JArray, buffer);
                        }
                        else if (t.Type == JTokenType.Object) {
                            count+=ProcessObject(t as JObject, buffer);
                        }
                        else {
                            throw new Exception("Invalid token returned by RootPath query: "+t.Type.ToString());
                        }
                    }
                    ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Succesfully parsed " + count + " tokens.", null, 0, ref cancel);
                }
                catch (Exception ex)
                {
                    ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, ex.Message + ex.StackTrace + ex.InnerException, null, 0, out cancel);
                    throw new Exception("Error occurred: " + ex.Message + ex.StackTrace + ex.InnerException);
                }
            }

        }

        private int ProcessObject(JObject obj, PipelineBuffer buffer)
        {
            bool cancel=false;
            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Processing Object...", null, 0, ref cancel);
            // Each objects corresponds to an output row.
            buffer.AddRow();

            // For each column requested from metadata, look for data into the object we parsed
            Parallel.ForEach<IOMapEntry>(_iomap, _opt, delegate(IOMapEntry e)
            {
                // If the user wants to get raw json, we should parse nothing: simply return all the json as a string
                if (e.OutputJsonColumnType == JsonTypes.RawJson)
                {
                    string val = null;
                    var vals = obj.SelectTokens(e.InputFieldPath);
                    if (vals.Count() > 1)
                    {
                        JArray arr = new JArray();
                        foreach (var t in vals)
                        {
                            arr.Add(t);
                        }
                        val = arr.ToString();
                    }
                    else {
                        val = vals.ElementAt(0).ToString();
                    }
                    
                    int colIndex = _outColsMaps[e.OutputColName];
                    buffer[colIndex] = val;
                }
                else {
                    // If it's not a json raw type, parse the value.
                    try
                    {
                        object val = obj.SelectToken(e.InputFieldPath);
                        int colIndex = _outColsMaps[e.OutputColName];
                        buffer[colIndex] = val;
                    }
                    catch (Newtonsoft.Json.JsonException ex) {
                        bool fireAgain = false;
                        ComponentMetaData.FireError(ComponentConstants.ERROR_SELECT_TOKEN, ComponentMetaData.Name, "SelectToken failed. This may be due to an invalid Xpath syntax / member name. However this error still happens if multiple tokens are returned and the value expected is single. Specific error was: " + ex.Message, null, 0, out fireAgain);
                        throw ex;
                    }
                }
                
            });
            return 1;
        }

        private int ProcessArray(JArray arr, PipelineBuffer buffer)
        {
            bool cancel = false;
            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Processing Array...", null, 0, ref cancel);
            int count = 0;
            foreach (JObject obj in arr)
            {
                // Each objects corresponds to an output row.
                buffer.AddRow();

                // For each column requested from metadata, look for data into the object we parsed
                Parallel.ForEach<IOMapEntry>(_iomap, _opt, delegate(IOMapEntry e)
                {
                    object val = obj.SelectToken(e.InputFieldPath);
                    int colIndex = _outColsMaps[e.OutputColName];
                    buffer[colIndex] = val;
                });
                count++;
            }
            return count;
        }

        public override System.Collections.ObjectModel.Collection<int> GetDependentInputs(int blockedInputID)
        {
            return base.GetDependentInputs(blockedInputID);
        }

        public override IDTSExternalMetadataColumn100 InsertExternalMetadataColumnAt(int iID, int iExternalMetadataColumnIndex, string strName, string strDescription)
        {
            return base.InsertExternalMetadataColumnAt(iID, iExternalMetadataColumnIndex, strName, strDescription);
        }

        public override IDTSExternalMetadataColumn100 MapInputColumn(int iInputID, int iInputColumnID, int iExternalMetadataColumnID)
        {
            return base.MapInputColumn(iInputID, iInputColumnID, iExternalMetadataColumnID);
        }

        

        public override void OnInputPathAttached(int inputID)
        {
            throw new Exception("This component is a source adapter and doesn't support any input.");
        }

        public override IDTSOutput100 InsertOutput(DTSInsertPlacement insertPlacement, int outputID)
        {
            throw new Exception("This component doesn't support any additional output");
        }

        public override IDTSInput100 InsertInput(DTSInsertPlacement insertPlacement, int inputID)
        {
            throw new Exception("This component doesn't support any additional input");
        }

        public override void DeleteInput(int inputID)
        {
            throw new Exception("You cannot delete the input lane");
        }

        public override void DeleteOutput(int outputID)
        {
            throw new Exception("You cannot delete the output lane");
        }

    }
    
}

using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JSONSource
{
    [DtsPipelineComponent(DisplayName = "JSON Source Component", Description = "Downloads and parses a JSON file from the web.", ComponentType = ComponentType.SourceAdapter, UITypeName = "JSONSource.JSONSourceComponentUI,JSONSource,Version=1.0.0.0,Culture=neutral", IconResource = "JSONSource.jsource.ico")]
    public class JSONSourceComponent : PipelineComponent
    {
        public const int WARNING_FILE_MISSING = 10;
        public const int WARNING_CUSTOM_TEMP_DIR_INVALID = 11;

        public const int ERROR_NO_INPUT_SUPPORTED = 1;
        public const int ERROR_FILE_PATH_MISSING = 10;
        public const int ERROR_WEB_URL_MISSING = 11;
        public const int ERROR_WEB_URL_VARIABLE_MISSING = 12;
        public const int ERROR_FILE_VARIABLE_WRONG = 13;
        public const int ERROR_IOMAP_EMPTY = 14;
        public const int ERROR_IOMAP_ENTRY_ERROR = 15;
        public const int ERROR_SINGLE_OUTPUT_SUPPORTED = 16;
        
        public const int RUNTIME_ERROR_MODEL_INVALID = 100;

        public const int RUNTIME_GENERIC_ERROR = 1000;

        public static readonly string PROPERTY_KEY_MODEL = "CONFIGURATION_MODEL_OBJECT";
        public override void ProvideComponentProperties()
        {
            // Questo componente non prevede alcun input!
            base.RemoveAllInputsOutputsAndCustomProperties();
            var output = ComponentMetaData.OutputCollection.New();
            output.Name = "Parsed Json lines";
            bool found = false;
            foreach (IDTSProperty100 prop in ComponentMetaData.CustomPropertyCollection)
                if (prop.Name == PROPERTY_KEY_MODEL)
                { 
                    // Trovato, non fare nulla.
                    found = true;
                    break;
                }
            if (!found)
            {
                // Non l'ho trovato. Aggiungi la proprietà, salvando l'oggetto MODEL serializzato
                // in XML. 
                var model = ComponentMetaData.CustomPropertyCollection.New();
                model.Description = "Contains information about the confiuguration of the item.";
                model.Name = PROPERTY_KEY_MODEL;
                model.Value = new Model().ToXmlString();
            }

        }

        /**
         * Questo metodo è invocato diverse volte durante il designtime. Al suo interno verifico che i metadati siano 
         * coerenti e consistenti. In caso di ambiguità o lacune, segnalo al designer le situazioni di inconsistenza,
         * generando opportunamente Warning o Errors.
        **/
        public override Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus Validate()
        {
            Model m = null;
            Boolean found = false;
            bool fireAgain = false;

            // Validazione di base
            // - Una sola linea di output.
            // - Nessuna linea di input.
            if (ComponentMetaData.InputCollection.Count > 0)
            {
                ComponentMetaData.FireError(ERROR_NO_INPUT_SUPPORTED, ComponentMetaData.Name, "This component doesn't support any input lane. Please detach or remove those inputs.", null, 0, out fireAgain);
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }
            if (ComponentMetaData.OutputCollection.Count != 1)
            {
                ComponentMetaData.FireError(ERROR_SINGLE_OUTPUT_SUPPORTED, ComponentMetaData.Name, "This component only supports a single output lane.", null, 0, out fireAgain);
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }

            // Recupera i metadati. Se non sono presenti, ritorna uno stato di invalido
            foreach (dynamic prop in ComponentMetaData.CustomPropertyCollection)
                if (prop.Name == PROPERTY_KEY_MODEL)
                {
                    // Trovato!
                    found = true;
                    try
                    {
                        m = Model.Load((string)prop.Value);
                    }
                    catch (Exception e)
                    {
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    break;
                }

            if (!found)
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;

            // Controlla la validità di ogni elemento del MODEL
            // Sorgente: controlla la validità di tutti i campi in base al tipo di sorgente specificato.
            switch (m.SourceType)
            { 
                case SourceType.filePath:
                    // l'URL deve essere corretto. Se non lo è, lancia un warning. Non faccio fallire il componente,
                    // pochè potrebbe essere interessante scaricare o posizionare il file a runtime.
                    if (string.IsNullOrEmpty(m.FilePath))
                    {
                        ComponentMetaData.FireError(ERROR_FILE_PATH_MISSING, ComponentMetaData.Name, "The filepath has not been set.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    if (!File.Exists(m.FilePath))
                        ComponentMetaData.FireWarning(WARNING_FILE_MISSING, ComponentMetaData.Name, "The file " + m.FilePath + " doesn't exist. Make sure it will at runtime.",null,0);
                    break;
                case SourceType.filePathVariable:
                    // La variabile deve esistere. Se non esiste, produci un errore
                    if (string.IsNullOrEmpty(m.FilePathVar) || !VariableDispenser.Contains(m.FilePathVar))
                    {
                        ComponentMetaData.FireError(ERROR_FILE_VARIABLE_WRONG, ComponentMetaData.Name, "The variable " + m.FilePathVar + " doesn't exist.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    break;
                case SourceType.WebUrlPath:
                    if (string.IsNullOrEmpty(m.WebUrl))
                    {
                        ComponentMetaData.FireError(ERROR_WEB_URL_MISSING, ComponentMetaData.Name, "Web URL has not been set.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    break;
                case SourceType.WebUrlVariable:
                    if (string.IsNullOrEmpty(m.WebUrlVariable))
                    {
                        ComponentMetaData.FireError(ERROR_WEB_URL_VARIABLE_MISSING, ComponentMetaData.Name, "Variable value can't be empty.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    if (!VariableDispenser.Contains(m.WebUrlVariable))
                    {
                        ComponentMetaData.FireError(ERROR_WEB_URL_VARIABLE_MISSING, ComponentMetaData.Name, "Variable "+m.WebUrlVariable+" isn't valid.", null, 0, out fireAgain);
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
                ComponentMetaData.FireError(ERROR_IOMAP_EMPTY, ComponentMetaData.Name, "This component must at least have one output column.", null, 0, out fireAgain);
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }

            // Assicurati di avere tutte le informazioni per ogni colonna
            foreach (IOMapEntry e in m.IoMap)
            {
                // FieldName and outputFiledName cannot be null, empty and must be unique.
                if (string.IsNullOrEmpty(e.InputFieldName))
                {
                    ComponentMetaData.FireError(ERROR_IOMAP_ENTRY_ERROR, ComponentMetaData.Name, "One row of the Input-Output mapping is invalid: null or empty input field name. Please review IO configuration.", null, 0, out fireAgain);
                    return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                }
                if (string.IsNullOrEmpty(e.OutputColName))
                {
                    ComponentMetaData.FireError(ERROR_IOMAP_ENTRY_ERROR, ComponentMetaData.Name, "One row of the Input-Output mapping is invalid: null or empty output field name. Please review IO configuration.", null, 0, out fireAgain);
                    return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                }
                // Checks for unique cols
                foreach (IOMapEntry e1 in m.IoMap)
                { 
                    if (!ReferenceEquals(e,e1) && e.InputFieldName==e1.InputFieldName)
                    {
                        // Not unique!
                        ComponentMetaData.FireError(ERROR_IOMAP_ENTRY_ERROR, ComponentMetaData.Name, "There are two or more rows with same InputFieldName. This is not allowed.", null, 0, out fireAgain);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                    if (!ReferenceEquals(e, e1) && e.OutputColName == e1.OutputColName)
                    {
                        // Not unique!
                        ComponentMetaData.FireError(ERROR_IOMAP_ENTRY_ERROR, ComponentMetaData.Name, "There are two or more rows with same OutputColName. This is not allowed.", null, 0, out fireAgain);
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
                    ComponentMetaData.FireWarning(WARNING_CUSTOM_TEMP_DIR_INVALID, ComponentMetaData.Name, "The path to "+m.CustomLocalTempDir+" doesn't exists on this FS. If you're going to deploy the package on another server, make sure the path is correct and the service has write permission on it.", null,0);
                    return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                }
            }

            // TODO: Parasare la validità del sottopercorso all'array dei dati.
            

            return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISVALID;
        }


        // Le seguenti variabili contengono gli oggetti da usare a runtime, instanziati dal metodo seguente,
        // invocato appena prima di processare l'input.
        private StreamReader _sr = null;
        private IOMapEntry[] _iomap;
        private Dictionary<string, int> _outColsMaps;
        private string _pathToArray = null;
        private OperationMode _opMode = OperationMode.SyncIO;
        public override void PreExecute()
        {
            /*
            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];

            m_FileNameColumnIndex = (int)BufferManager.FindColumnByLineageID(output.Buffer, output.OutputColumnCollection[0].LineageID);
            m_FileBlobColumnIndex = (int)BufferManager.FindColumnByLineageID(output.Buffer, output.OutputColumnCollection[1].LineageID);
             * */
            bool cancel = false;
            // Carico i dettagli dal model
            Model m = null;
            bool found = false;
            foreach (dynamic prop in ComponentMetaData.CustomPropertyCollection)
                if (prop.Name == PROPERTY_KEY_MODEL)
                {
                    // Trovato!
                    found = true;
                    try
                    {
                        m = Model.Load((string)prop.Value);
                    }
                    catch (Exception e)
                    {
                        ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Invalid Metadata for this component.", null, 0, out cancel);
                        return;
                    }
                    break;
                }
            if (!found)
            {
                ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Invalid Metadata for this component.", null, 0, out cancel);
                return;
            }

            // Ottenimento della sorgente: scaricarla da web oppure leggerla da file
            // Essendo passato per il validate, non effettuo nuovamente i controlli a questo livello.
            // Un utente folle potrebbe aprire il pacchetto ssis e modificare a mano l'XML serializzato
            // cui attinge il model, rendendolo inutilizzabile. Però se l'è andata a cercare!
            switch (m.SourceType)
            { 
                case SourceType.filePath:
                    // Provo ad aprire il file
                    if (!File.Exists(m.FilePath))
                    {
                        ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "File "+m.FilePath+" doesn't exist.", null, 0, out cancel);
                        return;
                    }
                    try
                    {
                        _sr = new StreamReader(new FileStream(m.FilePath, FileMode.Open));
                    }
                    catch (Exception e)
                    {
                        ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Cannot open file stream to " + m.FilePath + ". Check if the file exists and you have permission to read it.", null, 0, out cancel);
                        return;
                    }
                    break;
                case SourceType.filePathVariable:
                    // Assumo che la variabile esista e non sia nulla. Me lo conferma la corretta esecuzione del metodo VALIDATE.
                    // Provo ad aprire il file
                    IDTSVariables100 vars = null;
                    VariableDispenser.LockOneForRead(m.FilePathVar, ref vars);
                    string filePath = vars[m.FilePathVar].Value;
                    vars.Unlock();

                    if (!File.Exists(filePath))
                    {
                        ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "File " + filePath + " doesn't exist.", null, 0, out cancel);
                        return;
                    }
                    try
                    {
                        _sr = new StreamReader(new FileStream(filePath, FileMode.Open));
                    }
                    catch (Exception e)
                    {
                        ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Cannot open file stream to " + filePath + ". Check if the file exists and you have permission to read it.", null, 0, out cancel);
                        return;
                    }
                    break;
                case SourceType.WebUrlPath:
                    // Tento di scaricare il file. Se è stato specificato un percorso temporaneo dove appoggiarsi,
                    // utilizzo quel path.
                    string fName = null;
                    try
                    {
                         fName = DownloadJsonFile(m.WebUrl,m.CustomLocalTempDir);
                         _sr = new StreamReader(new FileStream(fName, FileMode.Open));
                    }
                    catch(Exception ex)
                    {
                        ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, ex.Message, null, 0, out cancel);
                        return;
                    }
                    
                    break;
                case SourceType.WebUrlVariable:
                    vars = null;
                    VariableDispenser.LockOneForRead(m.WebUrlVariable, ref vars);
                    filePath = DownloadJsonFile(vars[m.WebUrlVariable].Value,m.CustomLocalTempDir);
                    vars.Unlock();

                    if (!File.Exists(filePath))
                    {
                        ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "File " + filePath + " doesn't exist.", null, 0, out cancel);
                        return;
                    }
                    try
                    {
                        _sr = new StreamReader(new FileStream(filePath, FileMode.Open));
                    }
                    catch (Exception e)
                    {
                        ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Cannot open file stream to " + filePath + ". Check if the file exists and you have permission to read it.", null, 0, out cancel);
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
                    ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "The component is unable to locate the column named "+e.OutputColName+" inside the component metadata. Please review the component.", null, 0, out cancel);
                    return;
                }
            }

            // Salva una copia locale del percorso cui attingere l'array
            _pathToArray = m.JsonObjectRelativePath;
            // Copiati la opMode
            _opMode = m.OpMode;

        }

        private string DownloadJsonFile(string url, string customLocalTempDir=null)
        {
            string localTmp = null;
            string filePath = null;
            if (!string.IsNullOrEmpty(customLocalTempDir))
            {
                if (!Directory.Exists(customLocalTempDir))
                    throw new ArgumentException("Local tmp path doesn't exist: " + customLocalTempDir);
                localTmp = customLocalTempDir;
            }
            else
            {
                localTmp = Path.GetTempPath();
            }

            filePath = Path.Combine(localTmp, Guid.NewGuid().ToString() + ".json");

            using (StreamWriter sr = File.CreateText(Path.Combine(localTmp, Guid.NewGuid().ToString()) + ".txt"))
            {
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        webClient.DownloadFile(url, filePath);
                        return filePath;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Cannot download the json file from " + url + " to " + filePath,ex);
                    }
                }
                
            }
        }
        
        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
            PipelineBuffer buffer = buffers[0];
            
            try
            {

                if (_opMode == OperationMode.StoreInMemory)
                {
                    ProcessInMemory(_sr, buffer);
                }
                else if (_opMode == OperationMode.SyncIO)
                {
                    ProcessInFile(_sr,buffer);
                }
                else
                    throw new Exception("Invalid Operation mode specified: "+_opMode);

                buffer.SetEndOfRowset();
            }
            catch (Exception e)
            {
                bool fireAgain = false;
                ComponentMetaData.FireError(RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, "An error has occurred: "+e.Message+". \n"+e.StackTrace, null, 0, out fireAgain);
                return;
            }
        }

        /**
         Questo metodo prevede il processing del file Json utilizzando il reader e passando in rassegna le chiavi
         una alla volta, minimizzando l'uso della memoria ed aumentando l'uso del disco. 
        **/
        private void ProcessInFile(StreamReader _sr, PipelineBuffer buffer)
        {
            using (_sr)
            {
                using (JsonTextReader jr = new JsonTextReader(_sr))
                {
                    JsonTokenizer tokenizer = new JsonTokenizer(_pathToArray);
                    // Finchè non raggiungo il livello specificato
                    while (tokenizer.HasMoreTokens())
                    {
                        string curKey = tokenizer.Next();
                        int depth = 1;
                        bool keyFound = false;
                        while (jr.Read())
                        {
                            // Se sono allo stesso livello di interesse, controlla la tipologia ed il nome della chiave
                            if (jr.Depth == depth)
                            {
                                // Se il token è di tipo chiave e la chiave ha lo stesso valore che cerco, allora alzo il flag e rompo il ciclo
                                if (jr.TokenType == JsonToken.PropertyName && ((string)jr.Value) == curKey)
                                {
                                    keyFound = true;
                                    break; // Interrompe il while(jr.Read())
                                }
                            }
                        } // Fine While(jr.Read())

                        // Se non ho trovato la chiave che cercavo, lancia un errore
                        if (!keyFound)
                        {
                            //TODO
                            throw new Exception("Invalid Path to array specified in component properties. Cannot reach \"" + _pathToArray + "\"");
                        }

                        // altrimenti procedi con il ciclo
                        depth++;
                    } // Fine  While(tokenizer.HasMoreTokens())

                    // Esco dal ciclo quando non ho più token da cercare: significa che lo streamReader ha spostato il cursore esattamente dove
                    // mi interessa che sia, ovvero all'inizio di un array di oggetti.
                    // Controllo che l'oggetto in questione si di tipo ARRAY
                    if (!jr.Read())
                    {
                        // Non è presente il prossimo elemento! Errore, json malformato
                        throw new Exception("Invalid Json data. Cannot read the array start token. Error in line " + jr.LineNumber + " char " + jr.LinePosition);
                        //TODO
                    }
                    if (jr.TokenType != JsonToken.StartArray)
                    {
                        // Errore: mi aspettavo un array
                        throw new Exception("Invalid json data: element found at " + _pathToArray + " was of type " + jr.TokenType + ". Expecting " + JsonToken.StartArray + " instead. Error in line " + jr.LineNumber + " char " + jr.LinePosition);
                        //TODO
                    }
                    // A questo punto sono all'interno dell'array.
                    // Per ogni oggetto contenuto al suo interno, parso l'uscita
                    bool firstDone = false;
                    int referenceDepth = -1;
                    int count = 0;
                    while (jr.Read() && jr.Depth >= referenceDepth)
                    {
                        // Da eseguire solamente al primo giro. Il Do...While non mi piaceva :)
                        if (!firstDone)
                        {
                            referenceDepth = jr.Depth;
                            firstDone = true;
                        }

                        // Non parso gli oggetti innestati.
                        if (jr.Depth > (referenceDepth + 1))
                        {
                            jr.Skip();
                            continue;
                        }

                        // Se una nuova riga è rihiesta, aggiungila
                        if (jr.TokenType == JsonToken.StartObject)
                            buffer.AddRow();

                        if (jr.TokenType == JsonToken.PropertyName)
                        {
                            string propName = (string)jr.Value;
                            // Leggo direttamente il value da qui:
                            if (!jr.Read())
                            {
                                // Errore 
                                //TODO
                                throw new Exception("Error during JSON PARSING:cannot read property value for property name " + propName + " for item # " + count + ". Line " + jr.LineNumber + " char " + jr.LinePosition);
                            }
                            // Mi aspetto di avere in mano il valore.
                            // Cerco la colonna corrispondente nella lista delle collonne in IOMap ed effettuo l'inserimento
                            foreach (IOMapEntry e in _iomap)
                            {
                                if (e.InputFieldName == propName)
                                {
                                    // Identifico qual'è la colonna di output
                                    int destColIndex = _outColsMaps[e.OutputColName];
                                    // Effettuo la copia del valore
                                    buffer[destColIndex] = jr.Value;
                                    break;
                                }
                            }

                        }
                    }


                } // Fine using jr
            } // Fine using _sr
        }

        /**
         * Il processing in memory prevede il caricamento dell'intero dataset in ingresso, deserializzandolo in opportuni oggetti C#.
        **/
        private void ProcessInMemory(StreamReader _sr, PipelineBuffer buffer)
        {
            ParallelOptions opt = new ParallelOptions();
            opt.MaxDegreeOfParallelism = 4;

            using (_sr)
            {
                bool cancel = false;
                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Loading whole model into memory and deserializing...", null, 0, ref cancel);

                // Eseguo il caricamento in memoria e la deserializzazione dell'oggetto
                JObject o = JObject.Load(new JsonTextReader(_sr));
                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Object loaded.", null, 0, ref cancel);

                // Se è stato specificato un sottopath da seguire, lo cerco
                try
                {
                    JToken t = o.SelectToken(_pathToArray, true);
                    if (t.Type != JTokenType.Array)
                    {
                        // Not an array!
                        ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "The path specified: " + _pathToArray + " doesn't point to an array. This component only supports Array deserialization.", null, 0, out cancel);
                        throw new Exception("The path specified: " + _pathToArray + " doesn't point to an array. This component only supports Array deserialization.");
                    }
                    else
                    {
                        object[] arr = t.ToArray<object>();
                        int count = 0;
                        // Per ogni oggetto nell'array
                        foreach (Newtonsoft.Json.Linq.JObject obj in arr)
                        {
                            buffer.AddRow();
                            Parallel.ForEach<IOMapEntry>(_iomap, opt, delegate(IOMapEntry e) {
                                try {
                                    object val = obj.Property(e.InputFieldName).Value;
                                    int colIndex = _outColsMaps[e.OutputColName];
                                    buffer[colIndex] = val;
                                }
                                catch(Exception ex)
                                {
                                    ComponentMetaData.FireError(RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, "Cannot set property value for column " + e.OutputColName + " from the input field " + e.InputFieldName + " on object # " + count, null, 0, out cancel);
                                    throw new Exception("Cannot set property value for column "+e.OutputColName+" from the input field "+e.InputFieldName+" on object # "+count);
                                }

                            });
                            count++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ComponentMetaData.FireError(RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, "Cannot find an array to the object path specified: " + _pathToArray + ". Please check the path.", null, 0, out cancel);
                    throw new Exception("Cannot find an array to the object path specified: " + _pathToArray + ". Please check the path.");
                }
            }
            //TODO
            /*
            // TEST ------
            long bytes_old = GC.GetTotalMemory(false);
            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Before invocation using " + (bytes_old / 1024 / 1024) + " mb.",null,0,cancel);
            JObject o = JObject.Load(new JsonTextReader(_sr));
            long bytes = GC.GetTotalMemory(false);
            ComponentMetaData.FireInformation(1001, ComponentMetaData.Name, "After invocation using " + (bytes / 1024 / 1024) + " mb.", null, 0, cancel);
            ComponentMetaData.FireInformation(1002, ComponentMetaData.Name, "DELTA IS " + ((bytes - bytes_old) / 1024 / 1024) + " mb.", null, 0, cancel);
            o.SelectToken(m.JsonObjectRelativePath);
            // FINE TEST ----
            */
        }

        
        public override IDTSInput100 InsertInput(DTSInsertPlacement insertPlacement, int inputID)
        {
            throw new InvalidOperationException("This component doesn't support any input.");
        } 


        private class JsonTokenizer
        {
            private string _pathToArr;
            private string[] _tokens;
            private int _index;

            public JsonTokenizer(string pathToArr)
            {
                _pathToArr = pathToArr;
                if (_pathToArr == null)
                    _tokens = new string[] { };
                else
                    _tokens = pathToArr.Split('.');
                _index = 0;
            }

            public bool HasMoreTokens()
            {
                if (string.IsNullOrEmpty(_pathToArr))
                    return false;

                return (_index < _tokens.Length);
            }

            /**
             * Alla prima chiamata ritorna il primissimo token (con indice 0
            **/
            public string Next()
            {
                if (_index >= _tokens.Length)
                    throw new IndexOutOfRangeException("No more tokens available.");

                string res = _tokens[_index];
                _index++;
                return res;
            }

        }
    }
    
}

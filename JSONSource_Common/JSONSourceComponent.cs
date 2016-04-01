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
using System.Threading.Tasks;
#if LINQ_SUPPORTED
using System.Threading.Tasks;
using System.Windows.Forms;
#endif

namespace com.webkingsoft.JSONSource_Common
{
#if DTS120
    [DtsPipelineComponent(DisplayName = "JSON Source Component", Description = "Downloads and parses a JSON file from the web.", ComponentType = ComponentType.SourceAdapter, UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_120,Version=1.0.200.0,Culture=neutral", IconResource = "com.webkingsoft.JSONSource_120.jsource.ico")]
#elif DTS110
    [DtsPipelineComponent(DisplayName = "JSON Source Component", Description = "Downloads and parses a JSON file from the web.", ComponentType = ComponentType.SourceAdapter, UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_110,Version=1.0.200.0,Culture=neutral", IconResource = "com.webkingsoft.JSONSource_110.jsource.ico")]
#endif
    public class JSONSourceComponent : PipelineComponent
    {
        // TODO for next version: 
        // model serialization with custom properties
        // support one input line for parameters
        // support output error line
        // add httpparams support
        // oauth?
        // datatype guessing
        // jsonpath parser-highlighter
        // Parallel options into gui
        // Implement runtime debug option

        public override void ProvideComponentProperties()
        {
            // Clear all inputs and custom props, plus setup outputs
            base.RemoveAllInputsOutputsAndCustomProperties();
            var output = ComponentMetaData.OutputCollection.New();
            output.Name = "Parsed Json lines";
        }

        /// <summary>
        /// This method is invoked multiple times at design time. It is in charge of metadata checks. If some metadata is missing
        /// or inconsistent, Warnings and Errors will be thrown, so the user can fix them and the IDE will refuse running with bad metadata.
        /// </summary>
        /// <returns></returns>
        public override Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus Validate()
        {
            bool fireAgain = false;
            // basic component validation
            // - We do not support anyinput line
            // - We only support only one output line
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

            // The rest of the validation process is provided by the MODEL object itself
            JSONSourceComponentModel m = null;
            try
            {
                m = GetModel();
            }
            catch (Exception e)
            {
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }

            // Validation is left to the model object
            string err = null;
            string warn = null;

            m.Validate(out err, out warn);

            if (!string.IsNullOrEmpty(warn)) {
                // Fire the warning, but do not return any invalid state
                // Fire the error and return an invalid state
                bool cancel;
                ComponentMetaData.FireWarning(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, err, null, 0);
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }

            if (!string.IsNullOrEmpty(err))
            {
                // Fire the error and return an invalid state
                bool cancel;
                ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, err, null, 0, out cancel);
                return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
            }

            // Everything seems ok.
            return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISVALID;
        }

        /// <summary>
        /// This function takes care of deserializing the model from the configuration metadata.
        /// If no model has been previously defined, it creates a new one and adds it to the metadata.
        /// </summary>
        /// <returns></returns>
        private JSONSourceComponentModel GetModel(bool fail_if_not_found=false)
        {
            var m = ComponentMetaData.CustomPropertyCollection[ComponentConstants.PROPERTY_KEY_MODEL];
            JSONSourceComponentModel model = null;
            // If no model was set, add it now. The model is then serialized into a json string so it's easier to keep track of it.
            // TODO: align with best practices and use built-in props, so we do not break AdvancedView
            if (m == null || m.Value == null)
            {
                model = new JSONSourceComponentModel();
                m = ComponentMetaData.CustomPropertyCollection.New();
                m.Description = "Contains information about the confiuguration of the item.";
                m.Name = ComponentConstants.PROPERTY_KEY_MODEL;
                m.Value = model.ToJsonConfig();
            }
            else {
                if (fail_if_not_found)
                    throw new Exception("No model found");
                model = JSONSourceComponentModel.LoadFromJson(m.Value);
            }

            return model;
        }


        // The following variables are used as temporary storage when the validation has been finished and
        // the data process is happening at runtime. Their goal is to provide a fast way to lookup important
        // data while processing data.
        private StreamReader _sr = null;
        private IOMapEntry[] _iomap;
        private Dictionary<string, int> _outColsMaps;
        private string _pathToArray = null;
        private ParallelOptions _opt;
        private RootType _rootType;

        /// <summary>
        /// This function is invoked by the environment once, before data processing happens. So it's a great time to configure the basics
        /// before starting to process data. Basically, we'll fill up the fast-lookup variables defined above.
        /// </summary>
        public override void PreExecute()
        {
            try
            {
                _opt = new ParallelOptions();
                _opt.MaxDegreeOfParallelism = 4;

                bool cancel = false;

                // Load the model and fail if no model is found
                JSONSourceComponentModel m = GetModel(true);

                // Save the root type
                _rootType = m.DataMapping.RootType;

                // If the uri depends on a variable, get it now.
                Uri uri = null;
                if (m.DataSource.FromVariable)
                {
                    DataType type;
                    object varval = Utils.GetVariable(this.VariableDispenser, m.DataSource.VariableName, out type);
                    var uristr = varval.ToString();

                    // Parse the uri
                    uri = new Uri(uristr);
                }
                else {
                    uri = m.DataSource.SourceUri;
                }

                // Validation alredy happended. We just double check for some more runtime elements, such as variables mapped values or file presence/existance.
                if (uri.IsFile)
                {
                    if (!File.Exists(uri.LocalPath))
                        throw new Exception(String.Format("File {0} does not exist.", uri.LocalPath));

                    // Setup the stream reader
                    _sr = new StreamReader(new FileStream(uri.LocalPath, FileMode.Open));
                }
                else {
                    // Download the file and setup the stream reader
                    string fName = null;
                    _sr = new StreamReader(new FileStream(fName, FileMode.Open));    
                }


                // Now perform the IO mapping for fast lookup during JSON Reading
                // Dictionary<name_of_column, index_of_column_in_pipeline_row>
                _iomap = m.DataMapping.IoMap.ToArray<IOMapEntry>();                
                _outColsMaps = new Dictionary<string, int>();
                foreach (IOMapEntry e in _iomap)
                {
                    bool found = false;
                    foreach (IDTSOutputColumn100 col in base.ComponentMetaData.OutputCollection[0].OutputColumnCollection)
                    {
                        if (col.Name == e.OutputColName)
                        {
                            found = true;
                            int colIndex = BufferManager.FindColumnByLineageID(ComponentMetaData.OutputCollection[0].Buffer, col.LineageID);
                            _outColsMaps.Add(e.OutputColName, colIndex);
                            break;
                        }
                    }
                    if (!found)
                    {
                        // Inconsistency. Throw an error
                        throw new Exception(string.Format("The component is unable to locate the column named {0} inside the component metadata. Please review the component.", e.OutputColName));
                    }
                }

                // Save a local copy where to get the json path
                _pathToArray = m.DataMapping.JsonRootPath;
            }
            catch (Exception e) {
                // TODO!
                bool cancel;
                ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, ComponentMetaData.Name, e.Message, null, 0, out cancel);
            }
        }

        /// <summary>
        /// From MS Documentation:
        /// The PrimeOutput method is called when a component has at least one output, attached to a downstream component through an IDTSPath100 object, and the SynchronousInputID property of the output is zero. 
        /// The PrimeOutput method is called for source components and for transformations with asynchronous outputs. 
        /// Unlike the ProcessInput method described below, the PrimeOutput method is only called once for each component that requires it.
        /// Being an asynch source component, we process inputs here.
        /// </summary>
        /// <param name="outputs"></param>
        /// <param name="outputIDs"></param>
        /// <param name="buffers"></param>
        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
            PipelineBuffer buffer = buffers[0];

            try
            {
                ProcessInMemory(_sr, buffer, _rootType);
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
        private void ProcessInMemory(StreamReader _sr, PipelineBuffer buffer, RootType rootType)
        {
            using (_sr)
            {
                bool cancel = false;
                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Loading whole model into memory and deserializing...", null, 0, ref cancel);

                dynamic o = null;
                
                try
                {
                    // Load the whole json in memory.
                    using (var reader = new JsonTextReader(_sr))
                    {
                        if (rootType == RootType.JsonObject)
                        {
                            o = JObject.Load(new JsonTextReader(_sr));
                            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Object loaded.", null, 0, ref cancel);
                        }
                        else {
                            o = JArray.Load(new JsonTextReader(_sr));
                            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Array loaded.", null, 0, ref cancel);
                        }
                    }

                    // Get all the tokens returned by the XPath string specified
                    if (_pathToArray == null)
                        _pathToArray = "";

                    // Navigate to the relative Root.
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

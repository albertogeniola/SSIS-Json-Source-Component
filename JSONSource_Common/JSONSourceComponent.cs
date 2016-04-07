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
using System.Diagnostics;
#if LINQ_SUPPORTED
using System.Threading.Tasks;
using System.Windows.Forms;
#endif

namespace com.webkingsoft.JSONSource_Common
{
#if DTS120
    [DtsPipelineComponent(DisplayName = "JSON Source Component", Description = "Downloads and parses a JSON file from the web.", ComponentType = ComponentType.Transform, UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_120,Version=1.1.0000,Culture=neutral", IconResource = "com.webkingsoft.JSONSource_120.jsource.ico")]
#elif DTS110
    [DtsPipelineComponent(DisplayName = "JSON Source Component", Description = "Downloads and parses a JSON file from the web.", ComponentType = ComponentType.Transform, UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_110,Version=1.1.000.0,Culture=neutral", IconResource = "com.webkingsoft.JSONSource_110.jsource.ico")]
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

        /// Some implementation notes.
        /// This component is now a Transformation component, because we want to use Inputs as HTTP parameters. 
        /// So, the PrimeOutput method won't be called by the envirnment, instead ProcessInput is. Being an async component,
        /// the ide calls ProcessInputs asynch while data is received via the input stream. So here we basically keep track of inputs
        /// and process them later in the PrimeOutput method.
        /// -> inputs are passed to the ProcessInput()
        /// -> PrimeOutput() is then invoked

        /// Remember the lifecycle!
        /// AcquireConnections()
        /// Validate()
        /// ReleaseConnections()
        /// PrepareForExecute()
        /// AcquireConnections()
        /// PreExecute()
        /// PrimeOutput()
        /// ProcessInput()
        /// PostExecute()
        /// ReleaseConnections()
        /// Cleanup()

        public override void ProvideComponentProperties()
        {
            // Clear all inputs and custom props, plus setup outputs
            base.RemoveAllInputsOutputsAndCustomProperties();
            var output = ComponentMetaData.OutputCollection.New();
            output.Name = "Parsed Json lines";

            // Set the output as asynchronous. This will allow us to use a single buffer between input and output.
            output.SynchronousInputID = 0;

            // Prepare the input lane for possible httpparams
            var params_lane = ComponentMetaData.InputCollection.New();
            params_lane.Name = ComponentConstants.NAME_INPUT_LANE_PARAMS;

            // TODO: initialize here custom properties for the model. It would be clearer and follows the MS Specs.
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
            // - We only support up to 1 input lane
            // - We only support only one output line
            if (ComponentMetaData.InputCollection.Count > 1)
            {
                ComponentMetaData.FireError(ComponentConstants.ERROR_NO_INPUT_SUPPORTED, ComponentMetaData.Name, "This component only supports one input lane, for parameters.", null, 0, out fireAgain);
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

            // Although model is consistent, we must make sure the input columns it is refferring to are connected end existing.
            // Possible references to inputs are:
            // HTTP Params
            // CopyColumns
            foreach (var param in m.DataSource.HttpParameters) {
                if (param.IsInputMapped)
                {
                    bool found = false;
                    foreach (IDTSInputColumn100 inputcol in ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].InputColumnCollection) {
                        if (inputcol.Name == param.InputColumnName) {
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        bool cancel;
                        // This column is not mapped. This will cause an error
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, String.Format("HTTP parameter {0} requires input column {1} to be defined/connected. However there is no {1} column input attached.",param.Name,param.InputColumnName), err, null, 0, out cancel);
                        return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                    }
                }
            }

            // Also make sure copy columns are available
            foreach (var colname in m.DataMapping.InputColumnsToCopy) {
                bool found = false;
                foreach (IDTSInputColumn100 inputcol in ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].InputColumnCollection)
                {
                    if (inputcol.Name == colname)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    // This column is not mapped. This will cause an error
                    bool cancel;
                    ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, String.Format("However there is no {0} column input attached. Please update the component configuration.", colname), err, null, 0, out cancel);
                    return Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSValidationStatus.VS_ISBROKEN;
                }
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
                if (fail_if_not_found)
                    throw new Exception("No model found");

                model = new JSONSourceComponentModel();
                m = ComponentMetaData.CustomPropertyCollection.New();
                m.Description = "Contains information about the confiuguration of the item.";
                m.Name = ComponentConstants.PROPERTY_KEY_MODEL;
                m.Value = model.ToJsonConfig();
            }
            else {
                model = JSONSourceComponentModel.LoadFromJson(m.Value);    
            }

            return model;
        }

        // The following variables are used as temporary storage when the validation has been finished and
        // the data process is happening at runtime. Their goal is to provide a fast way to lookup important
        // data while processing data.
        private IOMapEntry[] _iomap;
        private Dictionary<string, int> _outColsMaps;
        private Dictionary<int, int> _inputCopyToOutputMaps;
        private ParallelOptions _opt;
        private IDTSInput100 _parametersInputLane;
        private JSONSourceComponentModel _model;
        private PipelineBuffer _outputbuffer = null;

        /// <summary>
        /// This function is invoked by the environment once, before data processing happens. So it's a great time to configure the basics
        /// before starting to process data. Basically, we'll fill up the fast-lookup variables defined above.
        /// </summary>
        public override void PreExecute()
        {
            DataType type;
            try
            {
                Utils.GetVariable(VariableDispenser, "WK_DEBUG", out type);
                MessageBox.Show("Attach debugger now, pid: " + Process.GetCurrentProcess().Id);
            }
            catch (Exception e)
            {
                // Do nothing
            }
            
            //MessageBox.Show("Attach the debugger now! PID: " + System.Diagnostics.Process.GetCurrentProcess().Id);
            try
            {
                _opt = new ParallelOptions();
                _opt.MaxDegreeOfParallelism = 4;

                bool cancel = false;

                // Load the model and fail if no model is found
                JSONSourceComponentModel m = GetModel(true);

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
                    
                }

                // Save the input column index, used to parse parameters for web-requests
                _parametersInputLane = ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS];

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

                _inputCopyToOutputMaps = new Dictionary<int, int>();
                // Fill the fast dictionary for Input to output cols
                foreach (var inputColName in m.DataMapping.InputColumnsToCopy) {
                    
                    // Retrieve the index of the input column and use it as key for the fast dict. Note that for our implementation, input column names matech output column names
                    // Input column index <-> OutputColumnIndex
                    int input_index = BufferManager.FindColumnByLineageID(ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].Buffer, ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].InputColumnCollection[inputColName].LineageID);
                    int output_index = BufferManager.FindColumnByLineageID(ComponentMetaData.OutputCollection[0].Buffer, ComponentMetaData.OutputCollection[0].OutputColumnCollection[inputColName].LineageID);
                    // Map the input index to the output index
                    _inputCopyToOutputMaps[input_index] = output_index;
                }

                _model = m;

                
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
        /// </summary>
        /// <param name="outputs"></param>
        /// <param name="outputIDs"></param>
        /// <param name="buffers"></param>
        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            if (buffers.Length != 0)
                _outputbuffer = buffers[0];
            else
                return;

            // This component might be used as Source or as Transformation. Therefore, the data processing might be done by ProcessInput (if input dependent)
            // or entirely here if no input has been defined.
            bool cancel = false;
            if (ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].IsAttached) {
                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Detected input lane attached, data processing will depend on inputs.", null, 0, ref cancel);
                return;
            }

            // In case there is no input, do the hard work here
            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "No input lane attached, data processing takes place immediately.", null, 0, ref cancel);

            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, String.Format("Executing request {0}", _model.DataSource.SourceUri.ToString()), null, 0, ref cancel);

            string fname = null;
            if (_model.DataSource.SourceUri.IsFile)
                fname = _model.DataSource.SourceUri.LocalPath;
            else {
                Utils.DownloadJson(this.VariableDispenser, _model.DataSource.SourceUri, _model.DataSource.WebMethod, null, _model.DataSource.CookieVariable);
                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, String.Format("Temp json downloaded to {0}. Parsing json now...", fname), null, 0, ref cancel);
            }

            // Process data according to IOMappings
            using (StreamReader sr = new StreamReader(File.Open(fname, FileMode.Open)))
                ProcessInMemory(sr, _model.DataMapping.RootType, null, _outputbuffer);

            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Json parsed correctly.", null, 0, ref cancel);

            _outputbuffer.SetEndOfRowset();
        }

        private PipelineBuffer AddOutputRow(PipelineBuffer inputbuffer) {
            // Add A row and pre-fill it
            _outputbuffer.AddRow();

            if (inputbuffer != null)
                foreach (var input_output in _inputCopyToOutputMaps)
                {
                    _outputbuffer[input_output.Value] = inputbuffer[input_output.Key];
                }

            return _outputbuffer;
        }

        public override void ProcessInput(int inputID, PipelineBuffer inputbuffer)
        {
            // This method is invoked only when the component has some inputs to process. Otherwise, if no input has been specified, the PrimeOutput will handle all the job.
            bool cancel = false;
            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Processing inputs...", null, 0, ref cancel);
            try
            {
                bool downloaded = false;
                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Detected HTTP Params lane attached. Executing in BATCH mode.", null, 0, ref cancel);
                while (inputbuffer.NextRow())
                {
                    // Perform the request with appropriate inputs as HTTP params...
                    ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, String.Format("Executing request {0}", _model.DataSource.SourceUri.ToString()), null, 0, ref cancel);
                        
                    var tmp = _model.DataSource.HttpParameters.ToArray();
                    fillParams(ref tmp, ref inputbuffer);

                    string fname = null;
                    if (_model.DataSource.SourceUri.IsFile)
                        fname = _model.DataSource.SourceUri.LocalPath;
                    else {
                        downloaded = true;
                        Utils.DownloadJson(this.VariableDispenser, _model.DataSource.SourceUri, _model.DataSource.WebMethod, null, _model.DataSource.CookieVariable);
                        ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, String.Format("Temp json downloaded to {0}. Parsing json now...", fname), null, 0, ref cancel);
                    }

                    // Process data according to IOMappings
                    using (StreamReader sr = new StreamReader(File.Open(fname, FileMode.Open)))
                        ProcessInMemory(sr, _model.DataMapping.RootType, inputbuffer, _outputbuffer);

                    if (downloaded)
                        File.Delete(fname);

                    ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Json parsed correctly.", null, 0, ref cancel);
                }
                
                if (inputbuffer.EndOfRowset)
                    _outputbuffer.SetEndOfRowset();

                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "All inputs processed.", null, 0, ref cancel);
            }
            catch (Exception e)
            {
                bool fireAgain = false;
                ComponentMetaData.FireError(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, "An error has occurred: " + e.Message + ". \n" + e.StackTrace, null, 0, out fireAgain);
                return;
            }
        }

        /// <summary>
        /// Given a list of parameters and the input buffer, lookup for input bind paramenters and retrieve their value from the input buffer.
        /// </summary>
        /// <param name="httpParameters"></param>
        /// <param name="buffer"></param>
        private void fillParams(ref HTTPParameter[] httpParameters, ref PipelineBuffer inputbuffer)
        {
            foreach (var p in httpParameters) {
                if (p.IsInputMapped) {
                    int colIndex = BufferManager.FindColumnByLineageID(_parametersInputLane.Buffer, _parametersInputLane.InputColumnCollection[p.InputColumnName].LineageID);
                    p.Value = inputbuffer[colIndex].ToString();
                }
            }
        }

        /**
         * Executes the navigation+parsing operation for the given json, putting results into the buffer.
         */
        private void ProcessInMemory(StreamReader sr, RootType rootType, PipelineBuffer inputbuffer, PipelineBuffer outputbuffer)
        {
            using (sr)
            {
                bool cancel = false;
                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Loading whole model into memory and deserializing...", null, 0, ref cancel);

                dynamic o = null;
                
                try
                {
                    // Load the whole json in memory.
                    using (var reader = new JsonTextReader(sr))
                    {
                        if (rootType == RootType.JsonObject)
                        {
                            o = JObject.Load(new JsonTextReader(sr));
                            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Object loaded.", null, 0, ref cancel);
                        }
                        else {
                            o = JArray.Load(new JsonTextReader(sr));
                            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Array loaded.", null, 0, ref cancel);
                        }
                    }

                    // Get all the tokens returned by the XPath string specified
                    if (_model.DataMapping.JsonRootPath == null)
                        _model.DataMapping.JsonRootPath = "";

                    // Navigate to the relative Root.
                    IEnumerable<JToken> els =  o.SelectTokens(_model.DataMapping.JsonRootPath);
                    int rootEls = els.Count();
                    ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Array: loaded " + rootEls + " tokens.", null, 0, ref cancel);

                    int count = 0;
                    // For each root element we got...
                    foreach (JToken t in els) {
                        if (t.Type == JTokenType.Array) {
                            count+=ProcessArray(t as JArray, inputbuffer);
                        }
                        else if (t.Type == JTokenType.Object) {
                            count+=ProcessObject(t as JObject, inputbuffer);
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

        private int ProcessObject(JObject obj, PipelineBuffer inputbuffer)
        {
            bool cancel=false;
            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Processing Object...", null, 0, ref cancel);
            // Each objects corresponds to an output row.
            var buffer = AddOutputRow(inputbuffer);

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

        private int ProcessArray(JArray arr, PipelineBuffer inputbuffer)
        {
            bool cancel = false;
            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Processing Array...", null, 0, ref cancel);
            int count = 0;
            foreach (JObject obj in arr)
            {
                // Each objects corresponds to an output row.
                var buffer = AddOutputRow(inputbuffer);

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

using com.webkingsoft.JSONSource_Common.Exceptions;
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
using System.Threading;
using System.Threading.Tasks;
#if LINQ_SUPPORTED
using System.Threading.Tasks;
using System.Windows.Forms;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    /// <summary>
    /// The JSON Source component has two major goals: retrieve JSON data and parse it. Data can be retriven by reading a file (which can also reside on the network),
    /// or can be downloaded via a web-service.
    /// This component can operate in two ways: SOURCE or TRANSFORMATION. The component is in source mode when no input is attached, thus data collection does not depend
    /// on any input. In this case, the runtime environment will only call PrimeOutput once and no call is performed against ProcessInput.
    /// If at least one input is specified, the component works in TRANSFORMATION mode. This means that, for each incoming row on the input, a request will be performed. Tehrefore,
    /// request parameters can depend on inputs. In this case, PrimeOutput() is still called once, but the component won't do anaything over there. The real work happens when
    /// ProcessInput() is called (multiple times). 
    /// </summary>
#if DTS130
    [DtsPipelineComponent(CurrentVersion = 3, DisplayName = "JSON Source Component", Description = "Downloads and parses a JSON file from the web.", ComponentType = ComponentType.Transform, UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_130,Version=1.1.000.0,Culture=neutral", IconResource = "com.webkingsoft.JSONSource_130.jsource.ico")]
#elif DTS120
    [DtsPipelineComponent(CurrentVersion = 3, DisplayName = "JSON Source Component", Description = "Downloads and parses a JSON file from the web.", ComponentType = ComponentType.Transform, UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_120,Version=1.1.000.0,Culture=neutral", IconResource = "com.webkingsoft.JSONSource_120.jsource.ico")]
#elif DTS110
    [DtsPipelineComponent(CurrentVersion = 3, DisplayName = "JSON Source Component", Description = "Downloads and parses a JSON file from the web.", ComponentType = ComponentType.Transform, UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_110,Version=1.1.000.0,Culture=neutral", IconResource = "com.webkingsoft.JSONSource_110.jsource.ico")]
#endif
    public class JSONSourceComponent : PipelineComponent
    {
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

        // Constant data: the following variables are prefilled during PreExecuted and will never change for the entire execution.
        private string _httpMethod;
        private RootType _dataRootType;
        private string _dataRootPath;
        private IEnumerable<HTTPParameter> _unboundHttpHeaders;
        private IEnumerable<HTTPParameter> _unboundHttpParams;
        private string _cookieVarname;
        private ParamBinding _uriBindingType;
        private string _uriBindingValue;
        private PipelineBuffer _outputbuffer = null;
        private PipelineBuffer _errorbuffer = null;
        private IDTSInput100 _parametersInputLane;
        private Dictionary<int, int> _inputCopyToOutputMaps;
        private IOMapEntry[] _iomap;
        private Dictionary<string, int> _outColsMaps;
        private DateParseHandling _dateParsePolicy = DateParseHandling.DateTime;
        private ParallelOptions _opt;
        private OperationMode _mode;
        private List<int> _warnNotified = new List<int>();
        private ErrorHandlingPolicy _networkErrorHandling = null;
        private Dictionary<int,ErrorHandlingPolicy> _httpErrorPolicies = null;

        public override void PerformUpgrade(int pipelineVersion)
        {
            AttachDebugger();

            DataType type;
            try
            {
                var value = Utils.GetVariable(VariableDispenser, "WK_DEBUG", out type);
                MessageBox.Show("Attach the debugger now! PID: " + System.Diagnostics.Process.GetCurrentProcess().Id);
            }
            catch (Exception e)
            {
                // Do nothing
            }
            
            // Obtain the current component version from the attribute.
            DtsPipelineComponentAttribute componentAttribute = (DtsPipelineComponentAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(DtsPipelineComponentAttribute), false);
            int binaryVersion = componentAttribute.CurrentVersion;
            int metaDataVersion = ComponentMetaData.Version;

            // Upgrade the metadata if needed.
            if (metaDataVersion < binaryVersion)
            {
                // Upgrade step by step every version so we are able to align to the latest.
                if (metaDataVersion == 0) // No verison to 1.1.000.XX
                {
                    // From 0 to 1 we added ParseDates value with TRUE as default.
                    // By design, we don't need to do anything thanks to the default value handling
                }

                if (metaDataVersion == 1) {
                    // In this version we changed some properties. So we need to align to the new ones.
                    var m = ComponentMetaData.CustomPropertyCollection[ComponentConstants.PROPERTY_KEY_MODEL].Value.ToString();
                    dynamic previous = JsonConvert.DeserializeObject(m);

                    JSONSourceComponentModel current = new JSONSourceComponentModel();

                    // We updated the portion of the model regarding the data source and added some advanced features. 
                    // Let's handle data-source first: we introduced UriBindingType and UriBindingValue instead of SourceUri and FromVariable.
                    JSONDataSourceModel sourceModel = new JSONDataSourceModel();

                    // We added the URIBinding type parameter. This is based on the previous combination of FromVariable.
                    if ((bool)previous.DataSource.FromVariable)
                    {
                        sourceModel.UriBindingType = ParamBinding.Variable;
                        sourceModel.UriBindingValue = (string)previous.DataSource.VariableName;
                    }
                    else {
                        sourceModel.UriBindingType = ParamBinding.CustomValue;
                        sourceModel.UriBindingValue = (string)previous.DataSource.SourceUri.ToString();
                    }

                    // The rest just remains the same
                    sourceModel.CookieVariable = (string)previous.DataSource.CookieVariable;
                    List<HTTPParameter> headers = new List<HTTPParameter>();

                    if (previous.DataSource.HttpHeaders!= null)
                        foreach (var param in previous.DataSource.HttpHeaders)
                        {
                            headers.Add((HTTPParameter)param);
                        }
                    sourceModel.HttpHeaders = headers;

                    List<HTTPParameter> parameters = new List<HTTPParameter>();
                    if (previous.DataSource.HttpParameters != null)
                        foreach (var param in previous.DataSource.HttpParameters)
                        {
                            parameters.Add((HTTPParameter)param);
                        }
                    sourceModel.HttpParameters = parameters;

                    sourceModel.WebMethod = (string)previous.DataSource.WebMethod;

                    // Save the modified portion of the model
                    current.DataSource = sourceModel;

                    // Let's Handle upgrade the Advanced feature part of the model
                    current.AdvancedSettings = JSONAdvancedSettingsModel.LoadFromJson(previous.AdvancedSettings.ToString());
                    current.AdvancedSettings.NetworkErrorPolicy = ComponentConstants.NewDefaultNetworkHandlingPolicy();
                    current.AdvancedSettings.HttpErrorPolicy = ComponentConstants.NewDefaultHttpHandlingPolicy();

                    // Now just copy remaining sub-model
                    current.DataMapping = JSONDataMappingModel.LoadFromJson(previous.DataMapping.ToString());
                    

                    // Now just overwrite the saved model
                    ComponentMetaData.CustomPropertyCollection[ComponentConstants.PROPERTY_KEY_MODEL].Value = current.ToJsonConfig();
                    
                }

                if (metaDataVersion == 2)
                {
                    // We added support for Error Outputs
                    var e = ComponentMetaData.OutputCollection.GetEnumerator();
                    bool found = false;
                    while (e.MoveNext()) {
                        if ((e.Current as IDTSOutput100).Name == ComponentConstants.NAME_OUTPUT_ERROR_LANE) {
                            found = true;
                            break;
                        }
                    }

                    if (!found) {
                        // Add it.
                        var i = ComponentMetaData.OutputCollection.New();
                        i.Name = ComponentConstants.NAME_OUTPUT_ERROR_LANE;
                        i.IsErrorOut = true;
                        
                        // Error output columns will contain some generic information about the errors that occurred during the execution, plus any input/request/filepath associated to that source
                        // -> ERROR_TYPE: can be "application", "parsing", "http", "generic"
                        // -> ERROR_DETAILS: contain some details regarding the error
                        // -> HTTP Code: used only if the error regards HTTP
                        
                        IDTSOutputColumn100 err_type = i.OutputColumnCollection.New();
                        err_type.SetDataTypeProperties(DataType.DT_WSTR, 50, 0, 0, 0);
                        err_type.Name = ComponentConstants.NAME_OUTPUT_ERROR_LANE_ERROR_TYPE;

                        IDTSOutputColumn100 err_details = i.OutputColumnCollection.New();
                        err_details.SetDataTypeProperties(DataType.DT_WSTR, 4000, 0, 0, 0);
                        err_details.Name = ComponentConstants.NAME_OUTPUT_ERROR_LANE_ERROR_DETAILS;

                        IDTSOutputColumn100 err_http_code = i.OutputColumnCollection.New();
                        err_http_code.SetDataTypeProperties(DataType.DT_UI4, 0, 0, 0, 0);
                        err_http_code.Name = ComponentConstants.NAME_OUTPUT_ERROR_LANE_ERROR_HTTP_CODE;

                    }

                }

                // At the end align the versions.
                ComponentMetaData.Version = binaryVersion;
            }

            // Forgot to upgrade the transformation on a server?
            if (metaDataVersion > binaryVersion)
            {
                throw new Exception("Runtime version of the component is out of date."
                + " Upgrading the installation can possibly solve this issue.");
            }
        }

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

            // Prepare error output column
            IDTSOutput100 errorOutput = ComponentMetaData.OutputCollection.New();
            errorOutput.IsErrorOut = true;
            errorOutput.Name = ComponentConstants.NAME_OUTPUT_ERROR_LANE;
            // The error lane is also asynchronous.
            errorOutput.SynchronousInputID = 0;

            IDTSOutputColumn100 err_type = errorOutput.OutputColumnCollection.New();
            err_type.SetDataTypeProperties(DataType.DT_WSTR, 50, 0, 0, 0);
            err_type.Name = ComponentConstants.NAME_OUTPUT_ERROR_LANE_ERROR_TYPE;

            IDTSOutputColumn100 err_details = errorOutput.OutputColumnCollection.New();
            err_details.SetDataTypeProperties(DataType.DT_WSTR, 4000, 0, 0, 0);
            err_details.Name = ComponentConstants.NAME_OUTPUT_ERROR_LANE_ERROR_DETAILS;

            IDTSOutputColumn100 err_http_code = errorOutput.OutputColumnCollection.New();
            err_http_code.SetDataTypeProperties(DataType.DT_UI4, 0, 0, 0, 0);
            err_http_code.Name = ComponentConstants.NAME_OUTPUT_ERROR_LANE_ERROR_HTTP_CODE;

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
            if (ComponentMetaData.OutputCollection.Count != 2)
            {
                ComponentMetaData.FireError(ComponentConstants.ERROR_SINGLE_OUTPUT_SUPPORTED, ComponentMetaData.Name, "This component only supports two output lanes: one for data and another for errors.", null, 0, out fireAgain);
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
                if (param.Binding==ParamBinding.InputField)
                {
                    bool found = false;
                    foreach (IDTSInputColumn100 inputcol in ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].InputColumnCollection) {
                        if (inputcol.Name == param.BindingValue) {
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        bool cancel;
                        // This column is not mapped. This will cause an error
                        ComponentMetaData.FireError(ComponentConstants.RUNTIME_ERROR_MODEL_INVALID, String.Format("HTTP parameter {0} requires input column {1} to be defined/connected. However there is no {1} column input attached.",param.Name,param.BindingValue), err, null, 0, out cancel);
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
                    throw new ModelNotFoundException();

                model = new JSONSourceComponentModel();
                m = ComponentMetaData.CustomPropertyCollection.New();
                m.Description = "Contains information about the confiuguration of the item.";
                m.Name = ComponentConstants.PROPERTY_KEY_MODEL;
                m.Value = model.ToJsonConfig();
            }
            else {
                try
                {
                    model = JSONSourceComponentModel.LoadFromJson(m.Value.ToString());
                }
                catch (Exception e) {
                    throw new BrokenModelException("Cannot parse the inner model.", e);
                }
            }

            // TODO: parametrize this
            _opt = new ParallelOptions();
            _opt.MaxDegreeOfParallelism = 4;

            return model;
        }


        private int _error_lane_output_id = -1;
        /// <summary>
        /// This function is invoked by the environment once, before data processing happens. So it's a great time to configure the basics
        /// before starting to process data. In here, we retrieve all the data that is not supposed to change later on.
        /// </summary>
        public override void PreExecute()
        {
            AttachDebugger();
            
            // Lookup basic lane ids for faster lookup afterwards in PrimeOutput. First ERROR LANE
            int errorOutputIndex=-1;
            GetErrorOutputInfo(ref _error_lane_output_id, ref errorOutputIndex);

            // Load the model and fail if no model is found
            JSONSourceComponentModel model = GetModel(true);
            
            // Save the input column index, used to parse parameters for web-requests
            _parametersInputLane = ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS];

            // Now perform the IO mapping for fast lookup during JSON Reading
            // Dictionary<name_of_column, index_of_column_in_pipeline_row>
            _iomap = model.DataMapping.IoMap.ToArray<IOMapEntry>();
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
            foreach (var inputColName in model.DataMapping.InputColumnsToCopy)
            {

                // Retrieve the index of the input column and use it as key for the fast dict. Note that for our implementation, input column names matech output column names
                // Input column index <-> OutputColumnIndex
                int input_index = BufferManager.FindColumnByLineageID(ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].Buffer, ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].InputColumnCollection[inputColName].LineageID);
                int output_index = BufferManager.FindColumnByLineageID(ComponentMetaData.OutputCollection[0].Buffer, ComponentMetaData.OutputCollection[0].OutputColumnCollection[inputColName].LineageID);
                // Map the input index to the output index
                _inputCopyToOutputMaps[input_index] = output_index;
            }

            // Configure json deserializer:
            // DateParsing is broken in json.net, since it does not take care of timezone.
            if (!model.AdvancedSettings.ParseDates)
            {
                _dateParsePolicy = DateParseHandling.None;
            }

            // Load runtime info from model and store them locally
            _httpMethod = model.DataSource.WebMethod;
            _uriBindingType = model.DataSource.UriBindingType;
            _uriBindingValue= model.DataSource.UriBindingValue;
            _dataRootType = model.DataMapping.RootType;
            _dataRootPath = model.DataMapping.JsonRootPath;
            _unboundHttpHeaders = model.DataSource.HttpHeaders;
            _unboundHttpParams = model.DataSource.HttpParameters;
            _cookieVarname = model.DataSource.CookieVariable;
            _networkErrorHandling = model.AdvancedSettings.NetworkErrorPolicy;
            _httpErrorPolicies = model.AdvancedSettings.HttpErrorPolicy;

            // Configure the operation mode. If there is at least one input, we consider ourself in TRANSFORM. If no input is attached, we are in SOURCE.
            _mode = ComponentMetaData.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].IsAttached ? OperationMode.TRANSFORM : OperationMode.SOURCE;
        }

        private Uri ResolveUri(ParamBinding bindingType, string value, PipelineBuffer inputBuffer =null) {
            // If the uri depends on a variable, get it now.
            Uri uri = null;
            switch (bindingType)
            {
                case ParamBinding.Variable:
                    DataType type;
                    object varval = Utils.GetVariable(this.VariableDispenser, value, out type);
                    var uristr = varval.ToString();
                    uri = new Uri(uristr);
                    break;
                case ParamBinding.CustomValue:
                    uri = new Uri(value);
                    break;

                case ParamBinding.InputField:
                    if (inputBuffer == null)
                        throw new ApplicationException("Bind data was called with a null inputbuffer. Contact the developer.");
                    int colIndex = BufferManager.FindColumnByLineageID(_parametersInputLane.Buffer, _parametersInputLane.GetVirtualInput().VirtualInputColumnCollection[value].LineageID);
                    //int colIndex = BufferManager.FindColumnByLineageID(_parametersInputLane.Buffer, _parametersInputLane.InputColumnCollection[value].LineageID);
                    uri = new Uri(inputBuffer[colIndex].ToString());
                    break;
                default:
                    throw new ApplicationException("Invalid model. Unsupported parambinding value.");
            }
            // Validation alredy happended. We just double check for some more runtime elements, such as variables mapped values or file presence/existance.
            if (uri.IsFile)
            {
                if (!File.Exists(uri.LocalPath))
                    throw new Exception(String.Format("File {0} does not exist.", uri.LocalPath));
            }

            return uri;
        }

        private Dictionary<string,string> ResolveParametersBinding(IEnumerable<HTTPParameter> parameters, PipelineBuffer inputBuffer=null)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (var p in parameters)
            {
                string val = null;
                switch (p.Binding) {
                    case ParamBinding.CustomValue:
                        val = p.BindingValue;
                        break;
                    case ParamBinding.InputField:
                        if (inputBuffer == null) {
                            throw new ApplicationException("BindParams was invoked with null inputbuffer and one column mapped to input. This is a logic error. Please contact the developer.");
                        }

                        int colIndex = BufferManager.FindColumnByLineageID(_parametersInputLane.Buffer, _parametersInputLane.GetVirtualInput().VirtualInputColumnCollection[p.BindingValue].LineageID);
                        //int colIndex = BufferManager.FindColumnByLineageID(_parametersInputLane.Buffer, _parametersInputLane.InputColumnCollection[].LineageID);
                        val = inputBuffer[colIndex].ToString();
                        break;
                    case ParamBinding.Variable:
                        DataType type;
                        object varval = Utils.GetVariable(this.VariableDispenser, p.BindingValue, out type);
                        val = varval.ToString();
                        break;
                    default:
                        throw new ApplicationException("Unexpected binding value for this column.");
                }

                if (p.Encode)
                    val = Uri.EscapeUriString(val);

                res[p.Name] = val;
            }

            return res;
        }
        
        /// <summary>
        /// This function is used to attach a debugger. Just declare a boolean WK_DEBUG variable with TRUE value and invoke this function
        /// where you'd like to break.
        /// </summary>
        private void AttachDebugger()
        {
            DataType type;
            try
            {
                var value = Utils.GetVariable(VariableDispenser, "WK_DEBUG", out type);
                MessageBox.Show("Attach the debugger now! PID: " + System.Diagnostics.Process.GetCurrentProcess().Id);
            }
            catch (Exception e)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// From MS Documentation:
        /// The PrimeOutput method is called when a component has at least one output, attached to a downstream component through an IDTSPath100 object, and the SynchronousInputID property of the output is zero. 
        /// The PrimeOutput method is called for source components and for transformations with asynchronous outputs. 
        /// Unlike the ProcessInput method described below, the PrimeOutput method is only called once for each component that requires it.
        /// </summary>
        /// <param name="outputs">Numbers of outputIDs provided</param>
        /// <param name="outputIDs">IDs of outputs</param>
        /// <param name="buffers">Buffers associated to outputs</param>
        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            // In case of SOURCE mode, we need to handle all the work here. Otherwise, in TRANSFORMATION mode, we just have to save a pointer
            // to the outputbuffer and do the work in ProcessInput.

            // Lookup buffers. We espect one error lane and one output lane. Indexes have been pre-calculated within PRE-EXECUTE
            for (int i=0;i<outputs;i++) {
                if (outputIDs[i] == _error_lane_output_id) {
                    _errorbuffer = buffers[i];
                } else {
                    // We simply assume that if the lane is not for error, then it regards output.
                    _outputbuffer = buffers[i];
                }
            }

            if (_mode == OperationMode.SOURCE)
            {
                // The following while loop is used to retry connection in case of errors. We brake when we
                // reach maximum error limit.
                while (true)
                {
                    try
                    {
                        Uri uri = ResolveUri(_uriBindingType, _uriBindingValue);

                        bool cancel = false;

                        // So we are clear to proceed with the HTTP request.
                        ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Component is running in SOURCE MODE.", null, 0, ref cancel);

                        bool downloaded;
                        ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, String.Format("Executing request {0}", uri.ToString()), null, 0, ref cancel);
                        string fname = GetFileFromUri(uri, null, null, out downloaded);

                        ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, String.Format("Temp json downloaded to {0}. Parsing json now...", fname), null, 0, ref cancel);

                        // Process data according to IOMappings
                        using (StreamReader sr = new StreamReader(File.Open(fname, FileMode.Open)))
                            ProcessInMemory(sr, _dataRootType, null, _outputbuffer);

                        ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Json parsed correctly.", null, 0, ref cancel);
                        
                        // TODO: UI option to prevent removal of downloaded data?
                        if (downloaded)
                        {
                            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, String.Format("Removing downloaded json file {0}.", fname), null, 0, ref cancel);
                            File.Delete(fname);
                        }
                    }
                    catch (DataCollectionException e)
                    {
                        // TODO: Fire warning.
                        ComponentMetaData.FireWarning(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, String.Format("An error occurred during json gathering. This might be a consequence of a variety of reasons, including network problems. Error was: {0}. Details: {1}",e.Message,e.StackTrace), null, 0);

                        _errorbuffer.AddRow();
                        // First two places reagard Error code and column index. We cannot mark any specific cause of the problem, so just put 0-0 here.
                        _errorbuffer[0] = 0;
                        _errorbuffer[1] = 0;
                        _errorbuffer[2] = "DATA_COLLECTION";
                        // Truncate error message if too long
                        _errorbuffer[3] = e.Message.Length > 4000 ? e.Message.Substring(0,4000):e.Message;
                        _errorbuffer[4] = null;  // No http response, so far.

                        if (!_networkErrorHandling.ShouldContinue())
                        {
                            ComponentMetaData.FireWarning(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, "Maximum number of failures reached. Aborting the execution.", null, 0);
                            break;
                        }
                        else
                        {
                            ComponentMetaData.FireWarning(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, String.Format("According to ErrorPolicy we should try again. Not aborting (yet), instead sleeping {0} seconds.", _networkErrorHandling.SleepTimeInSeconds), null, 0);
                            Thread.Sleep(_networkErrorHandling.SleepTimeInSeconds);
                        }
                    }
                    catch(BadHttpCodeException e)
                    {
                        // TODO: Fire warning.
                        ComponentMetaData.FireWarning(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, String.Format("The http response provided by the server wasn't successful. Error was: {0}. Details: {1}", e.Message, e.StackTrace), null, 0);

                        _errorbuffer.AddRow();
                        // First two places reagard Error code and column index. We cannot mark any specific cause of the problem, so just put 0-0 here.
                        _errorbuffer[0] = 0;
                        _errorbuffer[1] = 0;
                        _errorbuffer[2] = "HTTP_BAD_RESPONSE_CODE";
                        // Truncate error message if too long
                        _errorbuffer[3] = e.Message.Length > 4095 ? e.Message.Substring(0, 4095) : e.Message;
                        
                        // Check if the user has provided any specific error handling regarding this code, otherwise apply same strategy of datagathering error.
                        int code = e.GetResponseCode();
                        _errorbuffer[4] = code;

                        ErrorHandlingPolicy p = _networkErrorHandling;
                        if (_httpErrorPolicies.ContainsKey(code)) {
                            bool cancel = false;
                            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Found HTTP error handling policy for error code "+code, null, 0, ref cancel);
                            p = _httpErrorPolicies[code];
                        }

                        if (!p.ShouldContinue())
                        {
                            ComponentMetaData.FireWarning(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, "Maximum number of failures reached. Aborting the execution.", null, 0);
                            break;
                        }
                        else
                        {
                            ComponentMetaData.FireWarning(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, String.Format("According to ErrorPolicy we should try again. Not aborting (yet), instead sleeping {0} seconds.", p.SleepTimeInSeconds), null, 0);
                            Thread.Sleep(p.SleepTimeInSeconds);
                        }
                    }
                    // We intentionally avoid to catch any other exception, such as bad json parsing and so on. Those errors must case failure of entire component,
                    // which is normal when the user fails to correctly parse json.
                }

                // Close output lane
                _outputbuffer.SetEndOfRowset();

                // Close the error lane.
                _errorbuffer.SetEndOfRowset();

            }
            else {
                bool cancel = false;
                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Component is running in TRANSFORMATION MODE.", null, 0, ref cancel);
            }
        }

        /// <summary>
        /// This function takes care of retrieving the file data to be parsed. It alo handles retries.
        /// </summary>
        /// <param name="downloaded">True if the file was downloaded, false otherwise.</param>
        /// <returns>The path of the file to parse</returns>
        private string GetFileFromUri(Uri uri, Dictionary<string,string> parameters, Dictionary<string,string> headers, out bool downloaded)
        {
            // TODO: retry attempts.
            if (uri.IsFile)
            {
                downloaded = false;
                return uri.LocalPath;
            }
            else
            {
                CookieContainer cookies = new CookieContainer();
                downloaded = true;
                if (!String.IsNullOrEmpty(_cookieVarname))
                {
                    DataType type;
                    cookies = Utils.GetVariable(VariableDispenser, _cookieVarname, out type) as CookieContainer;
                }

                var res = Utils.DownloadJson(this.VariableDispenser, uri, _httpMethod, parameters, headers, cookies);
                
                // If the cookie container parameter was not null, assign the container to the variable
                if (!String.IsNullOrEmpty(_cookieVarname))
                {
                    IDTSVariables100 vars = null;
                    try
                    {
                        VariableDispenser.LockOneForWrite(_cookieVarname, ref vars);
                        vars[_cookieVarname].Value = cookies;
                    }
                    finally
                    {
                        if (vars != null)
                            vars.Unlock();
                    }

                }

                return res;
            }
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
            // This method is invoked only when we are in TRANSFORMATION MODE, i.e. we have input-dependant parameters to handle. 
            // Thus, in here we need to BIND parameters to respective inputs, perform the HTTP request and parse data.
            // Note: since we only support one input lane, we assume inputID/inputbuffers are always referring to that input lane.
            
            // This method is invoked only when the component has some inputs to process. Otherwise, if no input has been specified, the PrimeOutput will handle all the job.
            bool cancel = false;
            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Processing inputs...", null, 0, ref cancel);
            try
            {
                bool downloaded;
                ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Detected input lane attached. Executing in BATCH mode.", null, 0, ref cancel);
                while (inputbuffer.NextRow())
                {
                    // For every input row, bind the parameters
                    Uri uri = ResolveUri(_uriBindingType, _uriBindingValue, inputbuffer);

                    // Perform the request with appropriate inputs as HTTP params...
                    var headers = ResolveParametersBinding(_unboundHttpHeaders);
                    var parameters = ResolveParametersBinding(_unboundHttpParams);

                    // TODO FireInfo so that we log each request...
                    string fname = GetFileFromUri(uri, parameters, headers, out downloaded);
                    
                    // Process data according to IOMappings
                    using (StreamReader sr = new StreamReader(File.Open(fname, FileMode.Open)))
                        ProcessInMemory(sr, _dataRootType, inputbuffer, _outputbuffer);

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
                        reader.DateParseHandling = _dateParsePolicy;
                        if (rootType == RootType.JsonObject)
                        {
                            o = JObject.Load(reader);
                            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Object loaded.", null, 0, ref cancel);
                        }
                        else {
                            o = JArray.Load(reader);
                            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Array loaded.", null, 0, ref cancel);
                        }
                    }

                    // Get all the tokens returned by the XPath string specified
                    if (_dataRootPath == null)
                        _dataRootPath = "";

                    // Navigate to the relative Root.
                    IEnumerable<JToken> els =  o.SelectTokens(_dataRootPath);
                    int rootEls = els.Count();
                    ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Array: loaded " + rootEls + " tokens.", null, 0, ref cancel);

                    // TODO Warning if no elements are found

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

        // This is a possible alternative to current implementation. Maybe in future we will swap to something similar. 
        // It makes use of recursion for automaticaly exploding nested javascript. However this introduces some "complexity" that standard
        // user does not necessarily understand. Thus, for the moment, we swap back to classic implementation.
        /*
        private int ProcessObject(JObject obj, PipelineBuffer inputbuffer) {
            object[] prev_values = new object[_iomap.Length];
            int start_index = 0;

            return ProcessColumns(obj, inputbuffer, prev_values, start_index);
        }

        private int ProcessColumns(JObject obj, PipelineBuffer inputbuffer, object[] prev_values, int start_index) {

            // Base case: we reached the end of recursion (leafs).
            if (start_index == (_iomap.Length))
            {
                // Add the output row
                var buff = AddOutputRow(inputbuffer);

                // Copy the temporary buffer into the actual row
                for(int i=0;i<prev_values.Length;i++) {
                    try
                    {
                        buff[i] = prev_values[i];
                    }
                    catch (DoesNotFitBufferException ex)
                    {
                        IOMapEntry col_e = _iomap.ElementAt(i);
                        bool fireAgain = false;
                        ComponentMetaData.FireError(ComponentConstants.ERROR_INVALID_BUFFER_SIZE, ComponentMetaData.Name, String.Format("Maximum size of column {0} is smaller than provided data. Please increase buffer size.", col_e.OutputColName), null, 0, out fireAgain);
                        throw ex;
                    }
                }

                return 1;
            }

            // Otherwise we still have some columns to parse
            IOMapEntry e = _iomap.ElementAt(start_index);

            // Quickly retrieve the coulmn index of the mapped output to this column entry
            int colIndex = _outColsMaps[e.OutputColName];
            
            // Navigate to the wanted value
            var tokens = obj.SelectTokens(e.InputFieldPath, false);
            int count = tokens.Count();

            // In case no result is obtained, fill the current temp buffer with a null and invoke simple recursion
            if (count == 0)
            {
                // We obtained no result. This may be an error of the developer, so it is a good idea to fire a warning.
                // For now, we do not crash. In future we might implement row redirection to error output. Just provide NULL value.
                if (!_warnNotified.Contains(colIndex))
                {
                    _warnNotified.Add(colIndex);
                    ComponentMetaData.FireWarning(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, String.Format("No value has been found when parsing jsonpath {0} on column {1}. Is the jsonpath correct?", e.InputFieldPath, e.OutputColName), null, 0);
                }
                
                // Fill the null value and invoke simple recursion
                prev_values[start_index] = null;
                return ProcessColumns(obj, inputbuffer, prev_values, start_index + 1);
            }
            else if (count == 1)
            {
                // If we only have one value retrieve it directly and invoke simple recursion
                try
                {
                    // Fill the value and invoke simple recursion
                    prev_values[start_index] = tokens.ElementAt(0); ;
                    return ProcessColumns(obj, inputbuffer, prev_values, start_index + 1);

                }
                catch (DoesNotFitBufferException ex)
                {
                    bool fireAgain = false;
                    ComponentMetaData.FireError(ComponentConstants.ERROR_INVALID_BUFFER_SIZE, ComponentMetaData.Name, String.Format("Maximum size of column {0} is smaller than provided data. Please increase buffer size.", e.OutputColName), null, 0, out fireAgain);
                    throw ex;
                }
            }
            else
            {
                // This case requires explosions. Invoke recursion for each value.
                int res = 0;
                foreach (JToken t in tokens)
                {
                    prev_values[start_index] = t;
                    res += ProcessColumns(obj, inputbuffer, prev_values, start_index + 1);
                }

                return res;
            }

            // We should never hit this place
            throw new ApplicationException("This is a design error. Contact the developer.");

        }*/
        
        private int ProcessObject(JObject obj, PipelineBuffer inputbuffer)
        {
            int res = 0;

            // Each successful parsed object will produce a new output line, so we allocate now a buffer
            // that we will fill during parsing. We also pass the input buffer, because some columns have to be
            // prefilled with inputs (copied values)
            var buffer = AddOutputRow(inputbuffer);
            
            for(int i=0;i<_iomap.Count();i++) {
                IOMapEntry e = _iomap.ElementAt(i);
                // Quickly retrieve the coulmn index of the mapped output to this column entry
                int colIndex = _outColsMaps[e.OutputColName];

                // Prefill the outputvalue as NULL. In case we find nothing, we will produce a NULL element, instead of an empty string
                object val = null;

                // The following API call may produce multiple outputs. If that is the case, we manually build a json array and serialize that one into a string.
                var tokens = obj.SelectTokens(e.InputFieldPath, false);
                int count = tokens.Count();
                if (count == 0)
                {
                    // We obtained no result. This may be an error of the developer, so it is a good idea to fire a warning.
                    // For now, we do not crash. In future we might implement row redirection to error output. Just provide NULL value.
                    if (!_warnNotified.Contains(colIndex))
                    {
                        _warnNotified.Add(colIndex);
                        ComponentMetaData.FireWarning(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, String.Format("No value has been found when parsing jsonpath {0} on column {1}. Is the jsonpath correct?", e.InputFieldPath, e.OutputColName), null, 0);
                    }
                    val = null;
                }
                else if (count == 1)
                {
                    // If we only have one value retrieve it directly.
                    try
                    {
                        val = tokens.ElementAt(0);
                    }
                    catch (DoesNotFitBufferException ex)
                    {
                        bool fireAgain = false;
                        ComponentMetaData.FireError(ComponentConstants.ERROR_INVALID_BUFFER_SIZE, ComponentMetaData.Name, String.Format("Maximum size of column {0} is smaller than provided data. Please increase buffer size.", e.OutputColName), null, 0, out fireAgain);
                        throw ex;
                    }
                }
                else
                {
                    if (!_warnNotified.Contains(colIndex))
                    {
                        _warnNotified.Add(colIndex);
                        ComponentMetaData.FireWarning(ComponentConstants.RUNTIME_GENERIC_ERROR, ComponentMetaData.Name, String.Format("Multiple values have been found when parsing jsonpath {0} on column {1}. This will led to line explosion, so I won't explode this here to save memory. Put a filter in pipeline to explode the lines, if needed.", e.InputFieldPath, e.OutputColName), null, 0);
                    }

                    // This case requires explosions. We cannot perform it here, so we output raw json.
                    JArray arr = new JArray();
                    foreach(var t in tokens)
                    {
                        arr.Add(t);
                    }
                    val = arr;
                }
                
                // Now we might act differently in accordance to the type of parsing requested by the user.
                // In practice we handle two cases: output as RAW/Strings or Strong-Typed outputs. In the first case,
                // we simply transform every output into a string. In the latter, we try to parse the value, such as
                // numbers, booleans and dates.
                switch (e.OutputJsonColumnType)
                {
                    // First case: map the inputs to a simple string.
                    case JsonTypes.RawJson:
                    case JsonTypes.String:
                        // If the output has to be a string, just convert what we have so far into a string. Do not take care of explosion nor parsing.
                        if (val != null)
                            val = val.ToString();
                        break;
                    default:
                        // In all other cases, we might need data-parsing. Fortunately this is handled automaticaly by the assignment operation. So we do nothing in here.
                        break;
                }

                // Now assign to the column index the value we previously extracted.
                try
                {
                    buffer[colIndex] = val;
                }
                catch (DoesNotFitBufferException ex)
                {
                    bool fireAgain = false;
                    ComponentMetaData.FireError(ComponentConstants.ERROR_INVALID_BUFFER_SIZE, ComponentMetaData.Name, String.Format("Maximum size of column {0} is smaller than provided data. Please increase buffer size.", e.OutputColName), null, 0, out fireAgain);
                    throw ex;
                }
            };

            res++;

            return res;
        }

        private int ProcessArray(JArray arr, PipelineBuffer inputbuffer)
        {
            bool cancel = false;
            ComponentMetaData.FireInformation(1000, ComponentMetaData.Name, "Processing Array...", null, 0, ref cancel);
            int count = 0;
            foreach (JObject obj in arr)
            {
                count+=ProcessObject(obj, inputbuffer);
            }

            // If there is no item in this array, add the NULL inputs.
            if (count == 0)
                AddOutputRow(inputbuffer);
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

    enum OperationMode {
        TRANSFORM,
        SOURCE
    }

}

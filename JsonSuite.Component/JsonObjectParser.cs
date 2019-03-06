using com.webkingsoft.JsonSuite.Component.Model;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if LINQ_SUPPORTED
using System.Threading.Tasks;
using System.Windows.Forms;
#endif

namespace com.webkingsoft.JsonSuite.Component
{
    // TODO: add Custom UI, document the class
    [DtsPipelineComponent(
        UITypeName = "com.webkingsoft.JsonSuite.UI.JsonObjectParserUI, com.webkingsoft.JsonSuite.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=42a2313e1269904d",
        CurrentVersion = 1, 
        DisplayName = "JSON Object Parser", 
        Description = "Parses a JSON object into multiple columns.",
        ComponentType = ComponentType.Transform, 
        IconResource = "com.webkingsoft.JsonSuite.Component.Icons.JsonObjectParser.ico")
    ] 
    public class JsonObjectParser : PipelineComponent
    {
        public static readonly string INPUT_LANE_NAME = "Raw json input";
        public static readonly string OUTPUT_LANE_NAME = "Object Elements";
        public static readonly string ERROR_LANE_NAME = "Error elements";
        public static readonly DataType[] SUPPORTED_JSON_DATA_TYPE = new DataType[] {
            DataType.DT_NTEXT,
            DataType.DT_TEXT,
            DataType.DT_WSTR,
            DataType.DT_STR
        };

        // Public overrided methods - Design time
        public override void ProvideComponentProperties()
        {
            // This method is invoked by the design time when the component is added to the data flow for the very first time. 
            // In here, we need to configure the input and the output lanes and set the synchronization type.

            // Clear all inputs and custom props, plus setup outputs
            base.RemoveAllInputsOutputsAndCustomProperties();

            // Setup the input lane. 
            var input = ComponentMetaData.InputCollection.New();
            input.Name = INPUT_LANE_NAME;
            input.ErrorRowDisposition = DTSRowDisposition.RD_RedirectRow;

            // Setup the output lane. Every lane must be named, otherwise it won't work on the runtime.
            // We assume only one output will be available + 1 error output lane.
            var output = ComponentMetaData.OutputCollection.New();
            output.Name = OUTPUT_LANE_NAME;
            output.ExclusionGroup = 1;

            // Configure Error output
            ComponentMetaData.UsesDispositions = true;
            var error = ComponentMetaData.OutputCollection.New();
            error.Name = ERROR_LANE_NAME;
            error.IsErrorOut = true;
            error.ExclusionGroup = 1;

            // The Objectparser produces one line for every input, therefore it's a synchronous
            // trasnformation component. 
            output.SynchronousInputID = input.ID;
            error.SynchronousInputID = input.ID;

            // Custom property initialization
            var props = new ObjectParserProperties();
            props.CopyToComponent(ComponentMetaData.CustomPropertyCollection);

            // The current implementation does not handle external metadata validation.
            // Therefore we must inform the design time about that.
            // TODO: Change this once we implement external metadata validation
            ComponentMetaData.ValidateExternalMetadata = false;
        }

        public override DTSValidationStatus Validate()
        {
            // ------ Input checks ------
            var inputValidation = ValidateInput();
            if (inputValidation != DTSValidationStatus.VS_ISVALID)
                return inputValidation;

            // ------ Output checks ------
            var outputValidation = ValidateOutput();
            if (outputValidation != DTSValidationStatus.VS_ISVALID)
                return outputValidation;

            // ------ Connection checks ------
            // No external connection is expected for now.

            // ------ Custom properties checks ------
            ObjectParserProperties props;
            var customPropertiesValidation = ValidateCustomProperties(out props);
            if (customPropertiesValidation != DTSValidationStatus.VS_ISVALID)
                return customPropertiesValidation;

            // ------ Usage type -------
            var columnUsageTypeValidation = ValidateColumnsUsageType(props);
            if (columnUsageTypeValidation != DTSValidationStatus.VS_ISVALID)
                return columnUsageTypeValidation;
            
            // As very last step, invoke the base validation implementation which will check that every input 
            // column is associated to an output of the upstream component.
            return base.Validate();
        }
        
        public override void ReinitializeMetaData()
        {
            // Remove the previous output columns and the the relative metadata columns
            IDTSOutput100 output = ComponentMetaData.OutputCollection[OUTPUT_LANE_NAME];
            output.ExternalMetadataColumnCollection.RemoveAll();
            output.OutputColumnCollection.RemoveAll();

            // Make sure that every input column is treated as READONLY (for not interesting ones)
            // or READWRITE (for json array ones).
            AlignVirtualInputsToInputs();

            // Add them back from scratch
            CreateOutputAndMetaDataColumns(output);
        }
        
        public override void OnInputPathAttached(int inputID)
        {
            // We only have one input lane in this component, so we assume inputId that one.
            AlignVirtualInputsToInputs();
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

        // Private methods (design time)
        private void CreateOutputAndMetaDataColumns(IDTSOutput100 outlane)
        {
            ObjectParserProperties props = new ObjectParserProperties();
            props.LoadFromComponent(ComponentMetaData.CustomPropertyCollection);

            // Create an output column for every json attribute to parse
            foreach (var columnConf in props.AttributeMappings) {
                IDTSOutput100 outLane = ComponentMetaData.OutputCollection[OUTPUT_LANE_NAME];
                var outCol = outLane.OutputColumnCollection.New();
                outCol.Name = columnConf.Key;
                var opts = columnConf.Value.AttributeMappingOptions;
                outCol.SetDataTypeProperties(
                    opts.GetSSISDataType(),
                    opts.SSISLength, 
                    opts.SSISPrecision, 
                    opts.SSISScale, 
                    opts.SSISCodePage);
            }
        }

        private DTSValidationStatus ValidateColumnsUsageType(ObjectParserProperties props)
        {
            bool pbCancel = false;
            // Make sure that all the input columns are marked as DT_IGNORED, except the one that we 
            // read for parsing JSON (which must be in READONLY_MODE). 
            var inputLane = ComponentMetaData.InputCollection[INPUT_LANE_NAME];
            foreach (IDTSInputColumn100 col in inputLane.InputColumnCollection)
            {
                // If this is the selected json raw input, it must be READONLY
                if (props.RawJsonInputColumnName == col.Name && col.UsageType != DTSUsageType.UT_READONLY)
                {
                    ComponentMetaData.FireError(0, ComponentMetaData.Name, "Column " + props.RawJsonInputColumnName + " must be set as RO.", "", 0, out pbCancel);
                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                }
                else if (props.RawJsonInputColumnName != col.Name && col.UsageType != DTSUsageType.UT_IGNORED)
                {
                    ComponentMetaData.FireError(0, ComponentMetaData.Name, "Column " + col.Name + " must be set as IGNORED.", "", 0, out pbCancel);
                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                }
            }
            return DTSValidationStatus.VS_ISVALID;
        }

        private DTSValidationStatus ValidateCustomProperties(out ObjectParserProperties properties)
        {
            // TODO: handle validation for external metadata

            bool pbCancel = false;

            properties = new ObjectParserProperties();
            properties.LoadFromComponent(ComponentMetaData.CustomPropertyCollection);

            // The user should have specified a valid input column where to fetch json from
            if (string.IsNullOrEmpty(properties.RawJsonInputColumnName))
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Invalid input column specified.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISBROKEN;
            }

            // The selected input column name must reflect one of the available inputs
            // Moreover, the selected input column must be a valid STRING/TEXT type.
            bool found = false;
            foreach (IDTSInputColumn100 col in ComponentMetaData.InputCollection[INPUT_LANE_NAME].InputColumnCollection)
            {
                if (col.Name == properties.RawJsonInputColumnName)
                {
                    found = true;

                    // Check the data type of the column
                    if (!SUPPORTED_JSON_DATA_TYPE.Contains(col.DataType))
                    {
                        ComponentMetaData.FireError(0, ComponentMetaData.Name, "The selected input column must be in textual format (TEXT, NTEXT, STR, WSTR).", "", 0, out pbCancel);
                        return DTSValidationStatus.VS_ISBROKEN;
                    }

                    break;
                }
            }

            if (!found)
            {
                // Though, if the specified column is available within the virtual inputs, we can rebuild
                // the metadata.
                foreach (IDTSVirtualInputColumn100 vCol in ComponentMetaData.InputCollection[INPUT_LANE_NAME].GetVirtualInput().VirtualInputColumnCollection) {
                    if (vCol.Name == properties.RawJsonInputColumnName) {
                        ComponentMetaData.FireError(0, ComponentMetaData.Name, "Metadata changed.", "", 0, out pbCancel);
                        return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                    }
                }

                ComponentMetaData.FireError(0, ComponentMetaData.Name, "The specified input column \"" + properties.RawJsonInputColumnName +
                    "\" is not available among the columns of the input lane \"" + INPUT_LANE_NAME + "\"", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISBROKEN;
            }

            // TODO: Make sure there are no collisions on either input columns and outputColumns

            // Now, for every expected attribute to extract, there must be an associated output column.


            // TODO: check if there is any other output column. If so, it means that the output is out-of synch
            // so we need to rebuild the metadata.

            return DTSValidationStatus.VS_ISVALID;
        }

        private DTSValidationStatus ValidateInput()
        {
            // TODO: we might be able to return NEEDS_NEW_METADATA for some edge-cases. For now, just return CORRUPT.
            bool pbCancel = false;
            IDTSInput100 inputLane = null;

            // Only one input lane is expected
            if (ComponentMetaData.InputCollection.Count != 1)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Incorrect number of inputs.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            // Input lane must be named as INPUT_LANE_NAME
            try
            {
                inputLane = ComponentMetaData.InputCollection[INPUT_LANE_NAME];
                if (inputLane == null)
                    throw new Exception("Missing input lane");
            }
            catch (Exception e)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Incorrect number of inputs.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            // If a new column is added/removed from the upstream, we need to rebuild the metadata
            // accordingly. 
            if (!ComponentMetaData.AreInputColumnsValid)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Column metadata has changed.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_NEEDSNEWMETADATA;
            }

            return DTSValidationStatus.VS_ISVALID;
        }

        private DTSValidationStatus ValidateOutput()
        {
            // TODO: we might be able to return NEEDS_NEW_METADATA for some edge-cases. For now, just return CORRUPT.
            bool pbCancel = false;

            // We expect one output lane and one error lane
            if (ComponentMetaData.OutputCollection.Count != 2)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Incorrect number of outputs.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            // Output lane must be named as OUTPUT_LANE_NAME
            IDTSOutput100 outlane;
            try
            {
                outlane = ComponentMetaData.OutputCollection[OUTPUT_LANE_NAME];
                if (outlane == null)
                    throw new Exception("Missing output lane " + OUTPUT_LANE_NAME);
            }
            catch (Exception e)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Missing output lane " + OUTPUT_LANE_NAME, "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (outlane.IsErrorOut)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "The output lane " + OUTPUT_LANE_NAME + " should be used for output and not for error.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            // Error lane must be named as ERROR_LANE_NAME
            IDTSOutput100 errlane;
            try
            {
                errlane = ComponentMetaData.OutputCollection[ERROR_LANE_NAME];
                if (errlane == null)
                    throw new Exception("Missing output lane " + ERROR_LANE_NAME);
            }
            catch (Exception e)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Missing output lane " + ERROR_LANE_NAME, "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (!errlane.IsErrorOut)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "The output lane " + ERROR_LANE_NAME + " should be used for errors and not for result.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            return DTSValidationStatus.VS_ISVALID;
        }
        
        private void AlignVirtualInputsToInputs()
        {
            ObjectParserProperties props = new ObjectParserProperties();
            props.LoadFromComponent(ComponentMetaData.CustomPropertyCollection);

            // Set every input column as ignored, except for the one that we will use as JSON input, 
            // which must be set as readonly.
            IDTSInput100 input = ComponentMetaData.InputCollection[INPUT_LANE_NAME];
            var vInput = ComponentMetaData.InputCollection[INPUT_LANE_NAME].GetVirtualInput();
            foreach (IDTSVirtualInputColumn100 vCol in vInput.VirtualInputColumnCollection)
            {
                if (vCol.Name == props.RawJsonInputColumnName)
                    SetUsageType(input.ID, vInput, vCol.LineageID, DTSUsageType.UT_READONLY);
                else
                    SetUsageType(input.ID, vInput, vCol.LineageID, DTSUsageType.UT_IGNORED);
            }
        }
        
        public override void PerformUpgrade(int pipelineVersion)
        {
            // TODO: change this!
            //base.PerformUpgrade(pipelineVersion);
        }

        // Public overrided methods - Runtime
        private Dictionary<int, int> _ioColumnMapping;
        private int _jsonInputColumnIndex = -1;
        private int _jsonOutputColumnIndex = -1;
        private PipelineBuffer _outputBuffer;
        private PipelineBuffer _errorBuffer;

        delegate string JsonStringExtractor(PipelineBuffer inputBuffer);
        private JsonStringExtractor _extractor;

        // This array will contain the values from input lane that should be copied to the
        // output
        private object[] copyVals = null;

        public override void PreExecute()
        {
#if DEBUG
            MessageBox.Show("Attach the debugger now! PID: " + System.Diagnostics.Process.GetCurrentProcess().Id);
#endif
            ArraySplitterProperties config = new ArraySplitterProperties();
            config.LoadFromComponent(ComponentMetaData.CustomPropertyCollection);

            // This method is invoked once before the component starts doing its job. 
            // This is the perfect time to build up the fast-access data we will use later on during execution.
            // In particualr, we need to build a dictionary that holds up destination buffer index for every input
            // column index. 
            _ioColumnMapping = new Dictionary<int, int>();

            // To do so, we lookup every output column name with the relative input column name
            IDTSOutput100 output = ComponentMetaData.OutputCollection[OUTPUT_LANE_NAME];
            IDTSInput100 input = ComponentMetaData.InputCollection[INPUT_LANE_NAME];

            // TODO: handle the case some mapping is not found.
            // TODO: enforce the mapping check into VALIDATE() method.
            foreach(IDTSInputColumn100 iCol in input.InputColumnCollection) {
                bool isJsonIndex = false;
                int iindex = BufferManager.FindColumnByLineageID(input.Buffer, iCol.LineageID);
                if (iCol.Name == config.ArrayInputColumnName)
                {
                    isJsonIndex = true;
                    _jsonInputColumnIndex = iindex;

                    // Select the functio that will take care of extracting the json from the input buffer.
                    switch (iCol.DataType)
                    {
                        case DataType.DT_TEXT:
                            _extractor = AnsiDecode;
                            break;
                        case DataType.DT_NTEXT:
                            _extractor = UnicodeDecode;
                            break;
                        case DataType.DT_STR:
                        case DataType.DT_WSTR:
                            _extractor = SimpleStringExtractor;
                            break;
                        default:
                            throw new Exception("Unhandled data type for json array column.");
                    }
                }

                foreach (IDTSOutputColumn100 oCol in output.OutputColumnCollection) {
                    if (iCol.Name == oCol.Name) {
                        int oindex = BufferManager.FindColumnByLineageID(output.Buffer, oCol.LineageID);
                        _ioColumnMapping.Add(iindex, oindex);

                        if (isJsonIndex) {
                            _jsonOutputColumnIndex = oindex;
                        }

                        break;
                    }
                }
            }
        }

        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            // This is an Asynch transformation component, thus both PrimeOutput and ProcessInput are 
            // invoked by the runtime. In this method, we only save the refereces to output/buffers 
            // locally for later processing.
            int outputId = ComponentMetaData.OutputCollection[OUTPUT_LANE_NAME].ID;
            int errorId = ComponentMetaData.OutputCollection[ERROR_LANE_NAME].ID;
            for (int i = 0; i < outputs; i++) { 
                if (outputIDs[i] == outputId)
                        _outputBuffer = buffers[i];
                else if (outputIDs[i] == errorId)
                        _errorBuffer = buffers[i];
            }
        }

        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {
            // This method processes the data coming into some input.
            // However we only have one possible input, so we don't need to check the inputId.
            if (!buffer.EndOfRowset) {
                while (buffer.NextRow()) {
                    ProcessInputRow(buffer);
                }
            }

            // TODO: ensure there is at least one output lane attached.
            _outputBuffer?.SetEndOfRowset();
            _errorBuffer?.SetEndOfRowset();
        }

        public override string DescribeRedirectedErrorCode(int iErrorCode)
        {
            // Return the error description associated to this code.
            ERROR_CODES error = (ERROR_CODES)iErrorCode;
            DescriptionAttribute attr = error.GetType().GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            if (attr != null)
            {
                return attr.Description;
            }
            else
            {
                return "Unknown error code: no description is available.";
            }
        }

        private void ProcessInputRow(PipelineBuffer inputBuffer) {
            bool pbCancel = false;

            // In here, we need to read the input buffer, select the column with JSON array,
            // unpack the json elements in separate output rows. Every row will be copied 
            // from the input buffer and will hold one specific element of the array. 
            // For instance, assume the following row is received into the input buffer:
            // Name     | Surname | Age | Hobbies
            // Alberto  | Geniola | 28  | ["Coding", "Gaming", "Cooking"]
            // ...
            // 
            // The expected output would be:
            // Name     | Surname | Age | Hobbies:
            // Alberto  | Geniola | 28  | "Coding"
            // Alberto  | Geniola | 28  | "Gaming"
            // Alberto  | Geniola | 28  | "Cooking"
            // ...

            // Loop over all the columns in the buffer and prepare the items to be used to fill the
            // output buffer (e.g. all the input values except the json array)
            copyVals = new object[inputBuffer.ColumnCount];
            for (int i = 0; i < inputBuffer.ColumnCount; i++) {
                // Skip the json input column.
                if (i == _jsonInputColumnIndex)
                    continue;

                // Retrieve the mapped output column index on the output buffer
                int mappedOutputBufferIndex = _ioColumnMapping[i];

                copyVals[mappedOutputBufferIndex] = inputBuffer[i];
            }

            // Now parse the input json array and, for each item in the array, produce a new output line.
            string rawJson = _extractor(inputBuffer);
            using (var jtr = new JsonTextReader(new StringReader(rawJson)))
            {
                // Start reading the array. The very first token we expect is the array-start element.
                try
                {
                    if (!jtr.Read())
                    {
                        throw new NullOrEmptyArrayException();
                    }

                    // We expect "ArrayStart" element. If that differs, then the input is not well formed.
                    if (jtr.TokenType != JsonToken.StartArray)
                    {
                        ComponentMetaData.FireError(0, ComponentMetaData.Name, "The json string should start with the '[' symbol, but it does not.", "", 0, out pbCancel);
                        _errorBuffer.AddRow();
                        _errorBuffer.SetInt32(0, (int)ERROR_CODES.ERROR_MISSING_START_ARRAY);
                    }

                    // Now, we expect a variable number number of tokens 
                    int targetDepth = jtr.Depth;

                    JsonLoadSettings settings = new JsonLoadSettings();
                    JArray array = JArray.ReadFrom(jtr) as JArray;
                    int processedTokens = 0;
                    foreach (JToken token in array)
                    {
                        SendRowToOutputBuffer(token.ToString(Formatting.None));
                        processedTokens++;
                    }

                    // If there was no item in the array or if the array was empty, we still need to 
                    // provide one output row. So we do it here.
                    if (processedTokens == 0)
                    {
                        SendRowToOutputBuffer(null);
                    }

                    // Make sure there is nothing more to read
                    if (jtr.Read())
                    {
                        // This should not happen! There is no need to fail here. Just throw a warning.
                        ComponentMetaData.FireWarning(0, ComponentMetaData.Name, "The JSON array did contain some more data after the closing bracket. ']'.", "", 0);
                    }
                }
                catch (NullOrEmptyArrayException ex)
                {
                    // If we failed to read or there is nothing to read, just throw a warning and cotinue.
                    ComponentMetaData.FireWarning(0, ComponentMetaData.Name, "No JSON data recevied", "", 0);
                }
                catch (Exception ex) {
                    // Any other failure/error should cause row redirection
                    ComponentMetaData.FireError((int)ERROR_CODES.ERROR_BAD_JSON, ComponentMetaData.Name, "Json data \""+ rawJson +"\" could not be parsed as expected. Error: " + ex.Message, "", 0, out pbCancel);
                    SendErrorRow((int)ERROR_CODES.ERROR_BAD_JSON);
                }
            }
        }
        
        private string AnsiDecode(PipelineBuffer inputBuffer) {
            var length = inputBuffer.GetBlobLength(_jsonInputColumnIndex);
            if (length >= int.MaxValue) {
                // Too long!
                throw new Exception("Json element too long.");
            }
            var data = inputBuffer.GetBlobData(_jsonInputColumnIndex, 0, (int)length);
            return Encoding.Default.GetString(data);
        }

        private string UnicodeDecode(PipelineBuffer inputBuffer)
        {
            var length = inputBuffer.GetBlobLength(_jsonInputColumnIndex);
            if (length >= int.MaxValue)
            {
                // Too long!
                throw new Exception("Json element too long.");
            }
            var data = inputBuffer.GetBlobData(_jsonInputColumnIndex, 0, (int)length);
            return Encoding.Unicode.GetString(data);
        }

        private string SimpleStringExtractor(PipelineBuffer inputBuffer) {
            return inputBuffer.GetString(_jsonInputColumnIndex);
        }

        private void SendRowToOutputBuffer(string jsonArrayElement) {
            _outputBuffer.AddRow();
            for (int i = 0; i < copyVals.Length; i++) {
                // Skip the json item value, which will be filled later.
                if (i == _jsonInputColumnIndex)
                    continue;
                _outputBuffer[i] = copyVals[i];
            }

            _outputBuffer[_jsonOutputColumnIndex] = jsonArrayElement; 
        }

        private void SendErrorRow(int errorCode) {
            _errorBuffer.AddRow();
            _errorBuffer.SetErrorInfo(ComponentMetaData.OutputCollection[ERROR_LANE_NAME].ID, errorCode, 0);
        }
        
        public enum ERROR_CODES : int {
            [Description("The input json is invalid for this component.")]
            ERROR_BAD_JSON = -100,

            [Description("Array-start token was expected, but not found.")]
            ERROR_MISSING_START_ARRAY = -101,

            [Description("Array-end token was expected, but not found.")]
            ERROR_MISSING_END_ARRAY = -102
        }

        // TODO: move this into a stand-alone file 
        public class NullOrEmptyArrayException : Exception { }
    }
}

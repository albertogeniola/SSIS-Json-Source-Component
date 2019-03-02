using com.webkingsoft.JSONSuite_Common.Model;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if LINQ_SUPPORTED
using System.Threading.Tasks;
using System.Windows.Forms;
#endif

namespace com.webkingsoft.JSONSuite_Common
{
    // TODO: add Custom UI, document the class
#if DTS130
    [DtsPipelineComponent(CurrentVersion = 1, DisplayName = "JSON Array Splitter", Description = "Splits raw json arrays into single raw json elements.", ComponentType = ComponentType.Transform, IconResource = "com.webkingsoft.JSONSuite_130.jsource.ico")] // UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_130,Version=1.1.000.0,Culture=neutral"
#elif DTS120
    [DtsPipelineComponent(CurrentVersion = 1, DisplayName = "JSON Array Splitter", Description = "Splits raw json arrays into single raw json elements.", ComponentType = ComponentType.Transform, IconResource = "com.webkingsoft.JSONSuite_120.jsource.ico")] // UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_120,Version=1.1.000.0,Culture=neutral"
#elif DTS110
    [DtsPipelineComponent(CurrentVersion = 1, DisplayName = "JSON Array Splitter", Description = "Splits raw json arrays into single raw json elements.", ComponentType = ComponentType.Transform, IconResource = "com.webkingsoft.JSONSuite_110.jsource.ico")] // UITypeName = "com.webkingsoft.JSONSource_Common.JSONSourceComponentUI,com.webkingsoft.JSONSource_110,Version=1.1.000.0,Culture=neutral"
#endif
    public class JsonArraySplitter : PipelineComponent
    {
        private static readonly string INPUT_LANE_NAME = "Raw json input";
        private static readonly string OUTPUT_LANE_NAME = "Array Elements";
        private static readonly string ERROR_LANE_NAME = "Error elements";

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
            // output.ExclusionGroup = 1;

            // Configure Error output
            ComponentMetaData.UsesDispositions = true;
            var error = ComponentMetaData.OutputCollection.New();
            error.Name = ERROR_LANE_NAME;
            error.IsErrorOut = true;
            // error.ExclusionGroup = 1;

            // The ArraySplitter takes a line as input and produces 1+ output lines.
            // This means that the output is asynchronous to the input. By definition, this means that the associated input ID must be 0.
            output.SynchronousInputID = 0;
            error.SynchronousInputID = 0;

            // Custom property initialization
            var props = new ArraySplitterProperties();
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
            ArraySplitterProperties props;
            var customPropertiesValidation = ValidateCustomProperties(out props);
            if (customPropertiesValidation != DTSValidationStatus.VS_ISVALID)
                return customPropertiesValidation;

            // ------ Usage type -------
            var columnUsageTypeValidation = ValidateColumnsUsageType(props);
            if (columnUsageTypeValidation != DTSValidationStatus.VS_ISVALID)
                return columnUsageTypeValidation;

            // ------ Input:Output relationship ------
            var ioRelationshipValidation = ValidateIoRelationship();
            if (ioRelationshipValidation != DTSValidationStatus.VS_ISVALID)
                return ioRelationshipValidation;

            // As very last step, invoke the base validation implementation which will check that every input 
            // column is associated to an output of the upstream component.
            return base.Validate();
        }

        private DTSValidationStatus ValidateIoRelationship()
        {
            bool pbCancel = false;

            // Make sure that for every input column there is a matching output column
            // with same name/type
            var inputLane = ComponentMetaData.InputCollection[INPUT_LANE_NAME];
            var outputLane = ComponentMetaData.OutputCollection[OUTPUT_LANE_NAME];
            foreach (IDTSInputColumn100 icol in inputLane.InputColumnCollection) {
                bool found = false;
                bool matchType = false;
                foreach (IDTSOutputColumn100 ocol in outputLane.OutputColumnCollection) {
                    if (ocol.Name == icol.Name) {
                        found = true;

                        matchType = ocol.Length == icol.Length &&
                            ocol.CodePage == icol.CodePage &&
                            ocol.DataType == icol.DataType &&
                            ocol.Precision == icol.Precision;
                        break;
                    }
                }

                if (!found) {
                    ComponentMetaData.FireError(0,ComponentMetaData.Name, "Metadata changed: no output column matching input column " + icol.Name, "", 0, out pbCancel);
                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                }

                if (!matchType) {
                    ComponentMetaData.FireError(0, ComponentMetaData.Name, "Metadata changed: input column " + icol.Name, " type/length/precision/codepage differs from the ones of associated input column", 0, out pbCancel);
                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                }
            }

            return DTSValidationStatus.VS_ISVALID;
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
            // Since this component is asynchronous, we need to add output columns by ourself.
            // In fact, for asynchronous components, the row buffer is not shared.
            // We want to add an output column for every input column we have.
            IDTSInput100 input = ComponentMetaData.InputCollection[INPUT_LANE_NAME];
            IDTSOutput100 outLane = ComponentMetaData.OutputCollection[OUTPUT_LANE_NAME];
            var inputLane = ComponentMetaData.InputCollection[INPUT_LANE_NAME];
            foreach (IDTSInputColumn100 col in inputLane.InputColumnCollection)
            {
                var outCol = outLane.OutputColumnCollection.New();
                outCol.Name = col.Name;
                outCol.SetDataTypeProperties(col.DataType, col.Length, col.Precision, col.Scale, col.CodePage);
            }
        }

        private DTSValidationStatus ValidateColumnsUsageType(ArraySplitterProperties props)
        {
            bool pbCancel = false;
            // Make sure all the columns in the input component have been marked as READ-ONLY, except the one 
            // we are considering as JSON array. In case this is not the case, we might be able to recover by
            // using the ReinitializeMetadata.
            var inputLane = ComponentMetaData.InputCollection[INPUT_LANE_NAME];
            foreach (IDTSInputColumn100 col in inputLane.InputColumnCollection)
            {
                // If this is the selected json array column, it must be RW
                if (props.ArrayInputColumnName == col.Name && col.UsageType != DTSUsageType.UT_READWRITE)
                {
                    ComponentMetaData.FireError(0, ComponentMetaData.Name, "Column " + props.ArrayInputColumnName + " must be set as RW.", "", 0, out pbCancel);
                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                }
                else if (props.ArrayInputColumnName != col.Name && col.UsageType != DTSUsageType.UT_READONLY)
                {
                    ComponentMetaData.FireError(0, ComponentMetaData.Name, "Column " + col.Name + " must be set as RO.", "", 0, out pbCancel);
                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                }
            }
            return DTSValidationStatus.VS_ISVALID;
        }

        private DTSValidationStatus ValidateCustomProperties(out ArraySplitterProperties properties)
        {
            // TODO: handle validation for external metadata

            bool pbCancel = false;

            properties = new ArraySplitterProperties();
            properties.LoadFromComponent(ComponentMetaData.CustomPropertyCollection);

            // The user should have specified a valid input column where to fetch json from
            if (string.IsNullOrEmpty(properties.ArrayInputColumnName))
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Invalid input column specified.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISBROKEN;
            }

            // The selected input column name must reflect one of the available inputs
            // Moreover, the selected input column must be a valid STRING/TEXT type.
            bool found = false;
            foreach (IDTSInputColumn100 col in ComponentMetaData.InputCollection[INPUT_LANE_NAME].InputColumnCollection)
            {
                if (col.Name == properties.ArrayInputColumnName)
                {
                    found = true;

                    // Check the data type of the column
                    if (col.DataType != DataType.DT_NTEXT &&
                        col.DataType != DataType.DT_TEXT &&
                        col.DataType != DataType.DT_WSTR &&
                        col.DataType != DataType.DT_STR)
                    {

                        ComponentMetaData.FireError(0, ComponentMetaData.Name, "The selected input column must be in textual format (TEXT, NTEXT, STR, WSTR).", "", 0, out pbCancel);
                        return DTSValidationStatus.VS_ISBROKEN;
                    }

                    break;
                }
            }

            if (!found)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "The specified input column \"" + properties.ArrayInputColumnName +
                    "\" is not available among the columns of the input lane \"" + INPUT_LANE_NAME + "\"", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISBROKEN;
            }

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
            ArraySplitterProperties model = new ArraySplitterProperties();
            model.LoadFromComponent(ComponentMetaData.CustomPropertyCollection);

            // TODO: Set the usage type of every column to READONLY, except for the one selected by the user as JSON raw input. 
            // That one should be used as READ-WRITE.
            IDTSInput100 input = ComponentMetaData.InputCollection[INPUT_LANE_NAME];
            var vInput = ComponentMetaData.InputCollection[INPUT_LANE_NAME].GetVirtualInput();
            foreach (IDTSVirtualInputColumn100 vCol in vInput.VirtualInputColumnCollection)
            {
                // Set every column to readonly except made for the one that we will interpret as JSON
                if (vCol.Name == model.ArrayInputColumnName)
                    SetUsageType(input.ID, vInput, vCol.LineageID, DTSUsageType.UT_READWRITE);
                else
                    SetUsageType(input.ID, vInput, vCol.LineageID, DTSUsageType.UT_READONLY);
            }
        }
        /*
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
        }*/

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
                try {
                    if (!jtr.Read())
                    {
                        throw new NullOrEmptyArrayException();
                    }

                    // We expect "ArrayStart" element. If that differs, then the input is not well formed.
                    if (jtr.TokenType != JsonToken.StartArray) {
                        ComponentMetaData.FireError(0, ComponentMetaData.Name, "The json string should start with the '[' symbol, but it does not.", "",0, out pbCancel);
                        _errorBuffer.AddRow();
                        _errorBuffer.SetInt32(0, (int)ERROR_CODES.ERROR_MISSING_START_ARRAY);
                    }

                    // Now, we expect a variable number number of tokens 
                    int targetDepth = jtr.Depth;

                    JsonLoadSettings settings = new JsonLoadSettings();
                    JArray array = JArray.ReadFrom(jtr) as JArray;
                    int processedTokens = 0;
                    foreach (JToken token in array) {
                        SendRowToOutputBuffer(token.ToString(Formatting.None));
                        processedTokens++;
                    }

                    if (processedTokens == 0) {
                        // If there was no item in the array or if the array was empty, we still need to 
                        // provide one output row. So we do it here.
                        SendRowToOutputBuffer(null);
                    }

                    /*
                    while (jtr.Read()) {
                        // Read items to the target depth. Consume all the string until we get back to the 
                        // target depth
                        if (jtr.Depth > targetDepth) {
                            sb.Append(jtr.Value);
                            continue;
                        }
                        
                        if (jtr.TokenType == JsonToken.EndArray)
                            break;
                        else {
                            // We are done with this line. We can send it to the output buffer
                            SendRowToOutputBuffer(sb.ToString());
                            sb.Clear();
                        }
                    }*/


                    // TODO: Make sure there is nothing more to read
                    if (jtr.Read()) {
                        // This should not happen! There is no need to fail here. Just throw a warning.
                        ComponentMetaData.FireWarning(0, ComponentMetaData.Name, "The JSON array did contain some more data after the closing bracket. ']'.", "", 0);
                    }
                } catch (NullOrEmptyArrayException ex) {
                    // If we failed to read or there is nothing to read, just throw a warning and cotinue.
                    ComponentMetaData.FireWarning(0, ComponentMetaData.Name, "No JSON data recevied", "", 0);
                    // TODO: handle the case there is an empty input / empty input array.
                    // TODO: Add the output anyways
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

        public enum ERROR_CODES : int {
            ERROR_MISSING_START_ARRAY = -101,
            ERROR_MISSING_END_ARRAY = -102
        }

        // TODO: move this into a stand-alone file 
        public class NullOrEmptyArrayException : Exception { }
    }
}

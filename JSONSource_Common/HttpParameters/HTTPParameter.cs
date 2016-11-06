using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using com.webkingsoft.JSONSource_Common.Exceptions;
using Microsoft.SqlServer.Dts.Pipeline;
#if LINQ_SUPPORTED
using System.Linq;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    public class HTTPParameter
    {
        /// <summary>
        /// This method should be called at runtime, so that correct value is returned.
        /// </summary>
        /// <param name="variableDispenser"></param>
        /// <param name="inputBuffer"></param>
        /// <param name="inputLane"></param>
        /// <param name="bufferManager"></param>
        /// <returns></returns>
        public string GetValue(IDTSVariableDispenser100 variableDispenser, PipelineBuffer inputBuffer, IDTSInput100 inputLane, IDTSBufferManager100 bufferManager) {
            switch (Binding)
            {
                case HTTPParamBinding.CustomValue:
                    return CustomValue;
                case HTTPParamBinding.Variable:
                    DataType type;
                    return Utils.GetVariable(variableDispenser, VariableName, out type).ToString();
                case HTTPParamBinding.InputField:
                    int colIndex = bufferManager.FindColumnByLineageID(inputLane.Buffer, inputLane.InputColumnCollection[InputColumnName].LineageID);
                    return inputBuffer[colIndex].ToString();
                default:
                    throw new ApplicationException("Unhandled HTTPBinding type.");
            }
        }

        public string GetValueTest(IDTSVariableDispenser100 variableDispenser)
        {
            switch (Binding)
            {
                case HTTPParamBinding.CustomValue:
                    return CustomValue;
                case HTTPParamBinding.Variable:
                    DataType type;
                    string defaultValue = null;
                    try
                    {
                        defaultValue = Utils.GetVariable(variableDispenser, VariableName, out type).ToString();
                    }
                    catch (VariableNotFoundException ex) {}

                    var p = new UserPrompter("Variable " + VariableName, defaultValue);
                    p.ShowDialog();

                    return p.GetValue();
                     
                case HTTPParamBinding.InputField:
                    p = new UserPrompter("Variable " + VariableName);
                    p.ShowDialog();

                    return p.GetValue();
                default:
                    throw new ApplicationException("Unhandled HTTPBinding type.");
            }
        }

        public string FieldName
        {
            get;
            set;
        }

        public bool Validate(IDTSComponentMetaData100 metadata, IDTSVariableDispenser100 variableDispenser, out string msg) {
            msg = null;

            // General checks
            if (FieldName == null) {
                msg = "Invalid parameter name. It cannot be null.";
                return false;
            }

            // Perform binding-dependent checks first.
            switch (Binding) {
                
                // In case it is mapped to an input field, check if that is available.
                case HTTPParamBinding.InputField:

                    if (InputColumnName == null)
                    {
                        msg = "Unspecified binding column name for http parameter "+FieldName;
                        return false;
                    }

                    bool found = false;
                    foreach (IDTSInputColumn100 inputcol in metadata.InputCollection[ComponentConstants.NAME_INPUT_LANE_PARAMS].InputColumnCollection)
                    {
                        if (inputcol.Name == InputColumnName)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        msg = "Invalid HTTP parameter binding. Column "+InputColumnName+" is not available.";
                        return false;
                    }
                    break;

                // When depending on a variable, check the variable exists
                case HTTPParamBinding.Variable:
                    DataType type;

                    if (VariableName == null)
                    {
                        msg = "Unspecified binding variable name for http parameter " + FieldName;
                        return false;
                    }

                    try
                    {
                        Utils.GetVariable(variableDispenser, VariableName, out type).ToString();
                    }
                    catch (VariableNotFoundException ex) {
                        msg = "Invalid HTTP parameter binding. Variable " + VariableName + " is not defined.";
                        return false;
                    }

                    break;

                case HTTPParamBinding.CustomValue:
                    if (CustomValue == null) {
                        msg = "Unspecified Custom vlue binding for http parameter " + FieldName+ ".";
                        return false;
                    }
                    break;

                default:
                    // This should bever happen. 
                    throw new ApplicationException("Invalid/unhandled HTTP binding type. Contact the developer.");
            }

            msg = null;
            return true;
        }

        public string VariableName
        {
            get;
            set;
        }

        public string InputColumnName {
            get;
            set;
        }

        public string CustomValue
        {
            get;
            set;
        }

        public HTTPParamBinding Binding
        {
            get;
            set;
        }

        public bool Encode
        {
            get;
            set;
        }

    }
}

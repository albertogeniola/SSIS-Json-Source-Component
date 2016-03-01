using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
#if LINQ_SUPPORTED
using System.Linq;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    [Newtonsoft.Json.JsonObject(MemberSerialization.OptIn)]
    public class IOMapEntry
    {
        private string inFieldPath;
        [JsonProperty]
        public string InputFieldPath
        {
            get { return inFieldPath; }
            set { inFieldPath = value; }
        }

        private int inputFieldLen;
        [JsonProperty]
        public int InputFieldLen
        {
            get { return inputFieldLen; }
            set { inputFieldLen = value; }
        }

        private string outputColName;
        [JsonProperty]
        public string OutputColName
        {
            get { return outputColName; }
            set { outputColName = value; }
        }

        private JsonTypes outputColumnType;

        [JsonProperty]
        public JsonTypes OutputJsonColumnType
        {
            get { return outputColumnType; }
            set { outputColumnType = value; }
        }

        public DataType OutputColumnType
        {
            get {
                // Mao here a JSON type to the corresponding SSIS DataType
                switch (outputColumnType) { 
                    case JsonTypes.Boolean:
                        return DataType.DT_BOOL;
                    case JsonTypes.Number:
                        return DataType.DT_DECIMAL;
                    case JsonTypes.String:
                        return DataType.DT_WSTR;
                    case JsonTypes.RawJson:
                        return DataType.DT_WSTR;
                    default:
                        throw new Exception("Invalid column type specified");
                }
            }
        }
    }
}

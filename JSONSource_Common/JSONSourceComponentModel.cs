using Microsoft.SqlServer.Dts.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
#if LINQ_SUPPORTED
using System.Linq;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    /// <summary>
    /// This class represents the Model of the JSONSourceComponent.
    /// Since the component is quite complex, I've decided to split the whole configuration in three main areas:
    /// -> DataSource
    /// -> DataMapping
    /// -> AdvancedSettings
    /// So, each submodel has its own validation mechanisms and can be manipulated directly by views.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class JSONSourceComponentModel: IComponentModel
    {
        [JsonProperty]
        public JSONDataSourceModel DataSource { get; set; }
        [JsonProperty]
        public JSONDataMappingModel DataMapping { get; set; }
        [JsonProperty]
        public JSONAdvancedSettingsModel AdvancedSettings { get; set; }

        public string ToJsonConfig()
        {
            return JsonConvert.SerializeObject(this);
        }

        public JSONSourceComponentModel() {
            DataSource = new JSONDataSourceModel();
            DataMapping = new JSONDataMappingModel();
            AdvancedSettings = new JSONAdvancedSettingsModel();
        }

        public static JSONSourceComponentModel LoadFromJson(string jsonConfig)
        {
            JSONSourceComponentModel res = JsonConvert.DeserializeObject<JSONSourceComponentModel>(jsonConfig);
            return res;
        }

        public bool Validate(out string err, out string warn)
        {
            // Trick: C# will return as soon a FALSE is found when evaluating AND expressions.
            return DataSource.Validate(out err, out warn) && DataMapping.Validate(out err, out warn) && AdvancedSettings.Validate(out err, out warn);
        }
    }

    #region DataSource helper class

    /// <summary>
    /// This class wraps all the settings needed to gather data from the service.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class JSONDataSourceModel:IComponentModel {

        /// <summary>
        /// Validates the DataSource configuraiton.
        /// </summary>
        /// <param name="err"></param>
        /// <param name="warn"></param>
        /// <returns></returns>
        public bool Validate(out string err, out string warn)
        {
            err = "";
            warn = "";

            // If the component relies on a variable value, make sure it exists
            switch (UriBindingType) {
                case ParamBinding.Variable:
                    if (string.IsNullOrEmpty(UriBindingValue))
                    {
                        err = "Variable " + UriBindingValue + " isn't valid.";
                        return false;
                    }

                    // Note that we can't validate here the variable contents and we can't even check if the variable exists. 
                    // This checks are done at runtime, before parsing the data. Just fire a warning.
                    warn += "You've specified variable source. No validation check is performed by the designer. This value will be validated at runtime, so make sure it is consistent.\n";

                    // TODO: add run-time validation for this case.        
                    break;

                case ParamBinding.CustomValue:
                    if (UriBindingValue == null)
                    {
                        err = "URI is invalid";
                        return false;
                    }
                    break;
                default:
                    err = "Parameter binding for URI is invalid/unsupported.";
                    return false;
            }    
            return true;
        }

        public string ToJsonConfig()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static JSONDataSourceModel LoadFromJson(string jsonConfig)
        {
            JSONDataSourceModel res = JsonConvert.DeserializeObject<JSONDataSourceModel>(jsonConfig);
            return res;
        }

        /// <summary>
        /// Represents the binding value that is resolved at runtime according to the BindingType
        /// </summary>
        [JsonProperty]
        public string UriBindingValue
        {
            get; set;
        }

        /// <summary>
        /// Represents how do we retrieve the SourceUri value.
        /// </summary>
        [JsonProperty]
        public ParamBinding UriBindingType
        {
            get;
            set;
        }

        #region Web settings
        /// <summary>
        /// Set of HTTP parameters to send alongside the request. 
        /// </summary>
        [JsonProperty]
        public IEnumerable<HTTPParameter> HttpParameters
        {
            get;
            set;
        }

        /// <summary>
        /// Set of HTTP headers to send alongside the request. 
        /// </summary>
        [JsonProperty]
        public IEnumerable<HTTPParameter> HttpHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// The cookie variable defines the name of the variable where cookies have to be stored.
        /// </summary>
        [JsonProperty]
        public string CookieVariable
        {
            get;
            set;
        }
        
        /// <summary>
        /// Represents the HTTP method to be used to perform the request
        /// </summary>
        [JsonProperty]
        public string WebMethod
        {
            get;
            set;
        }
        
        #endregion
    }

    #endregion

    #region DataMapping helper class
    [JsonObject(MemberSerialization.OptIn)]
    public class JSONDataMappingModel: IComponentModel
    {
        /// <summary>
        /// Represents the type of the root element that this component expects during the parsing. If can either be a Json Object or a Json Array
        /// </summary>
        [JsonProperty]
        public RootType RootType
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the relative path to the Json Object/Array where to start the parsing. 
        /// </summary>
        [JsonProperty]
        public string JsonRootPath
        {
            get; set;
        }

        /// <summary>
        /// Dictionary mapping inputs IDs to outputs IDs of input columns that should only be copied
        /// </summary>
        [JsonProperty]
        public List<string> InputColumnsToCopy
        {
            get; set;
        }

        #region IOMapping
        /// <summary>
        /// Defines IO bindings
        /// </summary>
        [JsonProperty]
        private Dictionary<string, IOMapEntry> _ioMap;
        public IEnumerable<IOMapEntry> IoMap
        {
            get
            {
                if (_ioMap == null)
                    _ioMap = new Dictionary<string, IOMapEntry>();
                return _ioMap.Values;
            }
        }

        public void ClearMapping()
        {
            if (_ioMap != null)
                _ioMap.Clear();
        }

        public void AddMapping(IOMapEntry map)
        {
            if (_ioMap == null)
                _ioMap = new Dictionary<string, IOMapEntry>();
            _ioMap.Add(map.InputFieldPath, map);
        }
        #endregion

        public string ToJsonConfig()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static JSONDataMappingModel LoadFromJson(string jsonConfig)
        {
            JSONDataMappingModel res = JsonConvert.DeserializeObject<JSONDataMappingModel>(jsonConfig);
            return res;
        }

        public bool Validate(out string err, out string warn)
        {
            err = "";
            warn = "";

            // The relative path may be a null/empty string or a valid JSONPath expression. In case it is not null, check its validity
            if (!String.IsNullOrEmpty(JsonRootPath)) {
                //TODO: there is no library to check this at the moment. Consider to implement a simple parser by myself...
                warn += "This version of the component is unable to verify the syntax of the specified JSONPath expression. Please make sure that is correct, otherwise you might expect errors at runtime.";
            }

            // We expect at least one IO binding, otherwise this component is useless.
            if (IoMap == null || IoMap.Count() == 0)
            {
                err = "This component must at least have one output column.";
                return false;
            }

            // Check all the IO bindings and make sure data is consistent.
            foreach (IOMapEntry e in IoMap)
            {
                // FieldName and outputFiledName cannot be null, empty and must be unique.
                if (string.IsNullOrEmpty(e.InputFieldPath))
                {
                    err = "One row of the Input-Output mapping is invalid: null or empty input field name. Please review IO configuration.";
                    return false;
                }
                if (string.IsNullOrEmpty(e.OutputColName))
                {
                    err = "One row of the Input-Output mapping is invalid: null or empty output field name. Please review IO configuration.";
                    return false;
                }
                // Checks for unique cols
                foreach (IOMapEntry e1 in IoMap)
                {
                    if (!ReferenceEquals(e, e1) && e.InputFieldPath == e1.InputFieldPath)
                    {
                        // Not unique!
                        err = "There are two or more rows with same InputFieldName. This is not allowed.";
                        return false;
                    }
                    if (!ReferenceEquals(e, e1) && e.OutputColName == e1.OutputColName)
                    {
                        // Not unique!
                        err = "There are two or more rows with same OutputColName. This is not allowed.";
                        return false;
                    }
                }
            }

            return true;

        }
    }

    #endregion

    #region AdvancedSettings helper class 
    [JsonObject(MemberSerialization.OptIn)]
    public class JSONAdvancedSettingsModel: IComponentModel
    {
        /// <summary>
        /// Represents the temporary directory where to download the JSONFile from the internet, whenever is needed.
        /// </summary>
        [JsonProperty]
        public string CustomLocalTempDir
        {
            get; set;
        }

        // This property has been added at version 1. So we need to handle the default case.
        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool ParseDates
        {
            get; set;
        }

        public string ToJsonConfig()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static JSONAdvancedSettingsModel LoadFromJson(string jsonConfig)
        {
            JSONAdvancedSettingsModel res = JsonConvert.DeserializeObject<JSONAdvancedSettingsModel>(jsonConfig);
            return res;
        }

        public bool Validate(out string err, out string warn)
        {
            err = "";
            warn = "";

            // In case a custom local temp dir has been specified, check if it is available.
            if (!string.IsNullOrEmpty(CustomLocalTempDir))
            {
                // Give warning only if the user specified a custom value and that one is invalid
                if (!Directory.Exists(CustomLocalTempDir))
                {
                    warn += "The path to " + CustomLocalTempDir + " doesn't exists on this FS. If you're going to deploy the package on another server, make sure the path is correct and the service has write permission on it.\n";
                }
            }

            return true;
        }

    }
    #endregion

}

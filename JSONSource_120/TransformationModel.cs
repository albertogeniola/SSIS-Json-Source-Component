using JSONSource.webkingsoft.JSONSource_120;
using Microsoft.SqlServer.Dts.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace com.webkingsoft.JSONSource_120
{
    /*
        * Classe wrapper che contiene le informazioni per configurare il componente.
        * 
    */
    [JsonObject(MemberSerialization.OptIn)]
    public class TransformationModel
    {

        [JsonProperty]
        public Microsoft.SqlServer.Dts.Pipeline.ComponentType ComponentType {
            get {
                return Microsoft.SqlServer.Dts.Pipeline.ComponentType.Transform;
            }
        }

        // Definisce la cartella di appoggio temporanea per la gestione dei file a runtiModele
        private string _customLocalTempDir = null;
        [JsonProperty]
        public string CustomLocalTempDir
        {
            get { return _customLocalTempDir; }
            set { _customLocalTempDir = value; }
        }

        // Definisce il percorso relativo al file scaricato, nel quale recuperare l'array di oggetti da deserializzare.
        private string _jsonObjectRelativePath;
        [JsonProperty]
        public string JsonObjectRelativePath
        {
            get { return _jsonObjectRelativePath; }
            set { _jsonObjectRelativePath = value; }
        }

        [JsonProperty]
        public String InputColumnName
        {
            get;
            set;
        }

        private Dictionary<string, int> _inputs;
        [JsonProperty]
        public Dictionary<string, int> Inputs {
            get {
                if (_inputs == null)
                    _inputs = new Dictionary<string, int>();
                return _inputs;
            }
        }
        
        // Definisce il mapping Input-Output
        [JsonProperty]
        private Dictionary<string, IOMapEntry> _ioMap;
        public IEnumerable<IOMapEntry> IoMap
        {
            get {
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


        public string ToJsonConfig()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static TransformationModel LoadFromJson(string jsonConfig)
        {
            TransformationModel res = JsonConvert.DeserializeObject<TransformationModel>(jsonConfig);
            return res;
        }
    }
}

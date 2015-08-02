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
    public class Model
    {
        // Definisce il tipo di sorgente dello script JSON
        private SourceType _sourceType;
        [JsonProperty]
        public SourceType SourceType
        {
            get { return _sourceType; }
            set { _sourceType = value; }
        }

        // Path al file, in caso di tipo filePath
        private string _filePath;
        [JsonProperty]
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        // Variabile contenete il file path, in caso di tipo filePathVariable
        private string _filePathVar;
        [JsonProperty]
        public string FilePathVar
        {
            get { return _filePathVar; }
            set { _filePathVar = value; }
        }

        // URL per effettuare la richiesta, in caso di tipo webUrl
        private string _webUrl;
        [JsonProperty]
        public string WebUrl
        {
            get { return _webUrl; }
            set { _webUrl = value; }
        }

        // Variabile che contiene il webUrl, in caso di tipo webUrlVariable
        private string _webUrlVariable;
        [JsonProperty]
        public string WebUrlVariable
        {
            get { return _webUrlVariable; }
            set { _webUrlVariable = value; }
        }

        // Definisce la cartella di appoggio temporanea per la gestione dei file a runtime
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

        public static Model LoadFromJson(string jsonConfig)
        {
            Model res = JsonConvert.DeserializeObject<Model>(jsonConfig);
            return res;
        }
    }
}

using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace com.webkingsoft.JSONSource_110
{
    /*
        * Classe wrapper che contiene le informazioni per configurare il componente.
        * 
    */
    [Serializable]
    public class Model
    {
        // Definisce il tipo di sorgente dello script JSON
        private SourceType _sourceType;
        public SourceType SourceType
        {
            get { return _sourceType; }
            set { _sourceType = value; }
        }

        // Path al file, in caso di tipo filePath
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        // Variabile contenete il file path, in caso di tipo filePathVariable
        private string _filePathVar;
        public string FilePathVar
        {
            get { return _filePathVar; }
            set { _filePathVar = value; }
        }

        // URL per effettuare la richiesta, in caso di tipo webUrl
        private string _webUrl;
        public string WebUrl
        {
            get { return _webUrl; }
            set { _webUrl = value; }
        }

        // Variabile che contiene il webUrl, in caso di tipo webUrlVariable
        private string _webUrlVariable;
        public string WebUrlVariable
        {
            get { return _webUrlVariable; }
            set { _webUrlVariable = value; }
        }

        // Definisce la cartella di appoggio temporanea per la gestione dei file a runtime
        private string _customLocalTempDir = null;
        public string CustomLocalTempDir
        {
            get { return _customLocalTempDir; }
            set { _customLocalTempDir = value; }
        }

        // Definisce il percorso relativo al file scaricato, nel quale recuperare l'array di oggetti da deserializzare.
        private string _jsonObjectRelativePath;
        public string JsonObjectRelativePath
        {
            get { return _jsonObjectRelativePath; }
            set { _jsonObjectRelativePath = value; }
        }

        // Definisce il mapping Input-Output
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
            _ioMap.Add(map.InputFieldName, map);
        }

        private OperationMode _opMode;

        public OperationMode OpMode
        {
            get { return _opMode; }
            set { _opMode = value; }
        }

        /**
         * Consente la serializzazione del modello in formato XML. In questo modo 
         * sarà possibile salvarne lo stato in modo persistente. Purtroppo non posso
         * usare il serializer nativo in XML in quanto non può serializzare gli oggetti
         * IDictionary. Devo provvedere autonomamente alla definizione di metodi di 
         * marshaling e unmarshaling.
         */
        const string EL_MODEL = "MODEL";
        const string ATT_SOURCETYPE = "SourceType";
        const string ATT_FILEPATH = "FilePath";
        const string ATT_FILEPATHVAR = "FilePathVar";
        const string ATT_WEBURL = "WebUrl";
        const string ATT_WEBURLVAR = "WebUrlVariable";
        const string ATT_TMPDIR = "AttTmpDir";
        const string ATT_JSONRELPATH = "JsonObjectRelativePath";
        const string EL_IOMAPPING = "IOMAP";
        const string ATT_OPERATION_MODE = "OperationMode";

        public string ToXmlString()
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.ConformanceLevel = ConformanceLevel.Fragment;

                XmlWriter writer = XmlWriter.Create(sw,settings);
                
                writer.WriteStartElement(EL_MODEL);

                // Source Type
                writer.WriteAttributeString(ATT_SOURCETYPE, SourceType.ToString());
                // FilePath
                writer.WriteAttributeString(ATT_FILEPATH, FilePath);
                // FilePathVar
                writer.WriteAttributeString(ATT_FILEPATHVAR, FilePathVar);
                // WebUrl
                writer.WriteAttributeString(ATT_WEBURL, WebUrl);
                // WebUrlVar
                writer.WriteAttributeString(ATT_WEBURLVAR, WebUrlVariable);
                // Custom local temp dir
                writer.WriteAttributeString(ATT_TMPDIR, CustomLocalTempDir);
                // Json relative path
                writer.WriteAttributeString(ATT_JSONRELPATH, JsonObjectRelativePath);
                // Operation Mode
                writer.WriteAttributeString(ATT_OPERATION_MODE, OpMode.ToString());
                // IOMap
                writer.WriteStartElement(EL_IOMAPPING);
                
                foreach (IOMapEntry e in IoMap)
                {
                    e.WriteToXml(writer);
                }
                writer.WriteEndElement(); // Ends the dictionary serialization EL_IOMAPPING

                writer.WriteEndElement(); // Closing EL_MODEL
                writer.Flush();
                
                return sw.ToString();
            }
        }

        public static Model Load(string xmlText)
        {
            Model res = new Model();
            using (StringReader sr = new StringReader(xmlText))
            {
                using (XmlReader r = XmlReader.Create(sr))
                {
                    OperationMode opMod = OperationMode.SyncIO;
                    SourceType sourcetype = SourceType.filePath;
                    string filePath = null;
                    string filePathVar = null;
                    string webUrl = null;
                    string webUrlVariable = null;
                    string tempDir = null;
                    string jsonPath = null;
                    while (r.Read())
                    {
                        if (r.NodeType == XmlNodeType.Element && r.Name == EL_MODEL)
                        {
                            try
                            {
                                sourcetype = (SourceType)Enum.Parse(typeof(SourceType), r.GetAttribute(ATT_SOURCETYPE));
                            }
                            catch(ArgumentException ex)
                            {}

                            try 
                            {
                                filePath = r.GetAttribute(ATT_FILEPATH);
                            }
                            catch(ArgumentException ex)
                            {}

                            try
                            {
                                filePathVar = r.GetAttribute(ATT_FILEPATHVAR);
                            }
                            catch (ArgumentException ex)
                            { }

                            try
                            {
                                webUrl = r.GetAttribute(ATT_WEBURL);
                            }
                            catch (ArgumentException ex)
                            { }

                            try
                            {
                                webUrlVariable = r.GetAttribute(ATT_WEBURLVAR);
                            }
                            catch (ArgumentException ex)
                            { }

                            try
                            {
                                opMod = (OperationMode)Enum.Parse(typeof(OperationMode), r.GetAttribute(ATT_OPERATION_MODE));
                            }
                            catch (ArgumentException ex)
                            { }

                            try
                            {
                                tempDir = r.GetAttribute(ATT_TMPDIR);
                            }
                            catch (ArgumentException ex)
                            { }

                            try
                            {
                                jsonPath = r.GetAttribute(ATT_JSONRELPATH);
                            }
                            catch (ArgumentException ex)
                            { }                            
                        }
                        else if (r.NodeType == XmlNodeType.Element && r.Name == IOMapEntry.EL_IOMAPROW)
                        {
                            IOMapEntry e = IOMapEntry.Load(r);
                            res.AddMapping(e);
                        }
                    }

                    res.SourceType = sourcetype;
                    res.FilePath = filePath;
                    res.FilePathVar = filePathVar;
                    res.WebUrl = webUrl;
                    res.WebUrlVariable = webUrlVariable;
                    res.CustomLocalTempDir = tempDir;
                    res.JsonObjectRelativePath = jsonPath;
                    res.OpMode = opMod;
                }
            }

            return res;
        }
    }
}

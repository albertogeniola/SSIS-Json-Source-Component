using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace com.webkingsoft.JSONSource_Common
{
    public partial class ComponentTester : Form
    {
        private JSONSourceComponentModel _model;

        Uri uri = null;
        List<HTTPParameter> parameters = new List<HTTPParameter>();
        string fname = null;
        Dictionary<string, object> copyCols = new Dictionary<string, object>();

        private class ProgressState {
            public int index;
            public string text;
            public int status;
        }

        private class ResultInfo
        {
            public string json;
            public Exception ex;
            public Dictionary<string, object> parsedResult;
        }

        public ComponentTester(JSONSourceComponentModel model)
        {
            _model = model;
            InitializeComponent();
        }
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker1.ReportProgress(0, new ProgressState() { index = 0, text = "User inputs acquired", status = 100 });
            ResultInfo res = new ResultInfo();

            backgroundWorker1.ReportProgress(0, new ProgressState() { index = 1, text = "Retrieving json data...", status = 0 });
            try
            {
                #region  Download the file / Perform the request
                if (uri.IsFile)
                {
                    fname = uri.LocalPath;
                }
                else if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                {
                    // Need to perform the request
                    fname = Utils.DownloadJson(null, uri, _model.DataSource.WebMethod, parameters, null);
                }
                #endregion
            }
            catch (Exception ex) {
                backgroundWorker1.ReportProgress(0, new ProgressState() { index = 1, text = "Cannot get json data", status = -1 });
                res.ex = ex;
                e.Result = res;
                return;
            }
            backgroundWorker1.ReportProgress(0, new ProgressState() { index = 1, text = "JSON Data collected", status = 100 });

            // Check the json. TODO: advise the user whether he is mistyping the returned json type
            dynamic jsonData = null;
            List<JObject> jsonObjects = new List<JObject>();
            Dictionary<string, object> firstRes = null;
            backgroundWorker1.ReportProgress(0, new ProgressState() { index = 2, text = "Parsing JSON data...", status = 0 });
            try
            {
                #region Load Json and start the parsing
                using (var sr = File.Open(fname, FileMode.Open))
                {
                    using (var reader = new JsonTextReader(new StreamReader(sr)))
                    {
                        if (_model.DataMapping.RootType == RootType.JsonObject)
                        {
                            jsonData = JObject.Load(reader);
                        }
                        else {
                            jsonData = JArray.Load(reader);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex) {
                backgroundWorker1.ReportProgress(0, new ProgressState() { index = 1, text = "Cannot load json data", status = -1 });
                res.ex = ex;
                e.Result = res;
                return;
            }
            backgroundWorker1.ReportProgress(0, new ProgressState() { index = 2, text = "JSON Loaded, parsing the first object...", status = 50 });

            try
            {
                #region Navigate to the root and parse items
                IEnumerable<JToken> els = jsonData.SelectTokens(_model.DataMapping.JsonRootPath);
                // For each root element we got...
                foreach (JToken t in els)
                {
                    if (t.Type == JTokenType.Array)
                    {
                        firstRes = ProcessArray(t as JArray, _model.DataMapping.IoMap, copyCols);
                    }
                    else if (t.Type == JTokenType.Object)
                    {
                        firstRes = ProcessObject(t as JObject, _model.DataMapping.IoMap, copyCols);
                    }
                    else {
                        throw new Exception("Invalid token returned by RootPath query: " + t.Type.ToString());
                    }
                }

                #endregion
            }
            catch (Exception ex) {
                backgroundWorker1.ReportProgress(0, new ProgressState() { index = 1, text = "Cannot parse json data", status = -1 });
                res.ex = ex;
                res.json = jsonData.ToString();
                e.Result = res;
                return;
            }
            backgroundWorker1.ReportProgress(0, new ProgressState() { index = 2, text = "First json object OK", status = 100 });

            // If get here, everything was fine. Set the result
            res.json = jsonData.ToString();
            res.parsedResult = firstRes;
            e.Result = res;
            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var ps = e.UserState as ProgressState;
            Label l = null;
            ProgressBar p = null;

            switch (ps.index) {
                case 0:
                    l = label0;
                    p = pb0;
                    break;
                case 1:
                    l = label1;
                    p = pb1;
                    break;
                case 2:
                    l = label2;
                    p = pb2;
                    break;
                default:
                    throw new Exception("Index out of bounds!");
            }

            l.Text = ps.text;
            if (ps.status == 0) {
                p.Style = ProgressBarStyle.Marquee;
            }
            else if (ps.status == -1) {
                p.Style = ProgressBarStyle.Continuous;
                p.Value = 0;
            } else
            {
                p.Style = ProgressBarStyle.Continuous;
                p.Value = ps.status;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var ri = e.Result as ResultInfo;
            if (ri.ex != null)
                textBox1.Text = "Error occurred: " + ri.ex.Message;

            if (ri.json != null)
                textBox1.Text += "\nDownloaded: " + ri.json;

            if (ri.parsedResult != null)
            {
                foreach (var i in ri.parsedResult) {
                    dataGridView1.Rows.Add(new object[] { i.Key, i.Value });
                }
            }
            
        }

        private Dictionary<string, object> ProcessObject(JObject obj, IEnumerable<IOMapEntry> ioMap, Dictionary<string, object> copyCols)
        {
            // The object we return is a dictionary, with column-name <-> value
            Dictionary<string, object> res = new Dictionary<string, object>();

            // Add copy cols to res
            foreach (var cc in copyCols)
            {
                res.Add(cc.Key, cc.Value);
            }

            // For each column requested from metadata, look for data into the object we parsed
            foreach (IOMapEntry e in ioMap)
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

                    // Add the value to the dictionary representing our parsed object
                    res.Add(e.OutputColName, val);
                }
                else {
                    // If it's not a json raw type, parse the value.
                    try
                    {
                        object val = obj.SelectToken(e.InputFieldPath);
                        res.Add(e.OutputColName, val);
                    }
                    catch (Newtonsoft.Json.JsonException ex)
                    {
                        throw ex;
                    }
                }

            }
            return res;
        }

        private Dictionary<string, object> ProcessArray(JArray arr, IEnumerable<IOMapEntry> ioMap, Dictionary<string, object> copyCols)
        {
            // Just process up to the first object
            if (arr.Count < 1)
                throw new Exception("Array did not contain any valid object");

            JObject obj = null;

            try
            {
                obj = arr[0] as JObject;
            }
            catch (Exception e)
            {
                throw new Exception("Cannot retrieve first objct of result array", e);
            }

            return ProcessObject(obj, ioMap, copyCols);
        }

        private void ComponentTester_Load(object sender, EventArgs e)
        {

            backgroundWorker1.ReportProgress(0, new ProgressState() { index = 0, text = "Collecting user data...", status = 0 });
            
            #region Retrieve the URI
            // If depending on a variable, ask the user to input its value
            if (_model.DataSource.FromVariable)
            {
                UserPrompter prompt = new UserPrompter(_model.DataSource.VariableName, (string s) =>
                {
                    try
                    {
                        Uri u = new Uri(s);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                });

                var result = prompt.ShowDialog(this);
                if (result != DialogResult.OK)
                {
                    return;
                }
                else {
                    uri = new Uri(prompt.GetValue());
                }
            }
            else {
                uri = _model.DataSource.SourceUri;
            }
            #endregion
            #region Prepare the http parameters (if needed)
            // Only needed if the request is going to be directed to a webserver
            if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            {
                parameters = new List<HTTPParameter>();
                foreach (var param in _model.DataSource.HttpParameters)
                {
                    if (param.Binding == HTTPParamBinding.Variable || param.Binding == HTTPParamBinding.InputField)
                    {

                        UserPrompter prompt = null;
                        if (param.Binding == HTTPParamBinding.Variable)
                            prompt = new UserPrompter("Variable: " + param.Name);
                        else
                            prompt = new UserPrompter("Input column: " + param.InputColumnName);

                        var result = prompt.ShowDialog(this);
                        if (result != DialogResult.OK)
                        {
                            return;
                        }
                        else {
                            // I am going to cheat here. Treat every param like a custom input parameter
                            var item = new HTTPParameter();
                            item.Binding = HTTPParamBinding.CustomValue;
                            item.Encode = true;
                            item.InputColumnName = null;
                            if (param.Binding == HTTPParamBinding.Variable)
                                item.Name = param.Name;
                            else
                                item.Name = param.InputColumnName;
                            item.Value = prompt.GetValue();
                            parameters.Add(item);
                        }
                    }
                }
            }
            #endregion
            #region Collect any copy column as requested
            foreach (var param in _model.DataMapping.InputColumnsToCopy)
            {
                UserPrompter prompt = null;
                prompt = new UserPrompter("Input column: " + param);

                var result = prompt.ShowDialog(this);
                if (result != DialogResult.OK)
                {
                    return;
                }
                else {
                    copyCols.Add(param, prompt.GetValue());
                }
            }
            #endregion
            
            backgroundWorker1.RunWorkerAsync();
        }
    }
}

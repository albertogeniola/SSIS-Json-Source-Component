using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JSONSource.webkingsoft.JSONSource_120;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Design;

namespace com.webkingsoft.JSONSource_120.CustomControls
{
    public partial class SourceControl : UserControl
    {
        public WebSourceControl _webUriView;
        public FileSourceControl _filePathView;
        private Variables _vars;
        private IServiceProvider _sp;
        private SourceModel _model;

        private const string WEB_SERVICE_SOURCE = "Web HTTP Request";
        private const string FILE_SOURCE = "File";

        public SourceControl(Variables vars, IServiceProvider sp)
        {
            _vars = vars;
            _sp = sp;

            // Componenti da visualizzare centralmente
            _webUriView = new WebSourceControl(_vars,_sp);
            _webUriView.Visible = false;

            _filePathView = new FileSourceControl(_vars);
            _filePathView.Visible = false;

            InitializeComponent();

            main.Controls.Add(_webUriView);
            _webUriView.Dock = DockStyle.Fill;
            main.Controls.Add(_filePathView);
            _filePathView.Dock = DockStyle.Fill;


            uiSourceType.DataSource = new string[] { WEB_SERVICE_SOURCE,FILE_SOURCE };
            uiSourceType.SelectedIndex = 0;
        }

        private void uiSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string source = uiSourceType.SelectedItem.ToString();

            _webUriView.Visible = source == WEB_SERVICE_SOURCE;
            _filePathView.Visible = source == FILE_SOURCE;
        }

        public SourceType GetSourceType()
        {
            switch (uiSourceType.Text) { 
                case WEB_SERVICE_SOURCE:
                    return _webUriView.GetSourceType();
                case FILE_SOURCE:
                    return _filePathView.GetSourceType();
                default:
                    throw new ApplicationException("Invalid source type!");
            }
        }

        public void LoadModel(SourceModel m)
        {
            _model = m;
            switch (m.SourceType) { 
                case SourceType.FilePath:
                case SourceType.FilePathVariable:
                    uiSourceType.SelectedItem = FILE_SOURCE;
                    _filePathView.LoadModel(_model);
                    break;
                case SourceType.WebUrlPath:
                case SourceType.WebUrlVariable:
                    uiSourceType.SelectedItem = WEB_SERVICE_SOURCE;
                    _webUriView.LoadModel(_model);
                    break;
                default:
                    throw new ApplicationException("Invalid source type given.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            previewBtn.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            BackgroundWorker bw = new BackgroundWorker();
            
            // Download and Display Data
            #region
            bw.DoWork += (object s, DoWorkEventArgs a)=> {

                // Visualizza un'anteprima dei dati.
                // Per farlo devo acquisire i dati dalla view
                SourceType st = GetSourceType();
                string fpath = null;
                // Download the JSON file...
                switch (st)
                {
                    case SourceType.FilePath:
                        fpath = _filePathView.jsonFilePath.Text;
                        break;
                    case SourceType.FilePathVariable:
                        Variables vars = null;
                        try
                        {
                            IDtsPipelineEnvironmentService pipelineService = (IDtsPipelineEnvironmentService)_sp.GetService(typeof(IDtsPipelineEnvironmentService));
                            pipelineService.PipelineTaskHost.VariableDispenser.LockForRead(_filePathView.jsonFilePath.Text);
                            pipelineService.PipelineTaskHost.VariableDispenser.GetVariables(ref vars);
                            var v = vars[0];
                            if (v == null)
                                throw new Exception("Invalid variable provided.");
                            if (v.Value == null)
                                throw new Exception("Variable value is null.");
                            fpath = v.Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                            return;
                        }
                        finally
                        {
                            if (vars != null && vars.Locked)
                                vars.Unlock();
                        }
                        break;
                    case SourceType.WebUrlPath:
                    case SourceType.WebUrlVariable:
                        vars = null;
                        string method = _webUriView.GetHTTPMethod();
                        var pars = _webUriView.GetHttpParameters();
                        string cookievar = _webUriView.cookieVarTb.Text;
                        string input = _webUriView.uiWebURL.Text;
                        try
                        {
                            IDtsPipelineEnvironmentService pipelineService = (IDtsPipelineEnvironmentService)_sp.GetService(typeof(IDtsPipelineEnvironmentService));

                            if (String.IsNullOrEmpty(input))
                            {
                                throw new ArgumentException("Invalid Variable / URL Specified.");
                            }

                            if (st == SourceType.WebUrlVariable)
                            {
                                pipelineService.PipelineTaskHost.VariableDispenser.LockForRead(input);
                                pipelineService.PipelineTaskHost.VariableDispenser.GetVariables(ref vars);
                                var v = vars[0];
                                if (v == null)
                                    throw new Exception("Invalid variable provided.");
                                if (v.Value == null)
                                    throw new Exception("Variable value is null.");
                                string url = v.Value.ToString();
                                vars.Unlock();
                                fpath = Utils.DownloadJson(pipelineService.PipelineTaskHost.VariableDispenser, new Uri(url), method, pars, cookievar);
                            }
                            else if (st == SourceType.WebUrlPath)
                            {
                                fpath = Utils.DownloadJson(pipelineService.PipelineTaskHost.VariableDispenser, new Uri(input), method, pars, cookievar);
                            }
                        }
                        catch (Exception ex)
                        {
                            Exception i = ex;
                            while (i.InnerException != null)
                                i = i.InnerException;
                            MessageBox.Show("Inner Error: " + i.Message);
                            return;
                        }
                        finally
                        {
                            if (vars != null && vars.Locked)
                                vars.Unlock();
                        }
                        break;
                }
                a.Result = fpath;
            };
            bw.RunWorkerCompleted += (object s, RunWorkerCompletedEventArgs a)=> {
                previewBtn.Enabled = true;
                Cursor = Cursors.Default;

                if (a.Result != null)
                {
                    JSONPreview p = new JSONPreview();
                    p.Parse(a.Result as string);
                    p.ShowDialog();
                }
            };
            bw.RunWorkerAsync();
            #endregion
        }
    }
}

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
    }
}

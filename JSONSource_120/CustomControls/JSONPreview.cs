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

namespace com.webkingsoft.JSONSource_120.CustomControls
{
    public partial class JSONPreview : Form
    {
        public JSONPreview()
        {
            InitializeComponent();
        }

        public void Parse(string path)
        {
            if (String.IsNullOrEmpty(path) || !File.Exists(path)) {
                throw new ArgumentException("File path is invalid.");
            }

            using (StreamReader sr = new StreamReader(path)) {
                using (JsonTextReader jr = new JsonTextReader(sr)) {
                    // Prima lettura. Se è un array
                    JToken jt = JObject.ReadFrom(jr);
                    string formatted = jt.ToString(Newtonsoft.Json.Formatting.Indented);
                    richTextBox1.Text = formatted;
                }
            }
            

        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}

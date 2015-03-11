using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SSISInstaller
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.TopMost = true;
            this.FormClosing += delegate(object sender, FormClosingEventArgs e)
                                {
                                    Application.Exit();
                                };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void Append(string p)
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + p;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace com.webkingsoft.JSONSource_Common
{
    public partial class UserPrompter : Form
    {
        /// <summary>
        /// This delegate can be set to a function that checks whether the text value is acceptable by the caller.
        /// </summary>
        private Func<string,bool> _validator = new Func<string, bool>(UserPrompter.DummyValidator);
        private string _value = null;

        public string GetValue() {
            return _value;
        }

        public UserPrompter(string valueTitle,Func<string, bool> validator = null)
        {
            if (validator!=null)
                _validator = validator;
            
            InitializeComponent();

            this.uiValueTitleLabel.Text = valueTitle;
        }

        /// <summary>
        /// Represents the standard validator, which always returns TRUE
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool DummyValidator(string input) {
            return true;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            // Validate first
            bool valid = _validator(uiValueTitleLabel.Text);
            if (valid)
            {
                errorProvider1.SetError(uiValueTitleLabel, null);
                _value = valueBox.Text;
                // Set result = OK
                DialogResult = DialogResult.OK;
            }
            else
                errorProvider1.SetError(uiValueTitleLabel, "Invalid value detected.");
        }
    }
}

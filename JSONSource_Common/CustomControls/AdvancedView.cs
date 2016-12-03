using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
#if LINQ_SUPPORTED
using System.Linq;
#endif


namespace com.webkingsoft.JSONSource_Common
{
    public partial class AdvancedView : UserControl
    {
        public AdvancedView()
        {
            InitializeComponent();
            
            // Bind the combobox of datagrid to the values offered by ErrorHandling enumerable
            ((DataGridViewComboBoxColumn)httpErrorHandlingGv.Columns[1]).DataSource = Enum.GetNames(typeof(ErrorHandlingPolicy.ErrorHandling));
        }

        private void tmpBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog b = new FolderBrowserDialog();
            var r = b.ShowDialog();
            if (r == System.Windows.Forms.DialogResult.OK)
            {
                uiTempDir.Text = b.SelectedPath;
            }
        }

        public void LoadModel(JSONAdvancedSettingsModel advancedSettings)
        {
            // Load network error policy
            switch (advancedSettings.NetworkErrorPolicy.ErroHandlingMode) {
                case ErrorHandlingPolicy.ErrorHandling.STOP_IMMEDIATELY:
                    stopImmediatelyR.Checked = true;
                    break;
                case ErrorHandlingPolicy.ErrorHandling.SKIP:
                    skipR.Checked = true;
                    break;
                case ErrorHandlingPolicy.ErrorHandling.RETRY:
                    retryR.Checked = true;
                    retryLabel.Text = "" + advancedSettings.NetworkErrorPolicy.RetryAttempts;
                    waitTimeLabel.Text = "" + advancedSettings.NetworkErrorPolicy.SleepTimeInSeconds;
                    break;
                default:
                    throw new ApplicationException("invalid error handling policy. Please contact the developer.");
            }

            // Load the HTTP error policy
            httpErrorHandlingGv.Rows.Clear();
            foreach (var r in advancedSettings.HttpErrorPolicy) {
                int index = httpErrorHandlingGv.Rows.Add();
                httpErrorHandlingGv[0, index].Value = ""+r.Key;
                httpErrorHandlingGv[0, index].Tag = r.Key;

                httpErrorHandlingGv[1, index].Value = ""+r.Value.ErroHandlingMode;
                httpErrorHandlingGv[1, index].Tag = r.Value.ErroHandlingMode;

                httpErrorHandlingGv[2, index].Value = ""+r.Value.RetryAttempts;
                httpErrorHandlingGv[2, index].Tag = r.Value.RetryAttempts;

                httpErrorHandlingGv[3, index].Value = ""+ r.Value.SleepTimeInSeconds;
                httpErrorHandlingGv[3, index].Tag = r.Value.SleepTimeInSeconds;
            }

            // TODO: fixme. When loading the control, if ErrorPolicy is not retry, we should lock the waittime and retry attempts.

            if (advancedSettings.CustomLocalTempDir != null)
            {
                uiTempDir.Text = advancedSettings.CustomLocalTempDir;
            }

            cbParseJsonDate.Checked = advancedSettings.ParseDates;
        }

        public JSONAdvancedSettingsModel SaveToModel()
        {
            JSONAdvancedSettingsModel result = new JSONAdvancedSettingsModel();

            // Save network error policy
            ErrorHandlingPolicy networkErr = new ErrorHandlingPolicy();
            if (stopImmediatelyR.Checked)
                networkErr.ErroHandlingMode = ErrorHandlingPolicy.ErrorHandling.STOP_IMMEDIATELY;
            else if (skipR.Checked)
                networkErr.ErroHandlingMode = ErrorHandlingPolicy.ErrorHandling.SKIP;
            else if (retryR.Checked)
            {
                networkErr.ErroHandlingMode = ErrorHandlingPolicy.ErrorHandling.RETRY;
                int res;

                if (int.TryParse(retryLabel.Text, out res))
                    if (res > 0)
                        throw new ApplicationException("Please specify a positive integer value as retry attempts");
                    else
                        networkErr.RetryAttempts = res;
                else
                    throw new ApplicationException("Please specify a positive integer value as retry attempts");

                if (int.TryParse(waitTimeLabel.Text, out res))
                    if (res > 0)
                        throw new ApplicationException("Please specify a positive integer value sleep interval between failures");
                    else
                        networkErr.SleepTimeInSeconds = res;
                else
                    throw new ApplicationException("Please specify a positive integer value sleep interval between failures");

            }
            else
                throw new ApplicationException("No handling mode has been selected.");

            result.NetworkErrorPolicy = networkErr;

            // Save http error policy
            Dictionary<int, ErrorHandlingPolicy> httpcodes = new Dictionary<int, ErrorHandlingPolicy>();
            if (httpErrorHandlingGv.IsCurrentCellDirty || httpErrorHandlingGv.IsCurrentRowDirty)
            {
                httpErrorHandlingGv.CurrentRow.DataGridView.EndEdit();
                httpErrorHandlingGv.EndEdit();
                CurrencyManager cm = (CurrencyManager)httpErrorHandlingGv.BindingContext[httpErrorHandlingGv.DataSource, httpErrorHandlingGv.DataMember];
                cm.EndCurrentEdit();
            }

            // Save the handling policy regarding HTTP errors
            Dictionary<int, ErrorHandlingPolicy> http_err_policy = new Dictionary<int, ErrorHandlingPolicy>();
            foreach (DataGridViewRow r in httpErrorHandlingGv.Rows)
            {
                if (r.IsNewRow)
                {
                    continue;
                }

                if (r.ErrorText != "")
                {
                    throw new ApplicationException(String.Format("Invalid HTTP error policy specified in the table. Please check row {0}", (r.Index + 1)));
                }
                else {
                    ErrorHandlingPolicy p = new ErrorHandlingPolicy();
                    p.ErroHandlingMode = (ErrorHandlingPolicy.ErrorHandling)r.Cells[1].Tag;
                    p.RetryAttempts = (int)r.Cells[2].Tag;
                    p.SleepTimeInSeconds = (int)r.Cells[3].Tag;

                    http_err_policy.Add((int)r.Cells[0].Tag, p);
                }
            }

            result.HttpErrorPolicy = http_err_policy;

            if (!string.IsNullOrEmpty(uiTempDir.Text))
                result.CustomLocalTempDir = uiTempDir.Text;
            else
                result.CustomLocalTempDir = null;

            result.ParseDates = cbParseJsonDate.Checked;

            return result;
        }

        private void network_error_policy_CheckedChanged(object sender, EventArgs e)
        {
            // Just implement some UI logic to force basic constraints.
            waitTimeLabel.Enabled = retryR.Checked;
            retryLabel.Enabled = retryR.Checked;
        }

        private void validate_positive_integer(object sender, CancelEventArgs e)
        {
            // Only accept integer values
            var tbox = ((TextBox)sender);

            int r;
            if (!int.TryParse(tbox.Text, out r))
            {
                errorProvider.SetError(tbox, "Invalid value");
            }
            else {
                if (r < 0)
                {
                    errorProvider.SetError(tbox, "Invalid value");
                }
                else {
                    errorProvider.SetError(tbox, null);
                }
            }
        }

        private void cell_validation(object sender, DataGridViewCellValidatingEventArgs e)
        {
            object val = e.FormattedValue;
            switch (e.ColumnIndex) {
                case 0:
                    // HTTP CODE VALIDATION
                    int code = 0;
                    if (!int.TryParse(val.ToString(), out code)) {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Invalid http code value";
                        break;
                    }

                    if (code < 100 || code > 509) {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Invalid http code value";
                        break;
                    }

                    (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";

                    break;
                case 1:
                    // Handling policy
                    break;

                case 2:
                    // RetryLabel Attempts requires non-negative integer 
                    int attempts = 0;
                    if (!int.TryParse(val.ToString(), out attempts))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Invalid attempts value";
                        break;
                    }

                    if (attempts < 0)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Invalid attempts value";
                        break;
                    }

                    (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";

                    break;
                case 3:
                    // Wait time requires non-negative integer 
                    int waittime = 0;
                    if (!int.TryParse(val.ToString(), out waittime))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Invalid attempts value";
                        break;
                    }

                    if (waittime < 0)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Invalid attempts value";
                        break;
                    }

                    (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";

                    break;
            }
        }

        private void httpErrorHandlingGv_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Check row is consistent
            var row = (sender as DataGridView).Rows[e.RowIndex];

            // If error policy requires wait time and retries, check those are available
            if (row.Cells[1].Value == null) {
                row.ErrorText = "Please specify a valid error handling policy.";
                return;
            }

            if ((ErrorHandlingPolicy.ErrorHandling)Enum.Parse(typeof(ErrorHandlingPolicy.ErrorHandling),row.Cells[1].Value.ToString()) == ErrorHandlingPolicy.ErrorHandling.RETRY) {
                if (row.Cells[2].Value==null || row.Cells[2].ErrorText != "")
                {
                    row.ErrorText = "Please specify a valid retry attempts number.";
                    return;
                }

                if (row.Cells[3].Value==null || row.Cells[3].ErrorText != "")
                {
                    row.ErrorText = "Please specify a valid waiting time in seconds.";
                    return;
                }
            }

            // If we got here, everything is ok, so clear the error.
            row.ErrorText = "";
        }

        private void httpErrorHandlingGv_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            // If the Error Handling value is retry, enable other two cells (wait time and retry attempts)
            // Otherwise disable them
            if (e.ColumnIndex == 1)
            {
                var val = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                if (val != null)
                {
                    var policy = (ErrorHandlingPolicy.ErrorHandling)Enum.Parse(typeof(ErrorHandlingPolicy.ErrorHandling), val.ToString());
                    if (policy == ErrorHandlingPolicy.ErrorHandling.RETRY)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[2].ReadOnly = false;
                        (sender as DataGridView).Rows[e.RowIndex].Cells[2].Style.BackColor = Color.White;

                        (sender as DataGridView).Rows[e.RowIndex].Cells[3].ReadOnly = false;
                        (sender as DataGridView).Rows[e.RowIndex].Cells[3].Style.BackColor = Color.White;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[2].ReadOnly = true;
                        (sender as DataGridView).Rows[e.RowIndex].Cells[2].Style.BackColor = Color.LightGray;
                        (sender as DataGridView).Rows[e.RowIndex].Cells[3].ReadOnly = true;
                        (sender as DataGridView).Rows[e.RowIndex].Cells[3].Style.BackColor = Color.LightGray;
                    }

                    if ((sender as DataGridView).Rows[e.RowIndex].Cells[2].Value == null || (sender as DataGridView).Rows[e.RowIndex].Cells[2].Value == "")
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[2].Value = "1";
                    }

                    if ((sender as DataGridView).Rows[e.RowIndex].Cells[3].Value == null || (sender as DataGridView).Rows[e.RowIndex].Cells[3].Value == "")
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells[3].Value = "0";
                    }
                }
            }
        }

        private void httpErrorHandlingGv_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            // Save parsed values into TAG handles
            DataGridViewRow r = httpErrorHandlingGv.Rows[e.RowIndex];

            if (r.IsNewRow || r.ErrorText != "")
                return;

            int code = int.Parse(r.Cells[0].Value.ToString());
            ErrorHandlingPolicy.ErrorHandling policy = (ErrorHandlingPolicy.ErrorHandling)Enum.Parse(typeof(ErrorHandlingPolicy.ErrorHandling),r.Cells[1].Value.ToString());
            int retry = int.Parse(r.Cells[2].Value.ToString());
            int waittime = int.Parse(r.Cells[3].Value.ToString());

            r.Cells[0].Tag = code;
            r.Cells[1].Tag = policy;
            r.Cells[2].Tag = retry;
            r.Cells[3].Tag = waittime;
        }
    }
}

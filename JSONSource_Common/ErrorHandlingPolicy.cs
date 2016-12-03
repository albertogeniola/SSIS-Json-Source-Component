using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace com.webkingsoft.JSONSource_Common
{
    /// <summary>
    /// This is an helper class used to handle error cases during code execution at runtime. The user may specify how many times to retry an operation
    /// and how to "fail". In practice he may specify the maximum number of retry attempts and the "sleep" time in case of failure.
    /// </summary>
    public class ErrorHandlingPolicy
    {
        private int _attempts = 0;
        private int _max_attempts = 1;
        private int _sleep_time = 0;
        private ErrorHandling _mode;

        [JsonProperty]
        public int SleepTimeInSeconds
        {
            get { return _sleep_time; }
            set { _sleep_time = value; }
        }

        [JsonProperty]
        public int RetryAttempts {
            get { return _max_attempts; }
            set { _max_attempts = value; }
        }

        [JsonProperty]
        public ErrorHandling ErroHandlingMode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        /// <summary>
        /// This method should be used to check whether to retry the failed operation or not. This method is statefull, i.e. it keeps count 
        /// of failed operations.
        /// </summary>
        /// <returns>True when the caller should retry, False otherwise.</returns>
        public bool ShouldContinue() {
            switch (_mode) {
                case ErrorHandling.STOP_IMMEDIATELY:
                    return false;
                case ErrorHandling.SKIP:
                    // Do not increase attempts and simply move over
                    return true;
                case ErrorHandling.RETRY:
                    // If _max_attempts is 0, increase the counter and continue. We will continue indefinetely!
                    if (_max_attempts == 0) {
                        _attempts++;
                        return true;
                    }

                    // If below the failure threshold, retry. Otherwise fail.
                    if (_attempts < _max_attempts)
                    {
                        _attempts++;
                        return true;
                    }
                    else {
                        return false;
                    }
                default:
                    throw new ApplicationException("invalid ErrorHandling value. Contact the developer.");
            }
        }

        public enum ErrorHandling
        {
            [Description("Stop and fail")]
            STOP_IMMEDIATELY,

            [Description("Ignore")]
            SKIP,

            [Description("Retry")]
            RETRY
        }
    }
}

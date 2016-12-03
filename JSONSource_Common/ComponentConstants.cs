using System;
using System.Collections.Generic;
using System.Text;
#if LINQ_SUPPORTED
using System.Linq;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    public class ComponentConstants
    {
        public const int WARNING_FILE_MISSING = 10;
        public const int WARNING_CUSTOM_TEMP_DIR_INVALID = 11;

        public const int ERROR_NO_INPUT_SUPPORTED = 1;
        public const int ERROR_FILE_PATH_MISSING = 10;
        public const int ERROR_WEB_URL_MISSING = 11;
        public const int ERROR_WEB_URL_VARIABLE_MISSING = 12;
        public const int ERROR_FILE_VARIABLE_WRONG = 13;
        public const int ERROR_IOMAP_EMPTY = 14;
        public const int ERROR_IOMAP_ENTRY_ERROR = 15;
        public const int ERROR_SINGLE_OUTPUT_SUPPORTED = 16;
        public const int ERROR_INPUT_LANE_NOT_FOUND = 17;

        public const int ERROR_SELECT_TOKEN = 1001;
        public const int ERROR_INVALID_BUFFER_SIZE = 1002;

        public const int RUNTIME_ERROR_MODEL_INVALID = 100;

        public const int RUNTIME_GENERIC_ERROR = 0;

        public static readonly string PROPERTY_KEY_MODEL = "CONFIGURATION_MODEL_OBJECT";


        public const string NAME_INPUT_LANE_PARAMS = "Http Parameters Lane";

        // Error output constants
        public const string NAME_OUTPUT_ERROR_LANE = "Errors";
        public const string NAME_OUTPUT_ERROR_LANE_ERROR_TYPE = "Error Type";
        public const string NAME_OUTPUT_ERROR_LANE_ERROR_DETAILS = "Error Details";
        public const string NAME_OUTPUT_ERROR_LANE_ERROR_HTTP_CODE = "HttpCode";


        public static ErrorHandlingPolicy NewDefaultNetworkHandlingPolicy() {
            ErrorHandlingPolicy res = new ErrorHandlingPolicy();
            res.ErroHandlingMode = ErrorHandlingPolicy.ErrorHandling.STOP_IMMEDIATELY;
            res.RetryAttempts = 0;
            res.SleepTimeInSeconds = 0;

            return res;
        }

        /// <summary>
        /// This method returns a dictionary of ErrorHandling policies associated to some well-known http status codes.
        /// Bu default we will simply stop on errors 4xx and 5xx
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, ErrorHandlingPolicy> NewDefaultHttpHandlingPolicy()
        {
            Dictionary<int, ErrorHandlingPolicy> res = new Dictionary<int, ErrorHandlingPolicy>();

            // HTTP 400:
            ErrorHandlingPolicy p400 = new ErrorHandlingPolicy();
            p400.ErroHandlingMode = ErrorHandlingPolicy.ErrorHandling.STOP_IMMEDIATELY;
            p400.RetryAttempts = 0;
            p400.SleepTimeInSeconds = 0;
            res.Add(400, p400);

            // HTTP 500:
            ErrorHandlingPolicy p500 = new ErrorHandlingPolicy();
            p500.ErroHandlingMode = ErrorHandlingPolicy.ErrorHandling.STOP_IMMEDIATELY;
            p500.RetryAttempts = 0;
            p500.SleepTimeInSeconds = 0;
            res.Add(500, p500);

            return res;
        }
    }
}

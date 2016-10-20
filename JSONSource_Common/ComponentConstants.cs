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
    }
}

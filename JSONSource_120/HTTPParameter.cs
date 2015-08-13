using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.webkingsoft.JSONSource_120
{
    public class HTTPParameter
    {
        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public HTTPParamBinding Binding
        {
            get;
            set;
        }

        public bool Encode
        {
            get;
            set;
        }

    }
}

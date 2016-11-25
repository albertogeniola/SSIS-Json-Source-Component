using System;
using System.Collections.Generic;
using System.Text;
#if LINQ_SUPPORTED
using System.Linq;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    public class HTTPParameter
    {
        public string Name
        {
            get;
            set;
        }

        public string BindingValue
        {
            get;
            set;
        }

        public ParamBinding Binding
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

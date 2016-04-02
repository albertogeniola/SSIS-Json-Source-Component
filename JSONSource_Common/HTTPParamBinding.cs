using System;
using System.Collections.Generic;
using System.Text;
#if LINQ_SUPPORTED
using System.Linq;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    public enum HTTPParamBinding
    {
        Variable,
        CustomValue,
        InputField
    }
}

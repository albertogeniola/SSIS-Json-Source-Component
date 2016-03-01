using System;
using System.Collections.Generic;
using System.Text;
#if LING_SUPPORTED
using System.Linq;
#endif

namespace com.webkingsoft.JSONSource_Common
{
    public enum DataModelType
    {
        SingleObject,
        ArrayOfObjects,
        ArrayOfData,
        DataTable
    }
}

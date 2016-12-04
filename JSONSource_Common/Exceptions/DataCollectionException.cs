using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.webkingsoft.JSONSource_Common.Exceptions
{
    public class DataCollectionException:Exception
    {
        public DataCollectionException(string msg, Exception inner) : base(msg, inner) { }
    }
}

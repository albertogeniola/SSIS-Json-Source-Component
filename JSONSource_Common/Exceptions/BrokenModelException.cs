using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.webkingsoft.JSONSource_Common.Exceptions
{
    public class BrokenModelException : Exception
    {
        public BrokenModelException():base(){}
        public BrokenModelException(string msg, Exception inner) : base(msg,inner) { }
    }
}

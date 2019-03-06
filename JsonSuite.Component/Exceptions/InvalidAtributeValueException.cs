using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.webkingsoft.JsonSuite.Component.Exceptions
{
    public class InvalidAtributeValueException : Exception
    {
        public InvalidAtributeValueException(string msg) : base(msg) { }
    }
}

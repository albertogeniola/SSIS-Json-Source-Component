using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace com.webkingsoft.JSONSource_Common.Exceptions
{
    public class BadHttpCodeException : Exception
    {
        private HttpResponseMessage _m;

        public BadHttpCodeException(HttpResponseMessage m) : base() {
            _m = m;
        }

        public int GetResponseCode()
        {
            return (int)_m.StatusCode;
        }
    }
}

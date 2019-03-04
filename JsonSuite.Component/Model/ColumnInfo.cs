using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.webkingsoft.JSONSuite_Common.Model
{
    public struct ColumnInfo
    {
        public int bufferColumnIndex;
        public DTSRowDisposition columnDisposition;
        public int lineageId;
    }
}

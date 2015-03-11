using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SSISInstaller
{
    public enum SQLVersion
    {
        [Description("Microsoft SQL Server 2014")]
        SQL2014 = 120,
        [Description("Microsoft SQL Server 2012")]
        SQL2012 = 110,
        [Description("Microsoft SQL Server 2008 / 2008 R2")]
        SQL2008 = 100
    }
}

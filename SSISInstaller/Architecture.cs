using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SSISInstaller
{
    public enum Architecture
    {
        [Description("(x32)")]
        x32,
        [Description("(x64)")]
        x64
    }
}

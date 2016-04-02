using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.webkingsoft.JSONSource_Common
{
    public interface IComponentModel
    {
        bool Validate(out string err, out string warn);
        string ToJsonConfig();
    }
}

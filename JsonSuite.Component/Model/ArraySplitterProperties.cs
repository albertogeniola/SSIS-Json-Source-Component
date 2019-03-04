using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.webkingsoft.JsonSuite.Component.Model
{
    public class ArraySplitterProperties : AbstractCustomPropertyHelper
    {
        private static readonly string INPUT_COLUMN_NAME = "InputColumnName";
        private static readonly string INPUT_COLUMN_NAME_DESC = "Name of the column, belonging to the input lane, where the raw json is coming from.";

        public string ArrayInputColumnName { get; set; }
        
        protected override void InternalCopyToComponent(IDTSCustomPropertyCollection100 componentCustomProperties)
        {
            var p = CreateCustomProperty(componentCustomProperties, INPUT_COLUMN_NAME, INPUT_COLUMN_NAME_DESC);
            p.Value = ArrayInputColumnName;
        }

        public override void LoadFromComponent(IDTSCustomPropertyCollection100 componentCustomProperties)
        {
            ArrayInputColumnName = GetCustomProperty(componentCustomProperties, INPUT_COLUMN_NAME) as string;
        }
    }
}

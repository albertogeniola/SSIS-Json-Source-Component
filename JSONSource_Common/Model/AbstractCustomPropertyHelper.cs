using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.webkingsoft.JSONSuite_Common.Model
{
    public abstract class AbstractCustomPropertyHelper
    {
        /// <summary>
        /// Stores the custom properties into the component metadata.
        /// </summary>
        /// <param name="componentCustomProperties"></param>
        public void CopyToComponent(IDTSCustomPropertyCollection100 componentCustomProperties) {
            // Delete any previous custom property
            componentCustomProperties.RemoveAll();

            // Specific component logic
            InternalCopyToComponent(componentCustomProperties);
        }

        /// <summary>
        /// Child class implementation logic to copy its specific properties into the component
        /// </summary>
        /// <param name="componentCustomProperties"></param>
        protected abstract void InternalCopyToComponent(IDTSCustomPropertyCollection100 componentCustomProperties);

        /// <summary>
        /// Loads the properties from the component metadata.
        /// </summary>
        /// <param name="componentCustomProperties"></param>
        public abstract void LoadFromComponent(IDTSCustomPropertyCollection100 componentCustomProperties);

        protected void CreateCustomProperty(IDTSCustomPropertyCollection100 props, string name, string description) {
            var p = props.New();
            p.Name = name;
            p.Description = description;
        }

        protected object GetCustomProperty(IDTSCustomPropertyCollection100 props, string name) {
            return props[name]?.Value;
        }
    }
}

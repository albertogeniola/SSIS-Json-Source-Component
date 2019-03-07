using Microsoft.SqlServer.Dts.Pipeline.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using System.Windows.Forms;

namespace com.webkingsoft.JsonSuite.UI
{
    class ObjectParserUI : IDtsComponentUI
    {
        private ObjectParserGUI _view;

        public void Initialize(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            // This method is invoked before Edit/New. In here we simply create the
            // form to display and save reference to the component stuff we will later on be useful.
            _view = new ObjectParserGUI(dtsComponentMetadata, serviceProvider);
        }

        public void Delete(IWin32Window parentWindow)
        {
            // Do nothing!
        }

        public bool Edit(IWin32Window parentWindow, Variables variables, Connections connections)
        {
            // This method should return TRUE if the user performed any change and wants to save it.
            // False otherwise. The DesignTime will persist changes only if this method returns true.
            var result = _view.ShowDialog();
            return result == DialogResult.OK;
        }

        public void Help(IWin32Window parentWindow)
        {
            // Do nothing!
        }
        
        public void New(IWin32Window parentWindow)
        {
            // Do nothing!
        }
    }
}

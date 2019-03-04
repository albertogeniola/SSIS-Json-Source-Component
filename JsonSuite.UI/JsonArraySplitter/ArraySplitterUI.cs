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
    class ArraySplitterUI : IDtsComponentUI
    {
        public void Delete(IWin32Window parentWindow)
        {
            MessageBox.Show("works!");
            throw new NotImplementedException();
        }

        public bool Edit(IWin32Window parentWindow, Variables variables, Connections connections)
        {
            MessageBox.Show("works!");
            throw new NotImplementedException();
        }

        public void Help(IWin32Window parentWindow)
        {
            MessageBox.Show("works!");
            throw new NotImplementedException();
        }

        public void Initialize(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            MessageBox.Show("works!");
            throw new NotImplementedException();
        }

        public void New(IWin32Window parentWindow)
        {
            MessageBox.Show("works!");
            throw new NotImplementedException();
        }
    }
}

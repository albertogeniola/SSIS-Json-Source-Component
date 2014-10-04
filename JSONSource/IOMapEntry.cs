using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONSource
{
    public class IOMapEntry
    {
        private string inputFieldName;

        public string InputFieldName
        {
            get { return inputFieldName; }
            set { inputFieldName = value; }
        }

        private int inputFieldLen;

        public int InputFieldLen
        {
            get { return inputFieldLen; }
            set { inputFieldLen = value; }
        }

        private string outputColName;

        public string OutputColName
        {
            get { return outputColName; }
            set { outputColName = value; }
        }

        private DataType outputColumnType;

        public DataType OutputColumnType
        {
            get { return outputColumnType; }
            set { outputColumnType = value; }
        }

       

        public const string EL_IOMAPROW="IOMAPROW";
        public const string ATT_INPUTFIELDNAME = "InputFieldName";
        public const string ATT_INPUTFIELDLEN ="InputfieldLength";
        public const string ATT_OUTPUTFIELDNAME = "OutputFieldName";
        public const string ATT_OUTPUTFIELDTYPE = "OutputFieldType";
       
        public void WriteToXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement(EL_IOMAPROW);
            writer.WriteAttributeString(ATT_INPUTFIELDNAME, inputFieldName);
            writer.WriteAttributeString(ATT_INPUTFIELDLEN,inputFieldLen.ToString());
            writer.WriteAttributeString(ATT_OUTPUTFIELDNAME,outputColName);
            writer.WriteAttributeString(ATT_OUTPUTFIELDTYPE,outputColumnType.ToString());
            writer.WriteEndElement(); // Closing EL_IOMAPROW
        }

        public static IOMapEntry Load(System.Xml.XmlReader reader)
        {
            IOMapEntry e = new IOMapEntry();

            string fieldname = null;
            try
            {
                fieldname = reader.GetAttribute(ATT_INPUTFIELDNAME);
            }
            catch (ArgumentException ex)
            {}

            int fieldlen = 0;
            try
            {
                fieldlen = int.Parse(reader.GetAttribute(ATT_INPUTFIELDLEN));
            }
            catch (ArgumentException ex)
            {}

            string outputName = null;
            try
            {
                outputName = reader.GetAttribute(ATT_OUTPUTFIELDNAME);
            }
            catch (ArgumentException ex)
            {}

            DataType dt = 0;
            try
            {
                dt = (DataType)Enum.Parse(typeof(DataType),reader.GetAttribute(ATT_OUTPUTFIELDTYPE));
            }
            catch (ArgumentException ex)
            {}

            

            e.inputFieldName = fieldname;
            e.inputFieldLen = fieldlen;
            e.outputColName = outputName;
            e.OutputColumnType = dt;
            
            if (e.inputFieldName == null)
                return null;
            else
                return e;
        }
    }
}

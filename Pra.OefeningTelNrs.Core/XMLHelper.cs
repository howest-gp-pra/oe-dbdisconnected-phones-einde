using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace Pra.OefeningTelNrs.Core
{
    public class XMLHelper
    {
        public static DataSet ReadXML(string XMLFolder, string XMLFileName)
        {
            DataSet dsPhone = null;
            if (Directory.Exists(XMLFolder))
            {
                if (File.Exists(XMLFileName))
                {
                    dsPhone = new DataSet();
                    dsPhone.ReadXml(XMLFileName, XmlReadMode.ReadSchema);
                }
            }
            return dsPhone;
        }
        public static void WriteXML(string XMLFolder, string XMLFileName, DataSet dsToSave)
        {
            if (!Directory.Exists(XMLFolder))
                Directory.CreateDirectory(XMLFolder);
            if (File.Exists(XMLFileName))
                File.Delete(XMLFileName);
            dsToSave.WriteXml(XMLFileName, XmlWriteMode.WriteSchema);

        }
    }
}

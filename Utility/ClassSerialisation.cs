using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;


namespace Utility
{
   public class ClassSerialisation
    {
        public static void SeriliazeToXMl<T>(ref T inObject, string inFileName)
        {
            try
            {
                XmlSerializer writter = new XmlSerializer(typeof(T));
                StreamWriter file = new StreamWriter(inFileName);
                writter.Serialize(file, inObject);
                file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void DeserializeFromXML<T>(ref T inObject, string inFileName)
        {
            if (File.Exists(inFileName))
            {
                XmlSerializer reader = new XmlSerializer(typeof(T));
                StreamReader file = new StreamReader(inFileName);
                inObject = (T)reader.Deserialize(file);
                file.Close();
            }
        }
    }
}

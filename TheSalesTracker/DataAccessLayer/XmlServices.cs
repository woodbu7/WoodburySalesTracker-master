using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace TheSalesTracker
{
    public class XmlServices
    {
        #region Fields

        private string _dataFilePath;

        #endregion

        #region Constructors

        public XmlServices(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        #endregion

        #region Methods

        public Salesperson ReadSalespersonFromDataFile()
        {
            Salesperson salesperson = new Salesperson();

            StreamReader sReader = new StreamReader(_dataFilePath);

            XmlSerializer deserializer = new XmlSerializer(typeof(Salesperson));

            using (sReader)
            {
                object xmlObject = deserializer.Deserialize(sReader);
                Console.WriteLine(xmlObject);
                salesperson = (Salesperson)xmlObject;
            }

            return salesperson;
        }

        public void WriteSalespersonToDataFile(Salesperson salesperson)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Salesperson), new XmlRootAttribute("Salesperson"));

            StreamWriter sWriter = new StreamWriter(_dataFilePath);

            using (sWriter)
            {
                serializer.Serialize(sWriter, salesperson);
            }
        }

        #endregion
    }
}

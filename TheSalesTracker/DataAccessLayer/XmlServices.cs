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

        /// <summary>
        /// Read Salesperson Account from a the datafile
        /// </summary>
        /// <returns></returns>
        public Salesperson ReadSalespersonFromDataFile()
        {
            // instantiate a new Salesperson object
            //Salesperson salesperson = new Salesperson();
            Salesperson salesperson;

            StreamReader sReader = new StreamReader(_dataFilePath); //Data File = Data.xml

            XmlSerializer deserializer = new XmlSerializer(typeof(Salesperson));
      
            using (sReader)
            {
                object xmlObject = deserializer.Deserialize(sReader);
                salesperson = xmlObject as Salesperson;
            }

            return salesperson;
        }

        /// <summary>
        /// Write Salesperson account to Data File
        /// </summary>
        /// <param name="salesperson"></param>
        public void WriteSalespersonToDataFile(Salesperson salesperson)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Salesperson));

            StreamWriter sWriter = new StreamWriter(_dataFilePath); //Data File = Data.xml

            using (sWriter)
            {
                serializer.Serialize(sWriter, salesperson);
            }
        }

        #endregion
    }
}

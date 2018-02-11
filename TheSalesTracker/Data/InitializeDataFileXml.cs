using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace TheSalesTracker
{
    public class InitializeDataFileXml
    {
        private Salesperson InitializeSalesperson()
        {

            Salesperson salesperson = new Salesperson()
            {
                FirstName = "Madeleine",
                LastName = "Woodbury",
                AccountID = "123",
                CurrentStock = new List<Product>()
                {
                    new Product(Product.ProductType.Bike, 100, false)
                },
                CitiesVisited = new List<City>()
                {
                    new City("Oslo")
     
                }
            };
            
         
            return salesperson;
        }

        public void SeedDataFile()
        {
            XmlServices xmlService = new XmlServices(DataSettings.dataFilePathXml);

            xmlService.WriteSalespersonToDataFile(InitializeSalesperson());
        }
        
    }
}

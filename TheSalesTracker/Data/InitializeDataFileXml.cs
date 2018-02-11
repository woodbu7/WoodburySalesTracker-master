using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                //CurrentStock = new Product(Product.ProductType.Benchpress, 25, false),
                //CitiesVisited = new List<City>()
                //{
                //    new City("Oslo", 25, 10),
                //    new City("Bergen", 50, 30),
                //}
                
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

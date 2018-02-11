using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSalesTracker
{
    public class City
    {
        #region Fields

        private string _cityName;
        private List<Product> _salesInfo;

        #endregion

        #region Properties

        public string CityName
        {
            get { return _cityName; }
            set { _cityName = value; }
        }

        public List<Product> SalesInfo
        {
            get { return _salesInfo; }
            set { _salesInfo = value; }
        }

        #endregion

        #region Constructors

        public City()
        {

        }

        public City(string cityName)
        {
            _cityName = cityName;
            _salesInfo = new List<Product>();
        }

        #endregion

    }
}

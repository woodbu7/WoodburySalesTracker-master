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
        private List<Product.ProductType> _productSold;
        private int _numberOfProductsSold;
        private int _numberOfProductsBought;

        #endregion

        #region Properties

        public string CityName
        {
            get { return _cityName; }
            set { _cityName = value; }
        }

        public int NumberOfProductsSold
        {
            get { return _numberOfProductsSold; }
            set { _numberOfProductsSold = value; }
        }

        public int NumberOfProductsBought
        {
            get { return _numberOfProductsBought; }
            set { _numberOfProductsBought = value; }
        }

        #endregion

        #region Constructors

        public City()
        {

        }
        public City(string cityName, int numberOfProductsSold, int numberOfProductsBought)
        {
            _cityName = cityName;
           // _productsSold = new Product.ProductType();
           // _productsBought = new Product.ProductType();
            _numberOfProductsSold = numberOfProductsSold;
            _numberOfProductsBought = numberOfProductsBought;
        }

        #endregion

    }
}

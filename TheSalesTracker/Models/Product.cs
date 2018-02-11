using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSalesTracker
{
    public class Product
    {
        #region Enumerables
        public enum ProductType
        {
            None,
            Treadmill,
            Eliptical,
            Bike,
            Bowflex,
            Benchpress,
            
        }

        #endregion

        #region Fields

        private int _numberOfUnits;
        private bool _onBackorder;
        private ProductType _type;
        private int _productsBought;
        private int _productsSold;

        #endregion

        #region Properties

        public int NumberOfUnits
        {
            get { return _numberOfUnits; }
        }

        public bool OnBackorder
        {
            get { return _onBackorder; }
            set { _onBackorder = value; }
        }

        public ProductType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int ProductsBought
        {
            get { return _productsBought; }
            set { _productsBought = value; }
        }

        public int ProductsSold
        {
            get { return _productsSold; }
            set { _productsSold = value; }
        }

        #endregion

        #region Constructors

        public Product()
        {
            _type = ProductType.None;
            _numberOfUnits = 0;
        }

        public Product(ProductType type, int numberOfUnits, bool onBackorder)
        {
            _type = type;
            _numberOfUnits = numberOfUnits;
            _onBackorder = onBackorder;
        }

        public Product(ProductType type, int productsBought, int productsSold)
        {
            _type = type;
            _productsBought = productsBought;
            _productsSold = productsSold;
        }

        #endregion

        #region Methods

        /// <summary>
        /// increments NumberOfUnits property
        /// </summary>
        /// <param name="unitsToAdd"></param>
        public void AddProducts(int unitsToAdd)
        {
            _numberOfUnits += unitsToAdd;
        }

        /// <summary>
        /// decrements NumberOfUnits property and sets OnBackorder status
        /// </summary>
        /// <param name="unitsToSubtract"></param>
        public void SubtractProducts(int unitsToSubtract)
        {
            if (_numberOfUnits < unitsToSubtract)
            {
                _onBackorder = true;
            }

            _numberOfUnits -= unitsToSubtract;
        }

        public void BuyProducts(int unitsToAdd)
        {
            _productsBought += unitsToAdd;
        }

        #endregion
    }
}

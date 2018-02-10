﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSalesTracker
{
    /// <summary>
    /// MVC Controller class
    /// </summary>
    public class Controller
    {
        #region Fields

        private ConsoleView _consoleView;
        private Salesperson _salesperson;
        private bool _usingApplication;
        private City city;
        private Product _product;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public Controller()
        {
            InitializeController();

            //
            // instantiate a Salesperson object
            //
            _salesperson = new Salesperson();

            //
            // instantiate a ConsoleView object
            //
            _consoleView = new ConsoleView();

            //
            // begins running the application UI
            //
            ManageApplicationLoop();
        }


        #endregion

        #region Methods

        /// <summary>
        /// initialize the controller 
        /// </summary>
        private void InitializeController()
        {
            _usingApplication = true;
        }

        /// <summary>
        /// method to manage the application setup and control loop
        /// </summary>
        private void ManageApplicationLoop()
        {
            MenuOption userMenuChoice;

            _consoleView.DisplayWelcomeScreen();

            //
            // application loop
            //
            while (_usingApplication)
            {

                //
                // get a menu choice from the ConsoleView object
                //
                userMenuChoice = _consoleView.DisplayGetUserMenuChoice();

                //
                // choose an action based on the user's menu choice
                //
                switch (userMenuChoice)
                {
                    case MenuOption.None:
                        break;
                    case MenuOption.SetupAccount:
                        city = SetupAccount();
                        break;
                    case MenuOption.Travel:
                        city = Travel();
                        break;
                    case MenuOption.Buy:
                        PurchaseMenu();
                        break;
                    case MenuOption.Sell:
                        Sell();
                        break;
                    case MenuOption.AddNewProducts:
                        BuyNewProducts();
                        break;
                    case MenuOption.DisplayInventory:
                        DisplayInventory();
                        break;
                    case MenuOption.DisplayCities:
                        DisplayCities();
                        break;
                    case MenuOption.DisplayAccountInfo:
                        DisplayAccountInfo();
                        break;
                    case MenuOption.SaveAccountInfo:
                        DisplaySaveAccountInfo();
                        break;
                    case MenuOption.LoadAccountInfo:
                        DisplayLoadAcoountInfo();
                        break;
                    case MenuOption.Exit:
                        _usingApplication = false;
                        break;
                    default:
                        break;
                }
            }

            _consoleView.DisplayClosingScreen();

            //
            // close the application
            //
            Environment.Exit(1);
        }

        public void PurchaseMenu()
        {
            int userChoice = _consoleView.DisplayPurchaseMenu();

            switch (userChoice)
            {
                case 1:
                    BuyNewProducts();
                    break;
                case 2:
                    Buy();
                    break;
                case 3:
                    _consoleView.DisplayContinuePrompt();
                break;
                default:
                    break;
            }

        }

        /// <summary>
        /// add the next city location to the list of cities
        /// </summary>
        private City Travel()
        {
            // instantiate new city object
            City _city = new City();
            _city.CityName = _consoleView.DisplayGetNextCity();
            _city.NumberOfProductsBought = 0; // default value
            _city.NumberOfProductsSold = 0; // default value
            
            //
            // do not add empty strings to list for city names
            //
            if (_city.CityName != "")
            {
                _salesperson.CitiesVisited.Add(_city);
            }

            return _city;
        }

        /// <summary>
        /// display all cities traveled to
        /// </summary>
        private void DisplayCities()
        {
            _consoleView.DisplayCitiesTraveled(_salesperson);
        }

        /// <summary>
        /// display account information
        /// </summary>
        private void DisplayAccountInfo()
        {
            _consoleView.DisplayAccountInfo(_salesperson);
        }

        public void BuyNewProducts()
        {
            // instantiate a new product
            Product _product = new Product();
            _product.Type = _consoleView.DisplayBuyNewProducts(_salesperson);

            if (_product.Type != Product.ProductType.None)
            {
                int numberOfUnits = _consoleView.GetNumberOfUnitsToBuy();
                _product.AddProducts(numberOfUnits);
                _product.OnBackorder = false;

                _salesperson.CurrentStock.Add(_product);
            }
     
        }

        /// <summary>
        /// buy products
        /// </summary>
        private void Buy()
        {
            Product.ProductType productType = _consoleView.DisplayBuyExistingProducut(_salesperson);
            int numberOfUnits = 0;

            foreach (Product item in _salesperson.CurrentStock)
            {
                if (item.Type == productType && productType != Product.ProductType.None)
                {
                    numberOfUnits = _consoleView.GetNumberOfUnitsToBuy();
                    item.AddProducts(numberOfUnits);
                }
            }

            // add number of units bought
            //city.NumberOfProductsBought = city.NumberOfProductsBought + numberOfUnits;
            //city.NumberOfProductsBought = city.NumberOfProductsBought + numberOfUnits;
        }

        
        /// <summary>
        /// sell product
        /// </summary>
        private void Sell()
        {
            Product.ProductType productType = _consoleView.DisplaySellProducts(_salesperson);
            int numberOfUnits = 0;

            foreach (Product item in _salesperson.CurrentStock)
            {
                if (item.Type == productType && productType != Product.ProductType.None)
                {
                    numberOfUnits = _consoleView.GetNumberOfUnitsToSell();
                    item.SubtractProducts(numberOfUnits);

                    if (item.OnBackorder)
                    {
                        _consoleView.DisplayBackorderNotification(item, numberOfUnits);
                    }
                }
            }

            //int numberOfUnits = _consoleView.DisplayGetNumberOfUnitsToSell(_salesperson.CurrentStock);
            //_salesperson.CurrentStock.SubtractProducts(numberOfUnits);

            // if number of units to sell is less than current stock, run backorder notifaction
            //if (_salesperson.CurrentStock.OnBackorder)
            //{
            //    _consoleView.DisplayBackorderNotification(_salesperson.CurrentStock, numberOfUnits);
            //}

            //_city.NumberOfProductsSold = _city.NumberOfProductsSold - numberOfUnits;
            //city.NumberOfProductsSold = city.NumberOfProductsSold - numberOfUnits;
        }
        
    
        /// <summary>
        /// display inventory
        /// </summary>
        private void DisplayInventory()
        {
            _consoleView.DisplayInventory(_salesperson);
        }

    
        

        /// <summary>
        /// setup new user account
        /// </summary>
        private City SetupAccount()
        {

            _salesperson = _consoleView.DisplaySetupAccount(out City city);

            return city;

        }

        /// <summary>
        /// calls the method to display save account info from the ConsoleView and
        /// saves salesperson and travel log info to the data file
        /// </summary>
        private void DisplaySaveAccountInfo()
        {
            bool maxAttemptsExceeded = false;
            bool saveAccountInfo = false;

            saveAccountInfo = _consoleView.DisplaySaveAccountInfo(_salesperson, out maxAttemptsExceeded);

            if (saveAccountInfo && !maxAttemptsExceeded)
            {
                XmlServices xmlServices = new XmlServices(DataSettings.dataFilePathXml);

                xmlServices.WriteSalespersonToDataFile(_salesperson);

                // displays the confirmation that the account have been saved
                _consoleView.DisplayConfirmSaveAccountInfo();
            }
        }

        /// <summary>
        /// calls the method to display load account info from the ConsoleView and
        /// reads salesperson and travel log infor from data file
        /// </summary>
        private void DisplayLoadAcoountInfo()
        {
            bool maxAttemptsExceeded = false;
            bool loadAccountInfo = false;

            // the DisplayLoadAccountInfo method is overloaded so a null value is not passed
            // the if/else checks to see of there is an account ID
            if (_salesperson.AccountID != "")
            {
                loadAccountInfo = _consoleView.DisplayLoadAccountInfo(_salesperson, out maxAttemptsExceeded);
            }
            else
            {
                loadAccountInfo = _consoleView.DisplayLoadAccountInfo(out maxAttemptsExceeded);
            }

            if (loadAccountInfo && !maxAttemptsExceeded)
            {
                XmlServices xmlServices = new XmlServices(DataSettings.dataFilePathXml);

                _salesperson = xmlServices.ReadSalespersonFromDataFile();

                _consoleView.DisplayConfirmLoadAccountInfo(_salesperson);
            }
        }

        #endregion
    }
}
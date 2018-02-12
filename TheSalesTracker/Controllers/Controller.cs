using System;
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
            bool accountExist = false;

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
                        accountExist = true;
                        break;
                    case MenuOption.UpdateAccount:
                        if (accountExist)
                            UpdateAccount();
                        else
                            Error();
                        break;
                    case MenuOption.Travel:
                        if (accountExist)
                            city = Travel();
                        else
                            Error();
                        break;
                    case MenuOption.Buy:
                        if (accountExist)
                            PurchaseMenu();
                        else
                            Error();
                        break;
                    case MenuOption.Sell:
                        if (accountExist)
                            Sell();
                        else
                            Error();
                        break;
                    case MenuOption.DisplayInventory:
                        if (accountExist)
                            DisplayInventory();
                        else
                            Error();
                        break;
                    case MenuOption.DisplayCities:
                        if (accountExist)
                            DisplayCities();
                        else
                            Error();
                        break;
                    case MenuOption.DisplayAccountInfo:
                        if (accountExist)
                            DisplayAccountInfo();
                        else
                            Error();
                        break;
                    case MenuOption.SaveAccountInfo:
                        if (accountExist)
                            DisplaySaveAccountInfo();
                        else
                            Error();
                        break;
                    case MenuOption.LoadAccountInfo:
                        DisplayLoadAcoountInfo();
                        accountExist = true;
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

        /// <summary>
        /// Calls method that displays to the user that account is missing
        /// </summary>
        public void Error()
        {
            _consoleView.DisplayMissingAccountError();
        }

        /// <summary>
        /// displays menu for buying new or existing products
        /// </summary>
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

            // instantiate a new list of products
            _city.SalesInfo = new List<Product>();

            foreach (Product product in _salesperson.CurrentStock)
            {
                // add products in inventory to city salesInfo list
                _city.SalesInfo.Add(new Product(product.Type, 0, 0));

            }
            
            //
            // do not add empty strings to list for city names
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

        /// <summary>
        /// buy new products
        /// </summary>
        public void BuyNewProducts()
        {
            
            // instantiate a new product
            Product _product = new Product();
            _product.Type = _consoleView.DisplayBuyNewProducts(_salesperson);
            
            if (_product.Type != Product.ProductType.None)
            {
                // get the number of units to buy and adds them to the product
                int numberOfUnits = _consoleView.GetNumberOfUnitsToBuy();
                _product.AddProducts(numberOfUnits);
                _product.OnBackorder = false;

                // add products to the current stock list
                _salesperson.CurrentStock.Add(_product);

                // update city object
                city.SalesInfo.Add(new Product(_product.Type, numberOfUnits, 0));
            }
     
        }

        /// <summary>
        /// buy more of existing products
        /// </summary>
        private void Buy()
        {
            // get the product type the user wants to buy more of
            Product.ProductType productType = _consoleView.DisplayBuyExistingProducut(_salesperson);
            int numberOfUnits = 0;

            // foreach loop to find the existing product
            foreach (Product item in _salesperson.CurrentStock)
            {
                // if product type is found it prompts the user for number of products
                // and adds them to the product
                if (item.Type == productType && productType != Product.ProductType.None)
                {
                    numberOfUnits = _consoleView.GetNumberOfUnitsToBuy();
                    item.AddProducts(numberOfUnits);

                    // find the product in the salesInfo list and update the number of units bought
                    foreach (Product product in city.SalesInfo)
                    {
                        if (productType == product.Type)
                        {
                            product.BuyProducts(numberOfUnits);
                        }
                            
                    }

                }

            }

        }

        /// <summary>
        /// sell product
        /// </summary>
        private void Sell()
        {
            // get the product type the user wants to sell
            Product.ProductType productType = _consoleView.DisplaySellProducts(_salesperson);
            int numberOfUnits = 0;

            // foreach loop to find the existing product
            foreach (Product item in _salesperson.CurrentStock)
            {
                // if product type is found it prompts the user for number of products
                // and subtracts them from the product
                if (item.Type == productType && productType != Product.ProductType.None)
                {
                    numberOfUnits = _consoleView.GetNumberOfUnitsToSell();
                    item.SubtractProducts(numberOfUnits);

                    // if more products are sold than exist in inventory
                    // backorder notification is displayed to the user
                    if (item.OnBackorder)
                    {
                        _consoleView.DisplayBackorderNotification(item, numberOfUnits);
                    }

                    // find the product in the salesIndo list and update the number of units sold
                    foreach (Product product in city.SalesInfo)
                    {
                        if (productType == product.Type)
                        {
                            product.SellProducts(numberOfUnits);
                        }

                    }
                }
            }

        }

        /// <summary>
        /// calls display inventory
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

            // method returns a new city object
            return city;
        }

        /// <summary>
        ///  lets the user update account info
        /// </summary>
        private void UpdateAccount()
        {
            _salesperson = _consoleView.DisplayUpdateAccount(_salesperson);
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

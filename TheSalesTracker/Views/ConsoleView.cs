﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSalesTracker
{
    /// <summary>
    /// MVC View Class
    /// </summary>
    public class ConsoleView
    {
        #region Fields

        private const int MAXIMUM_ATTEMPTS = 3;
        private const int MAXIMUM_BUYSELL_AMOUNT = 1000;
        private const int MINIMUM_BUYSELL_AMOUNT = 0;

        #endregion

        #region Properties



        #endregion

        #region Constructors

        /// <summary>
        /// default constructor to create the console view objects
        /// </summary>
        public ConsoleView()
        {
            InitializeConsole();
        }

        #endregion

        #region Methods

        /// <summary>
        /// initialize all console settings
        /// </summary>
        private void InitializeConsole()
        {
            ConsoleUtil.WindowTitle = "Woodbury Productions";
            ConsoleUtil.HeaderText = "The Sales Tracker";
        }

        /// <summary>
        /// Display Account Details
        /// </summary>
        /// <param name="salesperson"></param>
        private void DisplayAccountDetail(Salesperson salesperson)
        {
            ConsoleUtil.DisplayMessage("First Name: " + salesperson.FirstName);
            ConsoleUtil.DisplayMessage("Last Name: " + salesperson.LastName);
            ConsoleUtil.DisplayMessage("Account ID: " + salesperson.AccountID);

        }

        /// <summary>
        /// display the current account information
        /// </summary>
        public void DisplayAccountInfo(Salesperson salesperson)
        {
            ConsoleUtil.HeaderText = "Account Information";
            ConsoleUtil.DisplayReset();

            DisplayAccountDetail(salesperson);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display information about the latest backorder
        /// </summary>
        /// <param name="product"></param>
        /// <param name="numberOfUnitsSold"></param>
        public void DisplayBackorderNotification(Product product, int numberOfUnitsSold)
        {
            ConsoleUtil.HeaderText = "Inventory Backorder Notification";
            ConsoleUtil.DisplayReset();

            // Math.Abs returns absolute value of numberOfUnits
            int numberOfUnitsBackordered = Math.Abs(product.NumberOfUnits);
            int numberOfUnitsShipped = numberOfUnitsSold - numberOfUnitsBackordered;

            ConsoleUtil.DisplayMessage("Products Sold: " + numberOfUnitsSold);
            ConsoleUtil.DisplayMessage("Products Shipped: " + numberOfUnitsShipped);
            ConsoleUtil.DisplayMessage("Products on Backorder: " + numberOfUnitsBackordered);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display a list of the cities traveled
        /// </summary>
        public void DisplayCitiesTraveled(Salesperson salesperson)
        {
            ConsoleUtil.HeaderText = "Travel Log";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("You have traveled to the following cities:");
            ConsoleUtil.DisplayMessage("");

            foreach (City city in salesperson.CitiesVisited)
            {
                ConsoleUtil.DisplayMessage("City: " + city.CityName.ToUpper());
                foreach (Product product in city.SalesInfo)
                {
                    ConsoleUtil.DisplayMessage("\tProduct Type: " + product.Type.ToString());
                    ConsoleUtil.DisplayMessage("\tUnits Bought: " + product.ProductsBought.ToString());
                    ConsoleUtil.DisplayMessage("\tUnits Sold: " + product.ProductsSold.ToString());
                    ConsoleUtil.DisplayMessage("");
                }
                ConsoleUtil.DisplayMessage("");
            }
            

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display a closing screen when the user quits the application
        /// </summary>
        public void DisplayClosingScreen()
        {
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Thank you for using The Sales Tracker Application.");
            ConsoleUtil.DisplayMessage("If you saved your account information, you can load it next time you use the application.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// notifies user of a successful load of the account and travel log info,
        /// and displays the loaded info
        /// </summary>
        /// <param name="salesperson"></param>
        public void DisplayConfirmLoadAccountInfo(Salesperson salesperson)
        {
            ConsoleUtil.HeaderText = "Load Account Information";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Account information loaded sucessfully.");

            DisplayAccountDetail(salesperson);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// notifies the user of a successful save of the account and travel log info
        /// </summary>
        public void DisplayConfirmSaveAccountInfo()
        {
            ConsoleUtil.HeaderText = "Save Account Information";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Account information saved successfully.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display the Continue prompt
        /// </summary>
        public void DisplayContinuePrompt()
        {
            Console.CursorVisible = false;

            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayMessage("Press any key to continue.");
            ConsoleKeyInfo response = Console.ReadKey();

            ConsoleUtil.DisplayMessage("");

            Console.CursorVisible = true;
        }

        /// <summary>
        /// get the next city to travel to from the user
        /// </summary>
        /// <returns>string City</returns>
        public string DisplayGetNextCity()
        {
            string nextCity = "";

            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayPromptMessage("Enter the name of the next city:");
            nextCity = Console.ReadLine();

            return nextCity;
        }

        /// <summary>
        /// Display buy new products and add to the salesperson
        /// </summary>
        /// <param name="salesperson"></param>
        /// <returns></returns>
        public Product.ProductType DisplayBuyNewProducts(Salesperson salesperson)
        {
            ConsoleUtil.HeaderText = "Buy New Products";
            ConsoleUtil.DisplayReset();

            DisplayAvailableProducts();

            Product.ProductType productType = GetTypeOfProduct(salesperson, "buy", out bool inStock);

            // if the product already exist in the inventory, the return will be set to none
            if (inStock)
            {
                productType = Product.ProductType.None;
                ConsoleUtil.DisplayMessage("This product already exist in your inventory.");

                DisplayContinuePrompt();
            }


            return productType;
        }

        /// <summary>
        /// Display buy existing products and add to the salesperson
        /// </summary>
        /// <param name="salesperson"></param>
        /// <returns></returns>
        public Product.ProductType DisplayBuyExistingProducut(Salesperson salesperson)
        {
            ConsoleUtil.HeaderText = "Buy Inventory";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Your current inventory:");
            ConsoleUtil.DisplayMessage("");
            CurrentInventory(salesperson);

            Product.ProductType productType = GetTypeOfProduct(salesperson, "buy", out bool inStock);

            if (!inStock)
            {
                ConsoleUtil.DisplayMessage("This product does not exist in your inventory.");

                DisplayContinuePrompt();
            }

            return productType;
        }

        /// <summary>
        /// Display sell products
        /// </summary>
        /// <param name="salesperson"></param>
        /// <returns></returns>
        public Product.ProductType DisplaySellProducts(Salesperson salesperson)
        {
            ConsoleUtil.HeaderText = "Sell Inventory";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Your current inventory:");
            ConsoleUtil.DisplayMessage("");
            CurrentInventory(salesperson);

            Product.ProductType productType = GetTypeOfProduct(salesperson, "sell", out bool inStock);

            if (!inStock)
            {
                ConsoleUtil.DisplayMessage("This product does not exist in your inventory.");

                DisplayContinuePrompt();
            }

            return productType;
        }

        /// <summary>
        /// the user is prompted to enter the number of units to buy
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public int GetNumberOfUnitsToBuy()
        {
            int numberOfUnitsToBuy = 0;

            // validate get number of units from user
            if (!ConsoleValidator.TryGetIntegerFromUser(MINIMUM_BUYSELL_AMOUNT, MAXIMUM_BUYSELL_AMOUNT, MAXIMUM_ATTEMPTS, "the product", out numberOfUnitsToBuy))
            {
                ConsoleUtil.DisplayMessage("It appears you are having difficulties setting the number of units to buy.");
                ConsoleUtil.DisplayMessage("The number of units to buy will be set to it's default value of 0.");

                numberOfUnitsToBuy = 0;

            }

            DisplayContinuePrompt();

            return numberOfUnitsToBuy;
        }

        /// <summary>
        /// the user is prompted to enter the number of units to sell
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public int GetNumberOfUnitsToSell()
        {

            // validate get number of units from user
            if (!ConsoleValidator.TryGetIntegerFromUser(MINIMUM_BUYSELL_AMOUNT, MAXIMUM_BUYSELL_AMOUNT, MAXIMUM_ATTEMPTS, "Product", out int numberOfUnitsToSell))
            {
                ConsoleUtil.DisplayMessage("It appears you are having difficulties setting the number of units to buy.");
                ConsoleUtil.DisplayMessage("The number of units to sell will be set to it's default value of 0.");
                
                numberOfUnitsToSell = 0;
                DisplayContinuePrompt();
                
            }

            DisplayContinuePrompt();

            return numberOfUnitsToSell;
        }

        /// <summary>
        /// Display purchase menu for the user to choose between buying new or existing products
        /// </summary>
        /// <returns></returns>
        public int DisplayPurchaseMenu()
        {
            int purchaseChoice;

            ConsoleUtil.HeaderText = "Buy Products";
            ConsoleUtil.DisplayReset();
            Console.CursorVisible = false;

            // display the menu
            ConsoleUtil.DisplayMessage("Please type the number of your menu choice.");
            ConsoleUtil.DisplayMessage("");
            Console.Write(
                "\t" + "1. Buy New Products" + Environment.NewLine +
                "\t" + "2. Buy Existing Products" + Environment.NewLine + 
                "\t" + "E. Main Menu" + Environment.NewLine);

            //
            // get and process the user's response
            ConsoleKeyInfo userResponse = Console.ReadKey(true);
            switch (userResponse.KeyChar)
            {
                case '1':
                    purchaseChoice = 1;
                    break;
                case '2':
                    purchaseChoice = 2;
                    break;
                case 'E':
                case 'e':
                    purchaseChoice = 0;
                    break;
                default:
                    ConsoleUtil.DisplayMessage(
                        "It appears you have selected an incorrect choice." + Environment.NewLine +
                        "Press any key to go back to the main menu.");
                    Console.ReadKey();
                    purchaseChoice = 0;
                    break;
            }

            Console.CursorVisible = true;

            return purchaseChoice;
        }

        /// <summary>
        /// get the menu choice from the user
        /// </summary>
        public MenuOption DisplayGetUserMenuChoice()
        {
            MenuOption userMenuChoice = MenuOption.None;
            bool usingMenu = true;

            while (usingMenu)
            {
                // set up display area
                ConsoleUtil.HeaderText = "The Sales Tracker";
                ConsoleUtil.DisplayReset();
                Console.CursorVisible = false;

                // display the menu
                ConsoleUtil.DisplayMessage("Please type the number of your menu choice.");
                ConsoleUtil.DisplayMessage("");
                Console.Write(
                    "\t" + "0. Setup Account" + Environment.NewLine +
                    "\t" + "1. Update Account" + Environment.NewLine +
                    "\t" + "2. Travel" + Environment.NewLine +
                    "\t" + "3. Buy" + Environment.NewLine +
                    "\t" + "4. Sell" + Environment.NewLine +
                    "\t" + "5. Display Inventory" + Environment.NewLine +
                    "\t" + "6. Display Cities" + Environment.NewLine +
                    "\t" + "7. Display Account Info" + Environment.NewLine +
                    "\t" + "8. Save Account Info" + Environment.NewLine +
                    "\t" + "9. Load Account Info" + Environment.NewLine +
                    "\t" + "E. Exit" + Environment.NewLine);

                //
                // get and process the user's response
                // note: ReadKey argument set to "true" disables the echoing of the key press
                ConsoleKeyInfo userResponse = Console.ReadKey(true);
                switch (userResponse.KeyChar)
                { 
                    case '0':
                        userMenuChoice = MenuOption.SetupAccount;
                        usingMenu = false;
                        break;
                    case '1':
                        userMenuChoice = MenuOption.UpdateAccount;
                        usingMenu = false;
                        break;
                    case '2':
                        userMenuChoice = MenuOption.Travel;
                        usingMenu = false;
                        break;
                    case '3':
                        userMenuChoice = MenuOption.Buy;
                        usingMenu = false;
                        break;
                    case '4':
                        userMenuChoice = MenuOption.Sell;
                        usingMenu = false;
                        break;
                    case '5':
                        userMenuChoice = MenuOption.DisplayInventory;
                        usingMenu = false;
                        break;
                    case '6':
                        userMenuChoice = MenuOption.DisplayCities;
                        usingMenu = false;
                        break;
                    case '7':
                        userMenuChoice = MenuOption.DisplayAccountInfo;
                        usingMenu = false;
                        break;
                    case '8':
                        userMenuChoice = MenuOption.SaveAccountInfo;
                        usingMenu = false;
                        break;
                    case '9':
                        userMenuChoice = MenuOption.LoadAccountInfo;
                        usingMenu = false;
                        break;
                    case 'E':
                    case 'e':
                        userMenuChoice = MenuOption.Exit;
                        usingMenu = false;
                        break;
                    default:
                        ConsoleUtil.DisplayMessage(
                            "It appears you have selected an incorrect choice." + Environment.NewLine +
                            "Press any key to continue.");
                        Console.ReadKey();
                        break;
                }
            }
            Console.CursorVisible = true;

            return userMenuChoice;
        }

        /// <summary>
        /// displays the current inventory to the user
        /// </summary>
        /// <param name="product"></param>
        public void DisplayInventory(Salesperson salesperson)
        {
            ConsoleUtil.HeaderText = "Current Inventory";
            ConsoleUtil.DisplayReset();

            // calls method to display inventory
            CurrentInventory(salesperson);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// loops through and displays the current inventory
        /// </summary>
        /// <param name="salesperson"></param>
        public void CurrentInventory(Salesperson salesperson)
        {
            foreach (Product item in salesperson.CurrentStock)
            {
                if (item.Type != Product.ProductType.None)
                {
                    ConsoleUtil.DisplayMessage("Product type: " + item.Type.ToString());
                    ConsoleUtil.DisplayMessage("Number of units: " + item.NumberOfUnits.ToString());
                    ConsoleUtil.DisplayMessage("");
                }
                
            }
        }

        /// <summary>
        /// prompts the user to load account and travel log information
        /// </summary>
        /// <param name="maxAttemptsExceeded"></param>
        /// <returns></returns>
        public bool DisplayLoadAccountInfo(out bool maxAttemptsExceeded)
        {
            string userResponse;
            maxAttemptsExceeded = false;

            ConsoleUtil.HeaderText = "Load Account";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("");
            userResponse = ConsoleValidator.GetYesNoFromUser(MAXIMUM_ATTEMPTS, "Load the account information?", out maxAttemptsExceeded);

            if (maxAttemptsExceeded)
            {
                ConsoleUtil.DisplayMessage("It appears you are having difficulties. You will return to the main menu.");
                return false;
            }
            else
            {
                // note use of ternary operator (takes three arguments)
                return userResponse == "YES" ? true : false;
            }
        }

        /// <summary>
        ///  prompts the user to load account and travel log information
        ///  overloaded method to avoid passing a null value
        /// </summary>
        /// <param name="salesperson"></param>
        /// <param name="maxAttemptsExceeded"></param>
        /// <returns></returns>
        public bool DisplayLoadAccountInfo(Salesperson salesperson, out bool maxAttemptsExceeded)
        {
            string userResponse;
            maxAttemptsExceeded = false;

            ConsoleUtil.HeaderText = "Load Account Information";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("");
            userResponse = ConsoleValidator.GetYesNoFromUser(MAXIMUM_ATTEMPTS, "Load the account information?", out maxAttemptsExceeded);

            if (maxAttemptsExceeded)
            {
                ConsoleUtil.DisplayMessage("It appears you are having difficulties. You will return to the main menu.");
                return false;
            }
            else
            {
                // note use of ternary operator (takes three arguments)
                return userResponse == "YES" ? true : false;
            }
        }

        /// <summary>
        /// prompts the user to save account and travel log
        /// </summary>
        /// <param name="salesperson"></param>
        /// <param name="maxAttemptsExceeded"></param>
        /// <returns></returns>
        public bool DisplaySaveAccountInfo(Salesperson salesperson, out bool maxAttemptsExceeded)
        {
            string userResponse;
            maxAttemptsExceeded = false;

            ConsoleUtil.HeaderText = "Save Account";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("The current account information.");
            DisplayAccountDetail(salesperson);

            ConsoleUtil.DisplayMessage("");
            userResponse = ConsoleValidator.GetYesNoFromUser(MAXIMUM_ATTEMPTS, "Save the account information?", out maxAttemptsExceeded);

            if (maxAttemptsExceeded)
            {
                ConsoleUtil.DisplayMessage("It appears you are having difficulties. You will return to the main menu.");
                return false;
            }
            else
            {
                // note use of ternary operator (takes three arguments)
                return userResponse == "YES" ? true : false;
            }
        }

        /// <summary>
        /// Display available product types
        /// </summary>
        public void DisplayAvailableProducts()
        {
            ConsoleUtil.DisplayReset();
            ConsoleUtil.DisplayMessage("Available Product Types: ");
            ConsoleUtil.DisplayMessage("");

            // list all types of products available
            // Enum.GetNames retrieves an array of the names in ProductType
            foreach (string productName in Enum.GetNames(typeof(Product.ProductType)))
            {
                // do not display "None" product type
                if (productName != "None")
                {
                    ConsoleUtil.DisplayMessage(productName);
                }
            }
        }

        /// <summary>
        /// Get type of product from user. Checks if product already exist in invetory
        /// </summary>
        /// <param name="salesperson"></param>
        /// <param name="notInStock"></param>
        /// <returns></returns>
        public Product.ProductType GetTypeOfProduct(Salesperson salesperson, string buyOrSell, out bool inStock)
        {
            Product.ProductType productType;
            inStock = false;

            ConsoleUtil.DisplayPromptMessage("Enter the type of product you wish to " + buyOrSell + ":");

            // new variable for product type
            if (Enum.TryParse<Product.ProductType>(UppercaseFirst(Console.ReadLine()), out productType))
            { 
                foreach (Product product in salesperson.CurrentStock)
                {
                    if (product.Type == productType)
                    {
                        inStock = true;
                    }
                }
            }
            else
            {
                // sets type of current stock to "none" product type
                ConsoleUtil.DisplayMessage("It appears you are having difficulty selecting a product.");
                ConsoleUtil.DisplayMessage("By default, no items will be added.");

                productType = Product.ProductType.None;

                DisplayContinuePrompt();
            }

            return productType;

        }

        /// <summary>
        /// This methods prompts the user for products and adds them to the list
        /// </summary>
        /// <param name="salesperson"></param>
        /// <returns></returns>
        public List<Product> DisplayGetProducts(Salesperson salesperson)
        {
            // initialize new list of products
            salesperson.CurrentStock = new List<Product>();

            bool keepAdding = true;
            string userResponse;
            bool maxAttemptsExceeded = false;
            Product.ProductType productType;

            while (keepAdding)
            {
                productType = GetTypeOfProduct(salesperson, "add",  out bool notInStock);

                // checks to see if product already exist in inventory or is set to type none
                if (!notInStock && productType != Product.ProductType.None)
                {
                    // get number of products
                    if (ConsoleValidator.TryGetIntegerFromUser(MINIMUM_BUYSELL_AMOUNT, MAXIMUM_BUYSELL_AMOUNT, MAXIMUM_ATTEMPTS, productType.ToString(), out int numberOfUnits))
                    {

                        // add product to current stock list
                        salesperson.CurrentStock.Add(new Product(productType, numberOfUnits, false));

                        userResponse = ConsoleValidator.GetYesNoFromUser(MAXIMUM_ATTEMPTS, "Do you wish to continue adding products?", out maxAttemptsExceeded);
                        ConsoleUtil.DisplayReset();

                        if (maxAttemptsExceeded || userResponse == "NO")
                        {
                            // exit the while loop
                            keepAdding = false;
                        }
                        else
                        {
                            //display available products to user
                            DisplayAvailableProducts();
                            ConsoleUtil.DisplayMessage("");
                        }

                    }
                    else
                    {
                        ConsoleUtil.DisplayMessage("It appears you are having difficulty setting the number of products in your stock.");
                        ConsoleUtil.DisplayMessage("By default, the number of products in your inventory are now set to 0.");

                        // add product to current stock list with default number of units set to 0
                        salesperson.CurrentStock.Add(new Product(productType, 0, false));

                        userResponse = ConsoleValidator.GetYesNoFromUser(MAXIMUM_ATTEMPTS, "Do you wish to continue adding products?", out maxAttemptsExceeded);
                        ConsoleUtil.DisplayReset();

                        if (maxAttemptsExceeded || userResponse == "NO")
                        {
                            // exit the while loop
                            keepAdding = false;

                        }
                        else
                        {
                            //display available products to user
                            DisplayAvailableProducts();
                            ConsoleUtil.DisplayMessage("");
                        }
                    }
                }
                else
                {
                    ConsoleUtil.DisplayMessage(productType.ToString() + " already exist in your inventory.");

                    userResponse = ConsoleValidator.GetYesNoFromUser(MAXIMUM_ATTEMPTS, "Do you wish to continue adding products?", out maxAttemptsExceeded);
                    ConsoleUtil.DisplayReset();

                    if (maxAttemptsExceeded || userResponse == "NO")
                    {
                        // exit the while loop
                        keepAdding = false;

                    }
                    else
                    {
                        //display available products to user
                        DisplayAvailableProducts();
                        ConsoleUtil.DisplayMessage("");
                    }
                }

                if (productType == Product.ProductType.None)
                {
                    salesperson.CurrentStock.Add(new Product(Product.ProductType.None, 0, false));

                    // exit while loop
                    keepAdding = false;

                }
                
            }

            return salesperson.CurrentStock;
        }

        /// <summary>
        /// method displays menu system that allow the user to update account info of their choosing
        /// </summary>
        /// <param name="salesperson"></param>
        /// <returns></returns>
        public Salesperson DisplayUpdateAccount(Salesperson salesperson)
        {
            bool usingMenu = true;

            ConsoleUtil.HeaderText = "Update Account";
            ConsoleUtil.DisplayReset();

            while (usingMenu)
            {
                // display account details to user
                DisplayAccountDetail(salesperson);

                ConsoleUtil.DisplayMessage("");
                ConsoleUtil.DisplayMessage("Type the number for which info you wish to update: ");
                ConsoleUtil.DisplayMessage("");
                Console.Write(
                    "\t" + "1. First Name" + Environment.NewLine +
                    "\t" + "2. Last Name" + Environment.NewLine +
                    "\t" + "3. Account ID" + Environment.NewLine +
                    "\t" + "E. Main Menu" + Environment.NewLine);

                //
                // get and process the user's response
                ConsoleKeyInfo userResponse = Console.ReadKey(true);
                switch (userResponse.KeyChar)
                {
                    case '1':
                        ConsoleUtil.DisplayReset();
                        ConsoleUtil.DisplayMessage("");
                        ConsoleUtil.DisplayPromptMessage("Update your first name:");
                        salesperson.FirstName = Console.ReadLine();
                        ConsoleUtil.DisplayReset();
                        break;
                    case '2':
                        ConsoleUtil.DisplayReset();
                        ConsoleUtil.DisplayMessage("");
                        ConsoleUtil.DisplayPromptMessage("Update your last name:");
                        salesperson.LastName = Console.ReadLine();
                        ConsoleUtil.DisplayReset();
                        break;
                    case '3':
                        ConsoleUtil.DisplayReset();
                        ConsoleUtil.DisplayMessage("");
                        ConsoleUtil.DisplayPromptMessage("Update your account ID name:");
                        salesperson.AccountID = Console.ReadLine();
                        ConsoleUtil.DisplayReset();
                        break;
                    case 'E':
                    case 'e':
                        usingMenu = false;
                        break;
                    default:
                        ConsoleUtil.DisplayMessage(
                            "It appears you have selected an incorrect choice." + Environment.NewLine +
                            "Press any key to return to continue");
                        userResponse = Console.ReadKey(true);
                        break;
                }
            }

            DisplayContinuePrompt();

            return salesperson;
        }

        /// <summary>
        /// setup the new salesperson object with the initial data
        /// </summary>
        public Salesperson DisplaySetupAccount(out City city)
        {
           // instantiate a new salesperson object
            Salesperson salesperson = new Salesperson();

            // instantiayte a new city object 
            city = new City();
            city.SalesInfo = new List<Product>();

            ConsoleUtil.HeaderText = "Account Setup";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Setup your account now.");
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayPromptMessage("Enter your first name: ");
            salesperson.FirstName = Console.ReadLine();
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayPromptMessage("Enter your last name: ");
            salesperson.LastName = Console.ReadLine();
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayPromptMessage("Enter your account ID: ");
            salesperson.AccountID = Console.ReadLine();
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayPromptMessage("Enter your starting city: ");
            city.CityName = Console.ReadLine();

            // get products from user
            DisplayAvailableProducts();
            ConsoleUtil.DisplayMessage("");

            salesperson.CurrentStock = DisplayGetProducts(salesperson);


            foreach (Product item in salesperson.CurrentStock)
	        {
                city.SalesInfo.Add(new Product(item.Type, item.NumberOfUnits, 0));
	        }

            // add city object to salesperson
            salesperson.CitiesVisited.Add(city);

            ConsoleUtil.DisplayMessage("Your account is setup");
            ConsoleUtil.DisplayMessage("");

            // display account details to user
            DisplayAccountDetail(salesperson);
            ConsoleUtil.DisplayMessage("Starting city: " + city.CityName);
            ConsoleUtil.DisplayMessage("Your current inventory:");
            ConsoleUtil.DisplayMessage("");
            CurrentInventory(salesperson);

            DisplayContinuePrompt();

            return salesperson;
        }

        /// <summary>
        /// display the welcome screen
        /// </summary>
        public void DisplayWelcomeScreen()
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.DisplayReset();
            Console.CursorVisible = false;

            ConsoleUtil.DisplayMessage("Written by Madeleine Woodbury");
            ConsoleUtil.DisplayMessage("Northwestern Michigan College");
            ConsoleUtil.DisplayMessage("");

            sb.Clear();
            sb.AppendFormat("Welcome to The Sales Tracker Application.");
            sb.AppendFormat("This application will help you, as a traveling salesperson, ");
            sb.AppendFormat("to keep track of your travels along with your sales and purchases. ");
            sb.AppendFormat("If you haven't already, use the menu to setup your Account information.");
            ConsoleUtil.DisplayMessage(sb.ToString());
            ConsoleUtil.DisplayMessage("");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// changes string to lowercase with first letter uppercase
        /// adapted from: https://www.dotnetperls.com/uppercase-first-letter
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concatenation substring.
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }

        /// <summary>
        /// display account error if there are no account setup or loadded
        /// </summary>
        public void DisplayMissingAccountError()
        {
            ConsoleUtil.HeaderText = "ERROR";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("No Account Info Found.");
            ConsoleUtil.DisplayMessage("Please return to the menu to setup or load your Account.");

            DisplayContinuePrompt();
        }

        #endregion
    }
}

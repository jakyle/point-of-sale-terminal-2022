namespace Point_of_sale_terminal
{
    public class PointOfSaleApp
    {
        private readonly IUI _ui;
        private readonly IProductManager _productManager;
        private ShoppingState _state = ShoppingState.MenuSelect;
        private readonly IEnumerable<string> _menu =  new List<string>
        {
            "Show Products",
            "Show Shopping Cart",
            "Add to shopping cart",
            "Remove from shopping cart",
            "Edit item in shopping cart",
            "Checkout"
        };

        public PointOfSaleApp(IUI ui, IProductManager productManager)
        {
            _ui = ui;
            _productManager = productManager;
        }

        public void Run()
        {
            bool isShopping = true;
            _ui.DisplayWelcomeMessage();
            while (isShopping)
            {
                _state = HandleState();

                if (_state == ShoppingState.Finish)
                {
                    var keepShopping = _ui.GetValidYesNoInput("Would you like to coninue shopping? please enter [yes] or [no]...");

                    if (!keepShopping)
                    {
                        isShopping = false;
                    }

                    _productManager.EmptyShoppingCart();
                    _state = ShoppingState.MenuSelect;
                }
                _ui.Reset();
            }
            _ui.PrintAndReadKey("Thank you for shopping with us, press any key to close app...");
        }

        private ShoppingState HandleState()
        {
            return _state switch
            {
                ShoppingState.MenuSelect            =>  MenuSelect(),
                ShoppingState.ShowProducts          =>  ShowProducts(),
                ShoppingState.ShowShoppingCart      =>  ShowShoppingCart(),
                ShoppingState.AddProduct            =>  AddProduct(),
                ShoppingState.RemoveProduct         =>  RemoveProduct(),
                ShoppingState.EditProductQuantity   =>  EditProductQuantity(),
                ShoppingState.Checkout              =>  Checkout(),
            };
        }

        private ShoppingState ShowProducts()
        {
            ShowProductList();
            _ui.PrintAndReadKey();
            return ShoppingState.MenuSelect;
        }

        private ShoppingState ShowShoppingCart()
        {
            if (_productManager.ShoppingCartItems.Any())
            {
                ShowShoppingCartList();
                _ui.PrintAndReadKey();
            }
            else
            {
                _ui.PrintAndReadKey("No items in shopping cart, press any key to return to main menu...");
            }
            
            return ShoppingState.MenuSelect;
        }

        private ShoppingState MenuSelect()
        {
            var menuNumber = _ui.GetValidUserNumberInput(
                "Please select what you want to do based on the corresponding numbers",
                DisplayMenuSelect,
                _menu.Count());


            return menuNumber switch
            {
                1 => ShoppingState.ShowProducts,
                2 => ShoppingState.ShowShoppingCart,
                3 => ShoppingState.AddProduct,
                4 => ShoppingState.RemoveProduct,
                5 => ShoppingState.EditProductQuantity,
                6 => ShoppingState.Checkout
            };
        }

        private ShoppingState AddProduct()
        {
            var productNumber = _ui.GetValidUserNumberInput(
                "Please select a valid product based on the product number...",
                ShowProductList,
                _productManager.Products.Count());

            var product = _productManager.Products[productNumber - 1];
            _ui.Reset();
            var quantity = _ui.GetValidUserNumberInput($"How many {product.Name}s would you like to purchase?\n\rPlease enter a number between 1 - 99.", 99);

            var existingProduct = _productManager
                .ShoppingCartItems
                .FirstOrDefault(shoppingCartItem => shoppingCartItem.Product.Name == product.Name);

            if (existingProduct == null)
            {
                var shoppingCartItem = new ShoppingCartItem(product, quantity);
                _productManager.AddShoppingCartItem(shoppingCartItem);
            }
            else
            {
                existingProduct.Quantity += quantity;
            }

            _ui.Reset();
            ShowShoppingCartList();
            _ui.PrintAndReadKey();

            return ShoppingState.MenuSelect;
        }

        private ShoppingState RemoveProduct()
        {
            var userSelectedProductNumber = _ui.GetValidUserNumberInput(
                "Please select a valid product based on the product number you want to REMOVE...",
                ShowShoppingCartList,
                _productManager.ShoppingCartItems.Count());

            var shoppingCartItemToBeRemoved = _productManager.ShoppingCartItems[userSelectedProductNumber - 1];
            _productManager.RemoveShoppingCartItem(shoppingCartItemToBeRemoved);
            _ui.PrintAndReadKey($"{shoppingCartItemToBeRemoved.Product.Name} has been removed, press any key to return to main menu...");
            return ShoppingState.MenuSelect;
        }

        private ShoppingState EditProductQuantity()
        {
            var userSelectedProductNumber = _ui.GetValidUserNumberInput(
                "Please select a valid product based on the product number quantity want to EDIT...",
                ShowShoppingCartList,
                _productManager.ShoppingCartItems.Count());

            var shoppingCartItem = _productManager.ShoppingCartItems[userSelectedProductNumber - 1];
            _ui.Reset();
            var quantity = _ui.GetValidUserNumberInput($"How many {shoppingCartItem.Product.Name}s would you like to purchase?\n\rPlease enter a number between 1 - 99.", 99);
            shoppingCartItem.Quantity = quantity;

            _ui.PrintAndReadKey($"{shoppingCartItem.Product.Name} has been updated to {shoppingCartItem.Quantity}, press any key to return to main menu...");
            return ShoppingState.MenuSelect;
        }

        private ShoppingState Checkout()
        {
            ShowShoppingCartList();
            _ui.DisplayMessage($"Sub Total: {_productManager.SubTotal,2}\tGrand Total: {_productManager.GrandTotal,2}");
            _ui.PrintAndReadKey("Press any key to input payment method...");
            _ui.Reset();

            var paymentTypeSelectionNumber = _ui.GetValidUserNumberInput(
                "Please select a valid payment type based on the payment number you want to use...",
                ShowPaymentTypeList,
                3);

            switch (paymentTypeSelectionNumber) 
            {
                case 1: 
                    HandleCash();
                    break;
                case 2:
                    HandleCheck();
                    break;
                default:
                    HandleCreditCard();
                    break;
            }

            return ShoppingState.Finish;
        }

        private void ShowShoppingCartList()
        {
            int i = 1;
            foreach (var shoppingCartItem in _productManager.ShoppingCartItems)
            {
                _ui.PrintLineItem(i, shoppingCartItem.Quantity, shoppingCartItem.Product.Name, shoppingCartItem.Total);
                i++;
            }
        }

        private void ShowPaymentTypeList()
        {
            var paymentTypes = new List<string>()
            {
                "Cash",
                "Check",
                "Credit"
            };

            int i = 1;
            foreach (var paymentType in paymentTypes)
            {
                _ui.PrintLineItem(i, paymentType);
                i++;
            }
        }

        private void ShowProductList()
        {
            int i = 1;
            foreach (var product in _productManager.Products)
            {
                _ui.PrintLineItem(i, product.Name);
                i++;
            }
        }

        private void DisplayMenuSelect()
        {
            int i = 1;
            foreach (var item in _menu)
            {
                _ui.PrintLineItem(i, item);
                i++;
            }
        }

        private void HandleCash()
        {

            var totalCash = _ui.GetValidUserNumberInput(
                $"How much money are you giving me?\nGrand total is: {_productManager.GrandTotal}, I cannot break more than $999.99...",
                _productManager.GrandTotal,
                1000M);

            _ui.PrintAndReadKey($"Your change is {totalCash - _productManager.GrandTotal,2}, press any key to return to main menu...");
        }

        private void HandleCheck()
        {
            var checkNumber = _ui.GetValidUserNumberInput(
               $"enter a valid check number (3 or 4 digits required)",
               100,
               9999);

            _ui.PrintAndReadKey($"Your grand toal  is {_productManager.GrandTotal,2}, press any key to return to main menu...");

        }

        private void HandleCreditCard()
        {
            var creditCardNumber = _ui.GetValidUserNumberInput(
                $"enter a 16 digit credit card number, no dashes",
                1_000_000_000_000_000,
                9_999_999_999_999_999);

            var expirationDate = _ui.GetValidUserDateTimeInput("Enter a valid expiration date that is greater than this month and year");

            var cvv = _ui.GetValidUserNumberInput(
                $"enter a valid CVV number, a either 3 or 4 digits",
                100,
                9999);

            _ui.PrintAndReadKey($"credit card number: {creditCardNumber}\n\rexpiration date: {expirationDate}\n\rcvv: {cvv}\n\rYour grand toal is {_productManager.GrandTotal,2}\n\rpress any key to return to main menu...");
        }
    }
}

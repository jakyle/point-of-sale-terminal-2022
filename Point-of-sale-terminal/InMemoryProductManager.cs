using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Point_of_sale_terminal
{
    public class InMemoryProductManager : IProductManager
    {
        // readonly is a modifier that allows fields to be READ only, which means
        // you CANNOT set the data after the contructor...
        private readonly List<Product> _products;
        private List<ShoppingCartItem> _shoppingCartItems;
        
        public InMemoryProductManager()
        {
            _products = GetInitialProducts();
            _shoppingCartItems = new List<ShoppingCartItem>();
        }

        // this is a proprety with a Getter only!
        // this is the same as... 
        //public IEnumerable<Product> Products { get { return _products; } }
        public IReadOnlyList<Product> Products => _products;
        public IReadOnlyList<ShoppingCartItem> ShoppingCartItems => _shoppingCartItems;

        public decimal SubTotal => Math.Round(_shoppingCartItems.Sum(x => x.Total), 2);
        public decimal GrandTotal => Math.Round(SubTotal * 1.06M, 2);

        public void AddShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            _shoppingCartItems.Add(shoppingCartItem);
        }

        private static List<Product> GetInitialProducts()
        {
            var products = new List<Product>();

            products.Add(new Product("Burger", "A yummy burger boy", ProductType.Entre, 5.99M));
            products.Add(new Product("Chicken Sandwhich", "for those not interested in beef", ProductType.Entre, 4.99M));
            products.Add(new Product("Fish Taco", "seafood wrap if you know what I mean", ProductType.Entre, 6.99M));
            products.Add(new Product("Salad", "stay healthy!", ProductType.Entre, 4.99M));

            products.Add(new Product("Coke", "Fizzy pop", ProductType.Beverage, 1.99M));
            products.Add(new Product("Coffee", "Stay awake!", ProductType.Beverage, 1.50M));
            products.Add(new Product("Milk Shake", "Brings all the... customers to the store?", ProductType.Beverage, 2.99M));
            products.Add(new Product("Tea", "When you are just defaulting to a drink...", ProductType.Beverage, 1.99M));

            products.Add(new Product("Fries", "Can't go wrong with this!", ProductType.Side, 0.99M));
            products.Add(new Product("Onion Rings", "straight upgrade to fries", ProductType.Side, 1.99M));
            products.Add(new Product("Tots", "Fries but like... more geometry", ProductType.Side, 0.99M));
            products.Add(new Product("Mozerella sticks", "Mmmmmm", ProductType.Side, 2.99M));

            return products;
        }

        public void RemoveShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            _shoppingCartItems.Remove(shoppingCartItem);
        }

        public void EmptyShoppingCart()
        {
            _shoppingCartItems = new List<ShoppingCartItem>();
        }
    }

    public interface IProductManager
    {
        // you do NOT need to use a access modifier or the abstract keyword, becase
        // interface members are all abstract and public by default and always
        IReadOnlyList<Product> Products { get; }
        IReadOnlyList<ShoppingCartItem> ShoppingCartItems { get; }
        decimal SubTotal { get; }
        decimal GrandTotal { get; }
        void AddShoppingCartItem(ShoppingCartItem shoppingCartItem);
        void RemoveShoppingCartItem(ShoppingCartItem shoppingCartItem);
        void EmptyShoppingCart();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Point_of_sale_terminal
{
    // this is a model, it exist soley to represent data... nothing else
    public class Product
    {
        public Product(string name, string description, ProductType category, decimal price)
        {
            Name = name;
            Description = description;
            Category = category;
            Price = price;
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public ProductType Category { get; set; }
        public decimal Price { get; set; }
    }
}

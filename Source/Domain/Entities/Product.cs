using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product
    {
        public Product()
        {

        }

        public Product(string title, float price, IEnumerable<string> sizes, string description)
        {
            Title = title;
            Price = price;
            Sizes = sizes;
            Description = description;
        }

        public string Title { get; set; }
        public float Price { get; set; }
        public IEnumerable<string> Sizes { get; set; }
        public string Description { get; set; }
    }
}
using Business.Models;
using Domain.Entities;
using System.Collections.Generic;

namespace Business.Queries.GetProducts
{
    public class GetProductsResponse
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public ProductFilter Filter { get; set; } = new ProductFilter();
    }
}
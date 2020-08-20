using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Tests.Infrastructure
{
    public static class ProductsDbMock
    {
        public static IEnumerable<Product> GetData()
        {
            return new List<Product>()
            {
                new Product("A Red Trouser", 10, new [] { "small", "medium", "large" }, "These are common five words."),
                new Product("A Blue Trouser", 15, new [] { "small", "medium" }, "These are common five words."),
                new Product("A Green Trouser", 20, new [] { "small", "large" }, "These are common five words."),
                new Product("A Yellow Trouser", 25, new [] { "medium", "large" }, "These are common five words."),
                new Product("A Purple Trouser", 30, new [] { "small", "medium", "large" }, "These are common five words."),
                new Product("A Purple Trouser", 30, new [] { "small", "medium", "large" }, "These are common five words."),
                new Product("A Test Trouser", 12, new [] { "small", "medium", "large" }, "This trousers look cool, this sentence has length of ten."),

            };
        }

        public static IEnumerable<Product> ThrowException()
        {
            throw new Exception("Database source threw exception");
        }

        public static IEnumerable<Product> GetEmptyData()
        {
            return new List<Product>();
        }

        public static IEnumerable<Product> GetNullData()
        {
            return null;
        }
    }
}

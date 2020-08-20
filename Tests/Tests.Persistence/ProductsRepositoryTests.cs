using Domain.Entities;
using Library.Communication.Interfaces;
using LinqKit;
using Moq;
using Persistence;
using Persistence.CommunicationModels;
using Persistence.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tests.Infrastructure;
using Xunit;

namespace Tests.Persistence
{
    public class ProductsRepositoryTests
    {
        [Fact]
        public async Task GetAll_Gets()
        {
            var response = new MockitoProducts()
            {
                Products = ProductsDbMock.GetData()
            };

            var communicationMock = new Mock<ICommunicationService>();
            communicationMock
                .Setup(x => x.GetAsync<MockitoProducts>("endpoint", It.IsAny<object>(), CancellationToken.None))
                .ReturnsAsync(response);

            var repository = new ProductRepository(new DatabaseSettings() { ProductsEndpoint = "endpoint" }, communicationMock.Object);

            var result = (await repository.GetAll(null, CancellationToken.None)).ToArray();

            Assert.Equal(7, result.Length);
            Assert.Equal(response.Products.ToArray()[0].Title, result[0].Title);
        }


        [Fact]
        public async Task GetAll_Filter_ReturnsFilteredSubset()
        {
            var response = new MockitoProducts()
            {
                Products = ProductsDbMock.GetData()
            };

            var communicationMock = new Mock<ICommunicationService>();
            communicationMock
                .Setup(x => x.GetAsync<MockitoProducts>("endpoint", It.IsAny<object>(), CancellationToken.None))
                .ReturnsAsync(response);

            var filter = PredicateBuilder.New<Product>();
            filter.DefaultExpression = product => product.Title == "A Red Trouser";

            var repository = new ProductRepository(new DatabaseSettings() { ProductsEndpoint = "endpoint" }, communicationMock.Object);

            var result = (await repository.GetAll(filter, CancellationToken.None)).ToArray();

            Assert.Single(result);
            Assert.Equal("A Red Trouser", result[0].Title);
        }

        [Fact]
        public async Task GetAll_NullData_ReturnsEmptyCollectionAndIgnoresFilter()
        {
            var response = new MockitoProducts()
            {
                Products = ProductsDbMock.GetNullData()
            };

            var communicationMock = new Mock<ICommunicationService>();
            communicationMock
                .Setup(x => x.GetAsync<MockitoProducts>("endpoint", It.IsAny<object>(), CancellationToken.None))
                .ReturnsAsync(response);

            var filter = PredicateBuilder.New<Product>();
            filter.DefaultExpression = product => product.Title == "A Red Trouser";

            var repository = new ProductRepository(new DatabaseSettings() { ProductsEndpoint = "endpoint" }, communicationMock.Object);

            var result = (await repository.GetAll(filter, CancellationToken.None)).ToArray();

            Assert.Empty(result);
        }


        [Fact]
        public async Task GetAll_EmptyData_ReturnsAndIgnoresFilter()
        {
            var response = new MockitoProducts()
            {
                Products = ProductsDbMock.GetEmptyData()
            };

            var communicationMock = new Mock<ICommunicationService>();
            communicationMock
                .Setup(x => x.GetAsync<MockitoProducts>("endpoint", It.IsAny<object>(), CancellationToken.None))
                .ReturnsAsync(response);

            var filter = PredicateBuilder.New<Product>();
            filter.DefaultExpression = product => product.Title == "A Red Trouser";

            var repository = new ProductRepository(new DatabaseSettings() { ProductsEndpoint = "endpoint" }, communicationMock.Object);

            var result = (await repository.GetAll(filter, CancellationToken.None)).ToArray();

            Assert.Empty(result);
        }

        private IEnumerable<Product> GetProducts()
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
    }
}

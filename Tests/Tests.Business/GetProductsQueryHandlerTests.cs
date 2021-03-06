using Business.Interfaces;
using Business.Queries.GetProducts;
using Business.Services;
using Common.Exceptions;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Tests.Infrastructure;
using Xunit;

namespace Tests.Business
{
    public class GetProductsQueryHandlerTests
    {

        [Fact]
        public async Task Handle_NoFilterParameters_ReturnsAllProducts()
        {
            var repositoryMock = new Mock<IRepository<Product>>();
            repositoryMock.Setup(x => x.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), CancellationToken.None)).ReturnsAsync(GetProducts());

            var emphasizeMock = new Mock<IEmphasizeService>();
            emphasizeMock.Setup(x => x.Emphasize(It.IsAny<string>(), It.IsAny<HashSet<string>>())).Returns("Emphasized");
            emphasizeMock.Setup(x => x.GetSeparators()).Returns(new[] { ' ', '?', '.', '!', ',' });

            var handler = new GetProductsQueryHandler(repositoryMock.Object, null, emphasizeMock.Object);

            var request = new GetProductsQuery(null, null, null);
            var result = await handler.Handle(request);

            var expected = GetProducts();
            Assert.Equal(expected.Count(), result.Products.Count());
        }

        [Fact]
        public async Task Handle_Throws_LogsAndRethrows()
        {
            var repositoryMock = new Mock<IRepository<Product>>();
            repositoryMock.Setup(x => x.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), CancellationToken.None)).Throws<Exception>();

            var loggerMock = new Mock<ILogger<GetProductsQueryHandler>>();

            var handler = new GetProductsQueryHandler(repositoryMock.Object, loggerMock.Object, null);

            var request = new GetProductsQuery(null, null, null);
            await Assert.ThrowsAsync<FetchException>(() => handler.Handle(request));

            loggerMock.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }

        [Fact]
        public async Task Handle_NoEntitiesFound_ReturnEmptyResponse()
        {
            var repositoryMock = new Mock<IRepository<Product>>();
            repositoryMock.Setup(x => x.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), CancellationToken.None)).ReturnsAsync(new List<Product>());

            var handler = new GetProductsQueryHandler(repositoryMock.Object, null, null);

            var request = new GetProductsQuery(null, null, null);
            var result = await handler.Handle(request);

            Assert.Empty(result.Products);
            Assert.Empty(result.Filter.CommonWords);
            Assert.Empty(result.Filter.Sizes);
            Assert.Equal(default, result.Filter.MaximumPrice);
            Assert.Equal(default, result.Filter.MinimumPrice);
        }

        [Fact]
        public async Task Handle_MinimumPriceFound()
        {
            var repositoryMock = new Mock<IRepository<Product>>();
            repositoryMock.Setup(x => x.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), CancellationToken.None)).ReturnsAsync(GetProducts());

            var emphasizeMock = new Mock<IEmphasizeService>();
            emphasizeMock.Setup(x => x.Emphasize(It.IsAny<string>(), It.IsAny<HashSet<string>>())).Returns("Emphasized");
            emphasizeMock.Setup(x => x.GetSeparators()).Returns(new[] { ' ', '?', '.', '!', ',' });

            var handler = new GetProductsQueryHandler(repositoryMock.Object, null, emphasizeMock.Object);

            var request = new GetProductsQuery(null, null, null);
            var result = await handler.Handle(request);

            Assert.Equal(10, result.Filter.MinimumPrice);
        }

        [Fact]
        public async Task Handle_MaxmimumPriceFound()
        {
            var repositoryMock = new Mock<IRepository<Product>>();
            repositoryMock.Setup(x => x.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), CancellationToken.None)).ReturnsAsync(GetProducts());

            var emphasizeMock = new Mock<IEmphasizeService>();
            emphasizeMock.Setup(x => x.Emphasize(It.IsAny<string>(), It.IsAny<HashSet<string>>())).Returns("Emphasized");
            emphasizeMock.Setup(x => x.GetSeparators()).Returns(new[] { ' ', '?', '.', '!', ',' });

            var handler = new GetProductsQueryHandler(repositoryMock.Object, null, emphasizeMock.Object);

            var request = new GetProductsQuery(null, null, null);
            var result = await handler.Handle(request);

            Assert.Equal(30, result.Filter.MaximumPrice);
        }

        [Fact]
        public async Task Handle_MostCommonWords()
        {
            var repositoryMock = new Mock<IRepository<Product>>();
            repositoryMock.Setup(x => x.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), CancellationToken.None)).ReturnsAsync(GetProducts());

            var emphasizeMock = new Mock<IEmphasizeService>();
            emphasizeMock.Setup(x => x.Emphasize(It.IsAny<string>(), It.IsAny<HashSet<string>>())).Returns("Emphasized");
            emphasizeMock.Setup(x => x.GetSeparators()).Returns(new[] { ' ', '?', '.', '!', ',' });

            var handler = new GetProductsQueryHandler(repositoryMock.Object, null, emphasizeMock.Object);

            var request = new GetProductsQuery(null, null, null);
            var result = await handler.Handle(request);

            Assert.NotEmpty(result.Filter.CommonWords);
            Assert.Equal(10, result.Filter.CommonWords.Count());
            Assert.DoesNotContain("These", result.Filter.CommonWords);
            Assert.DoesNotContain("are", result.Filter.CommonWords);
            Assert.DoesNotContain("most", result.Filter.CommonWords);
            Assert.DoesNotContain("common", result.Filter.CommonWords);
            Assert.DoesNotContain("five", result.Filter.CommonWords);
            Assert.DoesNotContain("words", result.Filter.CommonWords);

            Assert.Contains("This", result.Filter.CommonWords);
            Assert.Contains("trousers", result.Filter.CommonWords);
            Assert.Contains("look", result.Filter.CommonWords);
            Assert.Contains("cool", result.Filter.CommonWords);
            Assert.Contains("this", result.Filter.CommonWords);
            Assert.Contains("sentence", result.Filter.CommonWords);
            Assert.Contains("has", result.Filter.CommonWords);
            Assert.Contains("length", result.Filter.CommonWords);
            Assert.Contains("of", result.Filter.CommonWords);
            Assert.Contains("ten", result.Filter.CommonWords);
        }

        [Fact]
        public async Task Handle_TextEmphasized()
        {
            var repositoryMock = new Mock<IRepository<Product>>();
            repositoryMock.Setup(x => x.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), CancellationToken.None)).ReturnsAsync(GetProducts());

            var handler = new GetProductsQueryHandler(repositoryMock.Object, null, new EmphasizeService());

            var request = new GetProductsQuery(null, null, "trousers, cool");
            var result = await handler.Handle(request);

            Assert.Contains("<em>", result.Products.Last().Description);

            var start = 0;
            var end = result.Products.Count() - 2;
            foreach (var product in result.Products.ToArray()[start..end])
            {
                Assert.DoesNotContain("<em>", product.Description);
            }

        }

        [Fact]
        public async Task GetProductFilterExpression_BuildsPredicate()
        {
            var handler = new GetProductsQueryHandler(null, null, null);

            var predicate1 = handler.GetProductFilterExpression(null, 15);
            var predicate2 = handler.GetProductFilterExpression("small", null);
            var predicate3 = handler.GetProductFilterExpression("small", 15);

            var func1 = predicate1.Compile();
            var func2 = predicate2.Compile();
            var func3 = predicate3.Compile();

            var products = GetProducts().ToArray();

            Assert.True(func1(products[0]));
            Assert.False(func1(products[2]));

            Assert.True(func2(products[0]));
            Assert.False(func2(products[3]));

            Assert.True(func3(products[0]));
            Assert.False(func3(products[3]));
        }

        [Fact]
        public void GetProductFilterExpression_NoFilters_ReturnsNull()
        {
            var handler = new GetProductsQueryHandler(null, null, null);

            var result = handler.GetProductFilterExpression(null, null);

            Assert.Null(result);
        }

        private IEnumerable<Product> GetProducts()
        {
            return ProductsDbMock.GetData();
        }

    }
}

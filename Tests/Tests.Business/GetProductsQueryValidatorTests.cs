using Business.Queries.GetProducts;
using Xunit;

namespace Tests.Business
{
    public class GetProductsQueryValidatorTests
    {
        [Fact]
        public void Validate_ValidRequest()
        {
            var validator = new GetProductsQueryValidator();

            var query1 = new GetProductsQuery(null, null, null);
            var result1 = validator.Validate(query1);
            Assert.True(result1.IsValid);

            var query2 = new GetProductsQuery(2, "small", "test, test2");
            var result2 = validator.Validate(query2);
            Assert.True(result2.IsValid);
        }

        [Fact]
        public void Validate_NegativePrice_Invalid()
        {
            var validator = new GetProductsQueryValidator();

            var query = new GetProductsQuery(-1, "small", "test, test2");
            var result = validator.Validate(query);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_PriceIsZero_Valid()
        {
            var validator = new GetProductsQueryValidator();

            var query = new GetProductsQuery(0, "small", "test, test2");
            var result = validator.Validate(query);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_SizeIsEmptyString_Invalid()
        {
            var validator = new GetProductsQueryValidator();

            var query = new GetProductsQuery(0, " ", "test, test2");
            var result = validator.Validate(query);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_KeywordsContainEmptyString_Invalid()
        {
            var validator = new GetProductsQueryValidator();

            var query1 = new GetProductsQuery(0, "small", " , test2");
            var result1 = validator.Validate(query1);
            Assert.False(result1.IsValid);

            var query2 = new GetProductsQuery(0, "small", "");
            var result2 = validator.Validate(query2);
            Assert.False(result2.IsValid);

            var query3 = new GetProductsQuery(0, "small", ", test2");
            var result3 = validator.Validate(query3);
            Assert.False(result3.IsValid);
        }

    }
}

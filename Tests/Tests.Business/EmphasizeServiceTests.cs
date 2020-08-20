using Business.Services;
using System.Collections.Generic;
using Xunit;

namespace Tests.Business
{
    public class EmphasizeServiceTests
    {
        [Fact]
        public void Emphasize_KeywordsFound_Emphasized()
        {
            // arrange
            var service = new EmphasizeService();

            var text = "This trouser perfectly pairs with a blue shirt.";
            var keywords = new HashSet<string>() { "trouser", "shirt" };

            // act
            var result = service.Emphasize(text, keywords);

            // assert
            Assert.Equal("This <em>trouser</em> perfectly pairs with a blue <em>shirt</em>.", result);
        }

        [Fact]
        public void Emphasize_NoKeywordsFound_OriginalText()
        {
            // arrange
            var service = new EmphasizeService();

            var text = "This trouser perfectly pairs with a blue shirt.";
            var keywords = new HashSet<string>() { "clown" };

            // act
            var result = service.Emphasize(text, keywords);

            // assert
            Assert.Equal("This trouser perfectly pairs with a blue shirt.", result);
        }

        [Fact]
        public void Emphasize_NullKeywords_OriginalText()
        {
            // arrange
            var service = new EmphasizeService();

            var text = "This trouser perfectly pairs with a blue shirt.";
            HashSet<string> keywords = null;

            // act
            var result = service.Emphasize(text, keywords);

            // assert
            Assert.Equal("This trouser perfectly pairs with a blue shirt.", result);
        }

        [Fact]
        public void Emphasize_EmptyKeywords_OriginalText()
        {
            // arrange
            var service = new EmphasizeService();

            var text = "This trouser perfectly pairs with a blue shirt.";
            var keywords = new HashSet<string>();

            // act
            var result = service.Emphasize(text, keywords);

            // assert
            Assert.Equal("This trouser perfectly pairs with a blue shirt.", result);
        }

        [Fact]
        public void Emphasize_NullText_OriginalText()
        {
            // arrange
            var service = new EmphasizeService();

            string text = null;
            var keywords = new HashSet<string>() { "trouser", "shirt" };

            // act
            var result = service.Emphasize(text, keywords);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public void Emphasize_EmptyText_OriginalText()
        {
            // arrange
            var service = new EmphasizeService();

            var text = string.Empty;
            var keywords = new HashSet<string>() { "trouser", "shirt" };

            // act
            var result = service.Emphasize(text, keywords);

            // assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Emphasize_KeywordsAreNotWords_OriginalText()
        {
            // arrange
            var service = new EmphasizeService();

            var text = "This trouser perfectly pairs with a blue shirt.";
            var keywords = new HashSet<string>() { "." };

            // act
            var result = service.Emphasize(text, keywords);

            // assert
            Assert.Equal("This trouser perfectly pairs with a blue shirt.", result);
        }

        [Fact]
        public void Emphasize_GetSeparators_ReturnsSeparators()
        {
            // arrange
            var service = new EmphasizeService();

            // act
            var result = service.GetSeparators();

            // assert
            var expected = new[] { ' ', '?', '.', '!', ',' };

            foreach (var c in result)
            {
                Assert.Contains(c, expected);
            }
        }
    }
}

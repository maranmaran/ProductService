using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Business.Queries.GetProducts
{
    /// <summary>
    /// Query for products as per CQRS pattern
    /// Appropriate handler is defined in: <c>GetProductsQueryHandler</c>
    /// </summary>
    public class GetProductsQuery : IRequest<GetProductsResponse>
    {
        public GetProductsQuery(float? maxPrice, string size, string highlight)
        {
            MaxPrice = maxPrice;
            Size = size;
            HighlightKeywords = highlight?.Split(",")?.Select(p => p.Trim().ToLower())?.ToHashSet();
        }

        public float? MaxPrice { get; set; }
        public string Size { get; set; }
        public HashSet<string> HighlightKeywords { get; set; }
    }
}

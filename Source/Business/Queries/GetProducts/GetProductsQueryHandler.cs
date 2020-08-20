using Business.Interfaces;
using Business.Models;
using Common;
using Common.Exceptions;
using Domain.Entities;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Queries.GetProducts
{
    /// <summary>
    /// Handler for get products query request
    /// Orchestrates business logic for this query to complete
    /// </summary>
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsResponse>
    {
        private readonly IRepository<Product> _repository;
        private readonly ILogger<GetProductsQueryHandler> _logger;
        private readonly IEmphasizeService _emphasizeService;

        public GetProductsQueryHandler(IRepository<Product> repository, ILogger<GetProductsQueryHandler> logger, IEmphasizeService emphasizeService)
        {
            _repository = repository;
            _logger = logger;
            _emphasizeService = emphasizeService;
        }

        public async Task<GetProductsResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken = default)
        {
            try
            {
                // get entities
                var entities = (await _repository.GetAll(
                    filter: GetProductFilterExpression(request.Size, request.MaxPrice),
                    cancellationToken: cancellationToken
                )).ToList();

                if (entities.IsNullOrEmpty())
                {
                    return new GetProductsResponse();
                }

                // emphasize words
                // get necessary data for filter object 
                var sizesHash = new HashSet<string>();
                var frequencyCounter = new Dictionary<string, int>();
                var minimumPrice = float.MaxValue;
                var maximumPrice = float.MinValue;

                foreach (var entity in entities)
                {
                    // emphasize words
                    if (!request.HighlightKeywords.IsNullOrEmpty())
                    {
                        entity.Description = _emphasizeService.Emphasize(entity.Description, request.HighlightKeywords);
                    }

                    // set frequency of words
                    var words = entity.Description.Split(_emphasizeService.GetSeparators(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        frequencyCounter[word] = frequencyCounter.ContainsKey(word) ? frequencyCounter[word] + 1 : 1;
                    }

                    // set min
                    if (entity.Price < minimumPrice) minimumPrice = entity.Price;

                    // set max
                    if (entity.Price > maximumPrice) maximumPrice = entity.Price;

                    sizesHash.AddRange(entity.Sizes);
                }

                // take common words according to our business needs
                // top 10 after discarding top 5
                var commonWords = frequencyCounter
                                                            .OrderByDescending(x => x.Value)
                                                            .Skip(5)
                                                            .Take(10)
                                                            .Select(x => x.Key)
                                                            .ToList();

                return new GetProductsResponse()
                {
                    Products = entities,
                    Filter = new ProductFilter()
                    {
                        MaximumPrice = maximumPrice,
                        MinimumPrice = minimumPrice,
                        CommonWords = commonWords,
                        Sizes = sizesHash
                    }
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to retrieve products information", e);
                throw new FetchException($"{nameof(Product)}s", e);
            }
        }

        /// <summary>
        /// Builds expression for filtering products
        /// </summary>
        internal Expression<Func<Product, bool>> GetProductFilterExpression(string size, float? maxPrice)
        {
            if (string.IsNullOrWhiteSpace(size) && !maxPrice.HasValue)
                return null;

            // builder filter
            var predicate = PredicateBuilder.New<Product>(true);

            if (!string.IsNullOrWhiteSpace(size))
            {
                predicate.And(product => product.Sizes.Any(s => s.ToLower().Trim() == size.ToLower().Trim()));
            }

            if (maxPrice.HasValue)
            {
                predicate.And(product => product.Price <= maxPrice.Value);
            }

            return predicate;
        }
    }
}
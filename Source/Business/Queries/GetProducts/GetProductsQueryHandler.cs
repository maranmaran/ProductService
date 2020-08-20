using Business.Models;
using Common;
using Common.Exceptions;
using Domain.Entities;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Tests.Business")]
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

        public GetProductsQueryHandler(IRepository<Product> repository, ILogger<GetProductsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
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
                var minimumPrice = float.MaxValue;
                var maximumPrice = float.MinValue;

                foreach (var entity in entities)
                {
                    // set min
                    if (entity.Price < minimumPrice) minimumPrice = entity.Price;

                    // set max
                    if (entity.Price > maximumPrice) maximumPrice = entity.Price;

                }

                return new GetProductsResponse()
                {
                    Products = entities,
                    Filter = new ProductFilter()
                    {
                        MaximumPrice = maximumPrice,
                        MinimumPrice = minimumPrice
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
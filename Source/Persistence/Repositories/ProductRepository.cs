using Common;
using Domain.Entities;
using Library.Communication.Interfaces;
using Persistence.CommunicationModels;
using Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Tests.Persistence")]
namespace Persistence.Repositories
{
    internal class ProductRepository : IRepository<Product>
    {
        private readonly DatabaseSettings _settings;
        private readonly ICommunicationService _communicationService;

        public ProductRepository(DatabaseSettings settings, ICommunicationService communicationService)
        {
            _settings = settings;
            _communicationService = communicationService;
        }


        public async Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>> filter = null, CancellationToken cancellationToken = default)
        {

            var entities = (await _communicationService.GetAsync<MockitoProducts>(_settings.ProductsEndpoint, null, cancellationToken))
                ?.Products
                ?.AsQueryable();

            if (entities.IsNullOrEmpty())
            {
                return new List<Product>();
            }

            if (filter != null)
            {
                entities = entities.Where(filter);
            }

            return entities.ToList();
        }
    }
}

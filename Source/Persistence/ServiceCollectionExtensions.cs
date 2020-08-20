using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Interfaces;
using Persistence.Repositories;

namespace Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigurePersistanceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // configure database settings
            services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
            services.AddSingleton(x => x.GetService<IOptions<DatabaseSettings>>().Value);

            // register repository for DI
            services.AddTransient(typeof(IRepository<Product>), typeof(ProductRepository));
        }
    }
}

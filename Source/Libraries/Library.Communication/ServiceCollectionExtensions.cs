using Library.Communication.Interfaces;
using Library.Communication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Communication
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureCommunication(this IServiceCollection services)
        {
            services.AddTransient<ICommunicationService, CommunicationService>();
            services.AddHttpClient();
        }
    }
}

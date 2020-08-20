using Business.Interfaces;
using Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<IEmphasizeService, EmphasizeService>();
        }
    }
}

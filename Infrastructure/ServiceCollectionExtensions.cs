using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddMultitenancyServices(configuration)
                .AddPersistenceService(configuration)
                .AddIdentityServices()
                .AddPermission();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            return app.UseCurrentUser();
        }
    }
}

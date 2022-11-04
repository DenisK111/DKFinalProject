using Metflix.BL.Services.Contracts;
using Metflix.BL.Services.Implementations;
using Metflix.DL.Repositories.Contracts;
using Metflix.DL.Repositories.Implementations;
using Metflix.Models.DbModels.Configurations;

namespace Metflix.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services
                .AddSingleton<IMovieRepository, MovieRepository>()
                .AddSingleton<IIdentityRepository,IdentityRepository>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .AddTransient<IIDentityService, IdentityService>();

            return services;
        }
    }
}

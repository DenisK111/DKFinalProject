using Metflix.BL.Services.Contracts;
using Metflix.BL.Services.Implementations;
using Metflix.DL.Repositories.Contracts;
using Metflix.DL.Repositories.Implementations.MongoRepositories;
using Metflix.DL.Repositories.Implementations.SqlRepositories;
using Metflix.Models.DbModels.Configurations;

namespace Metflix.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services
                .AddSingleton<IMovieRepository, SqlMovieRepository>()
                .AddSingleton<IIdentityRepository, SqlIdentityRepository>()
                .AddSingleton<IUserMovieRepository, SqlUserMovieRepository>()
                .AddSingleton<IPurchaseRepository, MongoPurchaseRepository>();

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

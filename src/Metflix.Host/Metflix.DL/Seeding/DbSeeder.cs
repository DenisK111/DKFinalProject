using Metflix.DL.Repositories.Contracts;
using Metflix.DL.Seeding.Contracts;
using Metflix.DL.Seeding.Implementations;
using Metflix.Host.DL.Seeding.Contracts;
using Metflix.Host.DL.Seeding.Implementations;

namespace Metflix.DL.Seeding
{
    public class DbSeeder : IDbSeeder
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _identityRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public DbSeeder(IMovieRepository movieRepository, IUserRepository identityRepository, IPurchaseRepository purchaseRepository)
        {
            _movieRepository = movieRepository;
            _identityRepository = identityRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task SeedDb()
        {
            var seeders = new List<ISeeder>()
            {
                new MoviesSeeder(_movieRepository),
                new UserSeeder(_identityRepository),
                new PurchaseSeeder(_purchaseRepository,_identityRepository)
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IIdentityRepository _identityRepository;

        public DbSeeder(IMovieRepository movieRepository, IIdentityRepository identityRepository)
        {
            _movieRepository = movieRepository;
            _identityRepository = identityRepository;
        }

        public async Task SeedDb()
        {
            var seeders = new List<ISeeder>()
            {
                new MoviesSeeder(_movieRepository),
                new UserSeeder(_identityRepository)
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync();
            }
        }
    }
}

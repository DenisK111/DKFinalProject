using Metflix.DL.Repositories.Contracts;
using Metflix.Host.DL.Seeding.Contracts;
using Metflix.Models.DbModels;

namespace Metflix.DL.Seeding.Implementations
{
    public class PurchaseSeeder : ISeeder
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IIdentityRepository _identityRepository;

        public PurchaseSeeder(IPurchaseRepository purchaseRepository, IIdentityRepository identityRepository)
        {
            _purchaseRepository = purchaseRepository;
            _identityRepository = identityRepository;
        }

        public async Task SeedAsync()
        {
            if ((await _purchaseRepository.GetAll()).Any())
            {
                return;
            }

            var userAdminId = (await _identityRepository.GetUserByEmail("admin@admin.com"))!.Id;
            var userUserId = (await _identityRepository.GetUserByEmail("user@user.com"))!.Id;

            var movies = new List<MovieRecord>() {
             new MovieRecord()
            {
                Id = -1,
                Name = "Test Movie 1",
                PricePerDay = 5,
            },


              new MovieRecord()
            {
                Id = -2,
                Name = "Test Movie 2",
                PricePerDay = 4,
            },

            new MovieRecord()
            {
                Id = -3,
                Name = "Test Movie 3",
                PricePerDay = 6,
            },

            new MovieRecord()
            {
                Id = -4,
                Name = "Test Movie 7",
                PricePerDay = 7,
            }};

            var random = new Random(Guid.NewGuid().GetHashCode());            

            for (int i = 0; i < 10; i++)
            {
                var days = random.Next(1, 5);
                var num1 = random.Next(0, movies.Count);
                var num2 = random.Next(0, movies.Count);
                var daysBackPurchased = random.Next(-4, 1);

                while (num1 == num2)
                {
                    num2 = random.Next(0, movies.Count);
                }

                var moviesAdded = new List<MovieRecord>() { movies[num1], movies[num2] };

               await _purchaseRepository.AddPurchase(new Purchase()
                {
                    Id = Guid.NewGuid(),
                    Movies = moviesAdded,
                    Days = days,
                    UserId = random.Next(0, 2) == 0 ? userAdminId : userUserId,
                    DueDate = DateTime.UtcNow.AddDays(daysBackPurchased + days),
                    PurchaseDate = DateTime.UtcNow.AddDays(daysBackPurchased),
                    LastChanged = DateTime.UtcNow.AddDays(daysBackPurchased),
                    TotalPrice = moviesAdded.Sum(x => x.PricePerDay) * days,
                    UserMovieIds = new List<int>() {random.Next(0, movies.Count), random.Next(movies.Count, movies.Count * 2)},
                });
            }

        }
    }
}

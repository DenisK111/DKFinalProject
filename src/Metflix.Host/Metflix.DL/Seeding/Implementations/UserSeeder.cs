using Metflix.DL.Repositories.Contracts;
using Metflix.Host.DL.Seeding.Contracts;
using Metflix.Models.DbModels;
using Metflix.Models.DbModels.Helpers;
using Utils;

namespace Metflix.DL.Seeding.Implementations
{
    public class UserSeeder : ISeeder
    {
        private readonly IIdentityRepository _identityRepository;

        public UserSeeder(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task SeedAsync()
        {
            var users = new List<UserInfo>() {
             new UserInfo()
            {                
                Name = "Admin",
                Email = "admin@admin.com",
                Password = CustomPasswordHasher.GetSHA512Password("admin"),
                DateOfBirth = new DateTime(1992, 1, 1),
                Role = UserRoles.Admin
            },
              new UserInfo()
            {
                Name = "User",
                Email = "user@user.com",
                Password = CustomPasswordHasher.GetSHA512Password("user"),
                DateOfBirth = new DateTime(1992, 1, 1),
                Role = UserRoles.User
            }
            };

            if (await _identityRepository.CheckIfUserExists(users[0].Email))
            {
                return;
            }            

            foreach (var user in users)
            {
                await _identityRepository.CreateUser(user);
            }
        }
    }
}

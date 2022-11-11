using Metflix.Models.DbModels;

namespace Metflix.DL.Repositories.Contracts
{
    public interface IIdentityRepository
    {
        Task<UserInfo> GetUserByEmail(string email, CancellationToken cancellationToken = default);
        Task<UserInfo> GetById(string id, CancellationToken cancellationToken = default);
        Task<bool> CreateUser(UserInfo user, CancellationToken cancellationToken = default);
        Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken = default);
    }
}

using Metflix.Models.DbModels;
using Metflix.Models.Responses.Purchases.PurchaseDtos;

namespace Metflix.DL.Repositories.Contracts
{
    public interface IUserMovieRepository
    {
        Task<int> Add(UserMovie model, CancellationToken cancellationToken = default);
        Task<bool> MarkAsReturned(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserMovie>> GetAll(CancellationToken cancellationToken = default);
        Task<UserMovie?> GetById(int Id,CancellationToken cancellationToken = default);
        Task<IEnumerable<UserMovie>> GetAllByUserId(string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserMovie>> GetAllOverDue(CancellationToken cancellationToken = default);
        Task<UserMovie?> Update(UserMovie model, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserMovieDto>> GetAllUnreturnedByUserId(string userId, CancellationToken cancellationToken = default);
    }
}

using Metflix.Models.DbModels;

namespace Metflix.DL.Repositories.Contracts
{
    public interface IPurchaseRepository
    {
        Task AddPurchase(Purchase purchase,CancellationToken cancellationToken = default);

        Task<IEnumerable<Purchase>> GetAll(CancellationToken cancellationToken = default);
        Task<IEnumerable<Purchase>> GetAllByUserId(string userId,CancellationToken cancellationToken = default);
        Task<Purchase?> GetById(Guid id,CancellationToken cancellationToken = default);
        Task<Purchase?> GetByIdAndUserId(Guid id,string userId, CancellationToken cancellationToken = default);

        Task<decimal> GetTotalSales(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}

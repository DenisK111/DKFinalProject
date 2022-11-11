using Metflix.Models.DbModels;

namespace Metflix.DL.Repositories.Contracts
{
    public interface IInventoryLogRepository
    {
        public Task<IEnumerable<InventoryLog>> GetAll(CancellationToken cancellationToken = default);
        public Task AddLog(InventoryLog data,CancellationToken cancellationToken = default);

        public Task<InventoryLog> GetById(Guid id, CancellationToken cancellationToken = default);
    }
}

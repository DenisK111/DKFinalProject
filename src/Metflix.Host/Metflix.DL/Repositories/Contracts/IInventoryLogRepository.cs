using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.DbModels;
using Metflix.Models.KafkaModels;

namespace Metflix.DL.Repositories.Contracts
{
    public interface IInventoryLogRepository
    {
        public Task<IEnumerable<InventoryLog>> GetAll(CancellationToken cancellationToken = default);
        public Task AddLog(InventoryLog data,CancellationToken cancellationToken = default);

        public Task<InventoryLog> GetById(Guid id, CancellationToken cancellationToken = default);
    }
}

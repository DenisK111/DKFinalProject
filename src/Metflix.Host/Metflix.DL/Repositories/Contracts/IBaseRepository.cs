using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.DL.Repositories.Contracts
{
    public interface IBaseRepository<T, TId>
    {
        Task<T?> Add(T model,CancellationToken cancellationToken = default);
        Task<bool> Delete(TId modelId, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
        Task<T?> GetById(TId id, CancellationToken cancellationToken = default);
        Task<T?> Update(T model, CancellationToken cancellationToken = default);
    }
}

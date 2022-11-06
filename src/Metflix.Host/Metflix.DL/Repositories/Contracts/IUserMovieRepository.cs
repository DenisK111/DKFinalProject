using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.DbModels;

namespace Metflix.DL.Repositories.Contracts
{
    public interface IUserMovieRepository
    {
        Task Add(UserMovie model, CancellationToken cancellationToken = default);
        Task<bool> MarkAsReturned(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserMovie>> GetAll(CancellationToken cancellationToken = default);
        Task<UserMovie?> GetById(int Id,CancellationToken cancellationToken = default);
        Task<IEnumerable<UserMovie>> GetAllByUserId(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserMovie>> GetAllOverDue(CancellationToken cancellationToken = default);
        Task<UserMovie?> Update(UserMovie model, CancellationToken cancellationToken = default);
    }
}

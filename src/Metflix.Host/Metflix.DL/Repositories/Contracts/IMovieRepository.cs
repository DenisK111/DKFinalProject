using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.DbModels;
using Metflix.Models.Responses.Movies.MovieDtos;

namespace Metflix.DL.Repositories.Contracts
{
    public interface IMovieRepository : IBaseRepository<Movie,int>
    {
        Task<IEnumerable<Movie>> GetAllAvailableMovies(CancellationToken cancellationToken = default);
        Task DecreaseAvailableQuantity(int movieId,int amount = 1,CancellationToken cancellationToken = default);
        Task IncreaseAvailableQuantity(int movieId, int amount = 1, CancellationToken cancellationToken = default);
        Task<Movie> AdjustInventory(int movieId, int amount, CancellationToken cancellationToken = default);       
        Task<int> GetCurrentQuantity(int movieId, CancellationToken cancellationToken = default);
        Task<int> GetTotalQuantity(int movieId, CancellationToken cancellationToken = default);
    }
}

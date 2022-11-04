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
    }
}

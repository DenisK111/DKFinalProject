using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.Responses.Movies.MovieDtos;

namespace Metflix.Models.Responses.Movies
{
    public class AvailableMoviesResponse : BaseResponse<IEnumerable<AvailableMovieDto>>
    {
    }
}

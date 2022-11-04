using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Metflix.Models.Responses.Movies;

namespace Metflix.Models.Mediatr.Queries.Movies
{
    public record GetAvailableMoviesQuery : IRequest<AvailableMoviesResponse>
    {
    }
}

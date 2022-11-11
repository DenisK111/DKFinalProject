using MediatR;
using Metflix.Models.Requests.Movies;
using Metflix.Models.Responses.Movies;

namespace Metflix.Models.Mediatr.Queries.Movies
{
    public record GetMovieByIdQuery(ByIntIdRequest Request) : IRequest<MovieResponse>
    {
    }
}

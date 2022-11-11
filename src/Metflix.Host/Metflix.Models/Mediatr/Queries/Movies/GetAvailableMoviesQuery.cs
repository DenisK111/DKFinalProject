using MediatR;
using Metflix.Models.Responses.Movies;

namespace Metflix.Models.Mediatr.Queries.Movies
{
    public record GetAvailableMoviesQuery : IRequest<AvailableMoviesResponse>
    {
    }
}

using MediatR;
using Metflix.Models.Requests.Movies;
using Metflix.Models.Responses.Movies;

namespace Metflix.Models.Mediatr.Commands.Movies
{
    public record AddMovieCommand(AddMovieRequest Request) : IRequest<MovieResponse>
    {
    }
}

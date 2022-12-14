using MediatR;
using Metflix.Models.Responses.Movies;

namespace Metflix.Models.Mediatr.Commands.Movies
{
    public record AddInventoryCommand(AddInventoryRequest Request,string UserId) : IRequest<MovieResponse>
    {
    }
}

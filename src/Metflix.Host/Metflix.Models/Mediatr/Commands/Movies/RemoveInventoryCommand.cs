using MediatR;
using Metflix.Models.Responses.Movies;

namespace Metflix.Models.Mediatr.Commands.Movies
{
    public record RemoveInventoryCommand (RemoveInventoryRequest Request,string UserId) : IRequest<MovieResponse>
    {
    }    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Metflix.Models.Responses.Purchases;

namespace Metflix.Models.Mediatr.Commands.Purchases
{
    public record ReturnMovieCommand(int userMovieId, string UserId) : IRequest<ReturnMovieResponse>
    {
    }

}

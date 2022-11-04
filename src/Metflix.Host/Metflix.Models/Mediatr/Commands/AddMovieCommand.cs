using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Metflix.Models.Requests.Movies;
using Metflix.Models.Responses.Movies;

namespace Metflix.Models.Mediatr.Commands
{
    public record AddMovieCommand(AddMovieRequest Request): IRequest<MovieResponse>
    {
    }
}

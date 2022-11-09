using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Metflix.Models.Responses.Reports;

namespace Metflix.Models.Mediatr.Queries.Reports
{
    public record GetOverDueUserMoviesQuery : IRequest<UserMoviesOverdueResponse>
    {
    }
}

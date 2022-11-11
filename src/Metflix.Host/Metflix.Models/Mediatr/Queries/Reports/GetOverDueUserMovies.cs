using MediatR;
using Metflix.Models.Responses.Reports;

namespace Metflix.Models.Mediatr.Queries.Reports
{
    public record GetOverDueUserMoviesQuery : IRequest<UserMoviesOverdueResponse>
    {
    }
}

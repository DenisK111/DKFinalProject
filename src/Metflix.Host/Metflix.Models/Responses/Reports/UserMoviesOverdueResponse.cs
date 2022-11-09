using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.Responses.Reports.ReportDtos;

namespace Metflix.Models.Responses.Reports
{
    public class UserMoviesOverdueResponse : BaseResponse<IEnumerable<UserMovieOverDueDto>>
    {
    }
}

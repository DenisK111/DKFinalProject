using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Queries.Reports;
using Metflix.Models.Responses.Reports;
using Metflix.Models.Responses.Reports.ReportDtos;

namespace Metflix.BL.MediatR.QueryHandlers.Reports
{
    public class GetOverDueUserMoviesQueryHandler : IRequestHandler<GetOverDueUserMoviesQuery, UserMoviesOverdueResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserMovieRepository _userMovieRepository;

        public GetOverDueUserMoviesQueryHandler(IMapper mapper, IUserMovieRepository userMovieRepository)
        {
            _mapper = mapper;
            _userMovieRepository = userMovieRepository;
        }

        public async Task<UserMoviesOverdueResponse> Handle(GetOverDueUserMoviesQuery request, CancellationToken cancellationToken)
        {
            var result = await _userMovieRepository.GetAllOverDue(cancellationToken);
            var model = _mapper.Map<IEnumerable<UserMovieOverDueDto>>(result);

            return new UserMoviesOverdueResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = model
            };
        }
    }
}

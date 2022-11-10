using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Queries.Purchases;
using Metflix.Models.Responses.Purchases;
using Metflix.Models.Responses.Purchases.PurchaseDtos;

namespace Metflix.BL.MediatR.QueryHandlers.Purchases
{
    public class GetCurrentlyTakenMoviesQueryHandler : IRequestHandler<GetCurrentlyTakenMoviesQuery, GetCurrentlyTakenMoviesResponse>
    {
        private readonly IUserMovieRepository _userMovieRepository;
        private readonly IMapper _mapper;

        public GetCurrentlyTakenMoviesQueryHandler(IUserMovieRepository userMovieRepository,IMapper mapper)
        {
            _userMovieRepository = userMovieRepository;
            _mapper = mapper;
        }

        public async Task<GetCurrentlyTakenMoviesResponse> Handle(GetCurrentlyTakenMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _userMovieRepository.GetAllUnreturnedByUserId(request.userId,cancellationToken);
            var moviesDto = _mapper.Map<IEnumerable<UserMovieDto>>(movies);
            return new GetCurrentlyTakenMoviesResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = moviesDto,
            };
        }
    }
}

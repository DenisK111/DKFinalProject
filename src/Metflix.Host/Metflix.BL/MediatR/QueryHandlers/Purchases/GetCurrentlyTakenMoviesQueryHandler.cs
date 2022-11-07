using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Queries.Purchases;
using Metflix.Models.Responses.Purchases;

namespace Metflix.BL.MediatR.QueryHandlers.Purchases
{
    public class GetCurrentlyTakenMoviesQueryHandler : IRequestHandler<GetCurrentlyTakenMoviesQuery, GetCurrentlyTakenMoviesResponse>
    {
        private readonly IUserMovieRepository _userMovieRepository;        

        public GetCurrentlyTakenMoviesQueryHandler(IUserMovieRepository userMovieRepository, IMovieRepository movieRepository)
        {
            _userMovieRepository = userMovieRepository;            
        }

        public async Task<GetCurrentlyTakenMoviesResponse> Handle(GetCurrentlyTakenMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _userMovieRepository.GetAllUnreturnedByUserId(request.userId,cancellationToken);

            return new GetCurrentlyTakenMoviesResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = movies,
            };
        }
    }
}

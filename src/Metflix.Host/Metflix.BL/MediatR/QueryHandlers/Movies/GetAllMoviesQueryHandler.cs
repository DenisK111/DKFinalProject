using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Queries.Movies;
using Metflix.Models.Responses.Movies;
using Metflix.Models.Responses.Movies.MovieDtos;
using Microsoft.Extensions.Logging;

namespace Metflix.BL.MediatR.QueryHandlers.Movies
{
    public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, MovieCollectionResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;        

        public GetAllMoviesQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            
        }
        public async Task<MovieCollectionResponse> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {            
            var responseModel = await _movieRepository.GetAll(cancellationToken);

            var responseModelDto = _mapper.Map<IEnumerable<MovieDto>>(responseModel);

            var response = new MovieCollectionResponse()
            {
                Model = responseModelDto,
                HttpStatusCode = HttpStatusCode.OK,
            };

            return response;
        }
    }
}

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
using Utils;

namespace Metflix.BL.MediatR.QueryHandlers.Movies
{
    public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetMovieByIdQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        public async Task<MovieResponse> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var responseModel = await _movieRepository.GetById(request.id,cancellationToken);

            if (responseModel == null)
            {
                return new MovieResponse()
                {
                    Message = ResponseMessages.IdNotFound,
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var responseModelDto = _mapper.Map<MovieDto>(responseModel);

            return new MovieResponse()
            {
                Model = responseModelDto,
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
    }
}

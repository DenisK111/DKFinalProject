﻿using System.Net;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Common;
using Metflix.Models.Mediatr.Queries.Movies;
using Metflix.Models.Responses.Movies;
using Metflix.Models.Responses.Movies.MovieDtos;

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
            var responseModel = await _movieRepository.GetById(request.Request.Id,cancellationToken);

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

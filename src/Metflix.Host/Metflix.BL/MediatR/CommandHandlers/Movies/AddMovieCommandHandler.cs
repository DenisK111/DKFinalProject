﻿using System.Net;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.DbModels;
using Metflix.Models.Mediatr.Commands.Movies;
using Metflix.Models.Responses.Movies;
using Metflix.Models.Responses.Movies.MovieDtos;

namespace Metflix.BL.MediatR.CommandHandlers.Movies
{
    public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand, MovieResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public AddMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<MovieResponse> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            var movieToAdd = _mapper.Map<Movie>(request.Request);
            var addedModel = await _movieRepository.Add(movieToAdd, cancellationToken);
            var addedModelDto = _mapper.Map<MovieDto>(addedModel);

            return new MovieResponse()
            {
                HttpStatusCode = HttpStatusCode.Created,
                Model = addedModelDto,
            };
        }
    }
}

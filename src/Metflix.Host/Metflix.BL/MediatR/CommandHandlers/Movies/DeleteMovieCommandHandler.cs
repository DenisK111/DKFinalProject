﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Mediatr.Commands.Movies;
using Metflix.Models.Responses.Movies;
using Utils;

namespace Metflix.BL.MediatR.CommandHandlers.Movies
{
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, MovieResponse>
    {
        private readonly IMovieRepository _movieRepository;       

        public DeleteMovieCommandHandler(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;            
        }
        public async Task<MovieResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
           var isDeleted =  await _movieRepository.Delete(request.Request.Id, cancellationToken);

            if (!isDeleted)
            {
                return new MovieResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = ResponseMessages.IdNotFound
                };
            }

            return new MovieResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.NoContent,
            };

        }
    }
}

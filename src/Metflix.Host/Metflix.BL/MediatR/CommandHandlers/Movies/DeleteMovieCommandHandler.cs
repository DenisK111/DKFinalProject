using System;
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
    internal class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, MovieResponse>
    {
        private readonly IMovieRepository _movieRepository;       

        public DeleteMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;            
        }
        public async Task<MovieResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await _movieRepository.Delete(request.Id,cancellationToken);

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

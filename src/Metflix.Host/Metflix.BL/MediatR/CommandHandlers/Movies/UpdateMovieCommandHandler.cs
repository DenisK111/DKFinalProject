using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.DbModels;
using Metflix.Models.Mediatr.Commands;
using Metflix.Models.Responses.Movies;
using Metflix.Models.Responses.Movies.MovieDtos;
using Utils;

namespace Metflix.BL.MediatR.CommandHandlers.Movies
{
    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, MovieResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public UpdateMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<MovieResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var movieToUpdate = await _movieRepository.GetById(request.Request.Id,cancellationToken);

            if (movieToUpdate == null)
            {
                return new MovieResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.IdNotFound
                };
            }

            var updatedMovie = _mapper.Map<Movie>(request.Request);
            var updatedModel = await _movieRepository.Update(updatedMovie, cancellationToken);

            if (updatedModel == null)
            {
                return new MovieResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ResponseMessages.InvalidData
                };
            }

            var updatedModelDto = _mapper.Map<MovieDto>(updatedModel);

            return new MovieResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = updatedModelDto,
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Metflix.BL.Services.Contracts;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Common;
using Metflix.Models.Mediatr.Commands.Movies;
using Metflix.Models.Responses.Movies;
using Utils;

namespace Metflix.BL.MediatR.CommandHandlers.Movies
{
    public class AddInventoryCommandHandler : IRequestHandler<AddInventoryCommand, MovieResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IInventoryService _inventoryService;

        public AddInventoryCommandHandler(IMovieRepository movieRepository, IInventoryService inventoryService)
        {
            _movieRepository = movieRepository;
            _inventoryService = inventoryService;
        }

        public async Task<MovieResponse> Handle(AddInventoryCommand request, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetById(request.Request.MovieId, cancellationToken);

            if (movie == null)
            {
                return new MovieResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.IdNotFound
                };
            }

            return await _inventoryService.AdjustInventoryAndProduceToKafkaTopic(request.Request.MovieId, request.Request.Quantity, request.UserId, cancellationToken);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Metflix.BL.Services.Contracts;
using Metflix.DL.Repositories.Contracts;
using Metflix.Kafka.Contracts;
using Metflix.Models.Common;
using Metflix.Models.Configurations.KafkaSettings.Producers;
using Metflix.Models.KafkaModels;
using Metflix.Models.Responses.Movies;
using Metflix.Models.Responses.Movies.MovieDtos;
using Utils;

namespace Metflix.BL.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _userRepository;
        private readonly IGenericProducer<Guid, InventoryChangeData, KafkaInventoryChangesProducerSettings> _kafkaProducer;

        public InventoryService(IMovieRepository movieRepository, IMapper mapper, IIdentityRepository userRepository, IGenericProducer<Guid, InventoryChangeData, KafkaInventoryChangesProducerSettings> kafkaProducer)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<MovieResponse> AdjustInventoryAndProduceToKafkaTopic(int movieId, int amountToAdjust, string userId, CancellationToken cancellationToken = default)
        {
            var movie = await _movieRepository.GetById(movieId);

            if (movie == null)
            {
                return new MovieResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.IdNotFound
                };
            }

            if (movie.AvailableQuantity + amountToAdjust < 0)
            {
                return new MovieResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessages.NotEnoughInventoryToRemove
                };
            }

            var updatedMovie = await _movieRepository.AdjustInventory(movieId, amountToAdjust, cancellationToken);
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                return new MovieResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.InvalidUser
                };
            }

            var inventoryChangeData = new InventoryChangeData()
            {
                MovieId = movieId,
                UserName = user.Name,
                UserId = userId,
                MovieName = movie.Name,
                AmountChanged = amountToAdjust,
                LastChanged = DateTime.UtcNow
            };

            await _kafkaProducer.ProduceAsync(inventoryChangeData.GetKey(), inventoryChangeData,cancellationToken);

            return new MovieResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = _mapper.Map<MovieDto>(updatedMovie)
            };
        }
    }
}

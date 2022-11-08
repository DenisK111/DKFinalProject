using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.Responses.Movies;

namespace Metflix.BL.Services.Contracts
{
    public interface IInventoryService
    {
        public Task<MovieResponse> AdjustInventoryAndProduceToKafkaTopic(int movieId, int amountToAdjust, string userId, CancellationToken cancellationToken = default);
    }
}

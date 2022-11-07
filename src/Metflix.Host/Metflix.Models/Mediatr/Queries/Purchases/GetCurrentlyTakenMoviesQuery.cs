﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Metflix.Models.Responses.Purchases;

namespace Metflix.Models.Mediatr.Queries.Purchases
{
    public record GetCurrentlyTakenMoviesQuery(string userId) : IRequest<GetCurrentlyTakenMoviesResponse> 
    {
    }
}

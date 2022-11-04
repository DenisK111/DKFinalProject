﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.DbModels
{
    public class UserMovie
    {
        public string UserId { get; init; } = null!;
        public int MovieId { get; init; }
        public DateTime LastChanged { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime CreatedOn { get; init; }

        public bool IsReturned { get; set; }
    }
}

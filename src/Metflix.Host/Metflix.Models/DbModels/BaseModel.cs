﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.DbModels
{
    public class BaseModel<T> 
    {
        public T Id { get; set; }
        public DateTime LastChanged { get; init; }
    }
}

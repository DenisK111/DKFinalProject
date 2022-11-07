﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace Metflix.Models.KafkaModels
{
    [MessagePackObject]
    public record MovieInfoData
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; } = null!;
        [Key(2)]
        public decimal PricePerDay { get; set; }
    }
}

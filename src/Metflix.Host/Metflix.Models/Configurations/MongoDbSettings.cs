﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.Configurations
{
    public class MongoDbSettings
    { 
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionPurchase { get; set; } = null!;        
        public string CollectionInventoryLogData { get; set; } = null!;        
    }
}

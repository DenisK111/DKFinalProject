using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.DbModels;

namespace Metflix.BL.TempData
{
    public static class PurchaseTempData
    {
        public static ConcurrentDictionary<string, Purchase?> PendingPurchases = new();
    }
}

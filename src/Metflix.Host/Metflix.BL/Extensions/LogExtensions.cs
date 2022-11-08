using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.DbModels;

namespace Metflix.BL.Extensions
{
    public static class LogExtensions
    {
        public static string ToLogString(this InventoryLog log)
        {
            var action = log.AmountChanged > 0 ? "added" : "removed";
            var preposition = log.AmountChanged > 0 ? "to" : "from";

            return
                $"{log.LastChanged}||{log.UserName}(Id:{log.UserId}) {action} {Math.Abs(log.AmountChanged)} copies of movie {log.MovieName}(Id:{log.MovieId}) {preposition} the inventory.||";
        }
    }
}

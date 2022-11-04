using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.DL.Seeding.Contracts
{
    public interface IDbSeeder
    {
        Task SeedDb();
    }
}

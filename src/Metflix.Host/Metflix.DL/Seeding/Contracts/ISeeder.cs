using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Host.DL.Seeding.Contracts
{
    public interface ISeeder
    {
        Task SeedAsync();
    }
}

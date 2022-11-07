using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.BL.Dataflow.Contracts
{
    public interface IDataflow<T> : IDisposable
    {
        public Task ProcessData(T data,CancellationToken cancellationToken = default);
    }
}

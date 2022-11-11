namespace Metflix.BL.Dataflow.Contracts
{
    public interface IDataflow<T> : IDisposable
    {
        public Task ProcessData(T data,CancellationToken cancellationToken = default);
    }
}

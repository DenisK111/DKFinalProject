namespace Metflix.DL.Repositories.Contracts
{
    public interface ITempPurchaseDataRepository
    {
        public Task SetOrUpdateEntryAsync(string key, byte[] value);
        public Task<byte[]> GetValueAsync(string key);        
        public Task DeleteEntryAsync(string key);
        Task<bool> ContainsKeyAsync(string key);
    }
}

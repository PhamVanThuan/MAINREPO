namespace SAHL.UI.Halo.Shared.Configuration.Caching
{
    public class CacheItem
    {
        public CacheItem(string key, dynamic item)
        {
            this.Key = key;
            this.Item = item;
        }

        public string Key { get; protected set; }
        public dynamic Item { get; protected set; }
    }
}
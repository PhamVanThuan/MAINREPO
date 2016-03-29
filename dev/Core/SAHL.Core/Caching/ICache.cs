namespace SAHL.Core.Caching
{
    public interface ICache
    {
        bool Contains(string key);

        T GetItem<T>(string key);

        void SetItem<T>(string key, T value);

        void AddItem<T>(string key, T value);

        void AddOrSetItem<T>(string key, T value);

        void RemoveItem(string key);

        void Clear();
    }
}
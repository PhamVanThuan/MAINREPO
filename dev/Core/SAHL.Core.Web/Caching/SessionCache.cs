using System.Web;
using SAHL.Core.Caching;

namespace SAHL.Core.Web.Caching
{
    public class SessionCache : ICache
    {
        public bool Contains(string key)
        {
            return HttpContext.Current.Session[key] == null;
        }

        public T GetItem<T>(string key)
        {
            return (T) HttpContext.Current.Session[key];
        }

        public void AddItem<T>(string key, T value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public void RemoveItem(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        public void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        public void SetItem<T>(string key, T value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public void AddOrSetItem<T>(string key, T value)
        {
            if (Contains(key))
            {
                SetItem(key, value);
            }
            else
            {
                AddItem(key, value);
            }
        }
    }
}
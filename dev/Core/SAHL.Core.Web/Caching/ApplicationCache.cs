using SAHL.Core.Caching;
using System.Linq;
using System.Web;

namespace SAHL.Core.Web.Caching
{
    public class ApplicationCache : ICache
    {
        public bool Contains(string key)
        {
            return HttpContext.Current.Application.AllKeys.Contains(key);
        }

        public T GetItem<T>(string key)
        {
            return (T)HttpContext.Current.Application.Get(key);
        }

        public void SetItem<T>(string key, T value)
        {
            HttpContext.Current.Application.Set(key, value);
        }

        public void AddItem<T>(string key, T value)
        {
            HttpContext.Current.Application[key] = value;
        }

        public void RemoveItem(string key)
        {
            HttpContext.Current.Application.Remove(key);
        }

        public void Clear()
        {
            HttpContext.Current.Application.Clear();
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
using System.Collections.Generic;

namespace SAHL.Common.CacheData
{
    public class PresenterData
    {
        Dictionary<string, object> _data;

        public PresenterData()
        {
            _data = new Dictionary<string, object>();
        }

        public void Add(string Key, object Data)
        {
            if (_data.ContainsKey(Key))
                _data[Key] = Data;
            else
                _data.Add(Key, Data);
        }

        public bool ContainsKey(string Key)
        {
            return _data.ContainsKey(Key);
        }

        public object this[string key]
        {
            get
            {
                if (_data.ContainsKey(key))
                    return _data[key];

                return null;
            }
            set
            {
                _data[key] = value;
            }
        }

        public void Remove(string Key)
        {
            if (_data.ContainsKey(Key))
                _data.Remove(Key);
        }

        public void Clear()
        {
            _data.Clear();
            //for (int i = 0; i < keys.Length; i++)
            //{
            //    if (_data[keys[i]] is IDisposable)
            //    {
            //        IDisposable ID = _data[keys[i]] as IDisposable;
            //        ID.Dispose();
            //    }
            //}
            //_data.Clear();
        }
    }
}
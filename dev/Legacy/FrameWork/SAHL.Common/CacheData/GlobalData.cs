using System;
using System.Collections;
using System.Collections.Generic;

namespace SAHL.Common.CacheData
{
    public class GlobalData
    {
        Dictionary<string, GlobalDataItem> _data = new Dictionary<string, GlobalDataItem>();

        public void Add(string Key, object Data, IList<ICacheObjectLifeTime> ObjectLifeTimes)
        {
            if (ObjectLifeTimes == null)
                ObjectLifeTimes = new List<ICacheObjectLifeTime>();

            if (_data.ContainsKey(Key))
                _data[Key] = new GlobalDataItem(Data, ObjectLifeTimes);
            else
                _data.Add(Key, new GlobalDataItem(Data, ObjectLifeTimes));
        }

        public void Add(string Key, object Data, ICacheObjectLifeTime ObjectLifeTime)
        {
            List<ICacheObjectLifeTime> LT = new List<ICacheObjectLifeTime>();
            if (ObjectLifeTime != null)
                LT.Add(ObjectLifeTime);
            Add(Key, Data, LT);
        }

        public void Remove(string Key)
        {
            if (_data.ContainsKey(Key))
                _data.Remove(Key);
        }

        public object this[string key]
        {
            get
            {
                return _data[key].Data;
            }
            set
            {
                _data[key].Data = value;
            }
        }

        public bool ContainsKey(string Key)
        {
            return _data.ContainsKey(Key);
        }

        public void PerformLifeTimeEvent(ICacheObjectLifeTimeEventArgs Args)
        {
            ArrayList RemoveKeys = new ArrayList();
            string[] Keys = new string[_data.Count];
            _data.Keys.CopyTo(Keys, 0);
            for (int i = 0; i < Keys.Length; i++)
            {
                GlobalDataItem GDI = _data[Keys[i]];
                if (GDI.ObjectLifeTimes != null)
                {
                    for (int j = 0; j < GDI.ObjectLifeTimes.Count; j++)
                    {
                        if (GDI.ObjectLifeTimes[j].ProcessRaisedLifeTimeEvent(Args))
                        {
                            if (GDI.Data is IDisposable)
                            {
                                IDisposable ID = GDI.Data as IDisposable;
                                ID.Dispose();
                            }
                            _data.Remove(Keys[i]);
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            string[] keys = new string[_data.Keys.Count];
            _data.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                if (_data[keys[i]] is IDisposable)
                {
                    IDisposable ID = _data[keys[i]] as IDisposable;
                    ID.Dispose();
                }
            }
            _data.Clear();
        }
    }

    internal class GlobalDataItem
    {
        IList<ICacheObjectLifeTime> _objectLifeTimes;
        object _data;

        internal GlobalDataItem(object Data, IList<ICacheObjectLifeTime> ObjectLifeTimes)
        {
            _data = Data;
            _objectLifeTimes = ObjectLifeTimes;
        }

        internal IList<ICacheObjectLifeTime> ObjectLifeTimes
        {
            get
            {
                return _objectLifeTimes;
            }
        }

        internal object Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
    }
}
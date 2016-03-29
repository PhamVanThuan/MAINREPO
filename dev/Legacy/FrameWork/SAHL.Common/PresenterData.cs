using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common
{
    public class PresenterData
    {
        string _name;
        Dictionary<string, object> _data;

        public PresenterData(string Name)
        {
            _name = Name;
            _data = new Dictionary<string, object>();
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public Dictionary<string, object> Data
        {
            get
            {
                return _data;
            }
        }

        public void Clear()
        {
            _name = "";
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
}

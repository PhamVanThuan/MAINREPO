using System;
using System.Collections.Specialized;

namespace SAHL.Common.UserProfiles.Settings
{
    public class SearchHistorySetting : UserProfileSettingBase
    {
        StringCollection _history;
        int _maxHistory = 10;

        public SearchHistorySetting(string name)
            : base(name)
        {
            _history = new StringCollection();
        }

        public override void Load(string Data)
        {
            string[] Splits = Data.Split(new string[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);

            if (Splits.Length == 0)
                return;

            // get the max number of history elements
            _maxHistory = Convert.ToInt32(Splits[0].Trim());

            for (int i = 1; i < Splits.Length; i++)
            {
                if (Splits[i].Trim().Length > 0)
                    _history.Add(Splits[i].Trim());
            }
        }

        public override string Persist()
        {
            string Data = Delimit(MaxHistory.ToString());
            int Max = _history.Count < MaxHistory ? _history.Count : MaxHistory;

            for (int i = 0; i < Max; i++)
            {
                Data += Delimit(_history[i]);
            }

            return Data;
        }

        public int MaxHistory
        {
            get
            {
                return _maxHistory;
            }
            set
            {
                _maxHistory = value;
            }
        }

        public StringCollection History
        {
            get
            {
                return _history;
            }
        }

        private string Delimit(string Data)
        {
            return Data += Delimiter;
        }

        private string Delimiter
        {
            get
            {
                return "{|}";
            }
        }
    }
}
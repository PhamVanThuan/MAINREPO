using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.UI
{
    public class CBONodeSetCollection : Dictionary<string, CBONodeSet>
    {
        private string _currentNodeSet = CBONodeSet.CBONODESET;

        public string CurrentNodeSet
        {
            get
            {
                return _currentNodeSet;
            }
            set
            {
                _currentNodeSet = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2RebuildWorklistRequest : X2RequestBase
    {
        public List<ListRequestItem> ItemList;

        public X2RebuildWorklistRequest(List<ListRequestItem> ItemList)
            : base(false, null)
        {
            m_RequestType = RequestType.ProcessListRequest;
            this.ItemList = ItemList;
        }
    }
}
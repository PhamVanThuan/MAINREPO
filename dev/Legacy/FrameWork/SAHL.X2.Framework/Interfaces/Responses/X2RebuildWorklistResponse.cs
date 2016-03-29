using System;
using System.Collections.Generic;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2RebuildWorklistResponse : X2ResponseBase
    {
        public List<ListRequestItem> ItemList;

        public X2RebuildWorklistResponse(List<ListRequestItem> ItemList, string xml)
            : base(xml)
        {
            this.ItemList = ItemList;
        }
    }
}
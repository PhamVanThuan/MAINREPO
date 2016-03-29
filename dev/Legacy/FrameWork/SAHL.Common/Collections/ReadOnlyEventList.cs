using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.Collections
{
    public class ReadOnlyEventList<T1> : EventListBase<T1>, IReadOnlyEventList<T1>
    {
        public ReadOnlyEventList()
            : base()
        {
        }

        public ReadOnlyEventList(IEnumerable<T1> collection)
            : base(collection)
        {
        }
    }
}
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.V3.Framework.Specs.DomainObjects
{
    internal class StubReadOnlyEventList<T> : List<T>, IReadOnlyEventList<T>
    {
        public StubReadOnlyEventList()
        {
        }

        public StubReadOnlyEventList(IEnumerable<T> toCopy)
        {
            this.AddRange(toCopy);
        }
    }
}
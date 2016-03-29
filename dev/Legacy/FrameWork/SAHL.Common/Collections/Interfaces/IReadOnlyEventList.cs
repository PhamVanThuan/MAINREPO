using System.Collections;
using System.Collections.Generic;

namespace SAHL.Common.Collections.Interfaces
{
    public interface IReadOnlyEventList<T1> : IEnumerable<T1>, IEnumerable
    {
        bool Contains(T1 value);

        int IndexOf(T1 value);

        T1 this[int index] { get; set; }

        int Count { get; }
    }
}
using System.Collections.Generic;

namespace SAHL.Core.Metrics
{
    public interface ISample
    {
        int Count { get; }

        ICollection<long> Values { get; }

        void Clear();

        void Update(long value);
    }
}
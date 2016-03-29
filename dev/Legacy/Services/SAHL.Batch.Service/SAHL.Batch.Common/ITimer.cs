using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common
{
    public interface ITimer
    {
        void Start(int timeout, Action callback);

        void Reset();
    }
}

using System.Collections.Generic;

namespace SAHL.Core
{
    public interface IParticipatesInThreadLocalStorage
    {
        IDictionary<string, object> GetThreadLocalStore();

        void SetThreadLocalStore(IDictionary<string, object> threadContext);
    }
}
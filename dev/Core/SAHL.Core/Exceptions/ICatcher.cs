using System;

namespace SAHL.Core.Exceptions
{
    public interface ICatcher
    {
        void Silently(Action actionToSilentlyCatch);
    }
}
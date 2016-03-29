using System;

namespace SAHL.X2.Framework.Interfaces
{
    public interface IX2Info
    {
        string CurrentState { get; set; }

        string SessionID { get; set; }

        Int64 InstanceID { get; set; }

        string ActivityName { get; set; }
    }
}
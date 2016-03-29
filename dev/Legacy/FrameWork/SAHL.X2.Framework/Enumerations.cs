using System;

namespace SAHL.X2.Framework
{
    [Flags]
    public enum UIUserGroup : short
    {
        None = 1,
        Sales = 2,
        Admin = 4,
        Supervisor = 8,
        Manager = 16
    }

    public enum StandardPageState : short
    {
        View = 0,
        Create = 1,
        Update = 2,
        Delete = 4
    }
}
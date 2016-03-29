using System;

namespace SAHL.Core.SystemMessages
{
    [Serializable]
    public enum SystemMessageSeverityEnum
    {
        Warning,
        Error,
        Info,
        Exception,
        Debug
    }
}
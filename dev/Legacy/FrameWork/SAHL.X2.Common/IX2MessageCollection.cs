using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAHL.X2.Common
{
    public interface IX2MessageCollection : IList<IX2Message>
    {
        bool HasErrorMessages { get; }

        bool HasWarningMessages { get; }

        ReadOnlyCollection<IX2Message> ErrorMessages { get; }

        ReadOnlyCollection<IX2Message> WarningMessages { get; }
    }

    public interface IX2Message
    {
        string Message { get; }

        X2MessageType MessageType { get; }
    }

    public enum X2MessageType
    {
        Error,
        Warning
    }
}
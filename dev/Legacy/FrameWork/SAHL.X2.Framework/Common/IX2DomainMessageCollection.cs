using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace SAHL.X2.Framework.Common
{
    public interface IX2DomainMessageCollection : IList<IX2DomainMessage>
    {
        bool HasErrorMessages { get;}
        bool HasWarningMessages { get;}

        ReadOnlyCollection<IX2DomainMessage> ErrorMessages { get;}
        ReadOnlyCollection<IX2DomainMessage> WarningMessages { get;}
    }

    public interface IX2DomainMessage
    {
        string Message { get;}
        X2DomainMessageType MessageType { get;}
    }

    public enum X2DomainMessageType
    {
        Error,
        Warning
    }
}

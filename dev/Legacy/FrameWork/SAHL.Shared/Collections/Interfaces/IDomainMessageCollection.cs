using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAHL.Common.Collections.Interfaces
{
    public interface IDomainMessageCollection : IList<IDomainMessage>, IDisposable
    {
        bool HasErrorMessages { get; }

        bool HasWarningMessages { get; }

        bool HasInfoMessages { get; }

        ReadOnlyCollection<IDomainMessage> ErrorMessages { get; }

        ReadOnlyCollection<IDomainMessage> WarningMessages { get; }

        ReadOnlyCollection<IDomainMessage> InfoMessages { get; }
    }
}
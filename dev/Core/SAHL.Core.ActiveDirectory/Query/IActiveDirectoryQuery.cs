using System;
using System.DirectoryServices;

namespace SAHL.Core.ActiveDirectory.Query
{
    public interface IActiveDirectoryQuery : IDisposable
    {
        SearchResultCollection FindAll(params string[] propertiesToGet);
    }
}
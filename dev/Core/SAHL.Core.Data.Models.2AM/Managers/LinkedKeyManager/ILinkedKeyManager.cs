using System;

namespace SAHL.Core.Data.Models._2AM.Managers
{
    public interface ILinkedKeyManager
    {
        void DeleteLinkedKey(Guid guid);

        void LinkKeyToGuid(int key, Guid guid);

        int RetrieveLinkedKey(Guid guid);
    }
}
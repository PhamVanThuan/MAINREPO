using SAHL.Core.Data.Models._2AM.Managers;
using System;

namespace Managers
{
    public class LinkedKeyManager : ILinkedKeyManager
    {
        private ILinkedKeyDataManager linkedKeyDataManager;

        public LinkedKeyManager(ILinkedKeyDataManager linkedKeyDataManager)
        {
            this.linkedKeyDataManager = linkedKeyDataManager;
        }

        public void DeleteLinkedKey(Guid guid)
        {
            this.linkedKeyDataManager.DeleteLinkedKey(guid);
        }

        public void LinkKeyToGuid(int key, Guid guid)
        {
            this.linkedKeyDataManager.InsertLinkedKey(key, guid);
        }

        public int RetrieveLinkedKey(Guid guid)
        {
            return this.linkedKeyDataManager.RetrieveLinkedKey(guid);
        }
    }
}
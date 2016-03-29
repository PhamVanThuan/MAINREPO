using SAHL.Core.ActiveDirectory.Credentials;
using System.DirectoryServices;

namespace SAHL.Core.ActiveDirectory.Query
{
    public class ActiveDirectoryQuery : IActiveDirectoryQuery
    {
        private ICredentials Credentials { get; set; }

        private DirectoryEntry DirectoryEntry { get; set; }

        private DirectorySearcher DirectorySearcher { get; set; }

        public ActiveDirectoryQuery(ICredentials credentials, string filter)
        {
            Credentials = credentials;
            Initialise(null, null, filter);
        }

        public string Filter
        {
            get
            {
                return DirectorySearcher == null ? null : DirectorySearcher.Filter;
            }
        }

        private void Initialise(DirectoryEntry entry, DirectorySearcher searcher, string filter)
        {
            DirectoryEntry = entry ?? new DirectoryEntry();
            DirectorySearcher = searcher ?? new DirectorySearcher();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                DirectorySearcher.Filter = filter;
            }
        }

        public virtual SearchResultCollection FindAll(params string[] propertiesToGet)
        {
            propertiesToGet = propertiesToGet ?? new string[0];

            DirectorySearcher.PropertiesToLoad.Clear();
            foreach (var item in propertiesToGet)
            {
                DirectorySearcher.PropertiesToLoad.Add(item);
            }

            return DirectorySearcher.FindAll();
        }

        public virtual void Dispose()
        {
            if (DirectorySearcher != null)
            {
                DirectorySearcher.Dispose();
            }
            if (DirectoryEntry != null)
            {
                DirectoryEntry.Dispose();
            }
        }
    }
}
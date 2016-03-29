namespace SAHL.Core.ActiveDirectory.Query
{
    public static class SearchTerms
    {
        public const string SamAccountName = "(&(SAMAccountName={0}))";
        public const string DistinguishedName = "(&(distinguishedName={0}))";
    }
}
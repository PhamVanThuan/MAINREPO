namespace SAHL.Core.ActiveDirectory
{
    public static class UsernameHelpers
    {
        public static string RemoveDomainPrefixIfAny(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return username;
            }
            const string backslash = @"\";
            var indexOfBackslash = username.IndexOf(backslash);
            if (indexOfBackslash >= 0)
            {
                var indexToCopyFrom = indexOfBackslash + 1;
                return username.Substring(indexToCopyFrom);
            }
            return username;
        }
    }
}
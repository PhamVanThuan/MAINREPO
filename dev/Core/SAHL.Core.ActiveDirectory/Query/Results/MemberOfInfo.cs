using System;
using System.Security.Principal;

namespace SAHL.Core.ActiveDirectory.Query.Results
{
    public class MemberOfInfo : IMemberOfInfo
    {
        public SecurityIdentifier SecurityIdentifier { get; private set; }

        private const string CommonNameToken = "CN";

        public string CommonName { get; private set; }

        public string DistinguishedName { get; private set; }

        /// <summary>
        /// Creates the MemberOfInfo instance with the values provided via a DirectorySearch result string
        /// </summary>
        /// <param name="distinguishedName">A result string in the format "CN={0},OU={1},DC={2}" where {0}, {1} and {2} are the Common Name, Organisational Unit and Domain Controller respectively.
        ///  Note OU and DC are optional.</param>
        /// <param name="identifier">Optional. Supply the objectSid for the distinguishedName</param>
        /// <returns></returns>
        public MemberOfInfo(string distinguishedName, SecurityIdentifier identifier = null)
        {
            DistinguishedName = distinguishedName;
            SecurityIdentifier = identifier;
            ProcessResultString(distinguishedName);
        }

        private void ProcessResultString(string distinguishedName)
        {
            ProcessTokens(distinguishedName.Split(new[] { "=", "," }, StringSplitOptions.None));
        }

        private void ProcessTokens(string[] tokens)
        {
            for (var i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == CommonNameToken)
                {
                    CommonName = GetPairedValue(tokens, i);
                    return;
                }
            }
        }

        private static string GetPairedValue(string[] tokens, int indexOfKey)
        {
            var indexOfValue = indexOfKey + 1;
            return indexOfValue >= tokens.Length ? string.Empty : tokens[indexOfValue];
        }
    }
}
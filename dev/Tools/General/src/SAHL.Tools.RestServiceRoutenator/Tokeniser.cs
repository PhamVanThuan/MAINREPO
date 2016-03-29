using System.Linq;

namespace SAHL.Tools.RestServiceRoutenator
{
    internal class Tokeniser : ITokeniser
    {
        public string[] TokeniseStringForRest(string incomming)
        {
            return incomming.Split(new[] { '/' }).ToArray();
        }
    }
}
using System.Security.Principal;

namespace SAHL.Core.Web.Specs.CommandAuthorisersSpecs
{
    public class AuthenticatedIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get { return "FakeAuthType"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return "SomeGuy"; }
        }
    }
}
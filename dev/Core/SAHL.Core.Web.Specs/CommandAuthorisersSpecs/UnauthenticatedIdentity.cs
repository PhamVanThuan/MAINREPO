using System.Security.Principal;

namespace SAHL.Core.Web.Specs.CommandAuthorisersSpecs
{
    public class UnauthenticatedIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get { return "FakeAuthType"; }
        }

        public bool IsAuthenticated
        {
            get { return false; }
        }

        public string Name
        {
            get { return "SomeGuy"; }
        }
    }
}
using System.Security.Principal;

namespace SAHL.Core.ActiveDirectory.Query.Results
{
    public interface ISecurityIdentifierProperty : ICommonNameProperty
    {
        SecurityIdentifier SecurityIdentifier { get; }
    }
}
namespace Capitec.Core.Identity.Web
{
    public interface ISecurityModule
    {
        IAuthenticationManager AuthenticationManager { get; }
    }
}
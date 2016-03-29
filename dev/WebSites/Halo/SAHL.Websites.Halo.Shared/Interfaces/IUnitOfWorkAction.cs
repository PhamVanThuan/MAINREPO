using System.Security.Principal;

namespace SAHL.Websites.Halo.Shared
{
    public interface IUnitOfWorkAction
    {
        IPrincipal CurrentUser { get; set; }
        int Sequence { get; }

        bool Execute();
    }
}

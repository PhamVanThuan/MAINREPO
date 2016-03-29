using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ISecurityRepository
    {
        /// <summary>
        /// Gets an <see cref="IADUser"/> using a principal name.
        /// </summary>
        /// <param name="PrincipalName"></param>
        /// <returns></returns>
        IADUser GetADUserByPrincipal(string PrincipalName);

        /// <summary>
        /// Gets an <see cref="IADUser"/> using a principal.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        IADUser GetADUserByPrincipal(SAHLPrincipal principal);
    }
}
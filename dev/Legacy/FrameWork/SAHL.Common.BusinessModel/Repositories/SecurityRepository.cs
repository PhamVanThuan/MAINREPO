using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Security;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ISecurityRepository))]
    public class SecurityRepository : ISecurityRepository
    {
        /// <summary>
        /// Gets an <see cref="IADUser"/> using a principal name.
        /// </summary>
        /// <param name="PrincipalName"></param>
        /// <returns></returns>
        public IADUser GetADUserByPrincipal(string PrincipalName)
        {
            ADUser_DAO[] TheUsers = ADUser_DAO.FindAllByProperty("ADUserName", PrincipalName);

            ADUser_DAO TheUser = (TheUsers.Length == 1 ? TheUsers[0] : null);
            if (TheUser != null)
            {
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IADUser>(TheUser);
            }
            else
                return null;
        }

        /// <summary>
        /// Gets an <see cref="IADUser"/> using a principal.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public IADUser GetADUserByPrincipal(SAHLPrincipal principal)
        {
            return GetADUserByPrincipal(principal.Identity.Name);
        }

    }
}

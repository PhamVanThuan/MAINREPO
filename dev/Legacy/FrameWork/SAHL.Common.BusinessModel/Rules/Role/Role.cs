using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Role
{
    [RuleDBTag("ValidateUniqueAccountRole",
  "ValidateUniqueAccountRole prevents the same legalentity from being added to the account",
   "SAHL.Rules.DLL",
 "SAHL.Common.BusinessModel.Rules.Role.ValidateUniqueAccountRole")]
    [RuleInfo]
    public class ValidateUniqueAccountRole : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ValidateUniqueAccountRole rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IRole))
                throw new ArgumentException("The ValidateUniqueAccountRole rule expects the following objects to be passed: IRole.");

            // get the role
            IRole newAccountRole = Parameters[0] as IRole;
            IAccount account = newAccountRole.Account;

            bool roleExists = false;

            // check if this person already exists
            foreach (IRole existingRole in account.Roles)
            {
                if (existingRole.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                {
                    if (existingRole.LegalEntity.Key == newAccountRole.LegalEntity.Key)
                    {
                        roleExists = true;
                        break;
                    }
                }
            }

            if (roleExists == true)
            {
                AddMessage("This client already exists on this account.", "This client already exists on this account.", Messages);
                return 0;
            }

            return 1;

        }
    }


}

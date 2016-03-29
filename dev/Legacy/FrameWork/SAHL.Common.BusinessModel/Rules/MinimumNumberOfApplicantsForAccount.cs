using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;


namespace SAHL.Common.BusinessModel.Rules
{
    /// <summary>
    /// All Accounts must have a minimum number of applicants (configured in the DB default 1)
    /// Params:
    /// 0: AccountKey
    /// </summary>
    [RuleInfo]
    public class MinNumberApplicatsForAccount : BusinessRuleBase
    {
        #region IBusinessRule Members

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int AccountKey = Convert.ToInt32(Parameters[0]);
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccount Account = accRepo.GetAccountByKey(AccountKey);
            // find using the property on the rulesset (which is name)
            IEventList<IRuleParameter> RuleParams = RuleItem.RuleParameters;
            foreach (IRuleParameter Param in RuleParams)
            {
                if ("@NumApplicants" == Param.Name)
                {
                    // check we have the minium number of applicants for an account
                    if (Account.Roles.Count >= Convert.ToInt32(Param.Value))
                    {
                        // now check we have at least ONE Main Aplicant
                        IEventList<IRole> roles = Account.Roles;
                        foreach (IRole role in roles)
                        {
                            if (role.RoleType.Key == 2)// main applicant
                            {
                                // we have a main applicant so all is good.
                                return 1;
                            }
                        }
                        AddMessage("Must be at least one Main Applicant Role", "Bla", Messages);
                    }
                    else
                    {
                        AddMessage("Must be at least one applicant per account", "Bla", Messages);
                    }
                }
            }
            return 1;
        }

        #endregion
    }
}
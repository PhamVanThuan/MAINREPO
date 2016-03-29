using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Bond
{
    /// <summary>
    /// Bond Registration Amount must be between R 0.01 and R 999,999,999.99
    /// </summary>
    [RuleInfo]
    [RuleDBTag("BondRegistrationAmount",
    "Bond Registration Amount must be between R 0.01 and R 999,999,999.99",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Bond.BondRegistrationAmount")]
    public class BondRegistrationAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IBond))
                throw new ArgumentException("Parameter[0] is not of type IBond");

            IBond bond = (IBond)Parameters[0];

            if (bond.BondRegistrationAmount < 0.01 || bond.BondRegistrationAmount > 999999999.99)
                AddMessage("Bond Registration Amount must be between R 0.01 and R 999,999,999.99.", "Bond Registration Amount must be between R 0.01 and R 999,999,999.99.", Messages);

            return 0;
        }
    }

    /// <summary>
    /// Each Bond Registration Number can only exist once
    /// </summary>
    [RuleInfo]
    [RuleDBTag("BondRegistrationNumberUnique",
    "Each Bond Registration Number must be unique",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Bond.BondRegistrationNumberUnique")]
    public class BondRegistrationNumberUnique : BusinessRuleBase
    {
        public BondRegistrationNumberUnique(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IBond))
                throw new ArgumentException("Parameter[0] is not of type IBond");

            IBond bond = (IBond)Parameters[0];
            IBondRepository repo = RepositoryFactory.GetRepository<IBondRepository>();

            string sql = string.Format("select bondkey from [2AM].[dbo].[Bond] (nolock) where bondRegistrationNumber = '{0}' and deedsOfficeKey = {1}", bond.BondRegistrationNumber, bond.DeedsOffice.Key);

            ParameterCollection parameters = new ParameterCollection();

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);
            if (o != null)
            {
                int bondKey = Convert.ToInt32(o);
                if (bondKey != bond.Key)
                {
                    string msg = "This Bond Registration Number already exists at this deeds office.";
                    AddMessage(msg, msg, Messages);
                }
            }

            return 0;
        }
    }

    /// <summary>
    /// When updating the Bond, the Bond Registration Number must be entered
    /// </summary>
    [RuleDBTag("BondRegistrationNumberUpdateMandatory",
    "When updating the Bond, the Bond Registration Number must be entered",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Bond.BondRegistrationNumberUpdateMandatory")]
    [RuleInfo]
    public class BondRegistrationNumberUpdateMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IBond))
                throw new ArgumentException("Parameter[0] is not of type IBond");

            IBond bond = (IBond)Parameters[0];

            if (string.IsNullOrEmpty(bond.BondRegistrationNumber))
                AddMessage("The Bond Registration Number must be entered.", "The Bond Registration Number must be entered.", Messages);

            return 0;
        }
    }
}
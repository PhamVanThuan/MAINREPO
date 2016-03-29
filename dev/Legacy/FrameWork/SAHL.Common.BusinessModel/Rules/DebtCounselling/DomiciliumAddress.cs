using System;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Utils;
using SAHL.Common.DataAccess;
using System.Data.SqlClient;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.DAO;
using System.Data;
using System.Linq;
using SAHL.Common.BusinessModel.Helpers;

namespace SAHL.Common.BusinessModel.Rules.DebtCounselling
{
    [RuleInfo]
    [RuleDBTag("LegalEntitiesAllHaveDomiciliumOnDebtCounsellingCase",
    "Ensures that all clients on an debt counselling case have an active domicilium.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.LegalEntitiesAllHaveDomiciliumOnDebtCounsellingCase")]
    public class LegalEntitiesAllHaveDomiciliumOnDebtCounsellingCase : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntitiesAllHaveDomiciliumOnDebtCounsellingCase rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IDebtCounselling) )             
                throw new ArgumentException("The LegalEntitiesAllHaveDomiciliumOnDebtCounsellingCase rule expects the following objects to be passed: IDebtCounselling.");

            var debtCounselling = Parameters[0] as IDebtCounselling;

            var oneLegalEntityHasNoActiveDomicilium = debtCounselling.Clients.Any(x=>x.ActiveDomicilium==null);

            if (oneLegalEntityHasNoActiveDomicilium)
            {
                var message = "An Active Domicilium Address must be captured for all Clients on a Debt Counselling Case.";
                AddMessage(message, message, Messages);
                return 0;
            }

            return 1;
        }
    }
}

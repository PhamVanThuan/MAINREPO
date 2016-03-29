using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Utils;

namespace SAHL.Common.BusinessModel.Rules.EstateAgent
{
    [RuleDBTag("EstateAgentMultipleAgencies",
    "An Estate Agent can only be linked to one agency.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.EstateAgent.EstateAgentMultipleAgencies")]
    [RuleInfo]
    public class EstateAgentMultipleAgencies : BusinessRuleBase
    {
        public EstateAgentMultipleAgencies(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is ILegalEntity))
                throw new ArgumentException("Parameter[0] is not of type ILegalEntity.");

            ILegalEntity le = (ILegalEntity)Parameters[0];
            string errMsg = "";
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@LegalEntityKey", le.Key));

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "EstateAgentCheckForMultiples");
            object obj = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (obj != null)
            {
                int result = 0;
                Int32.TryParse(obj.ToString(), out result);
                if (result > 0)
                {
                    errMsg = String.Format("{0} already exists in the Estate Agent Channel and can not be added again.", le.DisplayName);
                    AddMessage(errMsg, errMsg, Messages);
                    return 1;
                }
            }

            return 0;
        }
    }
}
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Service.Interfaces;
using System;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Rules.Account
{
    [RuleDBTag("NaedoDebitOrderPending",
    "Alert the business user if a Naedo debit order is in the tracking period / pending",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Account.NaedoDebitOrderPending")]
    public class NaedoDebitOrderPending : BusinessRuleBase
    {
        public NaedoDebitOrderPending(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            if (parameters.Length == 0 ||
                !(parameters[0] is IAccount))
                throw new ArgumentException("The NaedoDebitOrderPending rule expects an Account.");

            var account = parameters[0] as IAccount;

            string sqlQuery = UIStatementRepository.GetStatement("Rule", "NaedoDebitOrderPending");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@accountKey", account.Key));

            object obj = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (obj != null
                && !String.IsNullOrEmpty(obj.ToString())
                && (int)obj > 0)
            {
                string errorMessage = "The NAEDO Debit Order for this account is in the tracking period.";
                AddMessage(errorMessage, errorMessage, messages);
                return 0;
            }

            return 1;
        }
    }
}
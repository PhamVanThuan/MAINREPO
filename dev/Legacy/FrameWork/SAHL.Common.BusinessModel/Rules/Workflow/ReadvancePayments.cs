using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using System.Data;

namespace SAHL.Common.BusinessModel.Rules.Workflow
{
    [RuleDBTag("Transaction140",
        "",
        "SAHL.Rules.DLL",
     "SAHL.Common.BusinessModel.Rules.Workflow.Transaction140")]
    [RuleInfo]
    public class Transaction140 : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("Parameters[0] is not of type IApplication.");
            IApplication app = Parameters[0] as IApplication;
            // Check that Tran 140 has been written in the past 24 hours.

            IDbConnection con = Helper.GetSQLDBConnection();
            try
            {
                string sqlQuery = UIStatementRepository.GetStatement("COMMON", "Transaction140");
                ParameterCollection prms = new ParameterCollection();
                Helper.AddIntParameter(prms, "@appKey", app.Key);

                object obj = Helper.ExecuteScalar(con, sqlQuery, prms);

                if (obj == null) //no transaction was found
                {
                    AddMessage("Transaction 140 has not been added in the past 24 hours.", "Transaction 140 has not been added in the past 24 hours.", Messages);
                    return 0;
                }
                return 1;
            }
            finally
            {
                if (con != null)
                    con.Dispose();
            }
        }
    }
}

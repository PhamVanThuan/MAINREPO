using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

using SAHL.Common.Globals;
using SAHL.Common;
using System.Data;
using SAHL.Common.DataAccess;
using System.Data.SqlClient;


namespace SAHL.Common.BusinessModel.Rules.Suretor
{

    [RuleDBTag("SuretorRemoveCheckConfirmedIncome",
       "Cannot remove a Suretor if its the only person linked to the account with an active confirmed income.",
       "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Suretor.SuretorRemoveCheckConfirmedIncome")]
    [RuleInfo]
    public class SuretorRemoveCheckConfirmedIncome : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The SuretorRemoveCheckConfirmedIncome rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IRole))
                throw new ArgumentException("The SuretorRemoveCheckConfirmedIncome rule expects the following objects to be passed: IRole.");


            IRole role = (IRole)Parameters[0];

            // if the roletypekey is not suretor then exit
            if (role.RoleType.Key != (int)Globals.RoleTypes.Suretor || role.GeneralStatus.Key != (int)Globals.GeneralStatuses.Inactive)
                return 1;

            // get a list of LegalEntities with ConfirmedIncome
            IList<int> legalEntitiesWithConfirmedIncome = new List<int>();
            using (IDbConnection conn = Helper.GetSQLDBConnection())
            {
                conn.Open();
                string query = UIStatementRepository.GetStatement("Rules.Suretor", "GetLegalEntitiesWithActiveConfirmedIncome");
                ParameterCollection parameters = new ParameterCollection();
                parameters.Add(new SqlParameter("@AccountKey", role.Account.Key));

                IDataReader reader = Helper.ExecuteReader(conn, query, parameters);

                while(reader.Read())
                {
                    legalEntitiesWithConfirmedIncome.Add(Convert.ToInt32(reader["LegalEntityKey"]));
                }
            }

            // validation fails if the query only returns 1 row and the legalentitykey matches that of the Suretor we are trying to remove
            if (legalEntitiesWithConfirmedIncome.Count == 1 && legalEntitiesWithConfirmedIncome[0] == role.LegalEntity.Key)
            {
                string msg = "Cannot remove suretor - this suretor has the only active confirmed income on the account.";
                AddMessage(msg,msg, Messages);
                return 0;
            }

            return 1; 
        }
    }








}
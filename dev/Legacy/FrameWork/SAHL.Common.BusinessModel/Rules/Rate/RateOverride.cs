using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.Globals;
using SAHL.Common.DataAccess;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.DAO;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Rules.Rate
{
    [RuleInfo]
    [RuleDBTag("RateOverrideCollectNoPaymentAdd",
    "?????",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Rate.RateOverrideCollectNoPaymentAdd")]
    public class RateOverrideCollectNoPaymentAdd : BusinessRuleBase
    {
        /// <summary>
        /// CollectNoPayment rate override can only be added to Accounts that are either:
        /// in the LossControl map with active StageTransition of type Debt Counselling 
        /// or of type QuickCash
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IRateOverride))
                throw new ArgumentException("Parameter[0] is not of type IRateOverride");

            IRateOverride ro = (IRateOverride)Parameters[0];
            // Only applies to RateOverrides of type CollectNoPaymentAdd
            if (ro.RateOverrideType.Key == (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.CollectNoPayment)
            {
                // Can be added to Quick Cash
                if (ro.FinancialService.Account.Product.Key != (int)SAHL.Common.Globals.Products.QuickCash)
                {
                    bool addrule = true;

                    // Can be added to account in Loss control map with active StageTransition of type Debt Counselling 
                    // Check to see if one exists and set addrule to false if found
                    SAHL.Common.BusinessModel.Interfaces.Repositories.IStageDefinitionRepository sdRepo = RepositoryFactory.GetRepository<SAHL.Common.BusinessModel.Interfaces.Repositories.IStageDefinitionRepository>();
                    IList<IStageTransition> stlist = sdRepo.GetStageTransitionsByGenericKey(ro.FinancialService.Account.Key);

                    if (stlist.Count > 0)
                    {
                        foreach (IStageTransition st in stlist)
                        {
                            if (st.StageDefinitionStageDefinitionGroup.StageDefinition.Key == (int)SAHL.Common.Globals.StageDefinitions.DebtCounselling)
                            {
                                Console.WriteLine("StageTransition with Definition of DebtCounselling not found.");
                                addrule = false;
                                break;
                            }
                        }
                    }

                    if (addrule)
                        AddMessage("CollectNoPayment rate override can only be applied to Quick Cash Accounts or Accounts under Debt Review.", "CollectNoPayment rate override can only be applied to Quick Cash Accounts or Accounts under Debt Review.", Messages);
                }
            }
            return 0;
        }
    }

    [RuleDBTag("RateOverrideStatusUpdate",
   "RateOverride Status Confirm",
   "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.Rate.RateOverrideStatusUpdate", false)]
    [RuleInfo]
    public class RateOverrideStatusUpdate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0 || !(Parameters[0] is IRateOverride))
                throw new Exception("Parameter[0] is not of type IRateOverride");

            IRateOverride ro = (IRateOverride)Parameters[0]; // New Object
            ILookupRepository _lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            IGeneralStatus InactiveStatus = _lookups.GeneralStatuses[GeneralStatuses.Inactive];

            if (ro.GeneralStatus.Key == InactiveStatus.Key)
            {

                AddMessage("A Rate Override cannot be set from Inactive to Active.  Do you want to create a new Rate Override?", "", Messages);
            }
            return 1;
        }
    }



  //  [RuleDBTag("RateOverrideCheckVariable",
  //  "Checks whether the loan is a variable loan",
  //  "SAHL.Rules.DLL",
  //"SAHL.Common.BusinessModel.Rules.Rate.RateOverrideCheckVariable")]
  //  [RuleInfo]
  //  public class RateOverrideCheckVariable : BusinessRuleBase
  //  {
  //      public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
  //      {
  //          int iLoanNumber = (int)Parameters[0];

  //          string sqlQuery = UIStatementRepository.GetStatement("Rules.RateOverrides", "CheckVariableLoan");
  //          ParameterCollection prms = new ParameterCollection();

  //          prms.Add(new SqlParameter("@LoanNumber", iLoanNumber));

  //          object obj = CommonRepository.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

  //          if (obj != null && (string)obj != "")
  //          {
  //              string errMsg = (string)obj;
  //              AddMessage(errMsg, errMsg, Messages);
  //          }

  //          return 0;
  //      }
  //  }
}

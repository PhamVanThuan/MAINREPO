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

namespace SAHL.Common.BusinessModel.Rules.FinancialAdjustments
{
    [RuleInfo]
    [RuleDBTag("FinancialAdjustmentCollectNoPaymentAdd",
    "?????",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FinancialAdjustments.FinancialAdjustmentCollectNoPaymentAdd")]
    public class FinancialAdjustmentCollectNoPaymentAdd : BusinessRuleBase
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
            if (!(Parameters[0] is IFinancialAdjustment))
                throw new ArgumentException("Parameter[0] is not of type IFinancialAdjustment");

            IFinancialAdjustment fa = (IFinancialAdjustment)Parameters[0];
            // Only applies to FinancialAdjustments of type CollectNoPaymentAdd
            if (fa.FinancialAdjustmentType.Key == (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.CollectNoPayment)
            {
                // Can be added to Quick Cash
                if (fa.FinancialService.Account.Product.Key != (int)SAHL.Common.Globals.Products.QuickCash)
                {
                    bool addrule = true;

                    // Can be added to account in Loss control map with active StageTransition of type Debt Counselling 
                    // Check to see if one exists and set addrule to false if found
                    SAHL.Common.BusinessModel.Interfaces.Repositories.IStageDefinitionRepository sdRepo = RepositoryFactory.GetRepository<SAHL.Common.BusinessModel.Interfaces.Repositories.IStageDefinitionRepository>();
                    IList<IStageTransition> stlist = sdRepo.GetStageTransitionsByGenericKey(fa.FinancialService.Account.Key);

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

    [RuleInfo]
    [RuleDBTag("FinancialAdjustmentPending",
    "There should only be one pending financial transaction",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FinancialAdjustments.FinancialAdjustmentPending")]
    public class FinancialAdjustmentPending : BusinessRuleBase
    {
        /// <summary>
        /// When calling OptIn and OptOut
        /// </summary>
        /// <param name="Messages"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IMortgageLoan))
                throw new ArgumentException("Parameter[0] is not of type IMortgageLoan");

            if (!(Parameters[1] is FinancialAdjustmentTypeSources))
                throw new ArgumentException("Parameter[1] is not of type FinancialAdjustmentTypeSources");

            Boolean alreadyPending = false;

            IMortgageLoan ml = (IMortgageLoan)Parameters[0];
            FinancialAdjustmentTypeSources fats = (FinancialAdjustmentTypeSources)Parameters[1];
            
            IFinancialAdjustment fadj = ml.GetPendingFinancialAdjustmentByTypeSource(fats);
            if  (fadj != null)
                alreadyPending = true;
                               
            
            if (alreadyPending)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("A Financial Adjustment of type {0} is pending with an effective date of {1}.", fadj.FinancialAdjustmentSource.Description,fadj.FromDate.Value.ToString(Constants.DateFormat));
                sb.AppendLine("</br>");
                sb.Append(" The financial adjustment will be set to active on the day following the effective date.");
                AddMessage(sb.ToString(), sb.ToString(), Messages);
                return 1;
            }
            
            return 0;
        }
    }


}

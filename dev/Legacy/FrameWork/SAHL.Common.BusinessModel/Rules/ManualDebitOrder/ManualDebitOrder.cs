using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.BusinessModel.Rules.ManualDebitOrder
{
    [RuleDBTag("ManualDebitOrderStartDayMinimum",
        "Effective Date must be greater than today, or if today's date it must be captured before 14h00",
        "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.ManualDebitOrder.ManualDebitOrderStartDayMinimum")]
    [RuleInfo]
    public class ManualDebitOrderStartDayMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IManualDebitOrder manDebitOrder = (IManualDebitOrder)Parameters[0];

            DateTime startDate = manDebitOrder.ActionDate;

            if ((startDate.Date == DateTime.Today && DateTime.Now.Hour > 13) ||
                (startDate < DateTime.Today))
            {

                string msg = "Effective Date must be greater than today, or if today's date it must be captured before 14h00";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ManualDebitOrderStartDayMaximum",
        "Effective Date cannot be greater further than 6 months in the future",
        "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.ManualDebitOrder.ManualDebitOrderStartDayMaximum")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@MaximumMonths,6,9" })]
    public class ManualDebitOrderStartDayMaximum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IManualDebitOrder manDebitOrder = (IManualDebitOrder)Parameters[0];
            int maxMonths = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            if (manDebitOrder.ActionDate > DateTime.Now.AddMonths(maxMonths))
            {
                string msg = String.Format("Effective Date cannot be more than {0} months from today's date", maxMonths);
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;

        }
    }

    [RuleDBTag("ManualDebitOrderUpdateExpired",
            "Recurring Transactions effective in the past cannot be updated",
            "SAHL.Rules.DLL",
          "SAHL.Common.BusinessModel.Rules.ManualDebitOrder.ManualDebitOrderUpdateExpired")]
    [RuleInfo]
    public class ManualDebitOrderUpdateExpired : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IManualDebitOrder manDebitOrder = (IManualDebitOrder)Parameters[0];
            IManualDebitOrder original = manDebitOrder.Original;

            // if the record is new or has no StartDate, ignore
            if (manDebitOrder.Key <= 0)  //|| !original.StartDate.HasValue)
                return 1;

            // if the original start date is today's date, we can only update up until 14h00
            if (original.ActionDate == DateTime.Today && DateTime.Now.Hour > 13)
            {
                string msg = "Recurring Transactions effective today cannot be updated after 14h00";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            // if the original start date is before today, fail
            if (original.ActionDate < DateTime.Today)
            {
                string msg = "Recurring Transactions effective in the past cannot be updated";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("ManualDebitOrderDeleteCheck",
            "Only Manual Debit Orders can be deleted.",
            "SAHL.Rules.DLL",
         "SAHL.Common.BusinessModel.Rules.ManualDebitOrder.ManualDebitOrderDeleteCheck")]
    [RuleInfo]
    public class ManualDebitOrderDeleteCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IManualDebitOrder manDebitOrder = (IManualDebitOrder)Parameters[0];

            if (manDebitOrder.TransactionType != null && manDebitOrder.TransactionType.Key != (int)TransactionTypes.ManualDebitOrderPayment)
            {
                string msg = "Only Manual Debit Orders can be deleted.";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("ManualDebitOrderBankAccountMandatory",
                "Bank Account is a mandatory field for certain transaction types.",
                "SAHL.Rules.DLL",
            "SAHL.Common.BusinessModel.Rules.ManualDebitOrder.ManualDebitOrderBankAccountMandatory")]
    [RuleInfo]
    public class ManualDebitOrderBankAccountMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IManualDebitOrder manDebitOrder = (IManualDebitOrder)Parameters[0];

            if (manDebitOrder.TransactionType != null && manDebitOrder.TransactionType.Key == (int)TransactionTypes.ManualDebitOrderPayment && manDebitOrder.BankAccount == null)
            {
                string msg = "Bank Account is a mandatory field for Manual Debit Order Payments.";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("ManualDebitOrderAmountMinimum",
                "Amount must be greater than 0.",
                "SAHL.Rules.DLL",
           "SAHL.Common.BusinessModel.Rules.ManualDebitOrder.ManualDebitOrderAmountMinimum")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@MinimumAmount,0,9" })]
    public class ManualDebitOrderAmountMinimum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IManualDebitOrder manDebitOrder = (IManualDebitOrder)Parameters[0];
            int min = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            if (manDebitOrder.Amount <= min)
            {
                string msg = String.Format("Amount must be greater than {0}.", min);
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("ManualDebitOrderDeleteDebitOrderCheckUser",
           "The user who is attempting to delete the manual debit order must = captured user (user who captured the record)",
            "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.ManualDebitOrder.ManualDebitOrderDeleteDebitOrderCheckUser", false)]
    [RuleInfo]
    public class ManualDebitOrderDeleteDebitOrderCheckUser : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ManualDebitOrderDeleteDebitOrderCheckUser rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IManualDebitOrder))
                throw new ArgumentException("The ManualDebitOrderDeleteDebitOrderCheckUser rule expects the following objects to be passed: IManualDebitOrder.");
            #endregion

            IManualDebitOrder manDebitOrder = (IManualDebitOrder)Parameters[0];
            string userName = (string)Parameters[1];

            if (String.Compare(manDebitOrder.UserID, userName, true) != 0)
            {
                string msg = String.Format("Only the User that captured the debit order is allowed to delete it. Would you like to create a delete request?");
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

	[RuleDBTag("ManualDebitOrderDeleteDebitOrderCheckExistingCase",
	"This rule checks that a delete request does not already exist for the Debit Order (ManualDebitOrders).",
    "SAHL.Rules.DLL", "SAHL.Common.BusinessModel.Rules.ManualDebitOrder.ManualDebitOrderDeleteDebitOrderCheckExistingCase", false)]
    [RuleInfo]
	public class ManualDebitOrderDeleteDebitOrderCheckExistingCase : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
				throw new ArgumentException("The ManualDebitOrderDeleteDebitOrderCheckExistingCase rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IManualDebitOrder))
				throw new ArgumentException("The ManualDebitOrderDeleteDebitOrderCheckExistingCase rule expects the following objects to be passed: IManualDebitOrder.");
            #endregion

            IManualDebitOrder manDebitOrder = (IManualDebitOrder)Parameters[0];
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
			IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
            DataTable deleteDebitOrder = x2Service.GetX2DataRowByFieldAndKey(Constants.WorkFlowDataTables.DeleteDebitOrder, Constants.WorkFlowCustomVariable.DeleteDebitOrder, manDebitOrder.Key);

			if (deleteDebitOrder.Rows.Count > 0)
			{
				foreach(DataRow row in deleteDebitOrder.Rows)
				{
					IInstance instance = x2Repo.GetInstanceByKey(long.Parse(row["InstanceID"].ToString()));
					if (instance.State.StateType.ID != (int)Common.Globals.X2StateTypes.Archive)
					{
						string msg = String.Format("A delete request already exist for this Debit Order.");
						AddMessage(msg, msg, Messages);
						return 0;
					}
				}
			}

            return 1;
        }
    }

    [RuleDBTag("ManualDebitOrderStartDateMaximumCheck",
    "The day of the Start Date cannot exceed the 28 days.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.ManualDebitOrder.ManualDebitOrderStartDateMaximumCheck")]
    [RuleInfo]
    public class ManualDebitOrderStartDateMaximumCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IManualDebitOrder manDebitOrder = (IManualDebitOrder)Parameters[0];

            //if (!rt.StartDate.HasValue)
            //    return 1;

            DateTime startDate = manDebitOrder.ActionDate;

            if (startDate.Date.Day > 28)
            {

                string msg = "The Effective Date cannot exceed the 28th day.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }
}

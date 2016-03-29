using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.QuickCash
{
    // [RuleDBTag("ApplicationInformationQuickCashApplicationExpenseValidate",
    // "ApplicationInformationQuickCashApplicationExpense object has validation errors",
    // "SAHL.Rules.DLL",
    //"SAHL.Common.BusinessModel.Rules.QuickCash.ApplicationInformationQuickCashApplicationExpenseValidate")]
    // [RuleInfo]
    // public class ApplicationInformationQuickCashApplicationExpenseValidate : BusinessRuleBase
    // {
    //     public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
    //     {
    //         IApplicationExpense appExpense = (IApplicationExpense)Parameters[0];

    //         if (appExpense.ExpenseType == null)
    //         {
    //             AddMessage("Expense Type must be selected", "", Messages);
    //             return 1;
    //         }
    //         if (appExpense.ExpenseType.Key == (int)ExpenseTypes.QuickCash || appExpense.ExpenseType.PaymentType.Key == (int)PaymentTypes.CashPaymentnointerest)
    //         {
    //             if (appExpense.ExpenseAccountName == null || appExpense.ExpenseAccountName.Length == 0)
    //                 AddMessage("Expense Account Name must be entered", "", Messages);

    //             if (appExpense.ExpenseAccountNumber == null || appExpense.ExpenseAccountNumber.Length == 0)
    //                 AddMessage("Expense Account Number must be entered", "", Messages);
    //             // For the Portion To client, the amount can be reduced to zero, if the entire amount is being paid to 3rd parties.
    //             if (appExpense.ExpenseType.Key != (int)ExpenseTypes.QuickCash && Convert.ToDouble(appExpense.TotalOutstandingAmount) <= 0)
    //                 AddMessage("Expense Amount must be greater than zero", "", Messages);
    //         }
    //         return 1;
    //     }
    // }

    [RuleDBTag("ApplicationInformationQuickCashDetailsValidate",
     "ApplicationInformationQuickCashDetails object has validation errors",
     "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.QuickCash.ApplicationInformationQuickCashDetailsValidate")]
    [RuleInfo]
    public class ApplicationInformationQuickCashDetailsValidate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplicationInformationQuickCashDetail appInfoQuickCash = (IApplicationInformationQuickCashDetail)Parameters[0];

            double paymentAmount = Convert.ToDouble(appInfoQuickCash.RequestedAmount);
            double sumOfAmtPerPayment = 0;

            if (Convert.ToBoolean(appInfoQuickCash.Disbursed))
            {
                AddMessage("This payment has been disbursed and may not be updated", "", Messages);
                return 1;
            }
            if (Convert.ToDouble(appInfoQuickCash.RequestedAmount) <= 0)
            {
                AddMessage("Amount must be greater than zero.", "", Messages);
                return 1;
            }


            #region ValidateExpenses
            for (int i = 0; i < appInfoQuickCash.ApplicationExpenses.Count; i++)
            {
                if (appInfoQuickCash.ApplicationExpenses[i].ExpenseType == null)
                {
                    AddMessage("Expense Type must be selected", "", Messages);
                    return 1;
                }

                if (appInfoQuickCash.QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.UpfrontPayment && appInfoQuickCash.ApplicationExpenses[i].ExpenseType.PaymentType.Key == (int)PaymentTypes.CashPaymentnointerest)
                {
                    AddMessage("Third Party payments may only be added to Regular Payments", "", Messages);
                    return 1;
                }

                if (appInfoQuickCash.ApplicationExpenses[i].ExpenseAccountName == null || appInfoQuickCash.ApplicationExpenses[i].ExpenseAccountName.Length == 0)
                    AddMessage("Expense Account Name must be entered", "", Messages);
                if (appInfoQuickCash.ApplicationExpenses[i].ExpenseAccountNumber == null || appInfoQuickCash.ApplicationExpenses[i].ExpenseAccountNumber.Length == 0)
                    AddMessage("Expense Account Number must be entered", "", Messages);
                if (appInfoQuickCash.ApplicationExpenses[i].ExpenseType.Key != (int)ExpenseTypes.QuickCash && Convert.ToDouble(appInfoQuickCash.ApplicationExpenses[i].TotalOutstandingAmount) <= 0)
                    AddMessage("Expense Amount must be greater than zero", "", Messages);

                sumOfAmtPerPayment += appInfoQuickCash.ApplicationExpenses[i].TotalOutstandingAmount;
            }
            #endregion

            if (paymentAmount < sumOfAmtPerPayment)
                AddMessage("The sum of the Third Party Payments exceed the Requested Amount", "", Messages);

            return 1;
        }
    }

    [RuleDBTag("ApplicationInformationQuickCashValidate",
    "ApplicationInformationQuickCash has validation errors",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.QuickCash.ApplicationInformationQuickCashValidate")]
    [RuleParameterTag(new string[] { "@QuickCashMinimumApprovedAmount,1000,7" })]
    [RuleInfo]
    public class ApplicationInformationQuickCashValidate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplicationInformationQuickCash applicationInfoQuickCash = (IApplicationInformationQuickCash)Parameters[0];

            double cashUpfrontApproved = 0;
            double totalQuickCashApproved = 0;

            double sumofCashUpfrontPayments = 0;
            double sumofAllPayments = 0;
            int numberOfNonDisbursedUpfrontPayments = 0;
            int numberofNonDisbursedRegularPayments = 0;

            Double minimumValue = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            if (applicationInfoQuickCash.ApplicationInformationQuickCashDetails != null && applicationInfoQuickCash.ApplicationInformationQuickCashDetails.Count > 0)
            {
                cashUpfrontApproved = Convert.ToDouble(applicationInfoQuickCash.CreditUpfrontApprovedAmount);
                totalQuickCashApproved = Convert.ToDouble(applicationInfoQuickCash.CreditApprovedAmount);

                for (int i = 0; i < applicationInfoQuickCash.ApplicationInformationQuickCashDetails.Count; i++)
                {
                    if (cashUpfrontApproved == 0 && applicationInfoQuickCash.ApplicationInformationQuickCashDetails[i].QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.UpfrontPayment)
                    {
                        AddMessage("Credit Cash Upfront approved amount is zero ! Upfront payment may not be added.", "", Messages);
                        break;
                    }

                    if (totalQuickCashApproved < minimumValue)
                    {
                        AddMessage("Credit Approved Amount is less than " + minimumValue.ToString(Constants.CurrencyFormat), "", Messages);
                        break;
                    }

                    sumofAllPayments += Convert.ToDouble(applicationInfoQuickCash.ApplicationInformationQuickCashDetails[i].RequestedAmount);

                    if (applicationInfoQuickCash.ApplicationInformationQuickCashDetails[i].QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.UpfrontPayment)
                        sumofCashUpfrontPayments += Convert.ToDouble(applicationInfoQuickCash.ApplicationInformationQuickCashDetails[i].RequestedAmount);

                    if (Convert.ToBoolean(applicationInfoQuickCash.ApplicationInformationQuickCashDetails[i].Disbursed) == false && applicationInfoQuickCash.ApplicationInformationQuickCashDetails[i].QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.UpfrontPayment)
                        numberOfNonDisbursedUpfrontPayments += 1;

                    if (Convert.ToBoolean(applicationInfoQuickCash.ApplicationInformationQuickCashDetails[i].Disbursed) == false && applicationInfoQuickCash.ApplicationInformationQuickCashDetails[i].QuickCashPaymentType.Key == (int)QuickCashPaymentTypes.RegularPayment)
                        numberofNonDisbursedRegularPayments += 1;
                }
            }

            if (numberOfNonDisbursedUpfrontPayments > 1)
            {
                AddMessage("You may not add more than one undisbursed Upfront Payment.", "", Messages);
                return 1;
            }

            if (numberofNonDisbursedRegularPayments > 1)
            {
                AddMessage("You may not add more than one undisbursed Regular Payment.", "", Messages);
                return 1;
            }

            if (cashUpfrontApproved < sumofCashUpfrontPayments)
            {
                AddMessage("The sum of all Cash Upfront payments exceeds the Credit Approved amount", "", Messages);
                return 1;
            }

            if (totalQuickCashApproved < sumofAllPayments)
            {
                AddMessage("The sum of all Quick Cash payments exceeds the Total Credit Approved amount", "", Messages);
                return 1;
            }

            if (cashUpfrontApproved > totalQuickCashApproved)
            {
                AddMessage("Credit Cash Upfront approved can not be greater than the Total Credit Approved amount.", "", Messages);
                return 1;
            }

            return 1;
        }
    }

    [RuleDBTag("ApplicationInformationQuickCashApplicationValidate",
     "ApplicationInformationQuickCash can not added",
     "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.QuickCash.ApplicationInformationQuickCashApplicationValidate")]
    [RuleInfo]
    public class ApplicationInformationQuickCashApplicationValidate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IApplicationInformationQuickCash applicationInfoQuickCash = (IApplicationInformationQuickCash)Parameters[0];

            ISupportsQuickCashApplicationInformation appInfoQuickCash = applicationInfoQuickCash.ApplicationInformation.Application as ISupportsQuickCashApplicationInformation;

            if (appInfoQuickCash == null)
            {
                AddMessage("Quick Cash may not be added to this application", "", Messages);
                return 1;
            }

            if (applicationInfoQuickCash.Key == 0 && applicationInfoQuickCash.ApplicationInformation.Application.Account.AccountStatus.Key == (int)AccountStatuses.Open)
            {
                AddMessage("The Mortgage Loan Account is Open - Quick Cash may not be added", "", Messages);
                return 1;
            }

            return 1;
        }
    }

}

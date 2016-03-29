using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common;
using SAHL.Common.Globals;
using System.Data;
using SAHL.Common.DataAccess;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;


namespace SAHL.Common.BusinessModel.Rules.Details
{
    /// <summary>
    /// Rule for the maximum amount for a detail.
    /// </summary>
    [RuleDBTag("DetailsMaximumAmount",
       "Ensures that if an amount is entered for details it is less than 999,999,999.99",
       "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Details.DetailsMaximumAmount")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@MaximumAmount,999999999.99,10" })]
    public class DetailsMaximumAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IDetail detail = (IDetail)Parameters[0];
            double maximumAmount = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            if (detail.Amount.HasValue && detail.Amount.Value >= maximumAmount)
            {
                string msg = String.Format("Detail amounts must be less than {0}", maximumAmount.ToString(Constants.CurrencyFormat));
                AddMessage(msg, msg, Messages);
            }

            return 1;
        }
    }

    /// <summary>
    /// Rule for the maximum amount for a detail.
    /// </summary>
    [RuleDBTag("DetailsMinimumAmount",
       "Ensures that if an amount is entered for details it is greater than 0.00",
       "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Details.DetailsMinimumAmount")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@MinimumAmount,0.00,10" })]
    public class DetailsMinimumAmount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IDetail detail = (IDetail)Parameters[0];
            double mimimumAmount = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            if (detail.Amount.HasValue && detail.Amount.Value <= mimimumAmount)
            {
                string msg = String.Format("Detail amounts must be greater than {0}", mimimumAmount.ToString(Constants.CurrencyFormat));
                AddMessage(msg, msg, Messages);
            }

            return 1;
        }
    }


    /// <summary>
    /// Rule for the mandatory date for detail.
    /// </summary>
    [RuleDBTag("DetailsMandatoryDate",
    "Ensures that detail date is not null",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Details.DetailsMandatoryDate")]
    [RuleInfo]
    public class DetailsMandatoryDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IDetail detail = Parameters[0] as IDetail;

            /*if (!detail.DetailDate.HasValue)
            {
                string msg = String.Format("Details Date must have a date selected");
                AddMessage(msg, msg, Messages);
            }
            */
            if (detail.DetailDate == new DateTime(1800, 1, 1))
            {
                string msg = String.Format("Detail Date must have a valid date selected");
                AddMessage(msg, msg, Messages);
            }

            return 1;
        }
    }

    /// <summary>
    /// Rule for prevented adding "Paid up no HOC" or "Paid up HOC" if client has positive loan balance
    /// </summary>
    [RuleDBTag("DetailsPositiveLoanBalanceHOC",
    "Ensures that detail date is not null",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Details.DetailsPositiveLoanBalanceHOC")]
    [RuleInfo]
    public class DetailsPositiveLoanBalanceHOC : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The DetailsPositiveLoanBalanceHOC rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IDetail))
                throw new ArgumentException("The DetailsPositiveLoanBalanceHOC rule expects the following objects to be passed: IDetail.");

            #endregion

            IDetail detail = Parameters[0] as IDetail;

            if (detail.DetailType.Key == (int)DetailTypes.PaidUpWithNoHOC || detail.DetailType.Key == (int)DetailTypes.PaidUpWithHOC)
            {
                IDbConnection con = Helper.GetSQLDBConnection();
                try
                {
                    decimal currBalance = 0;
                    string sqlQuery = UIStatementRepository.GetStatement("COMMON", "AccountCurrentBalance");
                    ParameterCollection prms = new ParameterCollection();
                    Helper.AddIntParameter(prms, "@accKey", detail.Account.Key);

                    object o = Helper.ExecuteScalar(con, sqlQuery, prms);

                    if (o != null)
                        currBalance = Math.Round(decimal.Parse(o.ToString(), System.Globalization.NumberStyles.Float));

                    if (currBalance > 0)
                    {
                        string errMsg = String.Format("Cannot load detail as client has a balance : {0}", currBalance.ToString(SAHL.Common.Constants.CurrencyFormat));
                        AddMessage(errMsg, errMsg, Messages);
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
            return 1;
        }
    }

    [RuleDBTag("DetailNonPerformingLoanLitigation",
    "restrictionsd for adding or updating loan details when loan is non-performing - prevents Litigation to close unless suspended interest has been zeroed",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Details.DetailNonPerformingLoanLitigation")]
    [RuleInfo]
    public class DetailNonPerformingLoanLitigation : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The DetailNonPerformingLoanLitigation rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IDetail))
                throw new ArgumentException("The DetailNonPerformingLoanLitigation rule expects the following objects to be passed: IDetail.");
            #endregion

            IDetail detail = Parameters[0] as IDetail;
            IFinancialServiceRepository financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();

            if (financialServiceRepo.IsLoanNonPerforming(detail.Account.Key) && detail.DetailType.Key == (int)DetailTypes.LoanClosed)
            {
                DateTime? dt;
                Decimal amount = financialServiceRepo.GetSuspendedInterest(detail.Account.Key, out dt);
                if (amount > 0)
                {
                    string msg = "This account is currently a non-performing loan. A Loan Closed detail type can only be added once the suspended interest has been zeroed. Please refer this account to Litigation";
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DetailCannotBeClosedWithCurrentBalanceNotZero",
    "A loan should never be allowed to have a detail type of 10 added to it if the current balance is not ZERO. Therefore a loan can only be closed when its current balance is ZERO.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Details.DetailCannotBeClosedWithCurrentBalanceNotZero")]
    [RuleInfo]
    public class DetailCannotBeClosedWithCurrentBalanceNotZero : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The DetailCannotBeClosedWithCurrentBalanceNotZero rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IDetail))
                throw new ArgumentException("The DetailCannotBeClosedWithCurrentBalanceNotZero rule expects the following objects to be passed: IDetail.");
            #endregion

            IDetail detail = Parameters[0] as IDetail;

            //If the Detail type is not of type Loan Closed, this rule should not be run
            if (detail.DetailType.Key != (int)DetailTypes.LoanClosed)
            {
                return 1;
            }

            IDbConnection con = Helper.GetSQLDBConnection();
            try
            {
                int hasBalance = 0;
                string sqlQuery = UIStatementRepository.GetStatement("COMMON", "DetailCannotBeClosedWithCurrentBalance");
                ParameterCollection prms = new ParameterCollection();
                Helper.AddIntParameter(prms, "@Accountkey", detail.Account.Key);

                object o = Helper.ExecuteScalar(con, sqlQuery, prms);

                if (o != null)
                    hasBalance = (int)o;

                if (hasBalance > 0)
                {
                    string errMsg = String.Format("Cannot load detail as client has a balance ");
                    AddMessage(errMsg, errMsg, Messages);
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


    [RuleDBTag("CheckNonPerformingLoanByDetailType",
"Prevent current non-performing loans from being set to performing by specific detail types",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Details.CheckNonPerformingLoanByDetailType")]
    [RuleInfo]
    public class CheckNonPerformingLoanByDetailType : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The CheckNonPerformingLoanByDetailType rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IDetail))
                throw new ArgumentException("The CheckNonPerformingLoanByDetailType rule expects the following objects to be passed: IDetail.");
            #endregion


            var detail = Parameters[0] as IDetail;

            var financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();

            if (detail != null)
                if (financialServiceRepo.IsLoanNonPerforming(detail.Account.Key) &&
                    (
                        detail.DetailType.Key == (int)DetailTypes.DeceasedEstateInsolvent ||
                        detail.DetailType.Key == (int)DetailTypes.UnderCancellation ||
                        detail.DetailType.Key == (int)DetailTypes.SequestrationOrLiquidation ||
                        detail.DetailType.Key == (int)DetailTypes.UnderCancellationguaranteeRecieved ||
                        detail.DetailType.Key == (int)DetailTypes.AuctionSold ||
                        detail.DetailType.Key == (int)DetailTypes.CancellationLodged ||
                        detail.DetailType.Key == (int)DetailTypes.DeseasedEstatesLitigationVelile ||
                        detail.DetailType.Key == (int)DetailTypes.DeseasedEstatesLitigationRandles ||
                        detail.DetailType.Key == (int)DetailTypes.DeseasedEstatesLitigationManoghMaharaj ||
                        detail.DetailType.Key == (int)DetailTypes.DeseasedEstatesLitigationHaroldGie ||
                        detail.DetailType.Key == (int)DetailTypes.DeseasedEstatesLitigationStraussDaly
                    )
                    )
                {
                    const string msg = "This account has detail types that prevent it from being set as performing again. Please refer this account to Litigation";
                    AddMessage(msg, msg, Messages);
                    return 0;
                }

            return 1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Mandates.Credit
{
    [MandateInfo()]
    public class LoanAmountLessThanXValue : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("LoanAmountLessThanXValue expects and IApplicationMortgageLoan");

            double ThreshHoldValue = Convert.ToDouble(Mandate.ParameterValue);
            double LoanAmount = app.LoanAgreementAmount.Value;
            // Further lending types
            if (app.ApplicationType.Key == 2 || app.ApplicationType.Key == 3 || app.ApplicationType.Key == 4)
            {
                // go get the existing loan amount and add to the total value
                LoanAmount += ((IMortgageLoanAccount)app.Account).LoanCurrentBalance;
            }
            if (LoanAmount < ThreshHoldValue)
            {
                return true;
            }
            return false;
        }
    }

    [MandateInfo()]
    public class LoanAmountGreaterThanOrEqualXValue : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("LoanAmountGreaterThanOrEqualXValue expects and IApplicationMortgageLoan");

            double ThreshHoldValue = Convert.ToDouble(Mandate.ParameterValue);
            double LoanAmount = app.LoanAgreementAmount.Value;
            // Further lending types
            if (app.ApplicationType.Key == 2 || app.ApplicationType.Key == 3 || app.ApplicationType.Key == 4)
            {
                // go get the existing loan amount and add to the total value
                LoanAmount += ((IMortgageLoanAccount)app.Account).LoanCurrentBalance;
            }
            if (LoanAmount >= ThreshHoldValue)
            {
                return true;
            }
            return false;
        }
    }

    [MandateInfo()]
    public class LoanAmountBetweenXValue : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("LoanAmountBetweenXValue expects and IApplicationMortgageLoan");

            string s = Mandate.ParameterValue;
            string[] ss = s.Split(',');
            double Max = Convert.ToDouble(ss[1]);
            double Min = Convert.ToDouble(ss[0]);
            //double ThreshHoldValue = Convert.ToDouble(Mandate.ParameterValue);
            double LoanAmount = app.LoanAgreementAmount.Value;
            // Further lending types
            if (app.ApplicationType.Key == 2 || app.ApplicationType.Key == 3 || app.ApplicationType.Key == 4)
            {
                // go get the existing loan amount and add to the total value
                LoanAmount += ((IMortgageLoanAccount)app.Account).LoanCurrentBalance;
            }

            if (LoanAmount >= Min && LoanAmount <= Max)
            {
                return true;
            }
            return false;
        }
    }

    [MandateInfo()]
    public class SelfEmployed : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("SelfEmployed expects and IApplicationMortgageLoan");
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            if (null != supportsVariableLoanApplicationInformation)
            {
                if (supportsVariableLoanApplicationInformation.VariableLoanInformation.EmploymentType.Key == 2)
                    return true;
            }

            return false;
        }
    }

    [MandateInfo()]
    public class EmployedOrSubsidy : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("EmployedOrSubsidy expects and IApplicationMortgageLoan");
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            if (null != supportsVariableLoanApplicationInformation)
            {
                if (supportsVariableLoanApplicationInformation.VariableLoanInformation.EmploymentType.Key == 1 || supportsVariableLoanApplicationInformation.VariableLoanInformation.EmploymentType.Key == 3)
                    return true;
            }
            return false;
        }
    }

    [MandateInfo()]
    public class NewPurchase : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("NewPurchse expects and IApplicationMortgageLoan");
            if (app.ApplicationType.Key == 7)
                return true;
            return false;
        }
    }

    [MandateInfo()]
    public class Switch : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("Switch expects and IApplicationMortgageLoan");
            if (app.ApplicationType.Key == 6)
                return true;
            return false;

        }
    }

    [MandateInfo()]
    public class Refinance : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("Refinance expects and IApplicationMortgageLoan");
            if (app.ApplicationType.Key == 8)
                return true;
            return false;

        }
    }

    [MandateInfo()]
    public class FurtherLoan : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("FurtherLoan expects and IApplicationMortgageLoan");
            if (app.ApplicationType.Key == 4)
                return true;
            return false;

        }
    }

    [MandateInfo()]
    public class ReAdvance : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("ReAdvance expects and IApplicationMortgageLoan");
            if (app.ApplicationType.Key == 2 || app.ApplicationType.Key == 3)
                return true;
            return false;

        }
    }

    [MandateInfo()]
    public class StaffLoan : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("StaffLoan expects and IApplicationMortgageLoan");
            foreach (ApplicationAttribute attr in app.ApplicationAttributes)
            {
                if (attr.ApplicationAttributeType.Key == (int)OfferAttributeTypes.StaffHomeLoan)
                {
                    return false;
                }
            }
            if (null != app.Account)
            {
                foreach (IFinancialService fs in app.Account.FinancialServices)
                {
					if (fs.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan || fs.FinancialServiceType.Key == (int)FinancialServiceTypes.FixedLoan)
                    {
                        foreach (IFinancialAdjustment financialAdjustment in fs.FinancialAdjustments)
                        {
                            if (financialAdjustment.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Active && 
								financialAdjustment.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.SAHLStaff)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}

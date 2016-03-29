using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.BusinessModel.Rules.Products
{
    [RuleDBTag("ApplicationProductEdgeTerm",
       "Ensures that the loan term is 276 months",
       "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.Products.ApplicationProductEdgeTerm")]
    [RuleInfo]
    public class ApplicationProductEdgeTerm : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationProductEdgeTerm rule expects a Domain object to be passed.");

            IApplicationMortgageLoan mortgageLoan = Parameters[0] as IApplicationMortgageLoan;
            if (mortgageLoan == null)
                throw new ArgumentException("The ApplicationProductEdgeTerm rule expects the following objects to be passed: IApplicationMortgageLoan.");

            #endregion

            IControlRepository controlRepository = RepositoryFactory.GetRepository<IControlRepository>();
            int edgeLoanTerm = Convert.ToInt32(controlRepository.GetControlByDescription("Edge Max Term").ControlNumeric);

            IApplicationProductEdge appProductEdge = (IApplicationProductEdge)mortgageLoan.CurrentProduct;

            if (appProductEdge.Term > edgeLoanTerm)
            {
                string msg = String.Format("The term used for calculating affordability of an Edge is limited to {0} months", edgeLoanTerm);
                AddMessage(msg, msg, Messages);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationProductEdgeLTV",
    "Ensures that the ltv is <= 80%",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.ApplicationProductEdgeLTV")]
    [RuleParameterTag(new string[] { "@MaxApplicationProductEdgeLTV,0.80,10"})]
    [RuleInfo]
    public class ApplicationProductEdgeLTV : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The ApplicationProductEdgeLTV rule expects a Domain object to be passed.");

            if (Parameters[0] is IApplicationUnknown)//this is a lead and should not throw an exception
                return 0; 

            IApplicationMortgageLoan mortgageLoan = Parameters[0] as IApplicationMortgageLoan;
            if (mortgageLoan == null)
                throw new ArgumentException("The ApplicationProductEdgeLTV rule expects the following objects to be passed: IApplicationMortgageLoan.");

            #endregion

            double ltv = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            IApplicationProductEdge appProductEdge = mortgageLoan.CurrentProduct as IApplicationProductEdge;

            if (appProductEdge != null
                && appProductEdge.VariableLoanInformation.LTV.HasValue
                && appProductEdge.VariableLoanInformation.LTV.Value > ltv)
            {
                string msg = String.Format("Edge applications require an LTV of < {0}%", Math.Round(ltv*100));
                AddMessage(msg, msg, Messages);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("MaxEdgeLoanAgreementAmountInBlueBanner",
    "Let the user know that when the Loan Agreement Amount is above 1.5 Million ZAR, SPV is Blue Banner and LTV is => 80%",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Products.MaxEdgeLoanAgreementAmountInBlueBanner")]
    [RuleParameterTag(new string[] { "@MaxEdgeLAAInBBValue,1500000,10", "@MaxEdgeLTVInBBValue,0.8,9" })]
    [RuleInfo]
    public class MaxEdgeLoanAgreementAmountInBlueBanner : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The MaxEdgeLoanAgreementAmountInBlueBanner rule expects a Domain object to be passed.");

            IApplication application = Parameters[0] as IApplication;

            if (application == null)
                throw new ArgumentException("The MaxEdgeLoanAgreementAmountInBlueBanner rule expects the following objects to be passed: IApplication.");

            #endregion

            double laa = Convert.ToDouble(RuleItem.RuleParameters[0].Value);
            double ltv = Convert.ToDouble(RuleItem.RuleParameters[1].Value);

            if ((int)application.CurrentProduct.ProductType == (int)SAHL.Common.Globals.Products.Edge)
            {
                ISupportsVariableLoanApplicationInformation vlai = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (vlai.VariableLoanInformation.LoanAgreementAmount > laa && vlai.VariableLoanInformation.LTV >= ltv)
                {
                    string msg = String.Format("A maximum loan amount of {0} is allowed on {1} products where LTV is greater than or equal to {2}. ", laa.ToString(Constants.CurrencyFormat),SAHL.Common.Globals.Products.Edge.ToString(), ltv.ToString(Constants.RateFormat));
                    AddMessage(msg, msg, Messages);
                    return 1;
                }
            }

            return 0;
        }
    }
}

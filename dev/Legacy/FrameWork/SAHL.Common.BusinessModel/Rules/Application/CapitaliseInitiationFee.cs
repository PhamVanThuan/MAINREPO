using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Rules.Application
{
    [RuleDBTag("CapitaliseInitiationFeeLTV",
    "LTV max when capitalising initiation fees is 100%",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV")]
    [RuleParameterTag(new string[] { "@MaxLTV,1.0,7" })]
    public class CapitaliseInitiationFeeLTV : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            if (parameters.Length == 0 ||
                !(parameters[0] is IApplication))
                throw new ArgumentException("The CapitaliseInitiationFeeLTV rule expects an Application.");

            var application = parameters[0] as IApplication;
            if (application is IApplicationFurtherLending //ignore further lending
                || !application.HasAttribute(OfferAttributeTypes.CapitaliseInitiationFee)) //rule only applies if the attribute exists
                return 1;

            double maxLTV = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            ISupportsVariableLoanApplicationInformation svlai = application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            if (svlai.VariableLoanInformation.LTV.Value > maxLTV)
            {
                string errorMessage = String.Format("LTV can not be greater than {0}% when the initiation fee is capitalised.", (maxLTV * 100).ToString());
                AddMessage(errorMessage, errorMessage, messages);
                return 0;
            }

            return 1;
        }
    }
}

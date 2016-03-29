using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.ApplicationDeclaration
{

    [RuleDBTag("ApplicationDeclarationCurrentDebtRearrangement",
    "ApplicationDeclarationCurrentDebtRearrangement rules: Check if the Application's roles has CurrentDebtRearrangement declaration.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ApplicationDeclaration.ApplicationDeclarationCurrentDebtRearrangement")]
    public class ApplicationDeclarationCurrentDebtRearrangement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("Parameters[0] is not of type IApplication.");

            IApplication application = Parameters[0] as IApplication;

            foreach (IApplicationRole _appRole in application.ApplicationRoles)
            {
                if (((_appRole.GeneralStatus.Key == (int)GeneralStatuses.Active) && 
                    (_appRole.ApplicationDeclarations != null && _appRole.ApplicationDeclarations.Count > 0)) &&
                    (_appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant
                    || _appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant
                    || _appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor
                    || _appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor))
                {
                    foreach (IApplicationDeclaration appDeclaration in _appRole.ApplicationDeclarations)
                    {
                        if (appDeclaration.ApplicationDeclarationQuestion.Key == (int)OfferDeclarationQuestions.CurrentDebtRearrangement)
                        {
                            if (appDeclaration.ApplicationDeclarationAnswer.Key == (int)OfferDeclarationAnswers.Yes)
                            {
                                string err = string.Format("Legal Entity {0} has application declaration {1} selected",_appRole.LegalEntity.DisplayName, OfferDeclarationQuestions.CurrentDebtRearrangement.ToString());
                                AddMessage(err, err, Messages);
                                return 0;
                            }
                        }
                    }
                }
            }
            return 1;
        }
     }
}

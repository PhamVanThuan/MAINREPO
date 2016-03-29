using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Common.BusinessModel.Rules.Application
{
    [RuleDBTag("ApplicationIsAlphaHousing",
    "Determines whether an application is alpha housing.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.Application.ApplicationIsAlphaHousing")]
    public class ApplicationIsAlphaHousing : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            var application = parameters[0] as IApplication;
            if (application.HasAttribute(OfferAttributeTypes.AlphaHousing))
            {
                string errorMessage = "Application Qualifies as an Alpha Housing Loan";
                AddMessage(errorMessage, errorMessage, messages);
            }

            return 1;
        }
    }

    [RuleDBTag("EnsureAllLegalEntitiesOnApplicationHasDomicilium",
   "Ensures that all legal entities on an application has a domicilium.",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Application.EnsureAllLegalEntitiesOnApplicationHasDomicilium")]
    public class EnsureAllLegalEntitiesOnApplicationHasDomicilium : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            var application = parameters[0] as IApplication;
            if (application is IApplicationFurtherLending)
                return 0;

            var oneLegalEntityOnApplicationDoesNotHaveDomicilium = application.ApplicationRoles.Where(x => x.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client
                && x.ApplicationRoleType.Key != (int)SAHL.Common.Globals.OfferRoleTypes.AssuredLife).Any(x => x.ApplicationRoleDomicilium == null);
            if (oneLegalEntityOnApplicationDoesNotHaveDomicilium)
            {
                var message = "A Domicilium Address must be captured for all Applicants on a New Business application";
                AddMessage(message, message, messages);
            }
            return 0;
        }
    }

    [RuleDBTag("ApplicationIsReturningClient",
    "Determines whether an application is returning client.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.ApplicationIsReturningClient")]
    public class ApplicationIsReturningClient : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            var application = parameters[0] as IApplication;
            if (application.HasAttribute(OfferAttributeTypes.DiscountedInitiationFeeReturningClient))
            {
                string errorMessage = "Returning Client - Application qualifies for discounted Initiation Fee";
                AddMessage(errorMessage, errorMessage, messages);
            }

            return 1;
        }
    }

    [RuleDBTag("ApplicationCannotHave30YearTerm",
    "Determines whether an application has a 30 year term.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.ApplicationCannotHave30YearTerm")]
    public class ApplicationCannotHave30YearTerm : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            if (parameters.Length == 0 ||
                !(parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationCannotHave30YearTerm rule expects an Application.");

            IApplication application = parameters[0] as IApplication;

            if (application.HasAttribute(OfferAttributeTypes.Loanwith30YearTerm) == true)
            {
                string errorMessage = "Application is already a 30 year term";
                AddMessage(errorMessage, errorMessage, messages);
                return 0;
            }

            return 1;
        }
    }
    [RuleDBTag("ApplicationHasPrevious30YearTermDisqualification",
    "Determines whether an application has a previous 30 year term disqualification.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.ApplicationHasPrevious30YearTermDisqualification")]
    public class ApplicationHasPrevious30YearTermDisqualification : BusinessRuleBase
    {
        private readonly IReasonRepository reasonRepository;
        public ApplicationHasPrevious30YearTermDisqualification(IReasonRepository reasonRepository)
        {
            this.reasonRepository = reasonRepository;
        }
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            if (parameters.Length == 0 ||
                !(parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationHasPrevious30YearTermDisqualification rule expects an Application.");

            IApplication application = parameters[0] as IApplication;

            if (!application.HasAttribute(OfferAttributeTypes.CreditDisqualified30YearTerm)) //not a current disqualification
            {
                var offerinformationKeyList = application.ApplicationInformations.Select(x => x.Key).ToList();
                var disqualifyReasons = reasonRepository.GetReasonByGenericKeyListAndReasonTypeKey(offerinformationKeyList, (int)ReasonTypes.CreditDisqualify30YearTerm);
                if (disqualifyReasons.Count() > 0)
                {
                    string errorMessage = "Application has a previous 30 year term disqualification";
                    AddMessage(errorMessage, errorMessage, messages);
                    return 0;
                }
            }

            return 1;
        }
    }

    [RuleDBTag("ApplicationHas30YearTermDisqualification",
    "Determines whether an application has a 30 year term disqualification.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.ApplicationHas30YearTermDisqualification")]
    public class ApplicationHas30YearTermDisqualification : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            if (parameters.Length == 0 ||
                !(parameters[0] is IApplication))
                throw new ArgumentException("The ApplicationHas30YearTermDisqualification rule expects an Application.");

            IApplication application = parameters[0] as IApplication;

            if (application.HasAttribute(OfferAttributeTypes.CreditDisqualified30YearTerm))
            {
                string errorMessage = "Application has a 30 year term disqualification";
                AddMessage(errorMessage, errorMessage, messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ApplicationIsNewBusiness",
    "Fails when an application is not a new business application.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.ApplicationIsNewBusiness")]
    public class ApplicationIsNewBusiness : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            var application = parameters[0] as IApplication;

            if (application.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan ||
                application.ApplicationType.Key == (int)OfferTypes.RefinanceLoan ||
                application.ApplicationType.Key == (int)OfferTypes.SwitchLoan)
            {
                return 1;
            }

            string errorMessage = "This application is not a new business application.";
            AddMessage(errorMessage, errorMessage, messages);
            return 0;
        }
    }

    [RuleDBTag("ApplicationHas30YearTerm",
    "Confirms whether an application can be converted to a 20 year Term.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Application.ApplicationHas30YearTerm")]
    public class ApplicationHas30YearTerm : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection messages, params object[] parameters)
        {
            var application = parameters[0] as IApplication;

            if (!application.HasAttribute(OfferAttributeTypes.Loanwith30YearTerm))
            {
                string errorMessage = "This application does not have a 30 year (360 month) term.";
                AddMessage(errorMessage, errorMessage, messages);
                return 0;
            }

            return 1;
        }
    }
}
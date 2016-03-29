using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantsMustBeBetween18And65YearsOld
{
    public class when_the_applicant_is_66_years_old : WithCoreFakes
    {
        private static ApplicantsMustBeBetween18And65YearsOldRule rule;
        private static AddLeadApplicantToApplicationCommand command;
        private static IApplicantDataManager applicantDataManager;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            applicantDataManager = An<IApplicantDataManager>();
            validationUtils = An<IValidationUtils>();
            applicantDataManager.WhenToldTo(x => x.GetClientDateOfBirth(Param.IsAny<int>())).Return(Param.IsAny<DateTime>());
            validationUtils.WhenToldTo(x => x.GetAgeFromDateOfBirth(Param.IsAny<DateTime>())).Return(66);
            rule = new ApplicantsMustBeBetween18And65YearsOldRule(applicantDataManager, validationUtils);
            command = new AddLeadApplicantToApplicationCommand(CombGuid.Instance.Generate(), 12345, 12345, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("An applicant on a mortgage loan application cannot be older than 65 years of age.");
        };
    }
}
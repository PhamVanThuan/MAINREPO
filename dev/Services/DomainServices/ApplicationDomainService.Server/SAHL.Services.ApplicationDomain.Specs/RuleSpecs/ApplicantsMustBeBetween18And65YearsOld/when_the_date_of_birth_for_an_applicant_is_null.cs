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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantsMustBeBetween18And65YearsOld
{
    public class when_the_date_of_birth_for_an_applicant_is_null : WithCoreFakes
    {
        private static ApplicantsMustBeBetween18And65YearsOldRule rule;
        private static AddLeadApplicantToApplicationCommand command;
        private static IApplicantDataManager applicantDataManager;
        private static IValidationUtils validationUtils;

        Establish context = () =>
        {
            applicantDataManager = An<IApplicantDataManager>();
            validationUtils = An<IValidationUtils>();
            messages = SystemMessageCollection.Empty();
            applicantDataManager.WhenToldTo(x => x.GetClientDateOfBirth(Arg.Any<int>())).Return((DateTime?)null);
            rule = new ApplicantsMustBeBetween18And65YearsOldRule(applicantDataManager, validationUtils);
            command = new AddLeadApplicantToApplicationCommand(CombGuid.Instance.Generate(), 12345, 12345, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldNotBeEmpty();
        };

        It should_not_try_determine_the_applicants_age = () =>
        {
            validationUtils.WasNotToldTo(x => x.GetAgeFromDateOfBirth(Arg.Any<DateTime>()));
        };
    }
}

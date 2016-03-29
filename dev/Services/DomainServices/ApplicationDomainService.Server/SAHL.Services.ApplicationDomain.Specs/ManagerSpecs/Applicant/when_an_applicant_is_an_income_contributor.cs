using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Applicant
{
    public class when_an_applicant_is_an_income_contributor : WithFakes
    {
        private static ApplicantManager applicantManager;
        private static IApplicantDataManager applicantDataManager;
        private static int applicantRoleKey;
        private static IEnumerable<OfferRoleAttributeDataModel> attributes;
        private static bool result;

        private Establish context = () =>
        {
            attributes = new OfferRoleAttributeDataModel[] { 
                new OfferRoleAttributeDataModel(12345, applicantRoleKey, (int)OfferRoleAttributeType.ReturningClient),
                new OfferRoleAttributeDataModel(12346, applicantRoleKey, (int)OfferRoleAttributeType.IncomeContributor)
            };
            applicantRoleKey = 111115;
            applicantDataManager = An<IApplicantDataManager>();
            applicantManager = new ApplicantManager(applicantDataManager);
            applicantDataManager.WhenToldTo(x => x.GetApplicantAttributes(applicantRoleKey)).Return(attributes);
        };

        private Because of = () =>
        {
            result = applicantManager.IsApplicantAnIncomeContributor(applicantRoleKey);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}

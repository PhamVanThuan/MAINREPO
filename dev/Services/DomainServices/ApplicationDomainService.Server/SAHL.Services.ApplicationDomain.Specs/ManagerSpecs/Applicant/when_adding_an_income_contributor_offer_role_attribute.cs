using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Applicant
{
    public class when_adding_an_income_contributor_offer_role_attribute : WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static IApplicantManager applicantManager;
        private static IReadWriteSqlRepository readWriteRepo;

        private static int offerRoleKey = 7;

        private Establish context = () =>
        {
            applicantDataManager = An<IApplicantDataManager>();
            readWriteRepo = MockRepositoryProvider.GetReadWriteRepository();

            applicantManager = new ApplicantManager(applicantDataManager);
        };

        private Because of = () =>
        {
            applicantManager.AddIncomeContributorOfferRoleAttribute(offerRoleKey);

        };

        private It should_insert_an_offer_attribute = () =>
        {
            applicantDataManager.WasToldTo(x => x.AddOfferRoleAttribute(Arg.Is<OfferRoleAttributeDataModel>(m => m.OfferRoleKey == offerRoleKey && 
                m.OfferRoleAttributeTypeKey == (int)OfferRoleAttributeType.IncomeContributor)));
        };
    }
}

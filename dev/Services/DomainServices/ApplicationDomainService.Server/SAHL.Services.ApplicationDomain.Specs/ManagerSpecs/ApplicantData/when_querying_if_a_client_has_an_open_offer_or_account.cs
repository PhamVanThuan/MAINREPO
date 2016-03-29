using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Applicant.Statements;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.ApplicantData
{
    public class when_querying_if_a_client_has_an_open_offer_or_account: WithFakes
    {
        private static IApplicantDataManager applicantDataManager;
        private static FakeDbFactory fakedDb;
        private Establish context = () =>
        {
            fakedDb = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(fakedDb);
        };

        private Because of = () =>
        {
            applicantDataManager.ApplicantHasCurrentSAHLBusiness(Param.IsAny<int>());
            
        };

        private It should_ = () =>
        {
            fakedDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne<bool>(Param.IsAny<ClientHasOpenAccountOrOfferStatement>()));
        };
    }
}

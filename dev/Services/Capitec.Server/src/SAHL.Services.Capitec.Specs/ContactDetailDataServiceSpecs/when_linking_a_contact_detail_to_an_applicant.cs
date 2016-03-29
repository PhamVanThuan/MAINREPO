using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.ContactDetail;
using SAHL.Services.Capitec.Managers.Lookup;
using System;

namespace SAHL.Services.Capitec.Specs.ContactDetailDataServiceSpecs
{
    public class when_linking_a_contact_detail_to_an_applicant : WithFakes
    {
        private static FakeDbFactory dbFactory;
        private static ILookupManager lookupService;
        private static ContactDetailDataManager service;
        private static Guid applicantId, contactDetailId;

        private Establish context = () =>
            {
                applicantId = Guid.NewGuid();
                contactDetailId = Guid.NewGuid();
                lookupService = An<ILookupManager>();
                dbFactory = new FakeDbFactory();
                service = new ContactDetailDataManager(dbFactory, lookupService);
            };

        private Because of = () =>
            {
                service.LinkContactDetailToApplicant(applicantId, contactDetailId);
            };

        private It should_create_an_applicant_contact_detail_record_using_parameters_provided = () =>
            {
                dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ApplicantContactDetailDataModel>(
                    f => f.ApplicantId == applicantId
                    && f.ContactDetailId == contactDetailId
                    )));
            };
    }
}
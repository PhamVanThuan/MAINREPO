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
using System.Linq;

namespace SAHL.Services.Capitec.Specs.ContactDetailDataServiceSpecs
{
    public class when_adding_an_email_address_contact_detail : WithFakes
    {
        private static FakeDbFactory dbFactory;
        private static ILookupManager lookupService;
        private static ContactDetailDataManager service;
        private static Guid contactDetailId, emailAddressContactDetailTypeEnumId, contactDetailTypeEnumId;
        private static string emailAddress;

        private Establish context = () =>
            {
                emailAddress = "clintons@sahomeloans.com";
                contactDetailId = Guid.NewGuid();
                emailAddressContactDetailTypeEnumId = Guid.Parse(EmailAddressContactDetailTypeEnumDataModel.HOME);
                contactDetailTypeEnumId = Guid.Parse(ContactDetailTypeEnumDataModel.EMAIL);
                lookupService = An<ILookupManager>();
                dbFactory = new FakeDbFactory();
                service = new ContactDetailDataManager(dbFactory, lookupService);
            };

        private Because of = () =>
            {
                service.AddEmailAddressContactDetail(contactDetailId, emailAddressContactDetailTypeEnumId, emailAddress);
            };

        private It should_create_a_contact_detail_record_using_the_parameters_provided = () =>
            {
                dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ContactDetailDataModel>(
                    f => f.Id == contactDetailId
                    && f.ContactDetailTypeEnumId == contactDetailTypeEnumId
                    )));
            };

        private It should_create_an_email_address_record_using_the_emailAddress_provided = () =>
            {
                dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<EmailAddressContactDetailDataModel>(
                    f => f.Id == contactDetailId
                    && f.EmailAddressContactDetailTypeEnumId == emailAddressContactDetailTypeEnumId
                    && f.EmailAddress == emailAddress
                    )));
            };
    }
}
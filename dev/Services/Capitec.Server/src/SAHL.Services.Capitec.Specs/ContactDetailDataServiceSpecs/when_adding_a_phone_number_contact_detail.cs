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
    public class when_adding_a_phone_number_contact_detail : WithFakes
    {
        private static ContactDetailDataManager service;
        private static ILookupManager lookupservice;
        private static Guid contactDetailId, phoneNumberContactDetailTypeEnumId;
        private static string code, number;
        private static ContactDetailDataModel contactDetailModel;
        private static PhoneNumberContactDetailDataModel phoneNumberContactDetailModel;
        private static Guid contactDetailTypeEnumId;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            code = "031";
            number = "5735000";
            contactDetailId = Guid.NewGuid();
            phoneNumberContactDetailTypeEnumId = Guid.Parse(PhoneNumberContactDetailTypeEnumDataModel.HOME);
            contactDetailTypeEnumId = Guid.Parse(ContactDetailTypeEnumDataModel.PHONE);
            contactDetailModel = new ContactDetailDataModel(contactDetailId, contactDetailTypeEnumId);
            phoneNumberContactDetailModel = new PhoneNumberContactDetailDataModel(contactDetailId, code, number);
            lookupservice = An<ILookupManager>();
            dbFactory = new FakeDbFactory();
            service = new ContactDetailDataManager(dbFactory, lookupservice);
        };

        private Because of = () =>
        {
            service.AddPhoneNumberContactDetail(contactDetailId, phoneNumberContactDetailTypeEnumId, code, number);
        };

        private It should_insert_a_new_contact_detail_record = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ContactDetailDataModel>(
                f => f.Id == contactDetailId
                && f.ContactDetailTypeEnumId == contactDetailTypeEnumId
                )));
        };

        private It should_insert_a_new_phone_number = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<PhoneNumberContactDetailDataModel>(
                f => f.PhoneCode == code
                && f.PhoneNumber == number
                && f.PhoneNumberContactDetailTypeEnumId == phoneNumberContactDetailTypeEnumId
                )));
        };
    }
}
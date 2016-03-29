using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Applicant;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantDataServiceSpecs
{
    public class when_adding_an_applicant_address : WithFakes
    {
        private static ApplicantDataManager service;
        private static FakeDbFactory dbFactory;
        private static Guid addressId;
        private static Guid applicantId;
        private static Guid addressType;

        Establish context = () =>
        {   
            addressId = Guid.NewGuid();
            applicantId = Guid.NewGuid();
            addressType = Guid.Parse(AddressTypeEnumDataModel.RESIDENTIAL);
            dbFactory = new FakeDbFactory();
            service = new ApplicantDataManager(dbFactory);
        };

        Because of = () =>
        {
            service.AddAddressToApplicant(addressId, applicantId, addressType);
        };

        It should_insert_an_applicantAddress_using_the_parameters_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<ApplicantAddressDataModel>(y => y.ApplicantId == applicantId && y.AddressId == addressId && y.AddressTypeEnumId == addressType)));
        };
    }
}
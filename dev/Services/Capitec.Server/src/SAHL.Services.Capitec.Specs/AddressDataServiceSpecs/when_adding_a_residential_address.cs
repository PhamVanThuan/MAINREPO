using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Address;
using System;

namespace SAHL.Services.Capitec.Specs.AddressDataServiceSpecs
{
    public class when_adding_a_residential_address : WithFakes
    {
        private static AddressDataManager service;
        private static FakeDbFactory dbFactory;
        private static IReadOnlySqlRepository readOnlyRepo;
        private static AddressDataModel model;
        private static AddressFormatEnumDataModel addressFormat;
        private static Guid suburbId, addressId, addressFormatId;

        Establish context = () =>
        {
            suburbId = Guid.NewGuid();
            addressId = Guid.NewGuid();
            addressFormatId = Guid.NewGuid();
            addressFormat = new AddressFormatEnumDataModel(addressFormatId, "street", true, 1);
            model = new AddressDataModel(addressId, addressFormat.Id, null, "5", "", "Camberwell", "21", "Somerset Drive", suburbId, null, null, null, null, null, null, null);
            dbFactory = new FakeDbFactory();
            service = new AddressDataManager(dbFactory);
        };

        Because of = () =>
        {
            service.AddResidentialAddress(addressId, model.UnitNumber, model.BuildingNumber, model.BuildingName, model.StreetNumber, model.StreetName, "", suburbId, "", "");
        };

        It should_insert_an_address_with_the_params_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert<AddressDataModel>(Arg.Is<AddressDataModel>(y => y.Id == addressId)));
        };
    }
}
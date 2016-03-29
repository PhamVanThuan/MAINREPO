using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Managers.Client.Statements;
using System;

namespace SAHL.Services.ClientDomain.Specs.Managers.ClientDataManagerSpecs
{
    public class when_a_client_cannot_be_found_by_passport_number : WithFakes
    {
        private static ClientDataManager clientDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static LegalEntityDataModel existingClient;
        private static LegalEntityDataModel result;
        private static string passportNumber;

        private Establish context = () =>
        {
            passportNumber = "1010212514";
            existingClient = null;
            fakeDbFactory = new FakeDbFactory();
            clientDataManager = new ClientDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(
                x => x.SelectOne(Param.IsAny<FindClientByPassportNumberStatement>())).Return(existingClient);
        };

        private Because of = () =>
        {
            result = clientDataManager.FindExistingClientByPassportNumber(passportNumber);
        };

        private It should_return_null= () =>
        {
            result.ShouldBeNull();
        };

        private It should_use_the_ID_number_in_the_statement = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(
                x => x.SelectOne(Arg.Is<FindClientByPassportNumberStatement>(y => y.PassportNumber == passportNumber)));
        };
    }
}
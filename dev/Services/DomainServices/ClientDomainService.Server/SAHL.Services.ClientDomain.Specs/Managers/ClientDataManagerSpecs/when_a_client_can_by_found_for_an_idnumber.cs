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
    public class when_a_client_can_by_found_for_an_idnumber : WithFakes
    {
        private static ClientDataManager clientDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static LegalEntityDataModel existingClient;
        private static LegalEntityDataModel result;
        private static string idNumber;

        private Establish context = () =>
        {
            idNumber = "8211045229080";
            existingClient = new LegalEntityDataModel(1, null, null, null, null, DateTime.Now, null, 
                "Clint", "BB", "Speed", "", "8211045229080", "", "", "", "", "", null, "", "", 
                "", "", "", "", "", "", "", null, null, "", null, "", null, null, null, 1, null);
            fakeDbFactory = new FakeDbFactory();
            clientDataManager = new ClientDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(
             x => x.SelectOne
              (Param.IsAny<FindClientByIdNumberStatement>()))
                .Return(existingClient);
        };

        private Because of = () =>
        {
            result = clientDataManager.FindExistingClientByIdNumber(idNumber);
        };

        private It should_return_the_client = () =>
        {
            result.ShouldEqual(existingClient);
        };

        private It should_use_the_ID_number_in_the_statement = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(
              x => x.SelectOne(Arg.Is<FindClientByIdNumberStatement>
               (y => y.IdentityNumber == idNumber)));
        };
    }
}
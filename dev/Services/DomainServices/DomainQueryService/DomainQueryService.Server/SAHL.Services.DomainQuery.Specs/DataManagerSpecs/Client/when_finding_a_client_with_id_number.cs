using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.DomainQuery.Managers.Client;
using SAHL.Services.DomainQuery.Managers.Client.Statements;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.DomainQuery.Specs.DataManagerSpecs.Client
{
    public class when_finding_a_client_with_id_number : WithCoreFakes
    {
        private static FakeDbFactory     fakeDbFactory;
        private static ClientDataManager clientDataManager;

        private Establish context = () =>
        {
            fakeDbFactory     = new FakeDbFactory();

            var legalEntity = new LegalEntityDataModel(1, LegalEntityType.NaturalPerson, MaritalStatus.Single, Gender.Male, PopulationGroup.Unknown, DateTime.Now,
                                                       SalutationType.Mr);

            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<LegalEntityDataModel>(Param.IsAny<FindClientByIdNumberStatement>())).Returns((LegalEntityDataModel)LegalEntityType.NaturalPerson);

            clientDataManager = new ClientDataManager(fakeDbFactory);

        };

        private Because of = () =>
        {

        };

        private It should_return_existing_client = () =>
        {

        };
    }
}

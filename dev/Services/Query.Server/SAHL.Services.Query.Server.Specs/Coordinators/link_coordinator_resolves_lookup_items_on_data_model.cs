using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Services.Query.LinkCoordinator;
using SAHL.Services.Query.Server.Specs.Factory;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Specs.LinkCoordinators
{
    public class datamodel_coordinator_resolves_relationships_on_data_model : WithFakes
    {

        public static DataModelCoordinator DataModelCoordinator;
        public static Guid attorneyId;

        Establish that = () =>
        {
            DataModelCoordinator = new DataModelCoordinator();
            attorneyId = new Guid();
        };
        
        private Because of = () =>
        {
            DataModelCoordinator.ResolveDataModelRelationships(AttorneyRepresentationTestDataFactory.GetAttorneyDetailDataModel(attorneyId));
        };

        private It should = () =>
        {

        };

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Query.Controllers.Test;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Server.Tests.DataManagers.Statements;
using SAHL.Services.Query.Server.Tests.Models;

namespace SAHL.Services.Query.Server.Tests.MetaData
{
    [TestFixture]
    public class DataManagerCollectionTests
    {

        [Test]
        public void GetDataManager_GivenRepresentationDataManagerCollection_ShouldReturnNewDataManager()
        {
            
            //arrange
            var collection = CreateDataManagerCollection();

            //action
            IQueryServiceDataManager manager = collection.Get(typeof (TestRepresentation));

            //assert
            Assert.IsNotNull(manager);

        }

        private IDataManagerCollection CreateDataManagerCollection()
        {
            IDbFactory dbFactory = Substitute.For<IDbFactory>();
            IDataModelCoordinator dataModelCoordinator = Substitute.For<IDataModelCoordinator>();
            var result = new Dictionary<Type, Type>();

            var type = typeof (QueryServiceDataManager<,>);
            var queryServiceDataModel = type.MakeGenericType(typeof (TestDataModel), typeof (GetTestStatement));
            result.Add(typeof (TestRepresentation), queryServiceDataModel);

            IDataManagerCollection collection = new DataManagerCollection(result, dbFactory, dataModelCoordinator);
            return collection;
        }
    }
}

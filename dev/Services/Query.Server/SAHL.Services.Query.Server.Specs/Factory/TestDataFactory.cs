using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Server.Specs.Coordinators;
using SAHL.Services.Query.Server.Specs.Fakes;

namespace SAHL.Services.Query.Server.Specs.Factory
{
    public static class TestDataFactory
    {
        public static List<TestDataModel> GetTestDataModels()
        {
            return new List<TestDataModel>()
            {
                GetTestDataModel(2)
            };
        }
        public static TestDataModel GetTestDataModel(int id)
        {
            return new TestDataModel()
            {
                Id = id,
                Description = "Test Data",
                Relationships = new List<IRelationshipDefinition>
                {
           
                }

            };
        }
 
    }
}
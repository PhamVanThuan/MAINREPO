using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using NSubstitute;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_creating_a_property
    {

        private static TestDataManager testDataManager;
        private static PropertyDataModel property;
        private static int propertyKey;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
          {
              fakeDb = new FakeDbFactory();
              testDataManager = new TestDataManager(fakeDb);
              property = new PropertyDataModel((int)PropertyType.House, (int)TitleType.Freehold, 1, (int)OccupancyType.OwnerOccupied, (int)AddressType.Residential,"", "", "", 
                                              (int)DeedsPropertyType.Unit, DateTime.Now, "", "", "", "", (int)ResidenceStatus.Permanent, "", "", (int)DataProvider.SAHL);
              propertyKey = property.PropertyKey;
          };

        private Because of = () =>
         {
             testDataManager.InsertProperty(property);
         };

        private It should_create_the_correct_property = () =>
         {
              fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.Insert(Arg.Is<PropertyDataModel>(y => y.PropertyKey == property.PropertyKey)));
         };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };

    }
}

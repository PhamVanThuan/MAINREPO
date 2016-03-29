using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.BusinessModel.Enums;
using NSubstitute;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_a_property
    {
        private static int propertyKey;
        private static PropertyDataModel property;
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
        {
            propertyKey = 123;
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            property = new PropertyDataModel(propertyKey,(int)PropertyType.House,12345,6789,(int)AddressType.Residential,"propertyDescritpion1","propertDescription2","PropertyDescription3", 
                999, DateTime.Now, "erfNumber","erfProtionNum", "sectionalScheme", "998", (int)DeedsPropertyType.Erf, "","",(int)DataProvider.RCS);
        };

        private Because of = () =>
        {
            testDataManager.UpdateProperty(property);
        };

        private It should_update_the_third_correct_property_with_the_correct_address = () =>
        {
            fakeDb.FakedDb.InAppContext().
               WasToldTo(x => x.Update(Arg.Is<PropertyDataModel>(y => y.AddressKey == property.AddressKey)));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}

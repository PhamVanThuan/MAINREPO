using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_a_valuator
    {
        private static string valuatorKey;
        private static ValuatorDataModel valuator;
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
        {
            valuatorKey = 123+"";
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            valuator = new ValuatorDataModel(valuatorKey, "password", 0 ,(int)GeneralStatus.Active, 1235888);
        };

        private Because of = () =>
        {
            testDataManager.UpdateValuator(valuator);
        };

        private It should_update_the_correct_valuator = () =>
        {
            fakeDb.FakedDb.InAppContext().
               WasToldTo(x => x.Update(Arg.Is<ValuatorDataModel>(y => y.ValuatorKey == valuator.ValuatorKey
                   && valuator.ValuatorPassword == y.ValuatorPassword)));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

    }
}

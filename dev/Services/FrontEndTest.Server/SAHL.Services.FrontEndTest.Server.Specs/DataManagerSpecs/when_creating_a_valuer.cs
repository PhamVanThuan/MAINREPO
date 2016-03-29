using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.FrontEndTest.Managers;
using Machine.Fakes;
using System;
using SAHL.Core.Testing.Fakes;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_creating_a_valuer
    {
        private static TestDataManager testDataManager;
        private static ValuatorDataModel valuator;
        private static byte limitedUserGroupKey;
        private static string valuatorContact;
        private static int legalEntityKey;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
          {
              fakeDb = new FakeDbFactory();
              valuatorContact = "VishavP";
              limitedUserGroupKey = 123;
              legalEntityKey = 1235888;
              valuator = new ValuatorDataModel(valuatorContact,"!@#$%^&*()",limitedUserGroupKey,(int)GeneralStatus.Active,legalEntityKey);
              testDataManager = new TestDataManager(fakeDb);
          };

        private Because of = () =>
         {
             testDataManager.InsertValuer(valuator);
         };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };

        private It should_insert_the_valuer = () =>
         {
             fakeDb.FakedDb.InAppContext().
                 WasToldTo(x => x.Insert(Arg.Is<ValuatorDataModel>(y => y.LimitedUserGroup == limitedUserGroupKey
                  && y.ValuatorContact == valuatorContact && y.LegalEntityKey == legalEntityKey)));
         };
    }
}

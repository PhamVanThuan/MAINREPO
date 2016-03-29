using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Statements;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_adding_application_attribute : WithCoreFakes
    {
        private static FakeDbFactory dbFactory;
        private static IFinancialDataManager financialDataManager;
        private static int applicationNumber;
        private static int offerAttributeTypeKey;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);
            applicationNumber = 1;
            offerAttributeTypeKey = 77;
        };

        private Because of = () =>
        {
            financialDataManager.AddApplicationAttribute(applicationNumber, offerAttributeTypeKey);
        };

        private It should_insert_the_application_attribute = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert<OfferAttributeDataModel>(Arg.Is<OfferAttributeDataModel>(y => y.OfferKey == applicationNumber
                                                                                                                                && y.OfferAttributeTypeKey == offerAttributeTypeKey)));
        };


    }
}

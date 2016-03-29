using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Models;
using SAHL.Services.FinancialDomain.Managers.Statements;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_checking_if_application_has_attribute : WithCoreFakes
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
            offerAttributeTypeKey = 99;
        };

        private Because of = () =>
        {
            financialDataManager.ApplicationHasAttribute(applicationNumber, offerAttributeTypeKey);
        };

        private It should_query_to_check_if_the_application_has_the_attribute = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.Select<OfferAttributeDataModel>(Arg.Is<GetApplicationAttributeByAttributeTypeStatement>(y => y.ApplicationNumber == applicationNumber
                                                                                                                                                           && y.OfferAttributeTypeKey == offerAttributeTypeKey)));
        };



    }
}

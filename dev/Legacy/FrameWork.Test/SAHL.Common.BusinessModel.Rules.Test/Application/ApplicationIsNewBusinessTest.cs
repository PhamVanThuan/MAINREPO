using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.Application;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Rules.Test.Application
{
    [TestFixture]
    public class ApplicationIsNewBusinessTest : RuleBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test, TestCaseSource("GetNewBusinessOfferTypes")]
        public void ApplicationNewBusiness_Pass(OfferTypes offerType)
        {
            RunApplicationIsNewBusinessRule(offerType, 0);
        }

        [Test, TestCaseSource("GetNonNewBusinessOfferTypes")]
        public void ApplicationNotNewPurchase_Fail(OfferTypes offerType)
        {
            RunApplicationIsNewBusinessRule(offerType, 1);
        }

        [Test]
        public void ApplicationNewPurchase_Pass()
        {
            RunApplicationIsNewBusinessRule(OfferTypes.NewPurchaseLoan, 0);
        }

        private void RunApplicationIsNewBusinessRule(OfferTypes offerType, int expectedMessageCount)
        {
            IApplication application = GetMockedApplicationType(offerType);

            ApplicationIsNewBusiness rule = new ApplicationIsNewBusiness();

            ExecuteRule(rule, expectedMessageCount, application);
        }

        private IApplication GetMockedApplicationType(OfferTypes offerType)
        {
            IApplication application = _mockery.StrictMock<IApplication>();
            IApplicationType applicationType = _mockery.StrictMock<IApplicationType>();

            SetupResult.For(applicationType.Key).Return((int)offerType);
            SetupResult.For(application.ApplicationType).Return(applicationType);
            
            return application;
        }

        private IEnumerable<OfferTypes> GetNewBusinessOfferTypes()
        {
            return new List<OfferTypes> {OfferTypes.NewPurchaseLoan, OfferTypes.RefinanceLoan, OfferTypes.SwitchLoan};
        }

        private IEnumerable<OfferTypes> GetNonNewBusinessOfferTypes()
        {
            return new List<OfferTypes> {OfferTypes.CAP2, 
                OfferTypes.FurtherAdvance,
                OfferTypes.FurtherLoan,
                OfferTypes.Life,
                OfferTypes.ReAdvance,
                OfferTypes.Unknown,
                OfferTypes.UnsecuredLending};
        }
    }
}

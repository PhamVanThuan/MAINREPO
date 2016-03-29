using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.Application;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.Application
{
    [TestFixture]
    public class ApplicationWith30YearTerm : RuleBase
    {
        private IStageDefinitionRepository stageDefinitionRepository;

        [SetUp()]
        public override void Setup()
        {
            base.Setup();

            stageDefinitionRepository = _mockery.StrictMock<IStageDefinitionRepository>();
            MockCache.Add(typeof(IStageDefinitionRepository).ToString(), stageDefinitionRepository);
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void ApplicationCannotHave30YearTerm_RuleTest_Pass()
        {
            IApplication application = _mockery.StrictMock<IApplication>();
            SetupResult.For(application.Key).Return(0);

            SetupResult.For(application.HasAttribute(OfferAttributeTypes.Loanwith30YearTerm)).Return(false);
            ApplicationCannotHave30YearTerm applicationHasPrevious30YearTermDisqualification = new ApplicationCannotHave30YearTerm();

            ExecuteRule(applicationHasPrevious30YearTermDisqualification, 0, application); // expect no messages back
        }

        [Test]
        public void ApplicationCannotHave30YearTerm_RuleTest_Fail()
        {
            IApplication application = _mockery.StrictMock<IApplication>();
            SetupResult.For(application.Key).Return(0);

            SetupResult.For(application.HasAttribute(OfferAttributeTypes.Loanwith30YearTerm)).Return(true);
            ApplicationCannotHave30YearTerm applicationHasPrevious30YearTermDisqualification = new ApplicationCannotHave30YearTerm();

            ExecuteRule(applicationHasPrevious30YearTermDisqualification, 1, application); // expect 1 message back
        }
    }
}
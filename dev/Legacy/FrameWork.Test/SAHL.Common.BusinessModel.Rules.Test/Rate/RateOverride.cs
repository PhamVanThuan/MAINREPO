using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.FinancialAdjustments;

namespace SAHL.Common.BusinessModel.Test.Rules.Rate
{
    [TestFixture]
    public class RateOverride : RuleBase
    {
        protected IFinancialAdjustment financialAdjustment = null;
        protected IFinancialAdjustmentType financialAdjustmentType = null;
        protected IFinancialService fins = null;
        protected IAccount acc = null;
        protected IProduct prod = null;
        protected IList<IStageTransition> stlist = null;
        protected IStageDefinitionRepository sdRepo = null;
        protected IStageDefinitionStageDefinitionGroup sdsdg = null;
        protected IStageDefinitionGroup sdg = null;
        protected IStageDefinition sd = null;
        protected IStageTransition st = null;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            sdRepo = _mockery.StrictMock<IStageDefinitionRepository>();
            MockCache.Add(typeof(IStageDefinitionRepository).ToString(), sdRepo);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        /// <summary>
        /// Add FinancialAdjustment of type CollectNoPayment for a QuickCash loan.
        /// </summary>
        [NUnit.Framework.Test]
        public void FinancialAdjustmentCollectNoPaymentAddQuickCashPass()
        {
            FinancialAdjustmentCollectNoPaymentAdd rule = new FinancialAdjustmentCollectNoPaymentAdd();

            financialAdjustmentType = _mockery.StrictMock<IFinancialAdjustmentType>();
            SetupResult.For(financialAdjustmentType.Key).Return((int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.CollectNoPayment);

            financialAdjustment = _mockery.StrictMock<IFinancialAdjustment>();
            SetupResult.For(financialAdjustment.FinancialAdjustmentType).Return(financialAdjustmentType);

            fins = _mockery.StrictMock<IFinancialService>();
            SetupResult.For(fins.Key).Return(1);
            acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return(1);
            prod = _mockery.StrictMock<IProduct>();
            SetupResult.For(prod.Key).Return(10);

            SetupResult.For(financialAdjustment.FinancialService).Return(fins);
            SetupResult.For(fins.Account).Return(acc);
            SetupResult.For(acc.Product).Return(prod);

            ExecuteRule(rule, 0, financialAdjustment);
        }

        /// <summary>
        /// Add FinancialAdjustment of type != CollectNoPayment, no checks and pass
        /// </summary>
        [NUnit.Framework.Test]
        public void FinancialAdjustmentInterestOnlyAddPass()
        {
            FinancialAdjustmentCollectNoPaymentAdd rule = new FinancialAdjustmentCollectNoPaymentAdd();

            financialAdjustmentType = _mockery.StrictMock<IFinancialAdjustmentType>();
            SetupResult.For(financialAdjustmentType.Key).Return((int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.InterestOnly);

            financialAdjustment = _mockery.StrictMock<IFinancialAdjustment>();
            SetupResult.For(financialAdjustment.FinancialAdjustmentType).Return(financialAdjustmentType);

            ExecuteRule(rule, 0, financialAdjustment);
        }

        /// <summary>
        /// Add FinancialAdjustment of type CollectNoPayment for a non QC loan, but with a "Debt Counselling" StageDefinition
        /// </summary>
        [NUnit.Framework.Test]
        public void FinancialAdjustmentCollectNoPaymentAddWithStageTransitionPass()
        {
            FinancialAdjustmentCollectNoPaymentAdd rule = new FinancialAdjustmentCollectNoPaymentAdd();

            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            financialAdjustmentType = _mockery.StrictMock<IFinancialAdjustmentType>();
            SetupResult.For(financialAdjustmentType.Key).Return((int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.CollectNoPayment);

            financialAdjustment = _mockery.StrictMock<IFinancialAdjustment>();
            SetupResult.For(financialAdjustment.FinancialAdjustmentType).Return(financialAdjustmentType);

            fins = _mockery.StrictMock<IFinancialService>();
            SetupResult.For(fins.Key).Return(1);
            acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return(1);
            prod = _mockery.StrictMock<IProduct>();
            SetupResult.For(prod.Key).Return(1);

            SetupResult.For(financialAdjustment.FinancialService).Return(fins);
            SetupResult.For(fins.Account).Return(acc);
            SetupResult.For(acc.Product).Return(prod);

            sdsdg = _mockery.StrictMock<IStageDefinitionStageDefinitionGroup>();
            sd = _mockery.StrictMock<IStageDefinition>();
            st = _mockery.StrictMock<IStageTransition>();

            SetupResult.For(sdsdg.Key).Return(1);
            SetupResult.For(st.StageDefinitionStageDefinitionGroup).Return(sdsdg);
            SetupResult.For(sdsdg.StageDefinition).Return(sd);
            SetupResult.For(sd.Key).Return((int)StageDefinitions.DebtCounselling);
            SetupResult.For(sd.Description).Return("Debt Counselling");

            stlist = new List<IStageTransition>();
            stlist.Add(st);

            //Expect.Call(StageDefinitionHelper.GetStageTransitionsByGenericKey((int)SAHL.Common.Globals.StageDefinitionGroups.LossControl, 1)).IgnoreArguments().Return(stlist);
            SetupResult.For(sdRepo.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(stlist);

            ExecuteRule(rule, 0, financialAdjustment);
        }

        /// <summary>
        /// Add FinancialAdjustment of type CollectNoPayment for non QC loan with no StageDefinition
        /// </summary>
        [NUnit.Framework.Test]
        public void FinancialAdjustmentCollectNoPaymentAddFail()
        {
            FinancialAdjustmentCollectNoPaymentAdd rule = new FinancialAdjustmentCollectNoPaymentAdd();
            
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            financialAdjustmentType = _mockery.StrictMock<IFinancialAdjustmentType>();
            SetupResult.For(financialAdjustmentType.Key).Return((int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.CollectNoPayment);

            financialAdjustment = _mockery.StrictMock<IFinancialAdjustment>();
            SetupResult.For(financialAdjustment.FinancialAdjustmentType).Return(financialAdjustmentType);

            fins = _mockery.StrictMock<IFinancialService>();
            SetupResult.For(fins.Key).Return(1);
            acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return(1);
            prod = _mockery.StrictMock<IProduct>();
            SetupResult.For(prod.Key).Return(1);

            SetupResult.For(financialAdjustment.FinancialService).Return(fins);
            SetupResult.For(fins.Account).Return(acc);
            SetupResult.For(acc.Product).Return(prod);

            sdg = _mockery.StrictMock<IStageDefinitionGroup>();
            SetupResult.For(sdg.Key).Return((int)SAHL.Common.Globals.StageDefinitionGroups.LossControl);

            SetupResult.For(sdRepo.GetStageDefinitionGroupByKey(0)).IgnoreArguments().Return(sdg);
            stlist = null;
            stlist = _mockery.StrictMock<IList<IStageTransition>>();
            SetupResult.For(stlist.Count).Return(0);
            SetupResult.For(sdRepo.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(stlist);

            ExecuteRule(rule, 1, financialAdjustment);
        }

        /// <summary>
        /// Add FinancialAdjustment of type CollectNoPayment for non QC loan with StageDefinition but not "Debt Counselling"
        /// </summary>
        [NUnit.Framework.Test]
        public void FinancialAdjustmentCollectNoPaymentAddWithStageTransitionFail()
        {
            FinancialAdjustmentCollectNoPaymentAdd rule = new FinancialAdjustmentCollectNoPaymentAdd();
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            financialAdjustmentType = _mockery.StrictMock<IFinancialAdjustmentType>();
            SetupResult.For(financialAdjustmentType.Key).Return((int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.CollectNoPayment);

            financialAdjustment = _mockery.StrictMock<IFinancialAdjustment>();
            SetupResult.For(financialAdjustment.FinancialAdjustmentType).Return(financialAdjustmentType);

            fins = _mockery.StrictMock<IFinancialService>();
            SetupResult.For(fins.Key).Return(1);
            acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(acc.Key).Return(1);
            prod = _mockery.StrictMock<IProduct>();
            SetupResult.For(prod.Key).Return(1);

            SetupResult.For(financialAdjustment.FinancialService).Return(fins);
            SetupResult.For(fins.Account).Return(acc);
            SetupResult.For(acc.Product).Return(prod);

            sdsdg = _mockery.StrictMock<IStageDefinitionStageDefinitionGroup>();
            SetupResult.For(sdsdg.Key).Return(1);
            sdg = _mockery.StrictMock<IStageDefinitionGroup>();
            SetupResult.For(sdg.Key).Return((int)SAHL.Common.Globals.StageDefinitionGroups.LossControl);

            SetupResult.For(sdRepo.GetStageDefinitionGroupByKey(0)).IgnoreArguments().Return(sdg);

            stlist = new List<IStageTransition>();
            sd = _mockery.StrictMock<IStageDefinition>();
            st = _mockery.StrictMock<IStageTransition>();

            SetupResult.For(st.StageDefinitionStageDefinitionGroup).Return(sdsdg);
            SetupResult.For(sdsdg.StageDefinition).Return(sd);
            SetupResult.For(sd.Key).Return(1);
            SetupResult.For(sd.Description).Return("blah");

            stlist.Add(st);

            SetupResult.For(sdRepo.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(stlist);

            ExecuteRule(rule, 1, financialAdjustment);
        }

        /// <summary>
        /// Argument Exception
        /// </summary>
        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Parameter[0] is not of type IFinancialAdjustment")]
        public void FinancialAdjustmentCollectNoPaymentAddInvalidArgumemntsPassedFail()
        {
            FinancialAdjustmentCollectNoPaymentAdd rule = new FinancialAdjustmentCollectNoPaymentAdd();
            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();
            ExecuteRule(rule, 0, legalEntityCompany);
        }
    }
}

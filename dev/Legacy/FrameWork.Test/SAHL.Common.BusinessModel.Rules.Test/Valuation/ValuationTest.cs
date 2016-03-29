using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.Valuations;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using SAHL.Test;
using SAHL.Common.Factories;
using SAHL.Common.DataAccess;
using System.Data;

namespace SAHL.Common.BusinessModel.Rules.Test.Valuation
{
    [TestFixture]
    public class ValuationTest : RuleBase
    {
        IValuation valuation;
        IValuationDataProviderDataService vdpds;
        IProperty property;
        
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            valuation = _mockery.StrictMock<IValuation>();

            vdpds = _mockery.StrictMock<IValuationDataProviderDataService>();

            property = _mockery.StrictMock<IProperty>();
            SetupResult.For(valuation.Property).IgnoreArguments().Return(property);
       }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void ValuationValuer_Pass()
        {
            ValuationValuer rule = new ValuationValuer();
            IValuation valuation = _mockery.StrictMock<IValuation>();
            IValuator valuator = _mockery.StrictMock<IValuator>();

            SetupResult.For(valuation.Valuator).Return(valuator);

            ExecuteRule(rule, 0, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuer_Fail()
        {
            ValuationValuer rule = new ValuationValuer();
            IValuation valuation = _mockery.StrictMock<IValuation>();

            SetupResult.For(valuation.Valuator).Return(null);

            ExecuteRule(rule, 1, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuationDateThresholdDateInFutureTestFail()
        {
            ValuationValuationDateThreshold rule = new ValuationValuationDateThreshold();

            DateTime dt = DateTime.Now.AddDays(+2);
            
            SetupResult.For(valuation.ValuationDate).Return(dt.Date);

            ExecuteRule(rule, 1, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuationDateThresholdTodaysDateTestPass()
        {
            ValuationValuationDateThreshold rule = new ValuationValuationDateThreshold();

            SetupResult.For(valuation.ValuationDate).Return(DateTime.Now.Date);

            ExecuteRule(rule, 0, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuationDateThresholdDateInPastTestPass()
        {
            ValuationValuationDateThreshold rule = new ValuationValuationDateThreshold();

            SetupResult.For(valuation.ValuationDate).Return(DateTime.Now.AddDays(-1).Date);

            ExecuteRule(rule, 0, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuationDateThresholdDateUpdateModeTestPass()
        {
            ValuationValuationDateThreshold rule = new ValuationValuationDateThreshold();

            SetupResult.For(valuation.ValuationDate).Return(DateTime.Now.AddDays(-5).Date);

            ExecuteRule(rule, 0, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuationAmountMinimumLessThanMinimumTestFail()
        {
            ValuationValuationAmountMinimum rule = new ValuationValuationAmountMinimum();

            double valAmount = 99000;
            double valMinimum = 100000;
            SetupResult.For(valuation.ValuationAmount).Return(valAmount);

            IRuleRepository RuleRepo = _mockery.StrictMock<IRuleRepository>();

            MockCache.Add(typeof(IRuleRepository).ToString(), RuleRepo);
            IRuleItem ruleItem = _mockery.StrictMock<IRuleItem>();

            SetupResult.For(RuleRepo.FindRuleItemByTypeName("")).IgnoreArguments().Return(ruleItem);

            // Setup ruleItem.parameters
            IEventList<IRuleParameter> ruleParameters = new EventList<IRuleParameter>();
            IRuleParameter ruleParameter = _mockery.StrictMock<IRuleParameter>();

            SetupResult.For(ruleParameter.Value).Return(valMinimum.ToString());
            ruleParameters.Add(Messages, ruleParameter);
            SetupResult.For(ruleItem.RuleParameters).Return(ruleParameters);

            // Setup ruleItem.Name
            SetupResult.For(ruleItem.Name).Return("ValuationAmountMinimum");

            ExecuteRule(rule, 1, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuationAmountMinimumEqualToMinTestFail()
        {
            ValuationValuationAmountMinimum rule = new ValuationValuationAmountMinimum();

            double valAmount = 100000;
            double valMinimum = 100000;
            SetupResult.For(valuation.ValuationAmount).Return(valAmount);

            IRuleRepository RuleRepo = _mockery.StrictMock<IRuleRepository>();

            MockCache.Add(typeof(IRuleRepository).ToString(), RuleRepo);
            IRuleItem ruleItem = _mockery.StrictMock<IRuleItem>();

            SetupResult.For(RuleRepo.FindRuleItemByTypeName("")).IgnoreArguments().Return(ruleItem);

            // Setup ruleItem.parameters
            IEventList<IRuleParameter> ruleParameters = new EventList<IRuleParameter>();
            IRuleParameter ruleParameter = _mockery.StrictMock<IRuleParameter>();

            SetupResult.For(ruleParameter.Value).Return(valMinimum.ToString());
            ruleParameters.Add(Messages, ruleParameter);
            SetupResult.For(ruleItem.RuleParameters).Return(ruleParameters);

            // Setup ruleItem.Name
            SetupResult.For(ruleItem.Name).Return("ValuationAmountMinimum");

            ExecuteRule(rule, 1, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuationAmountGreaterThanMinimumTestPass()
        {
            ValuationValuationAmountMinimum rule = new ValuationValuationAmountMinimum();

            double valAmount = 100000.01;
            double valMinimum = 100000;
            SetupResult.For(valuation.ValuationAmount).Return(valAmount);

            IRuleRepository RuleRepo = _mockery.StrictMock<IRuleRepository>();

            MockCache.Add(typeof(IRuleRepository).ToString(), RuleRepo);
            IRuleItem ruleItem = _mockery.StrictMock<IRuleItem>();

            SetupResult.For(RuleRepo.FindRuleItemByTypeName("")).IgnoreArguments().Return(ruleItem);

            // Setup ruleItem.parameters
            IEventList<IRuleParameter> ruleParameters = new EventList<IRuleParameter>();
            IRuleParameter ruleParameter = _mockery.StrictMock<IRuleParameter>();

            SetupResult.For(ruleParameter.Value).Return(valMinimum.ToString());
            ruleParameters.Add(Messages, ruleParameter);
            SetupResult.For(ruleItem.RuleParameters).Return(ruleParameters);

            // Setup ruleItem.Name
            SetupResult.For(ruleItem.Name).Return("ValuationAmountMinimum");

            ExecuteRule(rule, 0, valuation);
        }

      
        [NUnit.Framework.Test]
        public void ValuationHOCAmountTestFail()
        {
            ValuationHOCAmount rule = new ValuationHOCAmount();
            double valuationHOCAmount = 0;

            SetupResult.For(valuation.ValuationHOCValue).Return(valuationHOCAmount);
            ExecuteRule(rule, 1, valuation);
        }


        [NUnit.Framework.Test]
        public void ValuationHOCAmountTestPass()
        {
            ValuationHOCAmount rule = new ValuationHOCAmount();
            double valuationHOCAmount = 150000;

            SetupResult.For(valuation.ValuationHOCValue).Return(valuationHOCAmount);
            ExecuteRule(rule, 0, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuationActiveStatusTestFail()
        {
            ValuationActiveStatus rule = new ValuationActiveStatus();

            IValuation val1 = _mockery.StrictMock<IValuation>();
            IValuation val2 = _mockery.StrictMock<IValuation>();

            IEventList<IValuation> Valuations = new EventList<IValuation>();
            Valuations.Add(Messages, val1);
            Valuations.Add(Messages,val2);

            SetupResult.For(val1.IsActive).Return(true);
            SetupResult.For(val2.IsActive).Return(true);

            SetupResult.For(property.Valuations).Return(Valuations);

            ExecuteRule(rule, 1, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuationOneActiveStatusTestPass()
        {
            ValuationActiveStatus rule = new ValuationActiveStatus();

            IValuation val1 = _mockery.StrictMock<IValuation>();
            IValuation val2 = _mockery.StrictMock<IValuation>();

            IEventList<IValuation> Valuations = new EventList<IValuation>();
            Valuations.Add(Messages, val1);
            Valuations.Add(Messages, val2);

            SetupResult.For(val1.IsActive).Return(true);
            SetupResult.For(val2.IsActive).Return(false);

            SetupResult.For(property.Valuations).Return(Valuations);

            ExecuteRule(rule, 0, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationValuationActiveStatusTestPass()
        {
            ValuationActiveStatus rule = new ValuationActiveStatus();

            IValuation val = _mockery.StrictMock<IValuation>();
            IEventList<IValuation> Valuations = new EventList<IValuation>();
            Valuations.Add(Messages, val);
            SetupResult.For(val.IsActive).Return(true);

            SetupResult.For(property.Valuations).Return(Valuations);

            ExecuteRule(rule, 0, valuation);
        }

        [NUnit.Framework.Test]
        public void ValuationSAHLManualTypeValidationTestPass()
        {
            ValuationTypeValidation rule = new ValuationTypeValidation();

            IValuationDiscriminatedSAHLManual valSAHLManual = _mockery.StrictMock<IValuationDiscriminatedSAHLManual>();

            ExecuteRule(rule, 0, valSAHLManual);
        }

     
        [NUnit.Framework.Test]
        public void ValuationAdCheckTypeValidationTestPass()
        {
            ValuationTypeValidation rule = new ValuationTypeValidation();

            IValuationDiscriminatedAdCheckDesktop valADD = _mockery.StrictMock<IValuationDiscriminatedAdCheckDesktop>();

            ExecuteRule(rule, 0, valADD);
        }


        [NUnit.Framework.Test]
        public void ValuationLightStoneValidationTestPass()
        {
            ValuationTypeValidation rule = new ValuationTypeValidation();

            IValuationDiscriminatedLightstoneAVM valLS = _mockery.StrictMock<IValuationDiscriminatedLightstoneAVM>();

            ExecuteRule(rule, 0, valLS);
        }


        [NUnit.Framework.Test]
        public void ValuationClientEstimateTypeValidationTestPass()
        {
            ValuationTypeValidation rule = new ValuationTypeValidation();

            IValuationDiscriminatedSAHLClientEstimate valCE = _mockery.StrictMock<IValuationDiscriminatedSAHLClientEstimate>();

            ExecuteRule(rule, 0, valCE);
        }


        [NUnit.Framework.Test]
        public void ValuationTypeValidationTestFail()
        {
            ValuationTypeValidation rule = new ValuationTypeValidation();

            IValuationDiscriminatedLightstoneAVM valLS = null;

            ExecuteRule(rule, 1, valLS);
        }

        #region ValuationRecentExists

        [NUnit.Framework.Test]
        public void ValuationRecentExistsTest()
        {
            //valid app types with old vals
            ValuationRecentExistsHelper(DateTime.Now.AddMonths(-24), (int)OfferTypes.NewPurchaseLoan, 0, true);
            ValuationRecentExistsHelper(DateTime.Now.AddMonths(-24), (int)OfferTypes.SwitchLoan, 0, true);
            ValuationRecentExistsHelper(DateTime.Now.AddMonths(-24), (int)OfferTypes.RefinanceLoan, 0, true);
            //valid app types with recent active vals
            ValuationRecentExistsHelper(DateTime.Now, (int)OfferTypes.NewPurchaseLoan, 1, true);
            ValuationRecentExistsHelper(DateTime.Now, (int)OfferTypes.SwitchLoan, 1, true);
            ValuationRecentExistsHelper(DateTime.Now, (int)OfferTypes.RefinanceLoan, 1, true);
            //valid app types with recent inactive vals
            ValuationRecentExistsHelper(DateTime.Now, (int)OfferTypes.NewPurchaseLoan, 0, false);
            ValuationRecentExistsHelper(DateTime.Now, (int)OfferTypes.SwitchLoan, 0, false);
            ValuationRecentExistsHelper(DateTime.Now, (int)OfferTypes.RefinanceLoan, 0, false);
            //FL apps, even with recent vals should not be tested
            ValuationRecentExistsHelper(DateTime.Now, (int)OfferTypes.ReAdvance, 0, true);
            ValuationRecentExistsHelper(DateTime.Now, (int)OfferTypes.FurtherAdvance, 0, true);
            ValuationRecentExistsHelper(DateTime.Now, (int)OfferTypes.FurtherLoan, 0, true);
    }

        private void ValuationRecentExistsHelper(DateTime valDate, int ApplicationTypeKey, int msgCount, bool valActive)
        {
            ValuationRecentExists rule = new ValuationRecentExists();

            IProperty prop = _mockery.StrictMock<IProperty>();
            SetupResult.For(prop.Key).Return(1);

            IValuation val = _mockery.StrictMock<IValuation>();
            SetupResult.For(val.Key).Return(1);
            SetupResult.For(val.ValuationDate).Return(valDate);
            SetupResult.For(val.IsActive).Return(valActive);

            IEventList<IValuation> iel = new EventList<IValuation>();

            iel.Add(null, val);

            SetupResult.For(prop.Valuations).Return(iel);

            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(appType.Key).Return(ApplicationTypeKey);

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(appML.Key).Return(1);
            SetupResult.For(appML.ApplicationType).Return(appType);
            SetupResult.For(appML.Property).Return(prop);

            ExecuteRule(rule, msgCount, appML);

}

        #endregion

		/// <summary>
		/// Automated Valuation Further Loan Further Advance rule test
		/// </summary>
		[NUnit.Framework.Test]
		public void AutomatedValuationFurtherLoanFurtherAdvanceTest()
		{
			SetRepositoryStrategy(TypeFactoryStrategy.Mock);
			//Ensure that all the other types of Offers pass except for Further Loan and Further Advance
			//These should all pass
			//Pass
			AutomatedValuationFurtherLoanFurtherAdvanceHelper(OfferTypes.NewPurchaseLoan, StageDefinitionGroups.PhysicalValuation, 0);
			AutomatedValuationFurtherLoanFurtherAdvanceHelper(OfferTypes.ReAdvance, StageDefinitionGroups.PhysicalValuation, 0);
			AutomatedValuationFurtherLoanFurtherAdvanceHelper(OfferTypes.RefinanceLoan, StageDefinitionGroups.PhysicalValuation, 0);
			AutomatedValuationFurtherLoanFurtherAdvanceHelper(OfferTypes.SwitchLoan, StageDefinitionGroups.PhysicalValuation, 0);
			AutomatedValuationFurtherLoanFurtherAdvanceHelper(OfferTypes.Unknown, StageDefinitionGroups.PhysicalValuation, 0);

			//These should not pass
			//Fail
			AutomatedValuationFurtherLoanFurtherAdvanceHelper(OfferTypes.FurtherAdvance, StageDefinitionGroups.PhysicalValuation, 1);
			AutomatedValuationFurtherLoanFurtherAdvanceHelper(OfferTypes.FurtherLoan, StageDefinitionGroups.PhysicalValuation, 1);

			//These should pass
			AutomatedValuationFurtherLoanFurtherAdvanceHelper(OfferTypes.FurtherAdvance, StageDefinitionGroups.CompleteValuation, 0);
			AutomatedValuationFurtherLoanFurtherAdvanceHelper(OfferTypes.FurtherLoan, StageDefinitionGroups.CompleteValuation, 0);
			SetRepositoryStrategy(TypeFactoryStrategy.Default);
		}

		/// <summary>
		/// Helper method to do unit testing for the Automated Valuation Further Loan Further Advance rule
		/// </summary>
		/// <param name="offerType"></param>
		/// <param name="stageDefitionGroup"></param>
		/// <param name="expectedErrorCount"></param>
		private void AutomatedValuationFurtherLoanFurtherAdvanceHelper(OfferTypes offerType, StageDefinitionGroups stageDefinitionGroupKey, int expectedErrorCount)
		{
			AutomatedValuationFurtherLoanFurtherAdvance rule = new AutomatedValuationFurtherLoanFurtherAdvance();

			IStageDefinitionRepository stageDefinitionRepository = _mockery.StrictMock<IStageDefinitionRepository>();
			MockCache.Add((typeof(IStageDefinitionRepository)).ToString(), stageDefinitionRepository);

			IApplicationMortgageLoan application = _mockery.StrictMock<IApplicationMortgageLoan>();
			IApplicationType applicationType = _mockery.StrictMock<IApplicationType>();

			IStageTransition stageTransition = _mockery.StrictMock<IStageTransition>();
			IStageDefinitionGroup stageDefinitionGroup = _mockery.StrictMock<IStageDefinitionGroup>();
			IStageDefinitionStageDefinitionGroup stageDefinitionStageDefinitionGroup = _mockery.StrictMock<IStageDefinitionStageDefinitionGroup>();

			SetupResult.For(application.Key).Return(1);
			SetupResult.For(applicationType.Key).Return((int)offerType);
			SetupResult.For(application.ApplicationType).Return(applicationType);

			IList<IStageTransition> stageTransitions = new List<IStageTransition>();

			SetupResult.For(stageDefinitionGroup.Key).Return((int)stageDefinitionGroupKey);

			SetupResult.For(stageTransition.StageDefinitionStageDefinitionGroup).Return(stageDefinitionStageDefinitionGroup);
			SetupResult.For(stageDefinitionStageDefinitionGroup.StageDefinitionGroup).Return(stageDefinitionGroup);

			stageTransitions.Add(stageTransition);

			SetupResult.For(stageDefinitionRepository.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(stageTransitions);

			ExecuteRule(rule, expectedErrorCount, application);
		}

		/// <summary>
		/// Automated Valuation Rapid
		/// </summary>
		[NUnit.Framework.Test]
		public void AutomatedValuationRapidTest()
		{
			SetRepositoryStrategy(TypeFactoryStrategy.Mock);
			//Ensure that all the other types of Offers pass that are not ReAdvance
			//These should all pass
			//Pass
			AutomatedValuationRapidHelper(OfferTypes.FurtherAdvance, StageDefinitionGroups.PhysicalValuation, 0);
			AutomatedValuationRapidHelper(OfferTypes.FurtherLoan, StageDefinitionGroups.PhysicalValuation, 0);
			AutomatedValuationRapidHelper(OfferTypes.NewPurchaseLoan, StageDefinitionGroups.PhysicalValuation, 0);
			AutomatedValuationRapidHelper(OfferTypes.RefinanceLoan, StageDefinitionGroups.PhysicalValuation, 0);
			AutomatedValuationRapidHelper(OfferTypes.SwitchLoan, StageDefinitionGroups.PhysicalValuation, 0);
			AutomatedValuationRapidHelper(OfferTypes.Unknown, StageDefinitionGroups.PhysicalValuation, 0);

			//These should not pass
			//Fail
			AutomatedValuationRapidHelper(OfferTypes.ReAdvance, StageDefinitionGroups.PhysicalValuation, 1);

			//These should pass
			AutomatedValuationRapidHelper(OfferTypes.ReAdvance, StageDefinitionGroups.CompleteValuation, 0);
			SetRepositoryStrategy(TypeFactoryStrategy.Default);
		}

		/// <summary>
		/// Automated Valuation Rapid Test Helper
		/// </summary>
		/// <param name="offerType"></param>
		/// <param name="stageDefinitionGroupKey"></param>
		/// <param name="expectedErrorCount"></param>
		private void AutomatedValuationRapidHelper(OfferTypes offerType, StageDefinitionGroups stageDefinitionGroupKey, int expectedErrorCount)
		{
			AutomatedValuationRapid rule = new AutomatedValuationRapid();

			IStageDefinitionRepository stageDefinitionRepository = _mockery.StrictMock<IStageDefinitionRepository>();
			MockCache.Add((typeof(IStageDefinitionRepository)).ToString(), stageDefinitionRepository);

			IApplicationMortgageLoan application = _mockery.StrictMock<IApplicationMortgageLoan>();
			IApplicationType applicationType = _mockery.StrictMock<IApplicationType>();

			IStageTransition stageTransition = _mockery.StrictMock<IStageTransition>();
			IStageDefinitionGroup stageDefinitionGroup = _mockery.StrictMock<IStageDefinitionGroup>();
			IStageDefinitionStageDefinitionGroup stageDefinitionStageDefinitionGroup = _mockery.StrictMock<IStageDefinitionStageDefinitionGroup>();

			SetupResult.For(application.Key).Return(1);
			SetupResult.For(applicationType.Key).Return((int)offerType);
			SetupResult.For(application.ApplicationType).Return(applicationType);

			IList<IStageTransition> stageTransitions = new List<IStageTransition>();

			SetupResult.For(stageDefinitionGroup.Key).Return((int)stageDefinitionGroupKey);

			SetupResult.For(stageTransition.StageDefinitionStageDefinitionGroup).Return(stageDefinitionStageDefinitionGroup);
			SetupResult.For(stageDefinitionStageDefinitionGroup.StageDefinitionGroup).Return(stageDefinitionGroup);

			stageTransitions.Add(stageTransition);

			SetupResult.For(stageDefinitionRepository.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(stageTransitions);

			ExecuteRule(rule, expectedErrorCount, application);
		}

		/// <summary>
		/// Ensure that the property has valuations
		/// </summary>
		[NUnit.Framework.Test]
		public void ValuationApplicationPass()
		{
			ValuationApplication rule = new ValuationApplication();

			IProperty property = _mockery.StrictMock<IProperty>();
			IApplicationMortgageLoan application = _mockery.StrictMock<IApplicationMortgageLoan>();

			IEventList<IValuation> valuations = new EventList<IValuation>();
			IValuation valuation = _mockery.StrictMock<IValuation>();

			valuations.Add(Messages, valuation);

			SetupResult.For(application.Property).Return(property);
			SetupResult.For(property.Valuations).Return(valuations);

			ExecuteRule(rule, 0, application);
		}

		/// <summary>
		/// Ensure that the property has valuations
		/// </summary>
		[NUnit.Framework.Test]
		public void ValuationApplicationFail()
		{
			ValuationApplication rule = new ValuationApplication();

			IProperty property = _mockery.StrictMock<IProperty>();
			IApplicationMortgageLoan application = _mockery.StrictMock<IApplicationMortgageLoan>();

			SetupResult.For(application.Property).Return(property);
			SetupResult.For(property.Valuations).Return(null);

			ExecuteRule(rule, 1, application);
		}

		/// <summary>
		/// Valuation HOC Roof
		/// </summary>
		[NUnit.Framework.Test]
		public void ValuationHOCRoofPass()
		{
			ValuationHOCRoof rule = new ValuationHOCRoof();

			IValuation valuation = _mockery.StrictMock<IValuation>();
			IHOCRoof hocRoof = _mockery.StrictMock<IHOCRoof>();
			SetupResult.For(valuation.HOCRoof).Return(hocRoof);

			ExecuteRule(rule, 0, valuation);
		}

		/// <summary>
		/// Valuation HOC Roof
		/// </summary>
		[NUnit.Framework.Test]
		public void ValuationHOCRoofFail()
		{
			ValuationHOCRoof rule = new ValuationHOCRoof();

			IValuation valuation = _mockery.StrictMock<IValuation>();
			SetupResult.For(valuation.HOCRoof).Return(null);

			ExecuteRule(rule, 1, valuation);
		}
    }
}

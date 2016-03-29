using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Rules.RateAdjustment;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.Rate
{
	[TestFixture]
	public class RateAdjustment : RuleBase
	{
		/// <summary>
		/// Setup
		/// </summary>
		[SetUp]
		public override void Setup()
		{
			base.Setup();
			SetRepositoryStrategy(TypeFactoryStrategy.Mock);
		}

		/// <summary>
		/// Tear Down
		/// </summary>
		[TearDown]
		public override void TearDown()
		{
			base.TearDown();
		}

		/// <summary>
        /// Financial Adjustment Counter Rate Still Valid Pass
		/// </summary>
		[Test]
        public void FinancialAdjustmentCounterRateStillValidPass1()
		{
            FinancialAdjustmentCounterRateStillValid(1, 0, true, true, FinancialAdjustmentTypeSources.CounterRate);
		}

        [Test]
        public void FinancialAdjustmentCounterRateStillValidPass2()
        {
            FinancialAdjustmentCounterRateStillValid(0, 0, false, true, FinancialAdjustmentTypeSources.CounterRate);
        }

        [Test]
        public void FinancialAdjustmentCounterRateStillValidPass3()
        {
            FinancialAdjustmentCounterRateStillValid(0, 0, true, false, FinancialAdjustmentTypeSources.CounterRate);
        }

        [Test]
        public void FinancialAdjustmentCounterRateStillValidPass4()
        {
            FinancialAdjustmentCounterRateStillValid(0, 1, true, true, FinancialAdjustmentTypeSources.CounterRate);
        }

        [Test]
        public void FinancialAdjustmentCounterRateStillValidPass5()
        {
            FinancialAdjustmentCounterRateStillValid(0, 1, true, false, FinancialAdjustmentTypeSources.CounterRate);
        }

        [Test]
        public void FinancialAdjustmentCounterRateStillValidPass6()
        {
            FinancialAdjustmentCounterRateStillValid(0, 1, false, false, FinancialAdjustmentTypeSources.CounterRate);
        }

        [Test]
        public void FinancialAdjustmentCounterRateStillValidPass7()
        {
            FinancialAdjustmentCounterRateStillValid(0, 1, false, true, FinancialAdjustmentTypeSources.CounterRate);
        }


		/// <summary>
        /// FinancialAdjustmentCounterRateStillValid
		/// </summary>
		/// <param name="numberOfExpectedErrors"></param>
		/// <param name="rateOverrides"></param>
        private void FinancialAdjustmentCounterRateStillValid(int numberOfExpectedErrors, int ruleResult, bool hasFinancialAdjustments, bool hasRateAdjustment, FinancialAdjustmentTypeSources financialAdjustmentTypeSources)
		{
			var rule = new RateAdjustmentCounterRateStillValid();

			var ruleService = _mockery.StrictMock<IRuleService>();
			MockCache.Add(typeof(IRuleService).ToString(), ruleService);

            var financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();

			var rateAdjustmentElement = _mockery.StrictMock<IRateAdjustmentElement>();
			var ruleItem = _mockery.StrictMock<IRuleItem>();

			var applicationInformationFinancialAdjustment = _mockery.StrictMock<IApplicationInformationFinancialAdjustment>();
			var applicationInformation = _mockery.StrictMock<IApplicationInformation>();
			var applicationInformationAppliedRateAdjustment = _mockery.StrictMock<IApplicationInformationAppliedRateAdjustment>();
			var application = _mockery.StrictMock<IApplication>();

            var applicationInformationFinancialAdjustments = new EventList<IApplicationInformationFinancialAdjustment>(new IApplicationInformationFinancialAdjustment[] { applicationInformationFinancialAdjustment });
			var applicationInformationAppliedRateAdjustments = new EventList<IApplicationInformationAppliedRateAdjustment>(new IApplicationInformationAppliedRateAdjustment[] { applicationInformationAppliedRateAdjustment });

			SetupResult.For(ruleService.ExecuteRule(null, null, null)).IgnoreArguments().Return(ruleResult);
			SetupResult.For(applicationInformationAppliedRateAdjustment.RateAdjustmentElement).Return(rateAdjustmentElement);
            SetupResult.For(applicationInformation.ApplicationInformationFinancialAdjustments).Return(hasFinancialAdjustments ? applicationInformationFinancialAdjustments : null);
			SetupResult.For(rateAdjustmentElement.RuleItem).Return(ruleItem);
			SetupResult.For(ruleItem.Name).Return("RuleName");
			SetupResult.For(applicationInformationFinancialAdjustment.ApplicationInformationAppliedRateAdjustments).Return(hasRateAdjustment ? applicationInformationAppliedRateAdjustments : null);
            SetupResult.For(applicationInformationFinancialAdjustment.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);
			SetupResult.For(financialAdjustmentTypeSource.Key).Return((int)financialAdjustmentTypeSources);
			SetupResult.For(application.GetLatestApplicationInformation()).IgnoreArguments().Return(applicationInformation);

			ExecuteRule(rule, numberOfExpectedErrors, application);
		}
	}
}

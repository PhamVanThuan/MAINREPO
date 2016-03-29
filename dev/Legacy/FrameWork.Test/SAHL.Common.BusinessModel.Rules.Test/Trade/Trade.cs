using System;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;
using SAHL.Common.Factories;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Rules.Trade;

namespace SAHL.Common.BusinessModel.Rules.Test.Trade
{
    [TestFixture]
    public class Trade: RuleBase
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

        #region TradeCAPMandatory
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeCAPMandatorySuccess()
        {
            TradeCAPMandatory rule = new TradeCAPMandatory();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.TradeType).Return("C");
            SetupResult.For(trade.CapType).Return(captype);
            SetupResult.For(trade.Premium).Return(0.00);

            ExecuteRule(rule, 0, trade);
        }


        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeCAPMandatoryFailed()
        {
            TradeCAPMandatory rule = new TradeCAPMandatory();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.TradeType).Return("C");
            SetupResult.For(trade.CapType).Return(null);
            SetupResult.For(trade.Premium).Return(0.00);

            ExecuteRule(rule, 1, trade);
        }
        #endregion

        #region TradeStrikeRateMax
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeStrikeRateMaxSuccess()
        {
            TradeStrikeRateMax rule = new TradeStrikeRateMax();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.StrikeRate).Return(0.03);

            ExecuteRule(rule, 0, trade);
        }

        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeStrikeRateMaxFailed()
        {
            TradeStrikeRateMax rule = new TradeStrikeRateMax();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.StrikeRate).Return(1.00); 

            ExecuteRule(rule, 1, trade);
        }
        #endregion

        #region TradeStrikeRateMin
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeStrikeRateMinSuccess()
        {
            TradeStrikeRateMin rule = new TradeStrikeRateMin();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.StrikeRate).Return(0.01);

            ExecuteRule(rule, 0, trade);
        }

        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeStrikeRateMinFailed()
        {
            TradeStrikeRateMin rule = new TradeStrikeRateMin();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.StrikeRate).Return(0.00);

            ExecuteRule(rule, 1, trade);
        }
        #endregion

        #region TradeTermMin
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeTermMinSuccess()
        {
            TradeTermMin rule = new TradeTermMin();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.StartDate).Return(DateTime.Today);
            SetupResult.For(trade.EndDate).Return(DateTime.Today.AddMonths(1));

            ExecuteRule(rule, 0, trade);
        }

        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeTermMinFailed()
        {
            TradeTermMin rule = new TradeTermMin();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.StartDate).Return(DateTime.Today);
            SetupResult.For(trade.EndDate).Return(DateTime.Today);

            ExecuteRule(rule, 1, trade);
        }
        #endregion

        #region TradeCAPTermMax
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeCAPTermMaxSuccess()
        {
            TradeCAPTermMax rule = new TradeCAPTermMax();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.StartDate).Return(DateTime.Today);
            SetupResult.For(trade.EndDate).Return(DateTime.Today.AddMonths(24));

            ExecuteRule(rule, 0, trade);
        }

        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeCAPTermMaxFailed()
        {
            TradeCAPTermMax rule = new TradeCAPTermMax();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.StartDate).Return(DateTime.Today);
            SetupResult.For(trade.EndDate).Return(DateTime.Today.AddMonths(290));

            ExecuteRule(rule, 1, trade);
        }
        #endregion

        #region TradeTradeBalanceMin
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeTradeBalanceMinSuccess()
        {
            TradeTradeBalanceMin rule = new TradeTradeBalanceMin();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.TradeBalance).Return(111000.00);

            ExecuteRule(rule, 0, trade);
        }

        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeTradeBalanceMinFailed()
        {
            TradeTradeBalanceMin rule = new TradeTradeBalanceMin();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.TradeBalance).Return(-0.01);

            ExecuteRule(rule, 1, trade);
        }
        #endregion

        #region TradeTradePremiumMin
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeTradePremiumMinSuccess()
        {
            TradeTradePremiumMin rule = new TradeTradePremiumMin();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.Premium).Return(1.00);

            ExecuteRule(rule, 0, trade);
        }

        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeTradePremiumMinFailed()
        {
            TradeTradePremiumMin rule = new TradeTradePremiumMin();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.Premium).Return(-0.01);

            ExecuteRule(rule, 1, trade);
        }
        #endregion

        #region TradeTradeDate
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeTradeDateSuccess()
        {
            TradeTradeDate rule = new TradeTradeDate();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.TradeDate).Return(DateTime.Today);

            ExecuteRule(rule, 0, trade);
        }

        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeTradeDateFailed()
        {
            TradeTradeDate rule = new TradeTradeDate();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.TradeDate).Return(DateTime.Today.AddMonths(1));

            ExecuteRule(rule, 1, trade);
        }
        #endregion

        #region TradeCAPCommencement
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeCAPCommencementSuccess()
        {
            TradeCAPCommencement rule = new TradeCAPCommencement();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();
            IResetConfiguration resetConfig = _mockery.StrictMock<IResetConfiguration>();

            string sql = "SELECT TOP 1 * FROM Calendar C WHERE C.IsHoliday <> 1 AND C.IsSaturday <> 1 AND C.IsSunday <> 1";
            SimpleQuery<Calendar_DAO> cal = new SimpleQuery<Calendar_DAO>(QueryLanguage.Sql, sql);
            cal.AddSqlReturnDefinition(typeof(Calendar_DAO), "C");
            Calendar_DAO[] res = cal.Execute();

            if (res != null && res.Length > 0)
            {
                DateTime actiondate = res[0].CalendarDate.AddDays(1.00);
                SetupResult.For(resetConfig.ActionDate).Return(res[0].CalendarDate);
                SetupResult.For(trade.ResetConfiguration).Return(resetConfig);
                SetupResult.For(trade.StartDate).Return(actiondate);
                ExecuteRule(rule, 0, trade);
            }
            else
                Assert.Fail("No date found in calender");
        }

        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeCAPCommencementFailed()
        {
            TradeCAPCommencement rule = new TradeCAPCommencement();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();
            IResetConfiguration resetConfig = _mockery.StrictMock<IResetConfiguration>();

            string sql = "SELECT TOP 1 * FROM Calendar C WHERE C.IsHoliday = 1 OR C.IsSaturday = 1 OR C.IsSunday = 1";
            SimpleQuery<Calendar_DAO> cal = new SimpleQuery<Calendar_DAO>(QueryLanguage.Sql, sql);
            cal.AddSqlReturnDefinition(typeof(Calendar_DAO), "C");
            Calendar_DAO[] res = cal.Execute();

            if (res != null && res.Length > 0)
            {
                DateTime actiondate = res[0].CalendarDate.AddDays(-1.00);
                SetupResult.For(resetConfig.ActionDate).Return(res[0].CalendarDate);
                SetupResult.For(trade.ResetConfiguration).Return(resetConfig);
                SetupResult.For(trade.StartDate).Return(actiondate);
                ExecuteRule(rule, 1, trade);
            }
            else
                Assert.Fail("No date found in calender");
        }
        #endregion

        #region TradeDelete
        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeDeleteSuccess()
        {
            TradeDelete rule = new TradeDelete();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.CapBalance).Return(0.00);

            ExecuteRule(rule, 0, trade);
        }

        /// <summary>
        /// Expects Messages.Count = 1
        /// </summary>
        [NUnit.Framework.Test]
        public void TradeDeleteFailed()
        {
            TradeDelete rule = new TradeDelete();

            // Setup the correct object to pass along
            ITrade trade = _mockery.StrictMock<ITrade>();
            ICapType captype = _mockery.StrictMock<ICapType>();

            SetupResult.For(trade.CapBalance).Return(1.00);

            ExecuteRule(rule, 1, trade);
        }
        #endregion

		#region Trade Cap Balance Validation
		/// <summary>
		/// Pass: Trade Cap Balance Validation
		/// </summary>
		[NUnit.Framework.Test]
		public void TradeCapBalanceValidationPass()
		{
			TradeCapBalanceValidation rule = new TradeCapBalanceValidation();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.CapBalance).Return(0);

			ExecuteRule(rule, 0, trade);
		}

		/// <summary>
		/// Fail: Trade Cap Balance Validation
		/// </summary>
		[NUnit.Framework.Test]
		public void TradeCapBalanceValidationFail()
		{
			TradeCapBalanceValidation rule = new TradeCapBalanceValidation();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.CapBalance).Return(1000);

			ExecuteRule(rule, 1, trade);
		}
		#endregion

		#region Trade Check Effective Date
		/// <summary>
		/// Pass: Trade Check Effective Date
		/// </summary>
		[NUnit.Framework.Test]
		public void TradeCheckEffectiveDatePass()
		{
			TradeCheckEffectiveDate rule = new TradeCheckEffectiveDate();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.StartDate).Return(DateTime.Now.AddDays(1));

			ExecuteRule(rule, 0, trade);
		}

		/// <summary>
		/// Fail:Trade Check Effective Date
		/// </summary>
		[NUnit.Framework.Test]
		public void TradeCheckEffectiveDateFail()
		{
			TradeCheckEffectiveDate rule = new TradeCheckEffectiveDate();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.StartDate).Return(DateTime.Now.AddDays(-1));

			ExecuteRule(rule, 1, trade);
		}
		#endregion

		#region Trade Company Mandatory
		/// <summary>
		/// Pass: Trade Company Mandatory
		/// </summary>
		[NUnit.Framework.Test]
		public void TradeCompanyMandatoryPass()
		{
			TradeCompanyMandatory rule = new TradeCompanyMandatory();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.Company).Return("Test Company");

			ExecuteRule(rule, 0, trade);
		}

		/// <summary>
		/// Fail: Trade Company Mandatory
		/// </summary>
		[NUnit.Framework.Test]
		public void TradeCompanyMandatoryFail()
		{
			TradeCompanyMandatory rule = new TradeCompanyMandatory();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.Company).Return(null);

			ExecuteRule(rule, 1, trade);
		}
		#endregion

		#region Trade Premium Mandatory
		/// <summary>
		/// Pass: Trade Premium Mandatory
		/// </summary>
		[NUnit.Framework.Test]
		public void TradePremiumMandatoryWithTradeTypeWithPremiumPass()
		{
			TradePremiumMandatory rule = new TradePremiumMandatory();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.TradeType).Return("C");
			SetupResult.For(trade.Premium).Return(new double?(1.0d));

			ExecuteRule(rule, 0, trade);
		}

		/// <summary>
		/// Pass: Trade Premium Mandatory
		/// </summary>
		[NUnit.Framework.Test]
		public void TradePremiumMandatoryWithPremiumWithoutTradeTypePass()
		{
			TradePremiumMandatory rule = new TradePremiumMandatory();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.TradeType).Return("D");
			SetupResult.For(trade.Premium).Return(new double?(1.0d));

			ExecuteRule(rule, 0, trade);
		}

		/// <summary>
		/// Fail: Trade Premium Mandatory
		/// </summary>
		[NUnit.Framework.Test]
		public void TradePremiumMandatoryFail()
		{
			TradePremiumMandatory rule = new TradePremiumMandatory();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.Premium).Return(null);
			SetupResult.For(trade.TradeType).Return("C");

			ExecuteRule(rule, 1, trade);
		}
		#endregion

		#region TradeValidateEffectiveDate
		/// <summary>
		/// Pass: Trade Premium Mandatory
		/// </summary>
		[NUnit.Framework.Test]
		public void TradeValidateEffectiveDatePass()
		{
			TradeValidateEffectiveDate rule = new TradeValidateEffectiveDate();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.StartDate).Return(DateTime.Now);

			ExecuteRule(rule, 0, trade);
		}

		/// <summary>
		/// Fail: Trade Premium Mandatory
		/// </summary>
		[NUnit.Framework.Test]
		public void TradeValidateEffectiveDateFail()
		{
			TradeValidateEffectiveDate rule = new TradeValidateEffectiveDate();

			ITrade trade = _mockery.StrictMock<ITrade>();
			SetupResult.For(trade.StartDate).Return(DateTime.MinValue);

			ExecuteRule(rule, 1, trade);
		}
		#endregion
	}
}

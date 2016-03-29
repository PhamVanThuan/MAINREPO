using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.BusinessModel;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Rules.HOC;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Test.HOC
{
	[TestFixture]
	public class HOC : RuleBase
	{
		IHOC hoc = null;
		IHOCInsurer hocinsurer = null;
		IHOCRoof hocRoof = null;
		double conventionalAmt;
		double thatchAmt;
		IHOCStatus hocStatus = null;

		[SetUp]
		public override void Setup()
		{
			base.Setup();

			hoc = _mockery.StrictMock<IHOC>();
			hocinsurer = _mockery.StrictMock<IHOCInsurer>();
			SetRepositoryStrategy(TypeFactoryStrategy.Mock);

			SetupResult.For(hoc.HOCInsurer).IgnoreArguments().Return(hocinsurer);
			hocRoof = _mockery.StrictMock<IHOCRoof>();
			SetupResult.For(hoc.HOCRoof).IgnoreArguments().Return(hocRoof);
			hocStatus = _mockery.StrictMock<IHOCStatus>();
			SetupResult.For(hoc.HOCStatus).IgnoreArguments().Return(hocStatus);
		}

		[TearDown]
		public override void TearDown()
		{
			base.TearDown();
		}

		[NUnit.Framework.Test]
		public void HOCLessThanLatestValuationTestPass()
		{
			HOCLessThanLatestValuation rule = new HOCLessThanLatestValuation();
			IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
			IProperty property = _mockery.StrictMock<IProperty>();
			IValuation valuation = _mockery.StrictMock<IValuation>();
			IAccount hocAccount = _mockery.StrictMock<IAccount>();
			IEventList<IAccount> lstAccount = new EventList<IAccount>();
			IEventList<IFinancialService> lstFS = new EventList<IFinancialService>();
			IProduct product = _mockery.StrictMock<IProduct>();
			IFinancialService fsHOC = _mockery.StrictMock<IFinancialService>();
			IHOCRepository hocRepo = _mockery.StrictMock<IHOCRepository>();
			//
			MockCache.Add(typeof(IHOCRepository).ToString(), hocRepo);
			//
			SetupResult.For(valuation.IsActive).Return(true);
			SetupResult.For(valuation.HOCConventionalAmount).Return(Convert.ToDouble(100));
			SetupResult.For(valuation.HOCShingleAmount).Return(Convert.ToDouble(100));
			SetupResult.For(valuation.HOCThatchAmount).Return(Convert.ToDouble(100));
			//
			SetupResult.For(property.LatestCompleteValuation).Return(valuation);
			//
			SetupResult.For(appML.Property).Return(property);
			//
			SetupResult.For(product.Key).Return((int)SAHL.Common.Globals.Products.HomeOwnersCover);
			SetupResult.For(fsHOC.Key).Return(Convert.ToInt32(1));
			SetupResult.For(fsHOC.Account).Return(hocAccount);
			SetupResult.For(hoc.Key).Return(Convert.ToInt32(1));
			SetupResult.For(hoc.HOCTotalSumInsured).Return(Convert.ToDouble(300));
			//
			lstFS.Add(null, fsHOC);
			SetupResult.For(hocAccount.Product).Return(product);
			SetupResult.For(hocAccount.FinancialServices).Return(lstFS);
			//
			lstAccount.Add(null, hocAccount);
			SetupResult.For(appML.RelatedAccounts).Return(lstAccount);
			//
			SetupResult.For(hocRepo.GetHOCByKey(1)).IgnoreArguments().Return(hoc);

			ExecuteRule(rule, 0, appML);
		}

		[NUnit.Framework.Test]
		public void HOCLessThanLatestValuationTestFail()
		{
			HOCLessThanLatestValuation rule = new HOCLessThanLatestValuation();
			IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
			IProperty property = _mockery.StrictMock<IProperty>();
			IValuation valuation = _mockery.StrictMock<IValuation>();
			IAccount hocAccount = _mockery.StrictMock<IAccount>();
			IEventList<IAccount> lstAccount = new EventList<IAccount>();
			IEventList<IFinancialService> lstFS = new EventList<IFinancialService>();
			IProduct product = _mockery.StrictMock<IProduct>();
			IFinancialService fsHOC = _mockery.StrictMock<IFinancialService>();
			IHOCRepository hocRepo = _mockery.StrictMock<IHOCRepository>();
			//
			MockCache.Add(typeof(IHOCRepository).ToString(), hocRepo);
			//
			SetupResult.For(valuation.IsActive).Return(true);
			SetupResult.For(valuation.HOCConventionalAmount).Return(Convert.ToDouble(100));
			SetupResult.For(valuation.HOCShingleAmount).Return(Convert.ToDouble(100));
			SetupResult.For(valuation.HOCThatchAmount).Return(Convert.ToDouble(100));
			//
			SetupResult.For(property.LatestCompleteValuation).Return(valuation);
			//
			SetupResult.For(appML.Property).Return(property);
			//
			SetupResult.For(product.Key).Return((int)SAHL.Common.Globals.Products.HomeOwnersCover);
			SetupResult.For(fsHOC.Key).Return(Convert.ToInt32(1));
			SetupResult.For(fsHOC.Account).Return(hocAccount);
			SetupResult.For(hoc.Key).Return(Convert.ToInt32(1));
			SetupResult.For(hoc.HOCTotalSumInsured).Return(Convert.ToDouble(100));
			//
			lstFS.Add(null, fsHOC);
			SetupResult.For(hocAccount.Product).Return(product);
			SetupResult.For(hocAccount.FinancialServices).Return(lstFS);
			//
			lstAccount.Add(null, hocAccount);
			SetupResult.For(appML.RelatedAccounts).Return(lstAccount);
			//
			SetupResult.For(hocRepo.GetHOCByKey(1)).IgnoreArguments().Return(hoc);

			ExecuteRule(rule, 1, appML);
		}

		[NUnit.Framework.Test]
		public void HOCLessThanLoanAgreementAmountTestPass()
		{
			HOCLessThanLoanAgreementAmount rule = new HOCLessThanLoanAgreementAmount();
			IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
			IAccount hocAccount = _mockery.StrictMock<IAccount>();
			IEventList<IAccount> lstAccount = new EventList<IAccount>();
			IEventList<IFinancialService> lstFS = new EventList<IFinancialService>();
			IProduct product = _mockery.StrictMock<IProduct>();
			IApplicationProductSuperLoLoan vlai = _mockery.StrictMock<IApplicationProductSuperLoLoan>();
			IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();
			IFinancialService fsHOC = _mockery.StrictMock<IFinancialService>();
			IHOCRepository hocRepo = _mockery.StrictMock<IHOCRepository>();
			//
			MockCache.Add(typeof(IHOCRepository).ToString(), hocRepo);
			//
			SetupResult.For(vli.LoanAgreementAmount).Return(Convert.ToDouble(300));
			SetupResult.For(vlai.VariableLoanInformation).Return(vli);
			SetupResult.For(appML.CurrentProduct).Return(vlai);
			//
			SetupResult.For(product.Key).Return((int)SAHL.Common.Globals.Products.HomeOwnersCover);
			SetupResult.For(fsHOC.Key).Return(Convert.ToInt32(1));
			SetupResult.For(fsHOC.Account).Return(hocAccount);
			SetupResult.For(hoc.Key).Return(Convert.ToInt32(1));
			SetupResult.For(hoc.HOCTotalSumInsured).Return(Convert.ToDouble(300));
			//
			lstFS.Add(null, fsHOC);
			SetupResult.For(hocAccount.Product).Return(product);
			SetupResult.For(hocAccount.FinancialServices).Return(lstFS);
			//
			lstAccount.Add(null, hocAccount);
			SetupResult.For(appML.RelatedAccounts).Return(lstAccount);
			//
			SetupResult.For(hocRepo.GetHOCByKey(1)).IgnoreArguments().Return(hoc);

			ExecuteRule(rule, 0, appML);

		}

		[NUnit.Framework.Test]
		public void HOCLessThanLoanAgreementAmountTestFail()
		{
			HOCLessThanLoanAgreementAmount rule = new HOCLessThanLoanAgreementAmount();
			IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
			IAccount hocAccount = _mockery.StrictMock<IAccount>();
			IEventList<IAccount> lstAccount = new EventList<IAccount>();
			IEventList<IFinancialService> lstFS = new EventList<IFinancialService>();
			IProduct product = _mockery.StrictMock<IProduct>();
			IApplicationProductSuperLoLoan vlai = _mockery.StrictMock<IApplicationProductSuperLoLoan>();
			IApplicationInformationVariableLoan vli = _mockery.StrictMock<IApplicationInformationVariableLoan>();
			IFinancialService fsHOC = _mockery.StrictMock<IFinancialService>();
			IHOCRepository hocRepo = _mockery.StrictMock<IHOCRepository>();
			//
			MockCache.Add(typeof(IHOCRepository).ToString(), hocRepo);
			//
			SetupResult.For(vli.LoanAgreementAmount).Return(Convert.ToDouble(300));
			SetupResult.For(vlai.VariableLoanInformation).Return(vli);
			SetupResult.For(appML.CurrentProduct).Return(vlai);
			//
			SetupResult.For(product.Key).Return((int)SAHL.Common.Globals.Products.HomeOwnersCover);
			SetupResult.For(fsHOC.Key).Return(Convert.ToInt32(1));
			SetupResult.For(fsHOC.Account).Return(hocAccount);
			SetupResult.For(hoc.Key).Return(Convert.ToInt32(1));
			SetupResult.For(hoc.HOCTotalSumInsured).Return(Convert.ToDouble(100));
			//
			lstFS.Add(null, fsHOC);
			SetupResult.For(hocAccount.Product).Return(product);
			SetupResult.For(hocAccount.FinancialServices).Return(lstFS);
			//
			lstAccount.Add(null, hocAccount);
			SetupResult.For(appML.RelatedAccounts).Return(lstAccount);
			//
			SetupResult.For(hocRepo.GetHOCByKey(1)).IgnoreArguments().Return(hoc);

			ExecuteRule(rule, 1, appML);
		}

		[NUnit.Framework.Test]
		public void HOCInsurerMandatoryTestFail()
		{
			HOCInsurerMandatory rule = new HOCInsurerMandatory();

			SetupResult.For(hocinsurer.Key).IgnoreArguments().Return(0);

			ExecuteRule(rule, 1, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCInsurerMandatoryTestPass()
		{
			HOCInsurerMandatory rule = new HOCInsurerMandatory();

			SetupResult.For(hocinsurer.Key).IgnoreArguments().Return((int)HOCInsurers.SAHLHOC);

			ExecuteRule(rule, 0, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCApplicationMortgageLoanCollectionTestPass()
		{
			HOCApplicationMortgageLoanCollection rule = new HOCApplicationMortgageLoanCollection();

			IAccount account = _mockery.StrictMock<IAccount>();
			IFinancialService hocFS = _mockery.StrictMock<IFinancialService>();

			SetupResult.For(account.GetFinancialServiceByType(FinancialServiceTypes.HomeOwnersCover)).IgnoreArguments().Return(hocFS);

			ExecuteRule(rule, 0, account);
		}

		[NUnit.Framework.Test]
		public void HOCApplicationMortgageLoanCollectionTestFail()
		{
			HOCApplicationMortgageLoanCollection rule = new HOCApplicationMortgageLoanCollection();

			IAccount account = _mockery.StrictMock<IAccount>();
			IFinancialService hoc = _mockery.StrictMock<IFinancialService>();

			SetupResult.For(account.GetFinancialServiceByType(FinancialServiceTypes.HomeOwnersCover)).IgnoreArguments().Return(null);

			ExecuteRule(rule, 1, account);
		}

		[NUnit.Framework.Test]
		public void HOCRoofThatchAmountRequiredTestFail()
		{
			IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
			MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);
			HOCRoofDescriptionThatch rule = new HOCRoofDescriptionThatch();

			thatchAmt = 0.00;
			int hocKey = 0;
			SetupResult.For(hoc.Key).Return(hocKey);
			SetupResult.For(hoc.HOCThatchAmount).Return(thatchAmt);
			SetupResult.For(hocRoof.Key).Return((int)HOCRoofs.Thatch);
			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.SAHLHOC);

			//IEventList<IProperty> props = new EventList<IProperty>();
			IProperty prop = _mockery.StrictMock<IProperty>();
			IValuationDiscriminatedAdCheckDesktop val = _mockery.StrictMock<IValuationDiscriminatedAdCheckDesktop>();
			SetupResult.For(val.IsActive).Return(true);
			SetupResult.For(prop.LatestCompleteValuation).Return(val);
			//props.Add(Messages, prop);
			//SetupResult.For(hoc.Properties).Return(props);
			SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(prop);

			ExecuteRule(rule, 1, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCRoofThatchAmountRequiredTestPass()
		{
			IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
			MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);
			HOCRoofDescriptionThatch rule = new HOCRoofDescriptionThatch();

			thatchAmt = 300000;
			int hocKey = 0;
			SetupResult.For(hoc.Key).Return(hocKey);
			SetupResult.For(hoc.HOCThatchAmount).Return(thatchAmt);
			SetupResult.For(hocRoof.Key).Return((int)HOCRoofs.Thatch);
			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.SAHLHOC);

			//IEventList<IProperty> props = new EventList<IProperty>();
			IProperty prop = _mockery.StrictMock<IProperty>();
			IValuationDiscriminatedAdCheckDesktop val = _mockery.StrictMock<IValuationDiscriminatedAdCheckDesktop>();
			SetupResult.For(val.IsActive).Return(true);
			SetupResult.For(prop.LatestCompleteValuation).Return(val);
			//props.Add(Messages, prop);
			//SetupResult.For(hoc.Properties).Return(props);
			SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(prop);

			ExecuteRule(rule, 0, hoc);

		}

		[NUnit.Framework.Test]
		public void HOCRoofPartialAmountsRequiredTestFail()
		{
			IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
			MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);
			HOCRoofDescriptionCombined rule = new HOCRoofDescriptionCombined();

			thatchAmt = 0.0;
			conventionalAmt = 0.0;
			int hocKey = 0;
			SetupResult.For(hoc.Key).Return(hocKey);
			SetupResult.For(hoc.HOCThatchAmount).Return(thatchAmt);
			SetupResult.For(hoc.HOCConventionalAmount).Return(conventionalAmt);
			SetupResult.For(hocRoof.Key).Return((int)HOCRoofs.Partial);
			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.SAHLHOC);

			//IEventList<IProperty> props = new EventList<IProperty>();
			IProperty prop = _mockery.StrictMock<IProperty>();
			IValuationDiscriminatedAdCheckDesktop val = _mockery.StrictMock<IValuationDiscriminatedAdCheckDesktop>();
			SetupResult.For(val.IsActive).Return(true);
			SetupResult.For(prop.LatestCompleteValuation).Return(val);
			//props.Add(Messages, prop);
			//SetupResult.For(hoc.Properties).Return(props);
			SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(prop);
			ExecuteRule(rule, 1, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCRoofPartialAmountsRequiredTestPass()
		{
			IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
			MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);
			HOCRoofDescriptionCombined rule = new HOCRoofDescriptionCombined();

			thatchAmt = 50000;
			conventionalAmt = 200000;
			int hocKey = 0;
			SetupResult.For(hoc.Key).Return(hocKey);
			SetupResult.For(hoc.HOCThatchAmount).Return(thatchAmt);
			SetupResult.For(hoc.HOCConventionalAmount).Return(conventionalAmt);
			SetupResult.For(hocRoof.Key).Return((int)HOCRoofs.Partial);
			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.SAHLHOC);

			//IEventList<IProperty> props = new EventList<IProperty>();
			IProperty prop = _mockery.StrictMock<IProperty>();
			IValuationDiscriminatedAdCheckDesktop val = _mockery.StrictMock<IValuationDiscriminatedAdCheckDesktop>();
			SetupResult.For(val.IsActive).Return(true);
			SetupResult.For(prop.LatestCompleteValuation).Return(val);
			//props.Add(Messages, prop);
			//SetupResult.For(hoc.Properties).Return(props);
			SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(prop);

			ExecuteRule(rule, 0, hoc);
		}

		/// <summary>
		/// Ensure that the Lightstone Valuations are ignored in this rule
		/// </summary>
		[NUnit.Framework.Test]
		public void HOCTotalSumAssuredLessThanPreviousIgnoreLightstone()
		{
			//Setup
			IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
			MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);
			HOCTotalSumAssuredLessThanPrevious rule = new HOCTotalSumAssuredLessThanPrevious();

			//Setup default scenario
			int hocKey = 1;
			SetupResult.For(hoc.Key).Return(hocKey);

			//Setup the SAHL.Common.BusinessModel.Interfaces.IProperty
			IProperty prop = _mockery.StrictMock<IProperty>();

			IValuationDiscriminatedLightstoneAVM val = _mockery.StrictMock<IValuationDiscriminatedLightstoneAVM>();

			SetupResult.For(val.IsActive).Return(true);
			SetupResult.For(prop.LatestCompleteValuation).Return(val);

			SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(prop);

			//Ignore rule for Lightstone Valuation
			ExecuteRule(rule, 0, hoc);
		}

		/// <summary>
		/// Ensure that the rule New HOCTotalSumInsured > Old HOCTotalSumInsured
		/// </summary>
		[NUnit.Framework.Test]
		public void HOCTotalSumAssuredLessThanPreviousPass()
		{
			//Setup
			IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
			MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);
			HOCTotalSumAssuredLessThanPrevious rule = new HOCTotalSumAssuredLessThanPrevious();

			//Setup default scenario
			int hocKey = 1;
			SetupResult.For(hoc.Key).Return(hocKey);

			//Setup the SAHL.Common.BusinessModel.Interfaces.IProperty
			IProperty prop = _mockery.StrictMock<IProperty>();

			IValuationDiscriminatedAdCheckPhysical val = _mockery.StrictMock<IValuationDiscriminatedAdCheckPhysical>();

			SetupResult.For(val.IsActive).Return(true);
			SetupResult.For(prop.LatestCompleteValuation).Return(val);

			SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(prop);

			//If New Insured Sum > Original Insured Sum
			//Setup the Original HOC
			IHOC originalHoc = _mockery.StrictMock<IHOC>();

			SetupResult.For(hoc.HOCTotalSumInsured).Return(2);
			SetupResult.For(originalHoc.HOCTotalSumInsured).Return(1);
			hoc.Stub(hocMock => hocMock.Original).Return(originalHoc);

			ExecuteRule(rule, 0, hoc);
		}

		/// <summary>
		/// Ensure that the rule New HOCTotalSumInsured > Old HOCTotalSumInsured
		/// </summary>
		[NUnit.Framework.Test]
		public void HOCTotalSumAssuredLessThanPreviousFail()
		{
			//Setup
			IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
			MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);
			HOCTotalSumAssuredLessThanPrevious rule = new HOCTotalSumAssuredLessThanPrevious();

			//Setup default scenario
			int hocKey = 1;
			SetupResult.For(hoc.Key).Return(hocKey);

			//Setup the SAHL.Common.BusinessModel.Interfaces.IProperty
			IProperty prop = _mockery.StrictMock<IProperty>();

			IValuationDiscriminatedAdCheckPhysical val = _mockery.StrictMock<IValuationDiscriminatedAdCheckPhysical>();

			SetupResult.For(val.IsActive).Return(true);
			SetupResult.For(prop.LatestCompleteValuation).Return(val);

			SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(prop);

			//If New Insured Sum < Original Insured Sum
			//We are expecting to see a message count of 1
			//Setup the Original HOC
			IHOC originalHoc = _mockery.StrictMock<IHOC>();

			SetupResult.For(hoc.HOCTotalSumInsured).Return(1);
			SetupResult.For(originalHoc.HOCTotalSumInsured).Return(2);
			hoc.Stub(hocMock => hocMock.Original).Return(originalHoc);

			ExecuteRule(rule, 1, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCLoanClosedTestFail()
		{
			HOCLoanClosed rule = new HOCLoanClosed();

			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.LoanCancelled_Closed);
			SetupResult.For(hocStatus.Key).Return((int)HocStatuses.Open);

			ExecuteRule(rule, 1, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCLoanClosedTestPass()
		{
			HOCLoanClosed rule = new HOCLoanClosed();

			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.LoanCancelled_Closed);
			SetupResult.For(hocStatus.Key).Return((int)HocStatuses.Closed);

			ExecuteRule(rule, 0, hoc);
		}


		[NUnit.Framework.Test]
		public void HOCPolicyNumberNotNullTestFail()
		{
			HOCPolicyNumberNotNull rule = new HOCPolicyNumberNotNull();

			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.ABSAInsuranceCompanyLtd);
			SetupResult.For(hoc.HOCPolicyNumber).Return("");

			ExecuteRule(rule, 1, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCPolicyNumberNotNullTestPass()
		{
			HOCPolicyNumberNotNull rule = new HOCPolicyNumberNotNull();

			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.ABSAInsuranceCompanyLtd);
			SetupResult.For(hoc.HOCPolicyNumber).Return("33333333");

			ExecuteRule(rule, 0, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCPaidUpNoHOCTestPass()
		{
			HOCPaidUpNoHOC rule = new HOCPaidUpNoHOC();

			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.PaidupwithnoHOC);
			SetupResult.For(hocStatus.Key).Return((int)HocStatuses.PaidUpwithnoHOC);

			ExecuteRule(rule, 0, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCPaidUpNoHOCTestFail()
		{
			HOCPaidUpNoHOC rule = new HOCPaidUpNoHOC();

			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.PaidupwithnoHOC);
			SetupResult.For(hocStatus.Key).Return((int)HocStatuses.Open);

			ExecuteRule(rule, 1, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCSAHLCalculatePremiumTestFail()
		{
			HOCSAHLCalculatePremium rule = new HOCSAHLCalculatePremium();

			double premium = 0.0;

			SetupResult.For(hoc.Key).Return(99);
			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.SAHLHOC);
			SetupResult.For(hoc.HOCMonthlyPremium).Return(premium);
			IAccount acc = _mockery.StrictMock<IAccount>();
			IFinancialService fs = _mockery.StrictMock<IFinancialService>();
			SetupResult.For(fs.Account).Return(acc);
			SetupResult.For(hoc.FinancialService).Return(fs);
			IAccountStatus accStatus = _mockery.StrictMock<IAccountStatus>();
			SetupResult.For(acc.AccountStatus).Return(accStatus);
			SetupResult.For(accStatus.Key).Return((int)AccountStatuses.Open);

			ExecuteRule(rule, 1, hoc);
		}


		[NUnit.Framework.Test]
		public void HOCSAHLCalculatePremiumTestPass()
		{
			HOCSAHLCalculatePremium rule = new HOCSAHLCalculatePremium();

			double premium = 150.0;

			SetupResult.For(hoc.Key).Return(99);
			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.SAHLHOC);
			SetupResult.For(hoc.HOCMonthlyPremium).Return(premium);
			IAccount acc = _mockery.StrictMock<IAccount>();
			IFinancialService fs = _mockery.StrictMock<IFinancialService>();
			SetupResult.For(fs.Account).Return(acc);
			SetupResult.For(hoc.FinancialService).Return(fs);
			IAccountStatus accStatus = _mockery.StrictMock<IAccountStatus>();
			SetupResult.For(acc.AccountStatus).Return(accStatus);
			SetupResult.For(accStatus.Key).Return((int)AccountStatuses.Open);

			ExecuteRule(rule, 0, hoc);
		}


		[NUnit.Framework.Test]
		public void HOCSAHLCalculatePremiumUnOpenAccountTestPass()
		{
			HOCSAHLCalculatePremium rule = new HOCSAHLCalculatePremium();

			double premium = 0;

			SetupResult.For(hoc.Key).Return(0);
			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.SAHLHOC);
			SetupResult.For(hoc.HOCMonthlyPremium).Return(premium);
			IAccount acc = _mockery.StrictMock<IAccount>();
			IFinancialService fs = _mockery.StrictMock<IFinancialService>();
			SetupResult.For(fs.Account).Return(acc);
			SetupResult.For(hoc.FinancialService).Return(fs);
			IAccountStatus accStatus = _mockery.StrictMock<IAccountStatus>();
			SetupResult.For(acc.AccountStatus).Return(accStatus);
			SetupResult.For(accStatus.Key).Return((int)AccountStatuses.Application);

			ExecuteRule(rule, 0, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCCededStatusTestFail()
		{
			//HOCCededStatus rule = new HOCCededStatus();

			//SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.ABSAInsuranceCompanyLtd);

			//ExecuteRule(rule, 1, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCCededStatusTestPass()
		{
			HOCCededStatus rule = new HOCCededStatus();

			SetupResult.For(hocinsurer.Key).Return((int)HOCInsurers.SAHLHOC);

			ExecuteRule(rule, 0, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCTotalSumInsuredMinimumTestFail()
		{
			HOCTotalSumInsuredMinimum rule = new HOCTotalSumInsuredMinimum();

			double minSumInsured = 150000;
			double sumInsured = 50000;

			SetupResult.For(hoc.HOCTotalSumInsured).Return(sumInsured);

			IRuleRepository RuleRepo = _mockery.StrictMock<IRuleRepository>();

			MockCache.Add(typeof(IRuleRepository).ToString(), RuleRepo);
			IRuleItem ruleItem = _mockery.StrictMock<IRuleItem>();

			SetupResult.For(RuleRepo.FindRuleItemByTypeName("")).IgnoreArguments().Return(ruleItem);

			// Setup ruleItem.parameters
			IEventList<IRuleParameter> ruleParameters = new EventList<IRuleParameter>();
			IRuleParameter ruleParameter = _mockery.StrictMock<IRuleParameter>();

			SetupResult.For(ruleParameter.Value).Return(minSumInsured.ToString());
			ruleParameters.Add(Messages, ruleParameter);
			SetupResult.For(ruleItem.RuleParameters).Return(ruleParameters);

			// Setup ruleItem.Name
			SetupResult.For(ruleItem.Name).Return("HOCTotalSumInsuredMinimum");

			ExecuteRule(rule, 1, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCTotalSumInsuredMinimumTestPass()
		{
			HOCTotalSumInsuredMinimum rule = new HOCTotalSumInsuredMinimum();

			double minSumInsured = 150000;
			double sumInsured = 500000;

			SetupResult.For(hoc.HOCTotalSumInsured).Return(sumInsured);

			IRuleRepository RuleRepo = _mockery.StrictMock<IRuleRepository>();

			MockCache.Add(typeof(IRuleRepository).ToString(), RuleRepo);
			IRuleItem ruleItem = _mockery.StrictMock<IRuleItem>();

			SetupResult.For(RuleRepo.FindRuleItemByTypeName("")).IgnoreArguments().Return(ruleItem);

			// Setup ruleItem.parameters
			IEventList<IRuleParameter> ruleParameters = new EventList<IRuleParameter>();
			IRuleParameter ruleParameter = _mockery.StrictMock<IRuleParameter>();

			SetupResult.For(ruleParameter.Value).Return(minSumInsured.ToString());
			ruleParameters.Add(Messages, ruleParameter);
			SetupResult.For(ruleItem.RuleParameters).Return(ruleParameters);

			// Setup ruleItem.Name
			SetupResult.For(ruleItem.Name).Return("HOCTotalSumInsuredMinimum");

			ExecuteRule(rule, 0, hoc);
		}

		[NUnit.Framework.Test]
		public void HOCExistsNoHOCTestFail()
		{
			HOCExists rule = new HOCExists();

			IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
			IApplicationType appType = _mockery.StrictMock<IApplicationType>();
			IProperty prop = _mockery.StrictMock<IProperty>();
			SetupResult.For(appType.Key).Return((int)OfferTypes.SwitchLoan);
			SetupResult.For(appML.ApplicationType).Return(appType);
			SetupResult.For(appML.Property).Return(prop);
			IAccount hocAccount = _mockery.StrictMock<IAccount>();
			IEventList<IAccount> lstAccount = new EventList<IAccount>();
			IEventList<IFinancialService> lstFS = new EventList<IFinancialService>();
			IProduct product = _mockery.StrictMock<IProduct>();
			SetupResult.For(product.Key).Return((int)SAHL.Common.Globals.Products.LifePolicy);
			SetupResult.For(hocAccount.Product).Return(product);
			lstAccount.Add(null, hocAccount);
			SetupResult.For(appML.RelatedAccounts).Return(lstAccount);
			ExecuteRule(rule, 1, appML);
		}

		[NUnit.Framework.Test]
		public void HOCExistsTestPass()
		{
			HOCExists rule = new HOCExists();

			IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
			IApplicationType appType = _mockery.StrictMock<IApplicationType>();
			IProperty prop = _mockery.StrictMock<IProperty>();
			SetupResult.For(appType.Key).Return((int)OfferTypes.SwitchLoan);
			SetupResult.For(appML.ApplicationType).Return(appType);
			SetupResult.For(appML.Property).Return(prop);
			IAccount hocAccount = _mockery.StrictMock<IAccount>();
			IEventList<IAccount> lstAccount = new EventList<IAccount>();
			IEventList<IFinancialService> lstFS = new EventList<IFinancialService>();
			IProduct product = _mockery.StrictMock<IProduct>();
			SetupResult.For(product.Key).Return((int)SAHL.Common.Globals.Products.HomeOwnersCover);
			SetupResult.For(hocAccount.Product).Return(product);
			lstAccount.Add(null, hocAccount);
			SetupResult.For(appML.RelatedAccounts).Return(lstAccount);

			ExecuteRule(rule, 0, appML);
		}

		#region HOCTitleTypeSectionalTitle

		[NUnit.Framework.Test]
		public void HOCTitleTypeSectionalTitleTest()
		{
			//// APPLICATION TESTS
			// PASS
			HOCTitleTypeSectionalTitleTestHelper(0, (int)TitleTypes.SectionalTitle, (int)HOCInsurers.SectionalTitle);
			// PASS
			HOCTitleTypeSectionalTitleTestHelper(0, (int)TitleTypes.Leasehold, (int)HOCInsurers.SectionalTitle);
			// FAIL
			HOCTitleTypeSectionalTitleTestHelper(1, (int)TitleTypes.SectionalTitle, (int)HOCInsurers.SantamBeperk);


		}

		[NUnit.Framework.Test]
		public void HOCTitleTypeSectionalTitleTestWF()
		{
			//// WORKFLOW TESTS
			// PASS
			HOCTitleTypeSectionalTitleTestHelperWF(0, (int)TitleTypes.SectionalTitle, (int)HOCInsurers.SectionalTitle);
			// PASS
			HOCTitleTypeSectionalTitleTestHelperWF(0, (int)TitleTypes.Leasehold, (int)HOCInsurers.SectionalTitle);
			// FAIL
			HOCTitleTypeSectionalTitleTestHelperWF(1, (int)TitleTypes.SectionalTitle, (int)HOCInsurers.SantamBeperk);

		}

		private void HOCTitleTypeSectionalTitleTestHelper(int expectedMessageCount, int varTitleType, int varHocInsurer)
		{
			using (new SessionScope())
			{
				IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
				MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);
				HOCTitleTypeSectionalTitle rule = new HOCTitleTypeSectionalTitle();

				IHOC hoc = _mockery.StrictMock<IHOC>();

				//IEventList<IProperty> properties = new EventList<IProperty>();

				int hocKey = 0;
				SetupResult.For(hoc.Key).Return(hocKey);
				IProperty property = _mockery.StrictMock<IProperty>();
				ITitleType titleType = _mockery.StrictMock<ITitleType>();
				SetupResult.For(titleType.Key).Return(varTitleType);
				SetupResult.For(property.TitleType).Return(titleType);
				IHOCInsurer hocInsurer = _mockery.StrictMock<IHOCInsurer>();
				SetupResult.For(hocInsurer.Key).Return(varHocInsurer);

				//properties.Add(new DomainMessageCollection(), property);
				SetupResult.For(hoc.HOCInsurer).Return(hocInsurer);

				//SetupResult.For(hoc.Properties).Return(properties);
				SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(property);

				ExecuteRule(rule, expectedMessageCount, hoc);
			}

		}

		private void HOCTitleTypeSectionalTitleTestHelperWF(int expectedMessageCount, int varTitleType, int varHocInsurer)
		{
			using (new SessionScope())
			{
				IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
				MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);

				HOCTitleTypeSectionalTitle rule = new HOCTitleTypeSectionalTitle();
				SetRepositoryStrategy(TypeFactoryStrategy.Mock);
				IHOCRepository hocRepo = _mockery.StrictMock<IHOCRepository>();
				MockCache.Add(typeof(IHOCRepository).ToString(), hocRepo);
				IHOC _hoc = _mockery.StrictMock<IHOC>();
				int hocKey = 0;
				SetupResult.For(_hoc.Key).Return(hocKey);
				IApplication application = _mockery.StrictMock<IApplication>();
				IAccountHOC accHoc = _mockery.StrictMock<IAccountHOC>();
				//IEventList<IProperty> properties = new EventList<IProperty>();
				int applicationKey = 1;
				IProperty property = _mockery.StrictMock<IProperty>();
				ITitleType titleType = _mockery.StrictMock<ITitleType>();
				SetupResult.For(titleType.Key).Return(varTitleType);
				SetupResult.For(property.TitleType).Return(titleType);
				IHOCInsurer hocInsurer = _mockery.StrictMock<IHOCInsurer>();
				SetupResult.For(hocInsurer.Key).Return(varHocInsurer);
				SetupResult.For(application.Key).Return(applicationKey);
				//properties.Add(new DomainMessageCollection(), property);
				SetupResult.For(_hoc.HOCInsurer).Return(hocInsurer);

				//SetupResult.For(_hoc.Properties).Return(properties);
				SetupResult.For(accHoc.HOC).Return(_hoc);
				SetupResult.For(hocRepo.RetrieveHOCByOfferKey(applicationKey)).IgnoreArguments().Return(accHoc);

				SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(property);

				ExecuteRule(rule, expectedMessageCount, application);
			}
		}

		#endregion

		#region HOCValuationExpiredCheck

		[Test]
		public void HOCValuationExpiredCheckTestPass()
		{
			HOCValuationExpiredCheck rule = new HOCValuationExpiredCheck();
			SetRepositoryStrategy(TypeFactoryStrategy.Mock);
			IHOC hoc = _mockery.StrictMock<IHOC>();
			IProperty property = _mockery.StrictMock<IProperty>();
			IValuation valuation = _mockery.StrictMock<IValuation>();
			IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
			MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);
			DateTime dt = DateTime.Now;
			SetupResult.For(valuation.ValuationDate).Return(dt);
			SetupResult.For(property.LatestCompleteValuation).Return(valuation);
			SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(property);
			ExecuteRule(rule, 0, hoc);
		}

		[Test]
		public void HOCValuationExpiredCheckTestFail()
		{
			HOCValuationExpiredCheck rule = new HOCValuationExpiredCheck();
			SetRepositoryStrategy(TypeFactoryStrategy.Mock);
			IHOC hoc = _mockery.StrictMock<IHOC>();
			IProperty property = _mockery.StrictMock<IProperty>();
			IValuation valuation = _mockery.StrictMock<IValuation>();
			IPropertyRepository propRepo = _mockery.StrictMock<IPropertyRepository>();
			MockCache.Add(typeof(IPropertyRepository).ToString(), propRepo);
			DateTime dt = DateTime.Now.AddYears(-3);
			SetupResult.For(valuation.ValuationDate).Return(dt);
			SetupResult.For(property.LatestCompleteValuation).Return(valuation);
			SetupResult.For(propRepo.GetPropertyByHOC(hoc)).IgnoreArguments().Return(property);
			ExecuteRule(rule, 1, hoc);
		}

		#endregion

		/// <summary>
		/// When the HOC Insurer is of SAHL HOC and there is a Detail Type of HOCNoHOC, the rule will fail
		/// </summary>
		[NUnit.Framework.Test]
		public void HOCNoHOCDetailTypeToSAHLHOCFail()
		{
			HOCNoHOCDetailTypeToSAHLHOC rule = new HOCNoHOCDetailTypeToSAHLHOC();

			IHOC hoc = _mockery.StrictMock<IHOC>();
			IHOCInsurer hocInsurer = _mockery.StrictMock<IHOCInsurer>();
			IAccount account = _mockery.StrictMock<IAccount>();
			IAccount parentAccount = _mockery.StrictMock<IAccount>();
			IOriginationSourceProduct osp = _mockery.StrictMock<IOriginationSourceProduct>();
			IDetail detail = _mockery.StrictMock<IDetail>();
			IAccountRepository accountRepository = _mockery.StrictMock<IAccountRepository>();

			SetupResult.For(hoc.HOCInsurer).Return(hocInsurer);
			SetupResult.For(hocInsurer.Key).Return((int)HOCInsurers.SAHLHOC);

			IEventList<IAccount> parentAccounts = new EventList<IAccount>(new IAccount[] { parentAccount });
			SetupResult.For(parentAccount.Key).Return(1);
			SetupResult.For(account.OriginationSourceProduct).Return(osp);

			IFinancialService fs = _mockery.StrictMock<IFinancialService>();
			SetupResult.For(fs.Account).Return(account);
			SetupResult.For(hoc.FinancialService).Return(fs);
			SetupResult.For(account.ParentAccount).Return(parentAccount);

			IReadOnlyEventList<IDetail> detailTypes = new ReadOnlyEventList<IDetail>(new IDetail[] { detail });
			SetupResult.For(accountRepository.GetDetailByAccountKeyAndDetailType(1, (int)DetailTypes.HOCNoHoc)).Return(detailTypes).IgnoreArguments();

			MockCache.Add(typeof(IAccountRepository).ToString(), accountRepository);

			ExecuteRule(rule, 1, hoc);
		}

		/// <summary>
		/// When the HOC Insurer is of SAHL HOC and there is a Detail Type of HOCNoHOC, the rule will pass
		/// </summary>
		[NUnit.Framework.Test]
		public void HOCNoHOCDetailTypeToSAHLHOCPass()
		{
			HOCNoHOCDetailTypeToSAHLHOC rule = new HOCNoHOCDetailTypeToSAHLHOC();

			IHOC hoc = _mockery.StrictMock<IHOC>();
			IHOCInsurer hocInsurer = _mockery.StrictMock<IHOCInsurer>();
			IAccount account = _mockery.StrictMock<IAccount>();
			IAccount parentAccount = _mockery.StrictMock<IAccount>();
			IOriginationSourceProduct osp = _mockery.StrictMock<IOriginationSourceProduct>();
			IDetail detail = _mockery.StrictMock<IDetail>();
			IAccountRepository accountRepository = _mockery.StrictMock<IAccountRepository>();

			SetupResult.For(hoc.HOCInsurer).Return(hocInsurer);
			SetupResult.For(hocInsurer.Key).Return((int)HOCInsurers.SAHLHOC);

			IEventList<IAccount> parentAccounts = new EventList<IAccount>(new IAccount[] { parentAccount });
			SetupResult.For(parentAccount.Key).Return(1);
			SetupResult.For(account.OriginationSourceProduct).Return(osp);
			SetupResult.For(account.ParentAccount).Return(parentAccount);

			IFinancialService fs = _mockery.StrictMock<IFinancialService>();
			SetupResult.For(fs.Account).Return(account);
			SetupResult.For(hoc.FinancialService).Return(fs);

			IReadOnlyEventList<IDetail> detailTypes = new ReadOnlyEventList<IDetail>();
			SetupResult.For(accountRepository.GetDetailByAccountKeyAndDetailType(1, (int)DetailTypes.HOCNoHoc)).Return(detailTypes).IgnoreArguments();

			MockCache.Add(typeof(IAccountRepository).ToString(), accountRepository);

			ExecuteRule(rule, 0, hoc);
		}

		/// <summary>
		/// When the HOC Insurer is of other insurer and there is a Detail Type of HOCNoHOC, the rule will pass
		/// </summary>
		[NUnit.Framework.Test]
		public void HOCNoHOCDetailTypeToSAHLHOCOtherInsurerPass()
		{
			HOCNoHOCDetailTypeToSAHLHOC rule = new HOCNoHOCDetailTypeToSAHLHOC();

			IHOC hoc = _mockery.StrictMock<IHOC>();
			IHOCInsurer hocInsurer = _mockery.StrictMock<IHOCInsurer>();
			IAccount account = _mockery.StrictMock<IAccount>();
			IAccount parentAccount = _mockery.StrictMock<IAccount>();
			IDetail detail = _mockery.StrictMock<IDetail>();
			IAccountRepository accountRepository = _mockery.StrictMock<IAccountRepository>();

			SetupResult.For(hoc.HOCInsurer).Return(hocInsurer);
			SetupResult.For(hocInsurer.Key).Return((int)HOCInsurers.ABSAInsuranceCompanyLtd);

			IEventList<IAccount> parentAccounts = new EventList<IAccount>(new IAccount[] { parentAccount });
			SetupResult.For(parentAccount.Key).Return(1);

			IFinancialService fs = _mockery.StrictMock<IFinancialService>();
			SetupResult.For(fs.Account).Return(account);
			SetupResult.For(hoc.FinancialService).Return(fs);

			SetupResult.For(account.ParentAccount).Return(parentAccount);

			IReadOnlyEventList<IDetail> detailTypes = new ReadOnlyEventList<IDetail>();
			SetupResult.For(accountRepository.GetDetailByAccountKeyAndDetailType(1, (int)DetailTypes.HOCNoHoc)).Return(detailTypes).IgnoreArguments();

			MockCache.Add(typeof(IAccountRepository).ToString(), accountRepository);

			ExecuteRule(rule, 0, hoc);
		}
	}
}

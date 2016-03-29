using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Rules.BankAccount;
using System.Data;

namespace SAHL.Common.BusinessModel.Rules.Test.BankAccount
{
    [TestFixture]
    public class ApplicationDebitOrder : RuleBase
    {
        IApplicationDebitOrder appDebitOrder;

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
        
        [NUnit.Framework.Test]
        public void ApplicationDebitOrderPaymentBankAccountSusbidySelectionPass()
        {
            ApplicationDebitOrderPaymentBankAccountSelection rule = new ApplicationDebitOrderPaymentBankAccountSelection();

            appDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            IFinancialServicePaymentType fsPaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(appDebitOrder.FinancialServicePaymentType).Return(fsPaymentType);
            SetupResult.For(fsPaymentType.Key).Return((int)FinancialServicePaymentTypes.SubsidyPayment);
            SetupResult.For(appDebitOrder.BankAccount).Return(null);

            ExecuteRule(rule, 0, appDebitOrder);
        }

        [NUnit.Framework.Test]
        public void ApplicationDebitOrderPaymentBankAccountNullTestFail()
        {
            ApplicationDebitOrderPaymentBankAccountSelection rule = new ApplicationDebitOrderPaymentBankAccountSelection();

            appDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            IFinancialServicePaymentType fsPaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(appDebitOrder.FinancialServicePaymentType).Return(fsPaymentType);
            SetupResult.For(fsPaymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);
            SetupResult.For(appDebitOrder.BankAccount).Return(null);

            ExecuteRule(rule, 1, appDebitOrder);
        }

        [NUnit.Framework.Test]
        public void ApplicationDebitOrderPaymentBankAccountNotNullTestPass()
        {
            ApplicationDebitOrderPaymentBankAccountSelection rule = new ApplicationDebitOrderPaymentBankAccountSelection();

            appDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            IFinancialServicePaymentType fsPaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(appDebitOrder.FinancialServicePaymentType).Return(fsPaymentType);
            SetupResult.For(fsPaymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);
            IBankAccount ba = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(appDebitOrder.BankAccount).Return(ba);

            ExecuteRule(rule, 0, appDebitOrder);
        }

        [NUnit.Framework.Test]
        public void ApplicationDebitOrderPaymentBankAccountNonDOTestPass()
        {
            ApplicationDebitOrderPaymentBankAccountNonDO rule = new ApplicationDebitOrderPaymentBankAccountNonDO();

            appDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            IFinancialServicePaymentType fsPaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(appDebitOrder.FinancialServicePaymentType).Return(fsPaymentType);
            SetupResult.For(fsPaymentType.Key).Return((int)FinancialServicePaymentTypes.DirectPayment);
            SetupResult.For(appDebitOrder.BankAccount).Return(null);

            ExecuteRule(rule, 0, appDebitOrder);
        }


        [NUnit.Framework.Test]
        public void ApplicationDebitOrderPaymentBankAccountNonDOTestFail()
        {
            ApplicationDebitOrderPaymentBankAccountNonDO rule = new ApplicationDebitOrderPaymentBankAccountNonDO();

            appDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            IFinancialServicePaymentType fsPaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();

            SetupResult.For(appDebitOrder.FinancialServicePaymentType).Return(fsPaymentType);
            SetupResult.For(fsPaymentType.Key).Return((int)FinancialServicePaymentTypes.DirectPayment);
            IBankAccount ba = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(appDebitOrder.BankAccount).Return(ba);

            ExecuteRule(rule, 1, appDebitOrder);
        }

        [NUnit.Framework.Test]
        public void ApplicationDebitOrderCollectionTestFail()
        {
            ApplicationDebitOrderCollection rule = new ApplicationDebitOrderCollection();
            IEventList<IApplicationDebitOrder> appDO = _mockery.StrictMock<IEventList<IApplicationDebitOrder>>();
            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(app.ApplicationType).Return(appType);
            SetupResult.For(appType.Key).Return((int)OfferTypes.RefinanceLoan);
            SetupResult.For(app.ApplicationDebitOrders).Return(appDO);
            SetupResult.For(appDO.Count).Return(0);

            ExecuteRule(rule, 1, app);
        }

        [NUnit.Framework.Test]
        public void ApplicationDebitOrderCollectionTestPass()
        {
            ApplicationDebitOrderCollection rule = new ApplicationDebitOrderCollection();
            IEventList<IApplicationDebitOrder> appDO = new EventList<IApplicationDebitOrder>();
            IApplicationDebitOrder appDrOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            appDO.Add(Messages, appDrOrder);

            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(app.ApplicationType).Return(appType);
            SetupResult.For(appType.Key).Return((int)OfferTypes.SwitchLoan);
            SetupResult.For(app.ApplicationDebitOrders).Return(appDO);

            ExecuteRule(rule, 0, app);
        }

        [NUnit.Framework.Test]
        public void ApplicationDebitOrderCollectionFurtherAdvanceTestPass()
        {
            ApplicationDebitOrderCollection rule = new ApplicationDebitOrderCollection();
            IEventList<IApplicationDebitOrder> appDO = new EventList<IApplicationDebitOrder>();
            IApplicationDebitOrder appDrOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            appDO.Add(Messages, appDrOrder);

            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(app.ApplicationType).Return(appType);
            SetupResult.For(appType.Key).Return((int)OfferTypes.FurtherAdvance);
            SetupResult.For(app.ApplicationDebitOrders).Return(appDO);

            ExecuteRule(rule, 0, app);
        }

        [NUnit.Framework.Test]
        public void ApplicationDebitOrderCollectionFurtherLoanTestPass()
        {
            ApplicationDebitOrderCollection rule = new ApplicationDebitOrderCollection();
            IEventList<IApplicationDebitOrder> appDO = new EventList<IApplicationDebitOrder>();
            IApplicationDebitOrder appDrOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            appDO.Add(Messages, appDrOrder);

            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(app.ApplicationType).Return(appType);
            SetupResult.For(appType.Key).Return((int)OfferTypes.FurtherLoan);
            SetupResult.For(app.ApplicationDebitOrders).Return(appDO);

            ExecuteRule(rule, 0, app);
        }

        [NUnit.Framework.Test]
        public void ApplicationDebitOrderCollectionReAdvanceTestPass()
        {
            ApplicationDebitOrderCollection rule = new ApplicationDebitOrderCollection();
            IEventList<IApplicationDebitOrder> appDO = new EventList<IApplicationDebitOrder>();
            IApplicationDebitOrder appDrOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            appDO.Add(Messages, appDrOrder);

            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            SetupResult.For(app.ApplicationType).Return(appType);
            SetupResult.For(appType.Key).Return((int)OfferTypes.ReAdvance);
            SetupResult.For(app.ApplicationDebitOrders).Return(appDO);

            ExecuteRule(rule, 0, app);
        }

        #region ApplicationDebitOrderBankAccount

        // TODO: This test needs to be changed to use mocked data - we can't guarantee there is data in here to test
        [NUnit.Framework.Test]
        public void ApplicationDebitOrderBankAccountPass()
        {
            using (new SessionScope())
            {
                ApplicationDebitOrderBankAccount rule = new ApplicationDebitOrderBankAccount();

//                string HQL = @"select app from Application_DAO app
//                inner join app.ApplicationRoles r
//                inner join r.LegalEntity le
//                inner join le.LegalEntityBankAccounts leba
//                inner join app.ApplicationDebitOrders do
//                where do.BankAccount.Key = leba.BankAccount.Key";

                string sql = @"select top 1 o.OfferKey
                    from Offer o
                    inner join OfferRole ofr on o.OfferKey = ofr.OfferKey
                    inner join OfferRoleType ort on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
                    inner join LegalEntityBankAccount leba on leba.LegalEntityKey = ofr.LegalEntityKey
                    inner join OfferDebitOrder odo on odo.OfferKey = o.OfferKey
                    where odo.BankAccountKey = leba.BankAccountKey
                    and leba.GeneralStatusKey = 1
                    and ort.OfferRoleTypeGroupKey = 3
                    and odo.FinancialServicePaymentTypeKey = 1";
                DataTable dt = GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data to test with....");
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IApplication app = BMTM.GetMappedType<IApplication>(Application_DAO.Find(Convert.ToInt32(dt.Rows[0][0])));
                dt.Dispose();

                ExecuteRule(rule, 0, app);
            }
        }

        #endregion

		/// <summary>
		/// 
		/// </summary>
		[NUnit.Framework.Test]
		public void ApplicationDebitOrderPaymentTypeSubsidyTest()
		{
			//Pass
			ApplicationDebitOrderPaymentTypeSubsidyHelper(FinancialServicePaymentTypes.SubsidyPayment, EmploymentStatuses.Current, 0.0d, 100000.0d, 0);

			//Fail on the totalSubsidyStopOrderAmount < Convert.ToDouble(appVL.MonthlyInstalment)
			ApplicationDebitOrderPaymentTypeSubsidyHelper(FinancialServicePaymentTypes.SubsidyPayment, EmploymentStatuses.Current, 0, 0, 1);

			//Fail on the emp is IEmploymentSubsidised && emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current
			ApplicationDebitOrderPaymentTypeSubsidyHelper(FinancialServicePaymentTypes.SubsidyPayment, EmploymentStatuses.Previous, 0, 0, 1);
		}

		private void ApplicationDebitOrderPaymentTypeSubsidyHelper(FinancialServicePaymentTypes financialServicePaymentType, EmploymentStatuses employmentStatus, double? monthlyInstallment, double stopOrderAmount, int expectedErrorCount)
		{
			ApplicationDebitOrderPaymentTypeSubsidy rule = new ApplicationDebitOrderPaymentTypeSubsidy();

			IApplication application = _mockery.StrictMock<IApplication>();
			IApplicationDebitOrder applicationDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();
			IEmployment employment = _mockery.StrictMock<IEmployment>();
			IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
			IEmploymentSubsidised employmentSubsidised = _mockery.StrictMock<IEmploymentSubsidised>();
			IFinancialServicePaymentType paymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
			ISubsidy subsidy = _mockery.StrictMock<ISubsidy>();


			ILegalEntityNaturalPerson legalEntity = _mockery.StrictMock<ILegalEntityNaturalPerson>();
		
			IReadOnlyEventList<ILegalEntityNaturalPerson> applicants = new ReadOnlyEventList<ILegalEntityNaturalPerson>(new ILegalEntityNaturalPerson[]{legalEntity});
			IEventList<IEmployment> employments = new EventList<IEmployment>();

			employments.Add(Messages, employmentSubsidised);

			SetupResult.For(subsidy.StopOrderAmount).Return(stopOrderAmount);
			SetupResult.For(employmentSubsidised.Subsidy).Return(subsidy);
			SetupResult.For(paymentType.Key).Return((int)financialServicePaymentType);
			SetupResult.For(applicationDebitOrder.FinancialServicePaymentType).Return(paymentType);
			SetupResult.For(applicationDebitOrder.Application).Return(application);
			SetupResult.For(legalEntity.Employment).Return(employments);
			SetupResult.For(empStatus.Key).Return((int)employmentStatus);
			SetupResult.For(employment.EmploymentStatus).Return(empStatus);
			SetupResult.For(employmentSubsidised.EmploymentStatus).Return(empStatus);
			SetupResult.For(application.Key).Return(1);

			SetupResult.For(application.GetNaturalPersonLegalEntitiesByRoleType(Messages, new int[]{1}, GeneralStatusKey.Active)).IgnoreArguments().Return(applicants);

			IApplicationRepository applicationRepository = _mockery.StrictMock<IApplicationRepository>();
			MockCache.Add(typeof(IApplicationRepository).ToString(), applicationRepository);

			IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
			IApplicationInformationVariableLoan applicationInfoVariableLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();

			SetupResult.For(applicationInformation.Key).Return(1);
			SetupResult.For(application.GetLatestApplicationInformation()).IgnoreArguments().Return(applicationInformation);
			SetupResult.For(applicationRepository.GetApplicationInformationVariableLoan(1)).IgnoreArguments().Return(applicationInfoVariableLoan);

			SetupResult.For(applicationInfoVariableLoan.MonthlyInstalment).Return(monthlyInstallment);

			ExecuteRule(rule, expectedErrorCount, applicationDebitOrder);
		}
	}    
}

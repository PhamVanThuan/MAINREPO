using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.Application.FurtherLending;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.Application
{
    [TestFixture]
    public class FurtherLending : RuleBase
    {
        IAccount acc;
        IStageDefinitionRepository SDRepo;
        IApplicationRepository _appRepo;

        [NUnit.Framework.SetUp()]
        public override void Setup()
        {
            base.Setup();

            acc = _mockery.StrictMock<IAccount>();
            SDRepo = _mockery.StrictMock<IStageDefinitionRepository>();
            _appRepo = _mockery.StrictMock<IApplicationRepository>();

            MockCache.Add(typeof(IStageDefinitionRepository).ToString(), SDRepo);
            MockCache.Add(typeof(IApplicationRepository).ToString(), _appRepo);

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #region AccountDebtCounseling

        [NUnit.Framework.Test]
        public void AccountDebtCounselingPass()
        {
            AccountDebtCounseling rule = new AccountDebtCounseling();
            IAccount account = _mockery.StrictMock<IAccount>();

            SetupResult.For(account.UnderDebtCounselling).Return(false);
            ExecuteRule(rule, 0, account);
        }

        [NUnit.Framework.Test]
        public void AccountDebtCounselingFail()
        {
            AccountDebtCounseling rule = new AccountDebtCounseling();
            IAccount account = _mockery.StrictMock<IAccount>();

            SetupResult.For(account.UnderDebtCounselling).Return(true);
            ExecuteRule(rule, 1, account);
        }

        #endregion AccountDebtCounseling

        #region AccountDebtCounselingQuickCash

        [Ignore("replaced with Rule - AccountDebtCounseling")]
        [NUnit.Framework.Test]
        public void AccountDebtCounselingQuickCash()
        {
            AccountDebtCounselingQuickCash rule = new AccountDebtCounselingQuickCash();
            IAccount account = _mockery.StrictMock<IAccount>();

            SetupResult.For(account.UnderDebtCounselling).Return(true);

            ExecuteRule(rule, 1, acc);

            SetupResult.For(account.UnderDebtCounselling).Return(false);

            ExecuteRule(rule, 0, acc);
        }

        #endregion AccountDebtCounselingQuickCash

        #region AccountDebtCounselingLossControl

        [Ignore("replaced with Rule - AccountDebtCounseling")]
        [NUnit.Framework.Test]
        public void AccountDebtCounselingLossControl()
        {
            AccountDebtCounselingLossControl rule = new AccountDebtCounselingLossControl();

            IList<IStageTransition> list = new List<IStageTransition>();

            //No transitions, pass
            SetupResult.For(SDRepo.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(list);
            SetupResult.For(acc.Key).Return(1);

            ExecuteRule(rule, 0, acc);

            IList<IStageTransition> list1 = new List<IStageTransition>();

            //Add more in's than outs, fail
            AddTransitionToList(list1, (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlIn, 4);
            AddTransitionToList(list1, (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlOut, 3);

            SetupResult.For(SDRepo.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(list1);
            SetupResult.For(acc.Key).Return(1);

            ExecuteRule(rule, 1, acc);

            //Add equal in's and outs, pass
            IList<IStageTransition> list2 = new List<IStageTransition>();
            AddTransitionToList(list2, (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlIn, 1);
            AddTransitionToList(list2, (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlOut, 1);

            SetupResult.For(SDRepo.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(list2);
            SetupResult.For(acc.Key).Return(1);

            ExecuteRule(rule, 0, acc);
        }

        #endregion AccountDebtCounselingLossControl

        #region AccountDebtCounselingLossControlExternal

        [Ignore("replaced with Rule - AccountDebtCounseling")]
        [NUnit.Framework.Test]
        public void AccountDebtCounselingLossControlExternal()
        {
            AccountDebtCounselingLossControlExternal rule = new AccountDebtCounselingLossControlExternal();

            IList<IStageTransition> list = new List<IStageTransition>();

            //No transitions, pass
            SetupResult.For(SDRepo.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(list);
            SetupResult.For(acc.Key).Return(1);

            ExecuteRule(rule, 0, acc);

            IList<IStageTransition> list1 = new List<IStageTransition>();

            //Add more in's than outs, fail
            AddTransitionToList(list1, (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlExternalIn, 4);
            AddTransitionToList(list1, (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlExternalOut, 3);

            SetupResult.For(SDRepo.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(list1);
            SetupResult.For(acc.Key).Return(1);

            ExecuteRule(rule, 1, acc);

            //Add equal in's and outs, pass
            IList<IStageTransition> list2 = new List<IStageTransition>();
            AddTransitionToList(list2, (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlExternalIn, 1);
            AddTransitionToList(list2, (int)StageDefinitionStageDefinitionGroups.DebtCounsellingLossControlExternalOut, 1);

            SetupResult.For(SDRepo.GetStageTransitionsByGenericKey(1)).IgnoreArguments().Return(list2);
            SetupResult.For(acc.Key).Return(1);

            ExecuteRule(rule, 0, acc);
        }

        #endregion AccountDebtCounselingLossControlExternal

        #region ApplicationFurtherLendingLoanAgreementAmount

        [NUnit.Framework.Test]
        public void ApplicationFurtherLendingLoanAgreementAmountTest()
        {
            //  RequestedAmount + CurrentBalance is greater than AgreementAmount = FAIL
            ApplicationFurtherLendingLoanAgreementAmountHelper(1, 50000.00, (int)FinancialServiceTypes.VariableLoan, (int)AccountStatuses.Open, 100000.00, 140000.00);

            //  FinancialServiceType is not variableloan or fixedloan = PASS
            ApplicationFurtherLendingLoanAgreementAmountHelper(0, 50000.00, (int)FinancialServiceTypes.LifePolicy, (int)AccountStatuses.Open, 100000.00, 140000.00);

            //  AccountStatus is Closed = PASS
            ApplicationFurtherLendingLoanAgreementAmountHelper(0, 50000.00, (int)FinancialServiceTypes.VariableLoan, (int)AccountStatuses.Closed, 100000.00, 140000.00);

            //  RequestedAmount + CurrentBalance is less than AgreementAmount = PASS
            ApplicationFurtherLendingLoanAgreementAmountHelper(0, 50000.00, (int)FinancialServiceTypes.VariableLoan, (int)AccountStatuses.Open, 100000.00, 160000.00);
        }

        /// <summary>
        /// Helper method to set up the expectations for the ApplicationFurtherLendingLoanAgreementAmountTest test.
        /// </summary>
        /// <param name="gs"></param>
        private void ApplicationFurtherLendingLoanAgreementAmountHelper(int expectedMessageCount, double requestedAmount, int finServType, int accStatus, double curBal, double laAmount)
        {
            ApplicationFurtherLendingLoanAgreementAmount rule = new ApplicationFurtherLendingLoanAgreementAmount();

            IApplicationFurtherLoan applicationFurtherLoan = _mockery.StrictMock<IApplicationFurtherLoan>();
            IAccount acc = _mockery.StrictMock<IAccount>();

            IEventList<IFinancialService> fss = new EventList<IFinancialService>();
            IMortgageLoan fs = _mockery.StrictMock<IMortgageLoan>();

            IFinancialServiceType fst = _mockery.StrictMock<IFinancialServiceType>();
            SetupResult.For(fst.Key).Return(finServType);
            SetupResult.For(fs.FinancialServiceType).Return(fst);

            IAccountStatus accStat = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(accStat.Key).Return(accStatus);
            SetupResult.For(fs.AccountStatus).Return(accStat);
            SetupResult.For(fs.CurrentBalance).Return(curBal);

            IEventList<IBond> bonds = new EventList<IBond>();
            IBond bond = _mockery.StrictMock<IBond>();

            IEventList<ILoanAgreement> loanAgreements = new EventList<ILoanAgreement>();
            ILoanAgreement loanAgreement = _mockery.StrictMock<ILoanAgreement>();

            SetupResult.For(loanAgreement.Amount).Return(laAmount);
            loanAgreements.Add(new DomainMessageCollection(), loanAgreement);

            SetupResult.For(bond.LoanAgreements).Return(loanAgreements);

            bonds.Add(new DomainMessageCollection(), bond);

            SetupResult.For(fs.Bonds).Return(bonds);
            fss.Add(new DomainMessageCollection(), fs);
            SetupResult.For(acc.FinancialServices).Return(fss);
            SetupResult.For(applicationFurtherLoan.Account).Return(acc);
            SetupResult.For(applicationFurtherLoan.RequestedCashAmount).Return(requestedAmount);

            ExecuteRule(rule, expectedMessageCount, applicationFurtherLoan);
        }

        #endregion ApplicationFurtherLendingLoanAgreementAmount

        private void AddTransitionToList(IList<IStageTransition> list, int sdsdgKey, int numberToAdd)
        {
            for (int i = 0; i < numberToAdd; i++)
            {
                IStageTransition st = _mockery.StrictMock<IStageTransition>();
                IStageDefinitionStageDefinitionGroup sdsdg = _mockery.StrictMock<IStageDefinitionStageDefinitionGroup>();

                SetupResult.For(sdsdg.Key).Return(sdsdgKey);
                SetupResult.For(st.StageDefinitionStageDefinitionGroup).Return(sdsdg);

                list.Add(st);
            }
        }

        [NUnit.Framework.Test]
        public void ApplicationFurtherLendingAccountCancellationTest()
        {
            ApplicationFurtherLendingAccountCancellation rule = new ApplicationFurtherLendingAccountCancellation();
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            IAccount acc = _mockery.StrictMock<IAccount>();
            IFinancialService fs = _mockery.StrictMock<IFinancialService>();
            IFinancialAdjustment fa = _mockery.StrictMock<IFinancialAdjustment>();
            IFinancialAdjustmentStatus financialAdjustmentStatus = _mockery.StrictMock<IFinancialAdjustmentStatus>();
            IFinancialAdjustmentTypeSource financialAdjustmentTypeSource = _mockery.StrictMock<IFinancialAdjustmentTypeSource>();

            List<IFinancialAdjustment> faLst = new List<IFinancialAdjustment>();
            faLst.Add(fa);
            IEventList<IFinancialAdjustment> faLstFO = new EventList<IFinancialAdjustment>(faLst);

            List<IFinancialService> fsLst = new List<IFinancialService>();
            fsLst.Add(fs);
            IEventList<IFinancialService> fsLstRO = new EventList<IFinancialService>(fsLst);

            SetupResult.For(acc.Key).Return(1);
            SetupResult.For(financialAdjustmentTypeSource.Key).Return(13);
            SetupResult.For(fa.FinancialAdjustmentTypeSource).Return(financialAdjustmentTypeSource);
            SetupResult.For(financialAdjustmentStatus.Key).Return(1);
            SetupResult.For(fa.FinancialAdjustmentStatus).Return(financialAdjustmentStatus);
            SetupResult.For(acc.FinancialServices).Return(fsLstRO);

            ExecuteRule(rule, 0, acc);
        }

        #region ApplicationFurtherLendingNonperformingLoanTest

        [NUnit.Framework.Test]
        public void ApplicationFurtherLendingNonperformingLoanTest()
        {
            ApplicationFurtherLendingNonperformingLoan rule = new ApplicationFurtherLendingNonperformingLoan();
            IFinancialServiceRepository financialServiceRepo = _mockery.StrictMock<IFinancialServiceRepository>();
            MockCache.Add(typeof(IFinancialServiceRepository).ToString(), financialServiceRepo);
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            acc = _mockery.StrictMock<IAccount>();

            // Pass - IsNonPerforming = True
            bool result = false;
            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(result);
            SetupResult.For(acc.Key).Return(1);
            ExecuteRule(rule, 0, acc);

            // FAIL - IsNonPerforming = False
            result = true;
            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(result);
            SetupResult.For(acc.Key).Return(1);
            ExecuteRule(rule, 1, acc);
        }

        #endregion ApplicationFurtherLendingNonperformingLoanTest

        private IAccountRepository _accRepo;

        private IAccountRepository AccRepo
        {
            get
            {
                if (_accRepo == null)
                    _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accRepo;
            }
        }

        #region ApplicationFurtherLendingRapidGoToCreditCheckLTV

        [Test]
        public void ApplicationFurtherLendingRapidGoToCreditCheckLTVTestPass()
        {
            using (new SessionScope())
            {
                ApplicationFurtherLendingRapidGoToCreditCheckLTV rule = new ApplicationFurtherLendingRapidGoToCreditCheckLTV();
                IApplicationFurtherLoan appFL = _mockery.StrictMock<IApplicationFurtherLoan>();
                SetupResult.For(_appRepo.RapidGoToCreditCheckLTV(appFL)).IgnoreArguments().Return(true);
                ExecuteRule(rule, 0, appFL);
            }
        }

        [Test]
        public void ApplicationFurtherLendingRapidGoToCreditCheckLTVTestFail()
        {
            using (new SessionScope())
            {
                ApplicationFurtherLendingRapidGoToCreditCheckLTV rule = new ApplicationFurtherLendingRapidGoToCreditCheckLTV();
                IApplicationFurtherLoan appFL = _mockery.StrictMock<IApplicationFurtherLoan>();
                SetupResult.For(_appRepo.RapidGoToCreditCheckLTV(appFL)).IgnoreArguments().Return(false);
                ExecuteRule(rule, 1, appFL);
            }
        }

        #endregion ApplicationFurtherLendingRapidGoToCreditCheckLTV

        #region ApplicationFurtherLendingAdditionalSuretyCompositeTest

        [Test]
        public void ApplicationFurtherLendingAdditionalSuretyCompositeTestPass()
        {
            ApplicationFurtherLendingAdditionalSuretyComposite rule = new ApplicationFurtherLendingAdditionalSuretyComposite();
            IApplication application = _mockery.StrictMock<IApplication>();
            int applicationKey = -1;
            SetupResult.For(application.Key).Return(applicationKey);
            SetupResult.For(SDRepo.CountCompositeStageOccurance(applicationKey, (int)StageDefinitionStageDefinitionGroups.AdditionalSuretyOnRapid, false)).IgnoreArguments().Return(0);
            SetupResult.For(SDRepo.CountCompositeStageOccurance(applicationKey, (int)StageDefinitionStageDefinitionGroups.AdditionalSuretyOnFA, false)).IgnoreArguments().Return(0);
            ExecuteRule(rule, 0, application);
        }

        [Test]
        public void ApplicationFurtherLendingAdditionalSuretyCompositeTestFail()
        {
            ApplicationFurtherLendingAdditionalSuretyComposite rule = new ApplicationFurtherLendingAdditionalSuretyComposite();
            IApplication application = _mockery.StrictMock<IApplication>();
            int applicationKey = -1;
            SetupResult.For(application.Key).Return(applicationKey);
            SetupResult.For(SDRepo.CountCompositeStageOccurance(applicationKey, (int)StageDefinitionStageDefinitionGroups.AdditionalSuretyOnRapid, false)).IgnoreArguments().Return(1);
            SetupResult.For(SDRepo.CountCompositeStageOccurance(applicationKey, (int)StageDefinitionStageDefinitionGroups.AdditionalSuretyOnFA, false)).IgnoreArguments().Return(1);
            ExecuteRule(rule, 1, application);
        }

        #endregion ApplicationFurtherLendingAdditionalSuretyCompositeTest

        #region Account Under Fore Closure

        /// <summary>
        /// Account Under Foreclosure Test
        /// </summary>
        [Test]
        public void AccountUnderForeClosureTest()
        {
            try
            {
                //Pass
                foreach (DetailTypes detailType in Enum.GetValues(typeof(DetailTypes)))
                {
                    if (detailType != DetailTypes.ForeclosureUnderway)
                    {
                        AccountUnderForeClosureHelper(0, detailType);
                    }
                }

                //Pass on FUW
                AccountUnderForeClosureHelper(0, DetailTypes.ForeclosureUnderway);

                //Fail
                AccountUnderForeClosureHelper(1, DetailTypes.ForeclosureUnderway);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Account Under Fore Closure Helper
        /// </summary>
        /// <param name="expectedErrorCount"></param>
        /// <param name="detailType"></param>
        private void AccountUnderForeClosureHelper(int expectedErrorCount, DetailTypes detailType)
        {
            AccountUnderForeClosure rule = new AccountUnderForeClosure();

            IApplicationFurtherLending application = _mockery.StrictMock<IApplicationFurtherLending>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IAccountRepository accountRepository = _mockery.StrictMock<IAccountRepository>();

            MockCache.Add(typeof(IAccountRepository).ToString(), accountRepository);

            SetupResult.For(application.Key).Return(0);
            SetupResult.For(account.Key).Return(0);
            SetupResult.For(application.Account).Return(account);

            IDetail detail = _mockery.StrictMock<IDetail>();
            SetupResult.For(detail.DetailDate).Return(DateTime.Now);

            IReadOnlyEventList<IDetail> details = new ReadOnlyEventList<IDetail>(new IDetail[] { detail });

            if (detailType == DetailTypes.ForeclosureUnderway)
            {
                SetupResult.For(accountRepository.GetDetailByAccountKeyAndDetailType(0, (int)detailType)).Return(details);
                if (expectedErrorCount == 0)
                    SetupResult.For(accountRepository.GetDetailByAccountKeyAndDetailType(0, (int)DetailTypes.LegalActionStopped)).Return(details);
                else
                    SetupResult.For(accountRepository.GetDetailByAccountKeyAndDetailType(0, (int)DetailTypes.LegalActionStopped)).Return(null);
            }
            else
            {
                SetupResult.For(accountRepository.GetDetailByAccountKeyAndDetailType(0, (int)DetailTypes.ForeclosureUnderway)).Return(null);
                SetupResult.For(accountRepository.GetDetailByAccountKeyAndDetailType(0, (int)detailType)).Return(null);
            }

            ExecuteRule(rule, expectedErrorCount, application);
        }

        #endregion Account Under Fore Closure

        #region ApplicationFurtherAdvanceLoansNewValuationRequired

        /// <summary>
        /// Application Further Advance Loans New Valuation Required
        /// </summary>
        [Test]
        public void ApplicationFurtherAdvanceLoansNewValuationRequiredTest()
        {
            //Pass
            ApplicationFurtherAdvanceLoansNewValuationRequiredHelper(0, DateTime.Now.AddYears(-2));

            //Fail
            ApplicationFurtherAdvanceLoansNewValuationRequiredHelper(1, DateTime.Now.AddYears(-2).AddDays(-1));
        }

        /// <summary>
        /// Application Further Advance Loans New Valutaion Required Helper
        /// </summary>
        /// <param name="expectedErrorCount"></param>
        /// <param name="latestValuationDate"></param>
        private void ApplicationFurtherAdvanceLoansNewValuationRequiredHelper(int expectedErrorCount, DateTime latestValuationDate)
        {
            ApplicationFurtherAdvanceLoansNewValuationRequired rule = new ApplicationFurtherAdvanceLoansNewValuationRequired();

            IApplicationFurtherAdvance application = _mockery.StrictMock<IApplicationFurtherAdvance>();

            IProperty property = _mockery.StrictMock<IProperty>();
            IEventList<IValuation> valuations = new EventList<IValuation>();

            IValuation valuation = _mockery.StrictMock<IValuation>();

            SetupResult.For(valuation.ValuationDate).Return(latestValuationDate);
            SetupResult.For(property.Valuations).Return(valuations);
            SetupResult.For(application.Property).Return(property);

            valuations.Add(Messages, valuation);

            ExecuteRule(rule, expectedErrorCount, application);
        }

        #endregion ApplicationFurtherAdvanceLoansNewValuationRequired

        #region ApplicationFurtherLendingAccountStatus

        /// <summary>
        /// Application Further Lending Account Status Test
        /// </summary>
        [Test]
        public void ApplicationFurtherLendingAccountStatusTest()
        {
            //Pass
            ApplicationFurtherLendingAccountStatusHelper(0, AccountStatuses.Open);

            //Fail
            foreach (AccountStatuses accountStatus in Enum.GetValues(typeof(AccountStatuses)))
            {
                if (accountStatus != AccountStatuses.Open)
                {
                    ApplicationFurtherLendingAccountStatusHelper(1, accountStatus);
                }
            }
        }

        /// <summary>
        /// Application Further Lending Account Status Helper
        /// </summary>
        /// <param name="expectedErrorCount"></param>
        /// <param name="accountStatus"></param>
        private void ApplicationFurtherLendingAccountStatusHelper(int expectedErrorCount, AccountStatuses accountStatus)
        {
            ApplicationFurtherLendingAccountStatus rule = new ApplicationFurtherLendingAccountStatus();
            IApplicationFurtherLending application = _mockery.StrictMock<IApplicationFurtherLending>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IAccountStatus status = _mockery.StrictMock<IAccountStatus>();

            SetupResult.For(status.Key).Return((int)accountStatus);
            SetupResult.For(account.AccountStatus).Return(status);
            SetupResult.For(application.Account).Return(account);

            ExecuteRule(rule, expectedErrorCount, application);
        }

        #endregion ApplicationFurtherLendingAccountStatus

        #region ApplicationFurtherLendingAccountForeClosure

        [Test]
        public void ApplicationFurtherLendingAccountForeClosureFail()
        {
            var rule = new ApplicationFurtherLendingAccountForeClosure();
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IAccount account = _mockery.StrictMock<IAccount>();
            IDetail detail = _mockery.StrictMock<IDetail>();

            //set accountKey
            SetupResult.For(account.Key)
                .Return(1);

            //set detailType
            SetupResult.For(detail.Key)
                .Return(1);
            IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();

            //set strategy
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);

            //need an ReadOnlyEventList of detail types
            IEventList<IDetail> details = new EventList<IDetail>();
            details.Add(new DomainMessageCollection(), detail);

            IReadOnlyEventList<IDetail> readOnlyDetail = new ReadOnlyEventList<IDetail>(details);
            SetupResult.For(accRepo.GetDetailByAccountKeyAndDetailType(1, 1))
                .IgnoreArguments()
                .Return(readOnlyDetail);

            //run the rule
            ExecuteRule(rule, 1, account);
        }

        [Test]
        public void ApplicationFurtherLendingAccountForeClosurePass()
        {
            var rule = new ApplicationFurtherLendingAccountForeClosure();
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            var account = _mockery.StrictMock<IAccount>();
            var detail = _mockery.StrictMock<IDetail>();

            //set accountKey
            SetupResult.For(account.Key).Return(1);
            var accRepo = _mockery.StrictMock<IAccountRepository>();

            //mock the repository call
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);
            SetupResult.For(accRepo.GetDetailByAccountKeyAndDetailType(1, 1))
                .IgnoreArguments()
                .Return(null);

            //run the rule
            ExecuteRule(rule, 0, account);
        }

        #endregion ApplicationFurtherLendingAccountForeClosure
    }
}
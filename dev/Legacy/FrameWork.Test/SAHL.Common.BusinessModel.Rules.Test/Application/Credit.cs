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
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules.Application.Credit;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Test.Application
{
    [TestFixture]
    public class Credit : RuleBase
    {
        /// <summary>
        /// This interface is created for mocking purposes only, for rules that case IApplicationProduct objects
        /// to ISupportsVariableLoanApplicationInformation objects.
        /// </summary>
        public interface IApplicationProductSupportsVariableLoanApplicationInformation : IApplicationProduct, ISupportsVariableLoanApplicationInformation
        {
        }

        [NUnit.Framework.SetUp()]
        public void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public void TearDown()
        {
            base.TearDown();
        }

        #region CreditDisqualificationMaxLTV

        [NUnit.Framework.Test]
        public void CreditDisqualificationMaxLTVFailed()
        {
            CreditDisqualificationMaxLTV cdMaxLTV = new CreditDisqualificationMaxLTV();

            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            ISupportsVariableLoanApplicationInformation vli = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan aivl = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            //Not worried about mocking the control repo, just set the LTV high enough for a rule fail
            SetupResult.For(aivl.LTV).Return(new double?(2D));
            SetupResult.For(appProd.VariableLoanInformation).Return(aivl);
            SetupResult.For(app.CurrentProduct).Return(appProd);

            ExecuteRule(cdMaxLTV, 1, app);
        }

        [NUnit.Framework.Test]
        public void CreditDisqualificationMaxLTVPass()
        {
            CreditDisqualificationMaxLTV cdMaxLTV = new CreditDisqualificationMaxLTV();

            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationProductSupportsVariableLoanApplicationInformation appProd = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
            ISupportsVariableLoanApplicationInformation vli = _mockery.StrictMock<ISupportsVariableLoanApplicationInformation>();
            IApplicationInformationVariableLoan aivl = _mockery.StrictMock<IApplicationInformationVariableLoan>();

            //Not worried about mocking the control repo, just set the LTV high enough for a rule fail
            SetupResult.For(aivl.LTV).Return(new double?(0.10D));
            SetupResult.For(appProd.VariableLoanInformation).Return(aivl);
            SetupResult.For(app.CurrentProduct).Return(appProd);

            ExecuteRule(cdMaxLTV, 0, app);
        }

        #endregion CreditDisqualificationMaxLTV

        #region ApplicationCheckMinLoanAmount

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCheckMinLoanAmountSuccess()
        {
            using (new SessionScope())
            {
                ApplicationCheckMinLoanAmount rule = new ApplicationCheckMinLoanAmount(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string sql = @"select min(aivl.OfferInformationKey) from CreditCriteria cc,
                                OfferInformationVariableLoan aivl,
                                OfferInformation ai,
                                OfferMortgageLoan aml
                                where cc.CreditMatrixKey = aivl.CreditMatrixKey and cc.CategoryKey = aivl.CategoryKey and cc.EmploymentTypeKey = aivl.EmploymentTypeKey
                                and ai.OfferInformationKey = aivl.OfferInformationKey and aml.OfferKey = ai.OfferKey
                                and cc.MortgageLoanPurposeKey = aml.MortgageLoanPurposeKey and cc.MinIncomeAmount < aivl.LoanAgreementAmount
                                ";
                DataTable dt = base.GetQueryResults(sql);
                int appKey = Convert.ToInt32(dt.Rows[0][0]);

                ApplicationInformationVariableLoan_DAO aivl = ApplicationInformationVariableLoan_DAO.Find(appKey);

                // Setup the correct object to pass along
                IApplication app = _mockery.StrictMock<IApplication>();
                SetupResult.For(app.Key).Return(aivl.ApplicationInformation.Application.Key);

                IApplicationProductSupportsVariableLoanApplicationInformation appProduct = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
                IApplicationInformationVariableLoan appInfoVarLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();

                SetupResult.For(appProduct.VariableLoanInformation).Return(appInfoVarLoan);
                SetupResult.For(appInfoVarLoan.Key).Return(aivl.Key);
                SetupResult.For(appInfoVarLoan.LoanAgreementAmount).Return(aivl.LoanAgreementAmount);

                SetupResult.For(app.CurrentProduct).Return(appProduct);

                ExecuteRule(rule, 0, app);
            }
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCheckMinLoanAmountFail()
        {
            using (new SessionScope())
            {
                ApplicationCheckMinLoanAmount rule = new ApplicationCheckMinLoanAmount(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string sql = @"select min(aivl.OfferInformationKey) from CreditCriteria cc,
                                OfferInformationVariableLoan aivl,
                                OfferInformation ai,
                                OfferMortgageLoan aml
                                where cc.CreditMatrixKey = aivl.CreditMatrixKey and cc.CategoryKey = aivl.CategoryKey and cc.EmploymentTypeKey = aivl.EmploymentTypeKey
                                and ai.OfferInformationKey = aivl.OfferInformationKey and aml.OfferKey = ai.OfferKey
                                and cc.MortgageLoanPurposeKey = aml.MortgageLoanPurposeKey and cc.MinIncomeAmount > aivl.LoanAgreementAmount
                                ";
                DataTable dt = base.GetQueryResults(sql);
                int appKey = Convert.ToInt32(dt.Rows[0][0]);

                ApplicationInformationVariableLoan_DAO aivl = ApplicationInformationVariableLoan_DAO.Find(appKey);

                // Setup the correct object to pass along
                IApplication app = _mockery.StrictMock<IApplication>();
                SetupResult.For(app.Key).Return(aivl.ApplicationInformation.Application.Key);

                IApplicationProductSupportsVariableLoanApplicationInformation appProduct = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
                IApplicationInformationVariableLoan appInfoVarLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();

                SetupResult.For(appProduct.VariableLoanInformation).Return(appInfoVarLoan);
                SetupResult.For(appInfoVarLoan.Key).Return(aivl.Key);
                SetupResult.For(appInfoVarLoan.LoanAgreementAmount).Return(aivl.LoanAgreementAmount);

                SetupResult.For(app.CurrentProduct).Return(appProduct);

                ExecuteRule(rule, 1, app);
            }
        }

        #endregion ApplicationCheckMinLoanAmount

        #region ApplicationCheckMinIncome

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCheckMinIncomeSuccess()
        {
            using (new SessionScope())
            {
                ApplicationCheckMinIncome rule = new ApplicationCheckMinIncome(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string sql = @"select min(aivl.OfferInformationKey) from CreditCriteria cc,
                                OfferInformationVariableLoan aivl,
                                OfferInformation ai,
                                OfferMortgageLoan aml
                                where cc.CreditMatrixKey = aivl.CreditMatrixKey and cc.CategoryKey = aivl.CategoryKey and cc.EmploymentTypeKey = aivl.EmploymentTypeKey
                                and ai.OfferInformationKey = aivl.OfferInformationKey and aml.OfferKey = ai.OfferKey
                                and cc.MortgageLoanPurposeKey = aml.MortgageLoanPurposeKey and cc.MinIncomeAmount < aivl.HouseholdIncome
                                ";
                DataTable dt = base.GetQueryResults(sql);
                int appKey = Convert.ToInt32(dt.Rows[0][0]);

                ApplicationInformationVariableLoan_DAO aivl = ApplicationInformationVariableLoan_DAO.Find(appKey);

                // Setup the correct object to pass along
                IApplication app = _mockery.StrictMock<IApplication>();
                SetupResult.For(app.Key).Return(aivl.ApplicationInformation.Application.Key);

                IApplicationProductSupportsVariableLoanApplicationInformation appProduct = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
                IApplicationInformationVariableLoan appInfoVarLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();

                SetupResult.For(appProduct.VariableLoanInformation).Return(appInfoVarLoan);
                SetupResult.For(appInfoVarLoan.Key).Return(aivl.Key);
                SetupResult.For(appInfoVarLoan.HouseholdIncome).Return(aivl.HouseholdIncome);

                SetupResult.For(app.CurrentProduct).Return(appProduct);

                ExecuteRule(rule, 0, app);
            }
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationCheckMinIncomeFail()
        {
            using (new SessionScope())
            {
                ApplicationCheckMinIncome rule = new ApplicationCheckMinIncome(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string sql = @"select min(aivl.OfferInformationKey) from CreditCriteria cc,
                                OfferInformationVariableLoan aivl,
                                OfferInformation ai,
                                OfferMortgageLoan aml
                                where cc.CreditMatrixKey = aivl.CreditMatrixKey and cc.CategoryKey = aivl.CategoryKey and cc.EmploymentTypeKey = aivl.EmploymentTypeKey
                                and ai.OfferInformationKey = aivl.OfferInformationKey and aml.OfferKey = ai.OfferKey
                                and cc.MortgageLoanPurposeKey = aml.MortgageLoanPurposeKey and cc.MinIncomeAmount > aivl.HouseholdIncome
                                ";
                DataTable dt = base.GetQueryResults(sql);
                int appKey = Convert.ToInt32(dt.Rows[0][0]);

                ApplicationInformationVariableLoan_DAO aivl = ApplicationInformationVariableLoan_DAO.Find(appKey);

                // Setup the correct object to pass along
                IApplication app = _mockery.StrictMock<IApplication>();
                SetupResult.For(app.Key).Return(aivl.ApplicationInformation.Application.Key);

                IApplicationProductSupportsVariableLoanApplicationInformation appProduct = _mockery.StrictMock<IApplicationProductSupportsVariableLoanApplicationInformation>();
                IApplicationInformationVariableLoan appInfoVarLoan = _mockery.StrictMock<IApplicationInformationVariableLoan>();

                SetupResult.For(appProduct.VariableLoanInformation).Return(appInfoVarLoan);
                SetupResult.For(appInfoVarLoan.Key).Return(aivl.Key);
                SetupResult.For(appInfoVarLoan.HouseholdIncome).Return(aivl.HouseholdIncome);

                SetupResult.For(app.CurrentProduct).Return(appProduct);

                ExecuteRule(rule, 1, app);
            }
        }

        #endregion ApplicationCheckMinIncome

        #region ApplicationConfirmIncome

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationConfirmIncomeFail()
        {
            ApplicationConfirmIncome rule = new ApplicationConfirmIncome();

            IApplication app = _mockery.StrictMock<IApplication>();

            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(appRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(app.ApplicationRoles).Return(applicationRoles);

            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.AssuredLife);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);

            IEventList<IApplicationRoleAttribute> appRoleAttrs = new EventList<IApplicationRoleAttribute>();
            SetupResult.For(appRole.ApplicationRoleAttributes).Return(appRoleAttrs);
            IApplicationRoleAttribute appRoleAttr = _mockery.StrictMock<IApplicationRoleAttribute>();
            appRoleAttrs.Add(Messages, appRoleAttr);

            IApplicationRoleAttributeType appRoleAttrType = _mockery.StrictMock<IApplicationRoleAttributeType>();
            SetupResult.For(appRoleAttrType.Key).Return((int)OfferRoleAttributeTypes.IncomeContributor);
            SetupResult.For(appRoleAttr.OfferRoleAttributeType).Return(appRoleAttrType);

            ILegalEntity legalEnt = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(appRole.LegalEntity).Return(legalEnt);

            IEventList<IEmployment> emps = new EventList<IEmployment>();
            SetupResult.For(legalEnt.Employment).Return(emps);
            IEmployment emp = _mockery.StrictMock<IEmployment>();
            SetupResult.For(emp.IsConfirmed).Return(false);
            emps.Add(Messages, emp);

            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(empStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(emp.EmploymentStatus).Return(empStatus);

            ExecuteRule(rule, 1, app);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void ApplicationConfirmIncomeSuccess()
        {
            ApplicationConfirmIncome rule = new ApplicationConfirmIncome();

            IApplication app = _mockery.StrictMock<IApplication>();

            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(appRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(app.ApplicationRoles).Return(applicationRoles);

            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);

            IEventList<IApplicationRoleAttribute> appRoleAttrs = new EventList<IApplicationRoleAttribute>();
            SetupResult.For(appRole.ApplicationRoleAttributes).Return(appRoleAttrs);
            IApplicationRoleAttribute appRoleAttr = _mockery.StrictMock<IApplicationRoleAttribute>();
            appRoleAttrs.Add(Messages, appRoleAttr);

            IApplicationRoleAttributeType appRoleAttrType = _mockery.StrictMock<IApplicationRoleAttributeType>();
            SetupResult.For(appRoleAttrType.Key).Return((int)OfferRoleAttributeTypes.IncomeContributor);
            SetupResult.For(appRoleAttr.OfferRoleAttributeType).Return(appRoleAttrType);

            ILegalEntity legalEnt = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(appRole.LegalEntity).Return(legalEnt);

            IEventList<IEmployment> emps = new EventList<IEmployment>();
            SetupResult.For(legalEnt.Employment).Return(emps);
            IEmployment emp = _mockery.StrictMock<IEmployment>();
            SetupResult.For(emp.IsConfirmed).Return(true);
            emps.Add(Messages, emp);

            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(empStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(emp.EmploymentStatus).Return(empStatus);

            ExecuteRule(rule, 0, app);
        }

        #endregion ApplicationConfirmIncome

        #region ApplicationNonResidentLoanAgreementAmount

        [NUnit.Framework.Test]
        public void ApplicationNonResidentLoanAgreementAmountTest()
        {
            // FAIL - Foreigner
            ApplicationNonResidentLoanAgreementAmountHelper(1, (int)CitizenTypes.Foreigner);
            ApplicationNonResidentLoanAgreementAmountHelper(1, (int)CitizenTypes.NonResident);
            ApplicationNonResidentLoanAgreementAmountHelper(1, (int)CitizenTypes.NonResidentConsulate);
            ApplicationNonResidentLoanAgreementAmountHelper(1, (int)CitizenTypes.NonResidentDiplomat);
            ApplicationNonResidentLoanAgreementAmountHelper(1, (int)CitizenTypes.NonResidentHighCommissioner);
            ApplicationNonResidentLoanAgreementAmountHelper(1, (int)CitizenTypes.NonResidentRefugee);

            // PASS
            ApplicationNonResidentLoanAgreementAmountHelper(0, (int)CitizenTypes.SACitizenNonResident);
            ApplicationNonResidentLoanAgreementAmountHelper(0, (int)CitizenTypes.SACitizen);
            ApplicationNonResidentLoanAgreementAmountHelper(0, (int)CitizenTypes.NonResidentCMAResident_Citizen);
            ApplicationNonResidentLoanAgreementAmountHelper(0, (int)CitizenTypes.NonResidentContractWorker);
        }

        public void ApplicationNonResidentLoanAgreementAmountHelper(int expectedMessageCount, int citizenTypeInt)
        {
            ApplicationNonResidentLoanAgreementAmount rule = new ApplicationNonResidentLoanAgreementAmount();

            IApplication app = _mockery.StrictMock<IApplication>();

            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            IApplication appRepo = _mockery.StrictMock<IApplication>();

            IReadOnlyEventList<ILegalEntityNaturalPerson> lenps = new ReadOnlyEventList<ILegalEntityNaturalPerson>();
            IList<ILegalEntity> lenpsList = new List<ILegalEntity>();
            ILegalEntityNaturalPerson lenp = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            SetupResult.For(lenp.IDNumber).Return(null);

            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return(citizenTypeInt);
            SetupResult.For(lenp.CitizenType).Return(citizenType);
            lenpsList.Add(lenp);
            IReadOnlyEventList<ILegalEntity> les = new ReadOnlyEventList<ILegalEntity>(lenpsList);

            SetupResult.For(app.GetLegalEntitiesByRoleType(null)).IgnoreArguments().Return(les);

            ExecuteRule(rule, expectedMessageCount, app);
        }

        #endregion ApplicationNonResidentLoanAgreementAmount

        #region ApplicationNewPurchaseSellerRoleExists

        /// <summary>
        /// Application New Purchase Seller Role Exists Test
        /// </summary>
        [Test]
        public void ApplicationNewPurchaseSellerRoleExistsTest()
        {
            //Pass
            ApplicationNewPurchaseSellerRoleExistsHelper(0, OfferRoleTypes.Seller);

            //Fail
            foreach (OfferRoleTypes offerRoleType in Enum.GetValues(typeof(OfferRoleTypes)))
            {
                if (offerRoleType != OfferRoleTypes.Seller)
                {
                    ApplicationNewPurchaseSellerRoleExistsHelper(1, offerRoleType);
                }
            }
        }

        /// <summary>
        /// Application New Purchase Seller Role Exists Helper
        /// </summary>
        private void ApplicationNewPurchaseSellerRoleExistsHelper(int expectedErrorCount, OfferRoleTypes offerRole)
        {
            ApplicationNewPurchaseSellerRoleExists rule = new ApplicationNewPurchaseSellerRoleExists();
            IApplicationMortgageLoanNewPurchase application = _mockery.StrictMock<IApplicationMortgageLoanNewPurchase>();

            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(new IApplicationRole[] { applicationRole });

            IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(applicationRoleType.Key).Return((int)offerRole);
            SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);
            SetupResult.For(application.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, expectedErrorCount, application);
        }

        #endregion ApplicationNewPurchaseSellerRoleExists

        #region ApplicationSwitchSettlementBankAccount

        /// <summary>
        /// Application Switch Settlement Bank Account
        /// </summary>
        [Test]
        public void ApplicationSwitchSettlementBankAccountTest()
        {
            //Pass
            ApplicationSwitchSettlementBankAccountHelper(0, ExpenseTypes.Existingmortgageamount, true);

            //Fail
            foreach (ExpenseTypes expenseType in Enum.GetValues(typeof(ExpenseTypes)))
            {
                if (expenseType != ExpenseTypes.Existingmortgageamount)
                {
                    ApplicationSwitchSettlementBankAccountHelper(1, expenseType, true);
                }
            }
        }

        /// <summary>
        /// Application Switch Settlement Bank Account Helper
        /// </summary>
        /// <param name="expectedMessageCount"></param>
        /// <param name="expenseType"></param>
        /// <param name="addBankAccount"></param>
        private void ApplicationSwitchSettlementBankAccountHelper(int expectedErrorCount, ExpenseTypes expenseType, bool addBankAccount)
        {
            ApplicationSwitchSettlementBankAccount rule = new ApplicationSwitchSettlementBankAccount();
            IApplicationMortgageLoanSwitch application = _mockery.StrictMock<IApplicationMortgageLoanSwitch>();

            IEventList<IApplicationExpense> applicationExpenses = new EventList<IApplicationExpense>();
            IEventList<IApplicationDebtSettlement> applicationDebtSettlements = new EventList<IApplicationDebtSettlement>();

            IApplicationDebtSettlement applicationDebtSettlement = _mockery.StrictMock<IApplicationDebtSettlement>();
            IApplicationExpense applicationExpense = _mockery.StrictMock<IApplicationExpense>();
            IExpenseType applicationExpenseType = _mockery.StrictMock<IExpenseType>();

            SetupResult.For(applicationExpenseType.Key).Return((int)expenseType);

            SetupResult.For(applicationExpense.ExpenseType).Return(applicationExpenseType);

            applicationExpenses.Add(Messages, applicationExpense);

            IBankAccount bankAccount = _mockery.StrictMock<IBankAccount>();

            if (addBankAccount)
            {
                SetupResult.For(applicationDebtSettlement.BankAccount).Return(bankAccount);
            }
            else
            {
                SetupResult.For(applicationDebtSettlement.BankAccount).Return(null);
            }

            applicationDebtSettlements.Add(Messages, applicationDebtSettlement);
            SetupResult.For(applicationExpense.ApplicationDebtSettlements).Return(applicationDebtSettlements);
            SetupResult.For(application.ApplicationExpenses).Return(applicationExpenses);

            ExecuteRule(rule, expectedErrorCount, application);
        }

        #endregion ApplicationSwitchSettlementBankAccount
    }
}
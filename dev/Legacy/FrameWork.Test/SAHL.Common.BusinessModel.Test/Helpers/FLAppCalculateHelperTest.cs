using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Helpers
{
    [TestFixture]
    public class FLAppCalculateHelperTest : TestBase
    {
        // Limits
        const double readvanceLimit = 10000.00;

        const double readvanceRequired = 5000.00;
        const double furtherAdvanceLimit = 10000.00;
        const double furtherAdvanceRequired = 15000.00;
        const double furtherLoanLimit = 10000.00;
        const double furtherLoanRequired = 25000.00;
        const double bondToRegister = 55000;

        // Further Lending Applications
        IApplicationFurtherLoan _fl;

        IApplicationReAdvance _ra;
        IApplicationFurtherAdvance _fa;

        IApplicationFurtherLoan _florig;
        IApplicationReAdvance _raorig;
        IApplicationFurtherAdvance _faorig;

        IAccount _account;
        IMortgageLoanAccount _mla;
        IAccountRepository _accRepo;
        IApplicationRepository _appRepo;
        ILookupRepository _lookRepo;
        ICreditMatrixRepository _cmRepo;
        private IMortgageLoan _vML;

        public FLAppCalculateHelperTest()
        {
            _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _lookRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _cmRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();
        }

        /// <summary>
        /// This test does not test everything but it makes sure the Further Lending Calcuator does not fall over
        /// and does work for all three application types.
        /// The Test FL Account query could fail to return back a proper account hence it can
        /// have further checks/filters in the sql query i.e. Please fix query.
        /// More tests can be added would be better, so feel free to code away.
        /// </summary>
        [Test]
        public void CalculateFurtherLendingTest()
        {
            using (new SessionScope())
            {
                _account = FindSuitableFLAccount();

                if (_account != null)
                {
                    _mla = _account as IMortgageLoanAccount;

                    // Create Readvance Application
                    _ra = _appRepo.CreateReAdvanceApplication(_mla, false);
                    PopulateApplicationData(_ra.CurrentProduct, OfferTypes.ReAdvance);

                    // Create Further Advance Application
                    _fa = _appRepo.CreateFurtherAdvanceApplication(_mla, false);
                    PopulateApplicationData(_fa.CurrentProduct, OfferTypes.FurtherAdvance);

                    // Create Further Loan Application
                    _fl = _appRepo.CreateFurtherLoanApplication(_mla, false);
                    PopulateApplicationData(_fl.CurrentProduct, OfferTypes.FurtherLoan);

                    // Do calculation
                    FLAppCalculateHelper flH = new FLAppCalculateHelper(_account, _ra, _fa, _fl);
                    flH.CalculateFurtherLending(true);
                    double newAmortisingInstallment = flH.AmortisingInstalment;

                    // Test Further Lending Calculator Combined Data
                    Assert.IsTrue(flH.LTV > 0D);
                    Assert.IsTrue(flH.AmortisingInstalment > 0D);
                    Assert.IsTrue(flH.PTI > 0D);
                    Assert.IsTrue(flH.Instalment > 0D);
                    Assert.IsTrue(flH.SPV != null);
                    Assert.IsTrue(flH.IsExceptionCreditCriteria == true || flH.IsExceptionCreditCriteria == false);
                    Assert.IsTrue(flH.Fees > 0D);
                    Assert.IsTrue(flH.AppCategory != null);

                    ISupportsVariableLoanApplicationInformation svli = null;
                    IApplicationInformationVariableLoan vli = null;

                    // Test Readvance Data
                    Assert.IsNotNull(_ra != null);

                    svli = _ra.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                    vli = svli.VariableLoanInformation;

                    Assert.IsTrue(vli.PTI > 0D);
                    Assert.IsTrue(vli.LTV > 0D);
                    Assert.IsTrue(vli.MonthlyInstalment > 0D);
                    Assert.IsTrue(vli.SPV != null);
                    Assert.IsTrue(vli.Category != null);
                    Assert.IsTrue(vli.CreditCriteria != null);
                    Assert.IsTrue(vli.MarketRate.HasValue);
                    Assert.IsTrue(vli.Term.HasValue);
                    Assert.IsTrue(vli.ExistingLoan.HasValue);
                    Assert.IsTrue(vli.PropertyValuation.HasValue);

                    // Test Further Advance Data
                    Assert.IsNotNull(_fa != null);

                    svli = _fa.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                    vli = svli.VariableLoanInformation;

                    Assert.IsTrue(vli.PTI > 0D);
                    Assert.IsTrue(vli.LTV > 0D);
                    Assert.IsTrue(vli.MonthlyInstalment > 0D);
                    Assert.IsTrue(vli.SPV != null);
                    Assert.IsTrue(vli.Category != null);
                    Assert.IsTrue(vli.CreditCriteria != null);
                    Assert.IsTrue(vli.MarketRate.HasValue);
                    Assert.IsTrue(vli.Term.HasValue);
                    Assert.IsTrue(vli.ExistingLoan.HasValue);
                    Assert.IsTrue(vli.PropertyValuation.HasValue);

                    // Test Further Loan Data
                    Assert.IsNotNull(_fl != null);

                    svli = _fl.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                    vli = svli.VariableLoanInformation;

                    Assert.IsTrue(vli.PTI > 0D);
                    Assert.IsTrue(vli.LTV > 0D);
                    Assert.IsTrue(vli.MonthlyInstalment > 0D);
                    Assert.IsTrue(vli.SPV != null);
                    Assert.IsTrue(vli.FeesTotal > 0D);
                    Assert.IsTrue(vli.BondToRegister > 0D);
                    Assert.IsTrue(vli.Category != null);
                    Assert.IsTrue(vli.CreditCriteria != null);
                    Assert.IsTrue(vli.MarketRate.HasValue);
                    Assert.IsTrue(vli.Term.HasValue);
                    Assert.IsTrue(vli.ExistingLoan.HasValue);
                    Assert.IsTrue(vli.PropertyValuation.HasValue);
                }
                else
                {
                    //Account Cannot be found this is not a failure, this is just inconclusive
                    Assert.Inconclusive("Cannot find an Account to perform Further Lending. Please read comments for this test method.");
                }
            }
        }

        [Test]
        public void CalculateFurtherLendingAcceptedOfferTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                _account = FindSuitableFLAccountAcceptedOffer();

                if (_account != null)
                {
                    _mla = _account as IMortgageLoanAccount;

                    //Check existing values

                    ISupportsVariableLoanApplicationInformation svliOrig = null;
                    IApplicationInformationVariableLoan vliOrig = null;
                    double? _raPTIOrig = 0;
                    double? _raLTVOrig = 0;
                    double? _raMonthlyInstalmentOrig = 0;
                    int? _raTermOrig = 0;

                    double? _faPTIOrig = 0;
                    double? _faLTVOrig = 0;
                    double? _faMonthlyInstalmentOrig = 0;
                    int? _faTermOrig = 0;

                    double? _flPTIOrig = 0;
                    double? _flLTVOrig = 0;
                    double? _flMonthlyInstalmentOrig = 0;
                    int? _flTermOrig = 0;

                    //TODO: BackEnd Revamp
                    short _offset = 201;

                    foreach (IApplication app in _account.Applications)
                    {
                        if (app.ApplicationStatus.Key == (int)OfferStatuses.Open)
                        {
                            switch (app.ApplicationType.Key)
                            {
                                case (int)OfferTypes.ReAdvance:
                                    _raorig = app as IApplicationReAdvance;
                                    svliOrig = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                                    vliOrig = svliOrig.VariableLoanInformation;
                                    _raPTIOrig = vliOrig.PTI;
                                    _raLTVOrig = vliOrig.LTV;
                                    _raMonthlyInstalmentOrig = vliOrig.MonthlyInstalment;

                                    //if its not an accepted offer we can force a recalc by changing term
                                    if (_raorig.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                                    {
                                        _mla = _account as IMortgageLoanAccount;
                                        _vML = _mla.SecuredMortgageLoan;

                                        //TODO: BackEnd Revamp
                                        //_vML.RemainingInstallments = _offset;
                                    }
                                    else
                                    {
                                        _raTermOrig = vliOrig.Term;
                                    }

                                    break;
                                case (int)OfferTypes.FurtherAdvance:
                                    _faorig = app as IApplicationFurtherAdvance;
                                    svliOrig = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                                    vliOrig = svliOrig.VariableLoanInformation;
                                    _faPTIOrig = vliOrig.PTI;
                                    _faLTVOrig = vliOrig.LTV;
                                    _faMonthlyInstalmentOrig = vliOrig.MonthlyInstalment;
                                    if (_faorig.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                                    {
                                        _mla = _account as IMortgageLoanAccount;
                                        _vML = _mla.SecuredMortgageLoan;

                                        //TODO: BackEnd Revamp
                                        //_vML.RemainingInstallments = _offset;
                                    }
                                    else
                                    {
                                        _faTermOrig = vliOrig.Term;
                                    }
                                    break;
                                case (int)OfferTypes.FurtherLoan:
                                    _florig = app as IApplicationFurtherLoan;
                                    svliOrig = app.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                                    vliOrig = svliOrig.VariableLoanInformation;
                                    _flPTIOrig = vliOrig.PTI;
                                    _flLTVOrig = vliOrig.LTV;
                                    _flMonthlyInstalmentOrig = vliOrig.MonthlyInstalment;
                                    if (_florig.GetLatestApplicationInformation().ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
                                    {
                                        _mla = _account as IMortgageLoanAccount;
                                        _vML = _mla.SecuredMortgageLoan;

                                        //TODO: BackEnd Revamp
                                        //_vML.RemainingInstallments = _offset;
                                    }
                                    else
                                    {
                                        _flTermOrig = vliOrig.Term;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    // Do calculation
                    FLAppCalculateHelper flH = new FLAppCalculateHelper(_account);

                    flH.CalculateFurtherLending(false);
                    double newAmortisingInstallment = flH.AmortisingInstalment;

                    //Check if the calculations took place
                    foreach (IApplication app in _account.Applications)
                    {
                        if (app.ApplicationStatus.Key == (int)OfferStatuses.Open)
                        {
                            switch (app.ApplicationType.Key)
                            {
                                case (int)OfferTypes.ReAdvance:
                                    _ra = app as IApplicationReAdvance;
                                    break;
                                case (int)OfferTypes.FurtherAdvance:
                                    _fa = app as IApplicationFurtherAdvance;
                                    break;
                                case (int)OfferTypes.FurtherLoan:
                                    _fl = app as IApplicationFurtherLoan;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    // Test Readvance Data

                    ISupportsVariableLoanApplicationInformation svli = null;
                    IApplicationInformationVariableLoan vli = null;

                    // Test Readvance Data
                    if (_ra != null)
                    {
                        if (_ra.GetLatestApplicationInformation().ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                        {
                            svli = _ra.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                            vli = svli.VariableLoanInformation;
                            Assert.IsTrue(vli.PTI == _raPTIOrig);
                            Assert.IsTrue(vli.LTV == _raLTVOrig);
                            Assert.IsTrue(vli.MonthlyInstalment == _raMonthlyInstalmentOrig);
                            Assert.IsTrue(vli.Term == _raTermOrig);
                        }
                    }

                    // Test Further Advance Data

                    if (_fa != null)
                    {
                        if (_fa.GetLatestApplicationInformation().ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                        {
                            svli = _fa.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                            vli = svli.VariableLoanInformation;
                            Assert.IsTrue(vli.PTI == _faPTIOrig);
                            Assert.IsTrue(vli.LTV == _faLTVOrig);
                            Assert.IsTrue(vli.MonthlyInstalment == _faMonthlyInstalmentOrig);
                            Assert.IsTrue(vli.Term == _faTermOrig);
                        }
                    }

                    // Test Further Loan Data

                    if (_fl != null)
                    {
                        if (_fl.GetLatestApplicationInformation().ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                        {
                            svli = _fl.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                            vli = svli.VariableLoanInformation;
                            Assert.IsTrue(vli.PTI == _flPTIOrig);
                            Assert.IsTrue(vli.LTV == _flLTVOrig);
                            Assert.IsTrue(vli.MonthlyInstalment == _flMonthlyInstalmentOrig);
                            Assert.IsTrue(vli.Term == _flTermOrig);
                        }
                    }
                }
                else
                    Assert.Fail("Cannot find an Account to perform Further Lending. Please read comments for this test method.");
            }
        }

        #region Helper Methods

        private IAccount FindSuitableFLAccount()
        {
            string sql = string.Format(@"select top 1 a.accountKey
            from [2am].dbo.account a
            inner join [2am].dbo.FinancialService fs
	            on fs.AccountKey = a.AccountKey
            inner join [2am].fin.mortgageLoan ml
	            on fs.FinancialServiceKey = ml.FinancialServiceKey
	        inner join [2am].fin.Balance b
				on fs.FinancialServiceKey = b.FinancialServiceKey
	        inner join [2am].fin.LoanBalance lb
				on fs.FinancialServiceKey = lb.FinancialServiceKey
            inner join [2am].fin.FinancialAdjustment fa
	            on fs.FinancialServiceKey = fa.FinancialServiceKey
            left join [2am].dbo.offer ofr
	            on ofr.ReservedAccountKey = a.AccountKey and ofr.OfferTypeKey in (2,3,4)
            inner join
            (select sum(b.BondRegistrationAmount) as BondRegistrationAmount, bml.FinancialServiceKey
            from BondMortgageLoan bml
            inner join bond b
	            on bml.BondKey = b.BondKey
            group by bml.FinancialServiceKey) as bondKeys
	            on bondkeys.FinancialServiceKey = ml.FinancialServiceKey
            inner join [property] p
	            on ml.propertyKey = p.propertyKey
            inner join valuation v
	            on p.propertyKey = v.propertyKey
            where a.accountStatusKey = 1
            and fa.FinancialAdjustmentTypeKey = 6
            and fa.FinancialAdjustmentStatusKey = 1
            and ofr.offerKey is null
            and (lb.InitialBalance - b.Amount) > {0}
            and (bondKeys.BondRegistrationAmount - lb.InitialBalance) > {1}
            and (v.valuationAmount - bondKeys.BondRegistrationAmount) > {2}
            and v.IsActive = 1
            and a.rrr_productKey in (1,9,11)
            order by a.accountKey", readvanceLimit, furtherAdvanceLimit, furtherLoanLimit);

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
            if (obj != null)
            {
                int accountKey = Convert.ToInt32(obj);
                return _accRepo.GetAccountByKey(accountKey);
            }
            else
                return null;
        }

        private IAccount FindSuitableFLAccountAcceptedOffer()
        {
            string sql = @"
            select top 1 o.accountkey from [2am].[dbo].offer o (NOLOCK)
            join (
                select min(offerinformationkey) as OIKey,
                       offerkey
                from [2am].[dbo].offerinformation (nolock)
                group by offerkey
            ) maxoi on o.offerkey = maxoi.offerkey
            join [2am].[dbo].offerinformation oi (NOLOCK) on maxoi.OIKey = oi.offerinformationkey
            join [2am].[dbo].Account acc (NOLOCK) on o.AccountKey = acc.AccountKey
            where offertypekey in (2,3, 4)  and oi.offerinformationtypekey = 3 and acc.AccountStatusKey = 1";

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
            if (obj != null)
            {
                int accountKey = Convert.ToInt32(obj);
                return _accRepo.GetAccountByKey(accountKey);
            }
            else
                return null;
        }

        private void PopulateApplicationData(IApplicationProduct cp, OfferTypes appType)
        {
            ISupportsVariableLoanApplicationInformation svli = cp as ISupportsVariableLoanApplicationInformation;
            ISupportsVariableLoanApplicationInformation oldSvli = _mla.CurrentMortgageLoanApplication.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            svli.VariableLoanInformation.EmploymentType = _lookRepo.EmploymentTypes.ObjectDictionary[((int)EmploymentTypes.Salaried).ToString()];
            svli.VariableLoanInformation.RateConfiguration =
                _cmRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(oldSvli.VariableLoanInformation.RateConfiguration.Margin.Key, oldSvli.VariableLoanInformation.RateConfiguration.MarketRate.Key);

            if (appType == OfferTypes.FurtherLoan)
                svli.VariableLoanInformation.BondToRegister = bondToRegister;

            switch (appType)
            {
                case OfferTypes.ReAdvance:
                    svli.VariableLoanInformation.LoanAmountNoFees = readvanceRequired;
                    break;
                case OfferTypes.FurtherAdvance:
                    svli.VariableLoanInformation.LoanAmountNoFees = furtherAdvanceRequired;
                    break;
                case OfferTypes.FurtherLoan:
                    svli.VariableLoanInformation.LoanAmountNoFees = furtherLoanRequired;
                    break;
                default:
                    break;
            }
        }

        #endregion Helper Methods
    }
}
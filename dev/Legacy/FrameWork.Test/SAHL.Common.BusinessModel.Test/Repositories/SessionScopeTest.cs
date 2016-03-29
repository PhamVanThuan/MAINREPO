using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class SessionScopeTest : TestBase
    {
        [Test]
        public void Detach_ReattachAndSave()
        {
            using (new SessionScope())
            {
                IApplication _app = null;
                IApplicationRepository AppRepo = RepositoryFactory.GetRepository<IApplicationRepository>();//new ApplicationRepository();
                ILookupRepository LookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                ICreditMatrixRepository CMRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();
                IOriginationSource _os = AppRepo.GetOriginationSource((OriginationSources)1);
                IResetConfiguration resetConfig = AppRepo.GetApplicationDefaultResetConfiguration();
                IApplicationStatus _statusOpen = LookupRepo.ApplicationStatuses.ObjectDictionary["1"];
                ICreditMatrix _cm = CMRepo.GetCreditMatrixByKey(21);
                ICategory _cat = CMRepo.GetCategoryByKey(1);

                int _marketRateKey = 4;

                IEmploymentType _empType = null;

                if (LookupRepo.EmploymentTypes.ObjectDictionary.ContainsKey(2.ToString()))
                    _empType = LookupRepo.EmploymentTypes.ObjectDictionary[2.ToString()];
                try
                {
                    //Create application
                    switch (MortgageLoanPurposes.Newpurchase)
                    {
                        case MortgageLoanPurposes.Newpurchase:
                            IOriginationSource OriginationSource = _os;
                            ProductsNewPurchaseAtCreation ProductType = ProductsNewPurchaseAtCreation.NewVariableLoan;

                            ApplicationMortgageLoanNewPurchase_DAO App = null;
                            if (null != _app)
                            {
                                int appKey = _app.Key;

                                NHibernate.ISession Sess = null;
                                Castle.ActiveRecord.Framework.ISessionFactoryHolder sessionHolder = Castle.ActiveRecord.ActiveRecordMediator.GetSessionFactoryHolder();
                                Sess = sessionHolder.CreateSession(typeof(Application_DAO));
                                Sess.Evict(_app);

                                _app = null;

                                IDbConnection con = null;
                                try
                                {
                                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                    sb.AppendFormat("update offer set offertypekey={0} where offerkey={1}", 7, appKey);
                                    con = Helper.GetSQLDBConnection();
                                    Helper.ExecuteNonQuery(con, sb.ToString());
                                    con.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    con.Dispose();
                                }

                                //}

                                App = ApplicationMortgageLoanNewPurchase_DAO.Find(appKey);
                            }
                            else
                            {
                                // reserve an account key
                                AccountSequence_DAO AS = new AccountSequence_DAO();
                                AS.SaveAndFlush();

                                // Create an application
                                App = new ApplicationMortgageLoanNewPurchase_DAO();

                                // set the applications reserved account key
                                App.ReservedAccount = AS;
                            }

                            // Create a mortgageloan detail
                            ApplicationMortgageLoanDetail_DAO AMD = new ApplicationMortgageLoanDetail_DAO();

                            // set the applications mortgageloan detail and link the mortgageloan detail to its owning application
                            App.ApplicationMortgageLoanDetail = AMD;
                            AMD.Application = App;

                            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                            IApplicationMortgageLoanNewPurchase AppMNP = BMTM.GetMappedType<SAHL.Common.BusinessModel.ApplicationMortgageLoanNewPurchase>(App);
                            AppMNP.OriginationSource = OriginationSource;
                            try
                            {
                                AppMNP.SetProduct((ProductsNewPurchase)ProductType);
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }

                            AppMNP.ApplicationStatus = SAHL.Common.Factories.RepositoryFactory.GetRepository<SAHL.Common.BusinessModel.Interfaces.Repositories.ILookupRepository>().ApplicationStatuses.ObjectDictionary["1"];
                            AppMNP.ApplicationStartDate = DateTime.Now;

                            _app = AppMNP;

                            IApplicationMortgageLoanNewPurchase anp = _app as IApplicationMortgageLoanNewPurchase;
                            anp.SetInitiationFee(100, false);
                            anp.SetRegistrationFee(200, false);
                            break;
                        case MortgageLoanPurposes.Refinance:
                            _app = AppRepo.CreateRefinanceApplication(_os, ProductsRefinanceAtCreation.NewVariableLoan, (IApplicationUnknown)_app);
                            IApplicationMortgageLoanRefinance ar = _app as IApplicationMortgageLoanRefinance;
                            ar.SetInitiationFee(100, false);
                            ar.SetRegistrationFee(200, false);
                            break;
                        case MortgageLoanPurposes.Switchloan:
                            _app = AppRepo.CreateSwitchLoanApplication(_os, ProductsSwitchLoanAtCreation.NewVariableLoan, (IApplicationUnknown)_app);
                            IApplicationMortgageLoanSwitch asw = _app as IApplicationMortgageLoanSwitch;
                            asw.SetCancellationFee(300, false);
                            asw.SetInitiationFee(100, false);
                            asw.SetRegistrationFee(200, false);
                            asw.InterimInterest = 1000;
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                IApplicationProduct _prod = _app.CurrentProduct;
                SetupNVL(_prod);
                //Mortgage Loan details
                IApplicationMortgageLoan appML = (IApplicationMortgageLoan)_app;
                appML.ApplicationStartDate = DateTime.Now;
                appML.ApplicationStatus = _statusOpen;
                appML.ClientEstimatePropertyValuation = 1200000;
                appML.ResetConfiguration = resetConfig;

                AppRepo.SaveApplication((IApplication)_app);
            }
        }

        protected IApplicationProductNewVariableLoan SetupNVL(IApplicationProduct prod)
        {
            IApplicationProductNewVariableLoan nvl = prod as IApplicationProductNewVariableLoan;
            if (null != nvl)
            {
                IApplicationMortgageLoanNewPurchase npml = nvl.Application as IApplicationMortgageLoanNewPurchase;
                if (npml != null)
                {
                    npml.PurchasePrice = 1200000;
                }

                //nvl.LoanAgreementAmount = 850000;
                nvl.Term = 240;

                SetupVariableInformation(nvl.VariableLoanInformation);

                //SetupInterestOnly(nvl.InterestOnlyInformation);
                return nvl;
            }
            else
                throw new InvalidCastException("Not a IApplicationProductNewVariableLoan");
        }

        protected void SetupVariableInformation(IApplicationInformationVariableLoan aivl)
        {
            if (aivl != null)
            {
                IApplicationRepository AppRepo = RepositoryFactory.GetRepository<IApplicationRepository>();//new ApplicationRepository();
                ILookupRepository LookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                ICreditMatrixRepository CMRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();

                IOriginationSource _os = AppRepo.GetOriginationSource((OriginationSources)1);
                IResetConfiguration resetConfig = AppRepo.GetApplicationDefaultResetConfiguration();
                IApplicationStatus _statusOpen = LookupRepo.ApplicationStatuses.ObjectDictionary["1"];
                ICreditMatrix _cm = CMRepo.GetCreditMatrixByKey(21);
                ICategory _cat = CMRepo.GetCategoryByKey(1);

                aivl.BondToRegister = 900000; 
                aivl.CashDeposit = 100000;
                aivl.Category = LookupRepo.Categories[1];
                aivl.CreditMatrix = _cm;
                aivl.EmploymentType = LookupRepo.EmploymentTypes[0];
                aivl.ExistingLoan = 0; //this should always be 0 for a new loan
                aivl.FeesTotal = 500;
                aivl.HouseholdIncome = 50000;
                aivl.InterimInterest = 1000;

                //aivl.LoanAgreementAmount = 650000;
                aivl.LTV = 0.80;
                aivl.MarketRate = 0.12;
                aivl.MonthlyInstalment = 4500;
                aivl.PropertyValuation = 1500000;
                aivl.PTI = 0.3;
                aivl.RateConfiguration = CMRepo.GetRateConfigurationByMarginKeyAndMarketRateKey(1, 4);

                //                _rateConfigKey = aivl.RateConfiguration.Key;
                aivl.SPV = LookupRepo.SPVList[0];//NewSPVFromLTV(_view.LTV);
                aivl.Term = 240;
            }
        }
    }
}
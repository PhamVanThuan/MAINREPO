using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Test;
using System.Data;
using SAHL.Services.Capitec.Models.Shared;

namespace SAHL.Common.Service.Test
{
    [TestFixture]
    public class SPVServiceTest : TestBase
    {
        private IApplicationRepository appRepo;
        private IOriginationSource os;
        private IApplicationStatus status;
        private IEmploymentType salariedEmploymentType;
        private ILegalEntityRepository leRepo;
        private ISPVService spvService;
        private IAccountRepository accRepo;

        [SetUp()]
        public void Setup()
        {
            base.SetRepositoryStrategy(TypeFactoryStrategy.Default);

            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            os = RepositoryFactory.GetRepository<IApplicationRepository>().GetOriginationSource(OriginationSources.SAHomeLoans);
            status = RepositoryFactory.GetRepository<ILookupRepository>().ApplicationStatuses.ObjectDictionary["1"];
            salariedEmploymentType = RepositoryFactory.GetRepository<ILookupRepository>().EmploymentTypes.ObjectDictionary["1"];
            leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            spvService = ServiceFactory.GetService<ISPVService>();
        }

        [Test]
        public void GetSPVListForFurtherLendingTest()
        {
            using (new SessionScope())
            {
                ISPVService spvService = ServiceFactory.GetService<ISPVService>();
                IList<ISPV> spvList = spvService.GetSPVListForFurtherLending();
                Assert.Greater(spvList.Count, 0);
            }
        }

        [Test]
        public void DetermineSPVOnApplicationTest_AlphaHousing_NewBusiness()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                var app = appRepo.CreateNewPurchaseApplication(os, ProductsNewPurchaseAtCreation.NewVariableLoan, null);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;

                ISupportsVariableLoanApplicationInformation vli = prod as ISupportsVariableLoanApplicationInformation;
                vli.VariableLoanInformation.LTV = 0.90;
                vli.VariableLoanInformation.HouseholdIncome = 13000;
                vli.VariableLoanInformation.EmploymentType = salariedEmploymentType;

                ISPV oldSPV = vli.VariableLoanInformation.SPV;

                appRepo.SaveApplication(app);

                spvService.DetermineSPVOnApplication(app);

                ISPV newSPV = vli.VariableLoanInformation.SPV;

                Assert.NotNull(newSPV, "GetValidSPV proc did not return an SPV.");
            }
        }

        [Test]
        public void DetermineSPVOnApplicationTest_BlueBanner_NewBusiness()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                var app = appRepo.CreateNewPurchaseApplication(os, ProductsNewPurchaseAtCreation.NewVariableLoan, null);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;

                ISupportsVariableLoanApplicationInformation vli = prod as ISupportsVariableLoanApplicationInformation;

                vli.VariableLoanInformation.LTV = 0.85;
                vli.VariableLoanInformation.HouseholdIncome = 18000;
                vli.VariableLoanInformation.EmploymentType = salariedEmploymentType;

                ISPV oldSPV = vli.VariableLoanInformation.SPV;

                appRepo.SaveApplication(app);

                spvService.DetermineSPVOnApplication(app);

                ISPV newSPV = vli.VariableLoanInformation.SPV;

                Assert.NotNull(newSPV, "GetValidSPV proc did not return an SPV.");
            }
        }

        [Test]
        public void DetermineSPVOnApplicationTest_MainStreet_NewBusiness()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                var app = appRepo.CreateNewPurchaseApplication(os, ProductsNewPurchaseAtCreation.NewVariableLoan, null);
                app.ApplicationStatus = status;
                IApplicationProduct prod = app.CurrentProduct;

                ISupportsVariableLoanApplicationInformation vli = prod as ISupportsVariableLoanApplicationInformation;

                vli.VariableLoanInformation.LTV = 0.70;
                vli.VariableLoanInformation.HouseholdIncome = 25000;
                vli.VariableLoanInformation.EmploymentType = salariedEmploymentType;

                ISPV oldSPV = vli.VariableLoanInformation.SPV;

                appRepo.SaveApplication(app);

                spvService.DetermineSPVOnApplication(app);

                ISPV newSPV = vli.VariableLoanInformation.SPV;

                Assert.NotNull(newSPV, "GetValidSPV proc did not return an SPV.");
            }
        }

        [Test]
        public void DetermineSPVOnApplicationTest_MainStreet_to_BlueBanner_FurtherLending()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                int AccountKey = -1;
                string query = @"   select top 1 acc.AccountKey
                                    from fin.MortgageLoan ml (nolock)
                                    join FinancialService fs (nolock) on ml.FinancialServiceKey = fs.FinancialServiceKey
                                    join Account acc (nolock) on fs.AccountKey = acc.AccountKey
                                    join spv.SPV spv (nolock) on acc.SPVKey = spv.SPVKey
                                    where acc.accountstatuskey = 1
                                    and	  acc.rrr_productkey = 1
                                    and	  spv.SPVKey = 117 -- Main Street 65 (Pty) Ltd.";

                var connection = Helper.GetSQLDBConnection();

                AccountKey = Convert.ToInt32(ExecuteScalar(connection, query));

                IAccount acc = accRepo.GetAccountByKey(AccountKey);
                IMortgageLoanAccount mla = acc as IMortgageLoanAccount;
                if (null != mla)
                {
                    var app = appRepo.CreateFurtherLoanApplication(mla, true);
                    IApplicationFurtherLoan fl = app as IApplicationFurtherLoan;

                    appRepo.SaveApplication(fl);

                    IApplicationProduct prod = app.CurrentProduct;

                    ISupportsVariableLoanApplicationInformation vli = prod as ISupportsVariableLoanApplicationInformation;

                    vli.VariableLoanInformation.LTV = 0.85;
                    vli.VariableLoanInformation.HouseholdIncome = 18000;
                    vli.VariableLoanInformation.EmploymentType = salariedEmploymentType;

                    ISPV currentSPV = vli.VariableLoanInformation.SPV;

                    spvService.DetermineSPVOnApplication(app);

                    ISPV newSPV = vli.VariableLoanInformation.SPV;

                    Assert.NotNull(newSPV, "GetValidSPV proc did not return an SPV.");
                }
                else
                {
                    Assert.Ignore("Insufficient data to perform this test.");
                }
            }
        }

        #region Capitec SPVService tests

        [Test]
        public void DetermineSPVOnCapitecApplicationTest_MainStreet_NewBusiness()
        {
            IAddressRepository addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

            using (new TransactionScope(OnDispose.Rollback))
            {
                int applicationKey, addressKey;
                string iDNumber, firstNames, surname, homePhoneNumber, workPhoneNumber, cellPhoneNumber, emailAddress;
                DateTime? dateOfBirth;
                DateTime applicationDate;

                IADUser adUser = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserForAdUserName(@"SAHL\WebLeads");

                // get an unused OfferKey
                string query = @"select max(OfferKey)+1 from [2AM].dbo.Offer (nolock)";
                DataTable DT = base.GetQueryResults(query);
                applicationKey = Convert.ToInt32(DT.Rows[0][0]);

                // get legal entity data
                query = @"select top 1 le.IDNumber, le.FirstNames, le.Surname, le.HomePhoneNumber, le.WorkPhoneNumber, le.CellPhoneNumber, le.EmailAddress, le.DateOfBirth, a.AddressKey
                            from [2am].dbo.LegalEntity le (nolock)
                            join [2am].dbo.LegalEntityAddress lea (nolock) on lea.LegalEntityKey = le.LegalEntityKey
	                            and lea.AddressTypeKey = 1
                            join [2am].dbo.[Address] a (nolock) on a.AddressKey = lea.AddressKey
	                            and a.AddressFormatKey = 1
                            left join [2am].dbo.[Role] r (nolock) on r.LegalEntityKey = le.LegalEntityKey
	                            and r.RoleTypeKey = 2 -- Main Applicant
                            left join [2am].[dbo].[Employment] e (nolock) on e.LegalEntityKey = le.LegalEntityKey
                            where le.LegalEntityTypeKey = 2 -- Natural Person
                            and isnull(le.DateOfBirth, '') <> ''
                            and isnull(le.IDNumber, '') <> ''
                            and isnull(le.Surname, '') <> ''
                            and isnull(le.CellPhoneNumber, '') <> ''
                            and r.LegalEntityKey is null
                            and e.LegalEntityKey is null";

                DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 1)
                {
                    iDNumber = Convert.ToString(DT.Rows[0][0]);
                    firstNames = Convert.ToString(DT.Rows[0][1]);
                    surname = Convert.ToString(DT.Rows[0][2]);
                    homePhoneNumber = Convert.ToString(DT.Rows[0][3]);
                    workPhoneNumber = Convert.ToString(DT.Rows[0][4]);
                    cellPhoneNumber = Convert.ToString(DT.Rows[0][5]);
                    emailAddress = Convert.ToString(DT.Rows[0][6]);
                    dateOfBirth = Convert.ToDateTime(DT.Rows[0][7]);
                    addressKey = Convert.ToInt32(DT.Rows[0][8]);

                    applicationDate = DateTime.Now;
                    NewPurchaseLoanDetails newPurchaseloanDetails = new NewPurchaseLoanDetails((int)EmploymentTypes.Salaried, 22000, 600000, 120000, true, 240);

                    ApplicantInformation applicantInformation1 = new ApplicantInformation(iDNumber, firstNames, surname, (int)SAHL.Common.Globals.SalutationTypes.Mr, homePhoneNumber, workPhoneNumber, cellPhoneNumber, emailAddress, dateOfBirth, "", true);

                    IAddressStreet addressStreet = addressRepository.GetAddressByKey(addressKey) as IAddressStreet;
                    ApplicantResidentialAddress applicantResidentialAddress = new ApplicantResidentialAddress(addressStreet.UnitNumber, addressStreet.BuildingNumber, addressStreet.BuildingName, addressStreet.StreetNumber, addressStreet.StreetName, addressStreet.RRR_SuburbDescription, addressStreet.RRR_ProvinceDescription, addressStreet.RRR_CityDescription, addressStreet.RRR_PostalCode, addressStreet.Suburb.Key);

                    ApplicantEmploymentDetails applicantEmploymentDetails1 = new ApplicantEmploymentDetails((int)EmploymentTypes.Salaried, new SalariedDetails(20000));

                    ApplicantDeclarations applicantDeclarations = new ApplicantDeclarations(true, true, true, true);

                    string request = "itcrequest";
                    string response = "itcrequest";

                    List<Applicant> applicants = new List<Applicant>() { 
                    new Applicant(applicantInformation1, applicantResidentialAddress, applicantEmploymentDetails1, applicantDeclarations, 
                        new ApplicantITC(applicationDate, request, response)) };

                    ConsultantDetails consultantDetails = new ConsultantDetails("Capitec User1", "Branch1");

                    var messages = new List<string>();
                    NewPurchaseApplication newPurchaseApplication = new NewPurchaseApplication(applicationKey, 1, applicationDate, newPurchaseloanDetails, applicants, (int)EmploymentTypes.Salaried, consultantDetails, messages);
                    applicationRepository.CreateCapitecApplication(newPurchaseApplication);

                    int spvKey = 0;

                    query = string.Format(@"select SPVKey
                    from [2AM].dbo.Offer o (nolock) 
                    join (select max(OfferInformationKey) OfferInformationKey, OfferKey
		                    from [2AM].dbo.OfferInformation (nolock)
		                    group by OfferKey) max_oi on max_oi.OfferKey = o.OfferKey
                    join [2AM].[dbo].[OfferInformationVariableLoan] oivl (nolock) on oivl.OfferInformationKey = max_oi.OfferInformationKey
                    where o.OfferKey = {0}", applicationKey);

                    DT = base.GetQueryResults(query);

                    Assert.Greater(DT.Rows.Count, 0, "");

                    spvKey = Convert.ToInt32(DT.Rows[0][0]);

                    Assert.AreEqual(117, spvKey, "DetermineSPVOnApplication did not return a valid SPV key. sux.");
                }
                else
                    Assert.Ignore("Insufficient data available for test.");
            }
        }

        [Test]
        public void DetermineSPVOnCapitecApplicationTest_MainStreet_Switch()
        {
            IAddressRepository addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

            using (new TransactionScope(OnDispose.Rollback))
            {
                int applicationKey, addressKey;
                string iDNumber, firstNames, surname, homePhoneNumber, workPhoneNumber, cellPhoneNumber, emailAddress;
                DateTime? dateOfBirth;
                DateTime applicationDate;

                IADUser adUser = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserForAdUserName(@"SAHL\WebLeads");

                // get an unused OfferKey
                string query = @"select max(OfferKey)+1 from [2AM].dbo.Offer (nolock)";
                DataTable DT = base.GetQueryResults(query);
                applicationKey = Convert.ToInt32(DT.Rows[0][0]);

                // get legal entity data
                query = @"select top 1 le.IDNumber, le.FirstNames, le.Surname, le.HomePhoneNumber, le.WorkPhoneNumber, le.CellPhoneNumber, le.EmailAddress, le.DateOfBirth, a.AddressKey
                            from [2am].dbo.LegalEntity le (nolock)
                            join [2am].dbo.LegalEntityAddress lea (nolock) on lea.LegalEntityKey = le.LegalEntityKey
	                            and lea.AddressTypeKey = 1
                            join [2am].dbo.[Address] a (nolock) on a.AddressKey = lea.AddressKey
	                            and a.AddressFormatKey = 1
                            left join [2am].dbo.[Role] r (nolock) on r.LegalEntityKey = le.LegalEntityKey
	                            and r.RoleTypeKey = 2 -- Main Applicant
                            left join [2am].[dbo].[Employment] e (nolock) on e.LegalEntityKey = le.LegalEntityKey
                            where le.LegalEntityTypeKey = 2 -- Natural Person
                            and isnull(le.DateOfBirth, '') <> ''
                            and isnull(le.IDNumber, '') <> ''
                            and isnull(le.Surname, '') <> ''
                            and isnull(le.CellPhoneNumber, '') <> ''
                            and r.LegalEntityKey is null
                            and e.LegalEntityKey is null";

                DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 1)
                {
                    iDNumber = Convert.ToString(DT.Rows[0][0]);
                    firstNames = Convert.ToString(DT.Rows[0][1]);
                    surname = Convert.ToString(DT.Rows[0][2]);
                    homePhoneNumber = Convert.ToString(DT.Rows[0][3]);
                    workPhoneNumber = Convert.ToString(DT.Rows[0][4]);
                    cellPhoneNumber = Convert.ToString(DT.Rows[0][5]);
                    emailAddress = Convert.ToString(DT.Rows[0][6]);
                    dateOfBirth = Convert.ToDateTime(DT.Rows[0][7]);
                    addressKey = Convert.ToInt32(DT.Rows[0][8]);

                    applicationDate = DateTime.Now;
                    SwitchLoanDetails switchLoanDetails = new SwitchLoanDetails((int)EmploymentTypes.Salaried, 20000, 600000, 0, 420000, 1000, false, 240);

                    ApplicantInformation applicantInformation1 = new ApplicantInformation(iDNumber, firstNames, surname, (int)SAHL.Common.Globals.SalutationTypes.Mr, homePhoneNumber, workPhoneNumber, cellPhoneNumber, emailAddress, dateOfBirth, "", true);

                    IAddressStreet addressStreet = addressRepository.GetAddressByKey(addressKey) as IAddressStreet;
                    ApplicantResidentialAddress applicantResidentialAddress = new ApplicantResidentialAddress(addressStreet.UnitNumber, addressStreet.BuildingNumber, addressStreet.BuildingName, addressStreet.StreetNumber, addressStreet.StreetName, addressStreet.RRR_SuburbDescription, addressStreet.RRR_ProvinceDescription, addressStreet.RRR_CityDescription, addressStreet.RRR_PostalCode, addressStreet.Suburb.Key);

                    ApplicantEmploymentDetails applicantEmploymentDetails1 = new ApplicantEmploymentDetails((int)EmploymentTypes.Salaried, new SalariedDetails(20000));

                    ApplicantDeclarations applicantDeclarations = new ApplicantDeclarations(true, true, true, true);

                    string request = "itcrequest";
                    string response = "itcrequest";

                    List<Applicant> applicants = new List<Applicant>() { 
                    new Applicant(applicantInformation1, applicantResidentialAddress, applicantEmploymentDetails1, applicantDeclarations, 
                        new ApplicantITC(applicationDate, request, response)) };

                    ConsultantDetails consultantDetails = new ConsultantDetails("Capitec User1", "Branch1");

                    var messages = new List<string>();
                    SwitchLoanApplication switchLoanApplication = new SwitchLoanApplication(applicationKey, 1, applicationDate, switchLoanDetails, applicants, (int)EmploymentTypes.Salaried, consultantDetails, messages);
                    applicationRepository.CreateCapitecApplication(switchLoanApplication);

                    int spvKey = 0;

                    query = string.Format(@"select SPVKey
                    from [2AM].dbo.Offer o (nolock) 
                    join (select max(OfferInformationKey) OfferInformationKey, OfferKey
		                    from [2AM].dbo.OfferInformation (nolock)
		                    group by OfferKey) max_oi on max_oi.OfferKey = o.OfferKey
                    join [2AM].[dbo].[OfferInformationVariableLoan] oivl (nolock) on oivl.OfferInformationKey = max_oi.OfferInformationKey
                    where o.OfferKey = {0}", applicationKey);

                    DT = base.GetQueryResults(query);

                    Assert.Greater(DT.Rows.Count, 0, "");

                    spvKey = Convert.ToInt32(DT.Rows[0][0]);

                    Assert.AreEqual(117, spvKey, "DetermineSPVOnApplication did not return a valid SPV key. sux.");
                }
                else
                    Assert.Ignore("Insufficient data available for test.");
            }
        }

        [Test]
        public void DetermineSPVOnCapitecApplicationTest_Alpha_NewBusiness()
        {
            IAddressRepository addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

            using (new TransactionScope(OnDispose.Rollback))
            {
                int applicationKey, addressKey;
                string iDNumber, firstNames, surname, homePhoneNumber, workPhoneNumber, cellPhoneNumber, emailAddress;
                DateTime? dateOfBirth;
                DateTime applicationDate;

                IADUser adUser = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserForAdUserName(@"SAHL\WebLeads");

                // get an unused OfferKey
                string query = @"select max(OfferKey)+1 from [2AM].dbo.Offer (nolock)";
                DataTable DT = base.GetQueryResults(query);
                applicationKey = Convert.ToInt32(DT.Rows[0][0]);

                // get legal entity data
                query = @"select top 1 le.IDNumber, le.FirstNames, le.Surname, le.HomePhoneNumber, le.WorkPhoneNumber, le.CellPhoneNumber, le.EmailAddress, le.DateOfBirth, a.AddressKey
                            from [2am].dbo.LegalEntity le (nolock)
                            join [2am].dbo.LegalEntityAddress lea (nolock) on lea.LegalEntityKey = le.LegalEntityKey
	                            and lea.AddressTypeKey = 1
                            join [2am].dbo.[Address] a (nolock) on a.AddressKey = lea.AddressKey
	                            and a.AddressFormatKey = 1
                            left join [2am].dbo.[Role] r (nolock) on r.LegalEntityKey = le.LegalEntityKey
	                            and r.RoleTypeKey = 2 -- Main Applicant
                            left join [2am].[dbo].[Employment] e (nolock) on e.LegalEntityKey = le.LegalEntityKey
                            where le.LegalEntityTypeKey = 2 -- Natural Person
                            and isnull(le.DateOfBirth, '') <> ''
                            and isnull(le.IDNumber, '') <> ''
                            and isnull(le.Surname, '') <> ''
                            and isnull(le.CellPhoneNumber, '') <> ''
                            and r.LegalEntityKey is null
                            and e.LegalEntityKey is null";

                DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 1)
                {
                    iDNumber = Convert.ToString(DT.Rows[0][0]);
                    firstNames = Convert.ToString(DT.Rows[0][1]);
                    surname = Convert.ToString(DT.Rows[0][2]);
                    homePhoneNumber = Convert.ToString(DT.Rows[0][3]);
                    workPhoneNumber = Convert.ToString(DT.Rows[0][4]);
                    cellPhoneNumber = Convert.ToString(DT.Rows[0][5]);
                    emailAddress = Convert.ToString(DT.Rows[0][6]);
                    dateOfBirth = Convert.ToDateTime(DT.Rows[0][7]);
                    addressKey = Convert.ToInt32(DT.Rows[0][8]);

                    applicationDate = DateTime.Now;
                    NewPurchaseLoanDetails newPurchaseloanDetails = new NewPurchaseLoanDetails((int)EmploymentTypes.Salaried, 15500, 450000, 60000, true, 240);

                    ApplicantInformation applicantInformation1 = new ApplicantInformation(iDNumber, firstNames, surname, (int)SAHL.Common.Globals.SalutationTypes.Mr, homePhoneNumber, workPhoneNumber, cellPhoneNumber, emailAddress, dateOfBirth, "", true);

                    IAddressStreet addressStreet = addressRepository.GetAddressByKey(addressKey) as IAddressStreet;
                    ApplicantResidentialAddress applicantResidentialAddress = new ApplicantResidentialAddress(addressStreet.UnitNumber, addressStreet.BuildingNumber, addressStreet.BuildingName, addressStreet.StreetNumber, addressStreet.StreetName, addressStreet.RRR_SuburbDescription, addressStreet.RRR_ProvinceDescription, addressStreet.RRR_CityDescription, addressStreet.RRR_PostalCode, addressStreet.Suburb.Key);

                    ApplicantEmploymentDetails applicantEmploymentDetails1 = new ApplicantEmploymentDetails((int)EmploymentTypes.Salaried, new SalariedDetails(15500));

                    ApplicantDeclarations applicantDeclarations = new ApplicantDeclarations(true, true, true, true);

                    string request = "itcrequest";
                    string response = "itcrequest";

                    List<Applicant> applicants = new List<Applicant>() { 
                    new Applicant(applicantInformation1, applicantResidentialAddress, applicantEmploymentDetails1, applicantDeclarations, 
                        new ApplicantITC(applicationDate, request, response)) };

                    ConsultantDetails consultantDetails = new ConsultantDetails("Capitec User1", "Branch1");

                    var messages = new List<string>();
                    NewPurchaseApplication newPurchaseApplication = new NewPurchaseApplication(applicationKey, 1, applicationDate, newPurchaseloanDetails, applicants, (int)EmploymentTypes.Salaried, consultantDetails, messages);
                    applicationRepository.CreateCapitecApplication(newPurchaseApplication);

                    int spvKey = 0;

                    query = string.Format(@"select SPVKey
                    from [2AM].dbo.Offer o (nolock) 
                    join (select max(OfferInformationKey) OfferInformationKey, OfferKey
		                    from [2AM].dbo.OfferInformation (nolock)
		                    group by OfferKey) max_oi on max_oi.OfferKey = o.OfferKey
                    join [2AM].[dbo].[OfferInformationVariableLoan] oivl (nolock) on oivl.OfferInformationKey = max_oi.OfferInformationKey
                    where o.OfferKey = {0}", applicationKey);

                    DT = base.GetQueryResults(query);

                    Assert.Greater(DT.Rows.Count, 0, "");

                    spvKey = Convert.ToInt32(DT.Rows[0][0]);

                    Assert.AreEqual(160, spvKey, "DetermineSPVOnApplication did not return a valid SPV key. sux.");
                }
                else
                    Assert.Ignore("Insufficient data available for test.");
            }
        }

        [Test]
        public void DetermineSPVOnCapitecApplicationTest_Alpha_Switch()
        {
            IAddressRepository addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
            IApplicationRepository applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

            using (new TransactionScope(OnDispose.Rollback))
            {
                int applicationKey, addressKey;
                string iDNumber, firstNames, surname, homePhoneNumber, workPhoneNumber, cellPhoneNumber, emailAddress;
                DateTime? dateOfBirth;
                DateTime applicationDate;

                IADUser adUser = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserForAdUserName(@"SAHL\WebLeads");

                // get an unused OfferKey
                string query = @"select max(OfferKey)+1 from [2AM].dbo.Offer (nolock)";
                DataTable DT = base.GetQueryResults(query);
                applicationKey = Convert.ToInt32(DT.Rows[0][0]);

                // get legal entity data
                query = @"select top 1 le.IDNumber, le.FirstNames, le.Surname, le.HomePhoneNumber, le.WorkPhoneNumber, le.CellPhoneNumber, le.EmailAddress, le.DateOfBirth, a.AddressKey
                            from [2am].dbo.LegalEntity le (nolock)
                            join [2am].dbo.LegalEntityAddress lea (nolock) on lea.LegalEntityKey = le.LegalEntityKey
	                            and lea.AddressTypeKey = 1
                            join [2am].dbo.[Address] a (nolock) on a.AddressKey = lea.AddressKey
	                            and a.AddressFormatKey = 1
                            left join [2am].dbo.[Role] r (nolock) on r.LegalEntityKey = le.LegalEntityKey
	                            and r.RoleTypeKey = 2 -- Main Applicant
                            left join [2am].[dbo].[Employment] e (nolock) on e.LegalEntityKey = le.LegalEntityKey
                            where le.LegalEntityTypeKey = 2 -- Natural Person
                            and isnull(le.DateOfBirth, '') <> ''
                            and isnull(le.IDNumber, '') <> ''
                            and isnull(le.Surname, '') <> ''
                            and isnull(le.CellPhoneNumber, '') <> ''
                            and r.LegalEntityKey is null
                            and e.LegalEntityKey is null";

                DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 1)
                {
                    iDNumber = Convert.ToString(DT.Rows[0][0]);
                    firstNames = Convert.ToString(DT.Rows[0][1]);
                    surname = Convert.ToString(DT.Rows[0][2]);
                    homePhoneNumber = Convert.ToString(DT.Rows[0][3]);
                    workPhoneNumber = Convert.ToString(DT.Rows[0][4]);
                    cellPhoneNumber = Convert.ToString(DT.Rows[0][5]);
                    emailAddress = Convert.ToString(DT.Rows[0][6]);
                    dateOfBirth = Convert.ToDateTime(DT.Rows[0][7]);
                    addressKey = Convert.ToInt32(DT.Rows[0][8]);

                    applicationDate = DateTime.Now;
                    SwitchLoanDetails switchLoanDetails = new SwitchLoanDetails((int)EmploymentTypes.Salaried, 15500, 450000, 0, 420000, 1000, false, 240);

                    ApplicantInformation applicantInformation1 = new ApplicantInformation(iDNumber, firstNames, surname, (int)SAHL.Common.Globals.SalutationTypes.Mr, homePhoneNumber, workPhoneNumber, cellPhoneNumber, emailAddress, dateOfBirth, "", true);

                    IAddressStreet addressStreet = addressRepository.GetAddressByKey(addressKey) as IAddressStreet;
                    ApplicantResidentialAddress applicantResidentialAddress = new ApplicantResidentialAddress(addressStreet.UnitNumber, addressStreet.BuildingNumber, addressStreet.BuildingName, addressStreet.StreetNumber, addressStreet.StreetName, addressStreet.RRR_SuburbDescription, addressStreet.RRR_ProvinceDescription, addressStreet.RRR_CityDescription, addressStreet.RRR_PostalCode, addressStreet.Suburb.Key);

                    ApplicantEmploymentDetails applicantEmploymentDetails1 = new ApplicantEmploymentDetails((int)EmploymentTypes.Salaried, new SalariedDetails(15500));

                    ApplicantDeclarations applicantDeclarations = new ApplicantDeclarations(true, true, true, true);

                    string request = "itcrequest";
                    string response = "itcrequest";

                    List<Applicant> applicants = new List<Applicant>() { 
                    new Applicant(applicantInformation1, applicantResidentialAddress, applicantEmploymentDetails1, applicantDeclarations, 
                        new ApplicantITC(applicationDate, request, response)) };

                    ConsultantDetails consultantDetails = new ConsultantDetails("Capitec User1", "Branch1");

                    var messages = new List<string>();
                    SwitchLoanApplication switchLoanApplication = new SwitchLoanApplication(applicationKey, 1, applicationDate, switchLoanDetails, applicants, (int)EmploymentTypes.Salaried, consultantDetails, messages);
                    applicationRepository.CreateCapitecApplication(switchLoanApplication);

                    int spvKey = 0;

                    query = string.Format(@"select SPVKey
                    from [2AM].dbo.Offer o (nolock) 
                    join (select max(OfferInformationKey) OfferInformationKey, OfferKey
		                    from [2AM].dbo.OfferInformation (nolock)
		                    group by OfferKey) max_oi on max_oi.OfferKey = o.OfferKey
                    join [2AM].[dbo].[OfferInformationVariableLoan] oivl (nolock) on oivl.OfferInformationKey = max_oi.OfferInformationKey
                    where o.OfferKey = {0}", applicationKey);

                    DT = base.GetQueryResults(query);

                    Assert.Greater(DT.Rows.Count, 0, "");

                    spvKey = Convert.ToInt32(DT.Rows[0][0]);

                    Assert.AreEqual(160, spvKey, "DetermineSPVOnApplication did not return a valid SPV key. sux.");
                }
                else
                    Assert.Ignore("Insufficient data available for test.");
            }
        }

        #endregion Capitec SPVService tests
    }
}
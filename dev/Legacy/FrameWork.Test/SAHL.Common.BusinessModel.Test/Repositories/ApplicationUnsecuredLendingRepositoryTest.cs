using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    internal class ApplicationUnsecuredLendingRepositoryTest : TestBase
    {
        [Test]
        public void CalculateUnsecuredLendingWithoutLifeTest()
        {
            using (new SessionScope())
            {
                IApplicationUnsecuredLendingRepository apULRepo = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                IMarketRateRepository marketRateRepo = RepositoryFactory.GetRepository<IMarketRateRepository>();
                double monthlyFee = 57.0;
                double initiationFee = 1140.0;
                IMarketRate marketRate = marketRateRepo.GetMarketRateByKey((int)MarketRates.PrimeLendingRate);
                double amount = 50000;

                List<int> terms = new List<int>();
                terms.Add(12);
                bool creditLifePolicySelected = false;

                IResult calcUL = apULRepo.CalculateUnsecuredLending(amount, terms, creditLifePolicySelected);

                Assert.Greater(calcUL.CalculatedItems[0].TotalInstalment, 0);

                Assert.AreEqual(calcUL.Amount, amount, "Amount mismatch");
                Assert.AreEqual((double)calcUL.MonthlyFee, monthlyFee, "Monthly fee mismatch");
                Assert.AreEqual((double)calcUL.InitiationFee, initiationFee, "InitiationFee mismatch");
                Assert.GreaterOrEqual(calcUL.CreditLifePremium, 0, "Life Premium should not be calculated");
            }
        }

        [Test]
        public void CalculateUnsecuredLendingCreditLifePolicySelectedTest()
        {
            // Will not return creditCriteriaItems
            using (new SessionScope())
            {
                IApplicationUnsecuredLendingRepository apULRepo = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                IMarketRateRepository marketRateRepo = RepositoryFactory.GetRepository<IMarketRateRepository>();
                double monthlyFee = 57.0;
                double initiationFee = 1140.0;
                IMarketRate marketRate = marketRateRepo.GetMarketRateByKey((int)MarketRates.PrimeLendingRate);
                double amount = 50000;

                List<int> terms = new List<int>();
                terms.Add(12);

                bool creditLifePolicySelected = true;

                IResult calcUL = apULRepo.CalculateUnsecuredLending(amount, terms, creditLifePolicySelected);
                Assert.AreEqual(calcUL.Amount, amount, "Amount mismatch");
                Assert.AreEqual((double)calcUL.MonthlyFee, monthlyFee, "Monthly fee mismatch");
                Assert.AreEqual((double)calcUL.InitiationFee, initiationFee, "InitiationFee mismatch");
                Assert.Greater(calcUL.CreditLifePremium, 0, "LifePremium is not being calculated");
            }
        }

        [Test]
        public void CalculateUnsecuredLendingDefaultCalculatedItemsCount()
        {
            using (new SessionScope())
            {
                IApplicationUnsecuredLendingRepository apULRepo = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                IMarketRateRepository marketRateRepo = RepositoryFactory.GetRepository<IMarketRateRepository>();
                IMarketRate marketRate = marketRateRepo.GetMarketRateByKey((int)MarketRates.PrimeLendingRate);
                double amount = 49000;
                List<int> terms = new List<int>();
                terms.Add(6);
                bool creditLifePolicySelected = true;
                IResult calcUL = apULRepo.CalculateUnsecuredLending(amount, terms, creditLifePolicySelected);
                Assert.AreEqual(calcUL.CalculatedItems.Count, 8, "The item list should only include the 8 defaults");
            }
        }

        [Test]
        public void CalculateUnsecuredLendingCalculatedItemsCountIncludingUserDefined()
        {
            using (new SessionScope())
            {
                IApplicationUnsecuredLendingRepository apULRepo = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                IMarketRateRepository marketRateRepo = RepositoryFactory.GetRepository<IMarketRateRepository>();
                IMarketRate marketRate = marketRateRepo.GetMarketRateByKey((int)MarketRates.PrimeLendingRate);
                double amount = 49000;
                List<int> terms = new List<int>();
                terms.Add(10);
                bool creditLifePolicySelected = true;
                IResult calcUL = apULRepo.CalculateUnsecuredLending(amount, terms, creditLifePolicySelected);
                Assert.Greater(calcUL.CalculatedItems[0].TotalInstalment, 0);
                Assert.AreEqual(calcUL.CalculatedItems.Count, 9, "The item list should only include the 8 defaults and 1 User defined line item");
            }
        }

        [Test]
        public void CalculateUnsecuredLendingDefaultCalculatedItemsAreAllPresent()
        {
            using (new SessionScope())
            {
                IApplicationUnsecuredLendingRepository apULRepo = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                IMarketRateRepository marketRateRepo = RepositoryFactory.GetRepository<IMarketRateRepository>();
                IMarketRate marketRate = marketRateRepo.GetMarketRateByKey((int)MarketRates.PrimeLendingRate);
                double amount = 49000;
                List<int> terms = new List<int>();
                terms.Add(6);
                bool creditLifePolicySelected = true;
                IResult calcUL = apULRepo.CalculateUnsecuredLending(amount, terms, creditLifePolicySelected);
                Assert.AreEqual(calcUL.CalculatedItems[0].Term, 6, "The 6 month term item is missing");
                Assert.AreEqual(calcUL.CalculatedItems[1].Term, 12, "The 12 month term item is missing");
                Assert.AreEqual(calcUL.CalculatedItems[2].Term, 18, "The 18 month term item is missing");
                Assert.AreEqual(calcUL.CalculatedItems[3].Term, 24, "The 24 month term item is missing");
                Assert.AreEqual(calcUL.CalculatedItems[4].Term, 30, "The 30 month term item is missing");
                Assert.AreEqual(calcUL.CalculatedItems[5].Term, 36, "The 36 month term item is missing");
                Assert.AreEqual(calcUL.CalculatedItems[6].Term, 42, "The 42 month term item is missing");
                Assert.AreEqual(calcUL.CalculatedItems[7].Term, 48, "The 48 month term item is missing");
            }
        }

        [Test]
        public void CalculateUnsecuredLendingDefaultCalculatedItemsAreAllPresentFromList()
        {
            using (new SessionScope())
            {
                IApplicationUnsecuredLendingRepository apULRepo = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                IMarketRateRepository marketRateRepo = RepositoryFactory.GetRepository<IMarketRateRepository>();
                IMarketRate marketRate = marketRateRepo.GetMarketRateByKey((int)MarketRates.PrimeLendingRate);
                double amount = 49000;
                // int? term = new int?();
                bool creditLifePolicySelected = true;

                List<int> terms = new List<int>();
                terms.Add(10);
                terms.Add(11);
                terms.Add(6);
                terms.Add(12);
                IResult calcUL = apULRepo.CalculateUnsecuredLending(amount, terms, creditLifePolicySelected);
                Assert.AreEqual(calcUL.CalculatedItems.Count, 10, "The custom term item are missing");
            }
        }

        [Test]
        public void CreateAndOpenPersonalLoanTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                // Find an offer that has been accepted but not disbursed yet (no account created)
                var query = @"  select top 1 o.OfferKey, o.ReservedAccountKey, acc.AccountKey from Offer o
                                left join Account acc on o.ReservedAccountKey = acc.AccountKey
                                where OfferTypeKey = 11 and acc.AccountKey is null and OfferStatusKey = 3
                                order by o.OfferKey desc";
                DataTable results = base.GetQueryResults(query);
                if (results.Rows.Count == 0)
                {
                    Assert.Ignore("No Data to perform CreateAndOpenPersonalLoan Test");
                }
                int applicationKey = Convert.ToInt32(results.Rows[0][0]);
                int reservedAccountKey = Convert.ToInt32(results.Rows[0][1]);
                string userID = "System";
                IApplicationUnsecuredLendingRepository apULRepo = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                // Create the Account
                apULRepo.CreateAndOpenPersonalLoan(reservedAccountKey, userID);
                // Check if the account was created.
                query = string.Format("select top 1 * from Account (nolock) where AccountKey = {0}", reservedAccountKey);
                results = base.GetQueryResults(query);
                Assert.Greater(results.Rows.Count, 0);
            }
        }

        public void DisbursePersonalLoanTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                // Find an offer that has been accepted but not disbursed yet (no account created)
                var query = @"   select top 1 o.OfferKey, o.ReservedAccountKey from Offer o
                                 left join Account acc on o.ReservedAccountKey = acc.AccountKey
                                 where OfferTypeKey = 11 and acc.AccountKey is null and OfferStatusKey = 3
                                 order by o.OfferKey desc";

                DataTable results = base.GetQueryResults(query);

                if (results.Rows.Count == 0)
                {
                    Assert.Ignore("No Data to perform DisbursePersonalLoanTest Test");
                }

                int applicationKey = Convert.ToInt32(results.Rows[0][0]);
                int reservedAccountKey = Convert.ToInt32(results.Rows[0][1]);
                string userID = "System";

                var apULRepo = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();

                apULRepo.CreateAndOpenPersonalLoan(reservedAccountKey, userID);

                // disburse the funds
                apULRepo.DisbursePersonalLoan(reservedAccountKey, userID);

                // find the disbursement transaction
                query = string.Format(@"select top 1 ft.FinancialTransactionKey
                                        from fin.FinancialTransaction ft (nolock)
                                        join FinancialService fs (nolock) on ft.FinancialServiceKey = fs.FinancialServiceKey
                                        where TransactionTypeKey = 2014 and fs.AccountKey = {0}", reservedAccountKey);

                results = base.GetQueryResults(query);

                Assert.Greater(results.Rows.Count, 0);
            }
        }

        [Ignore]
        [Test]
        public void CreatePersonalLoanLeadTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IApplicationUnsecuredLendingRepository apULRepo = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                // Create the Account
                apULRepo.CreatePersonalLoanLead("6812095219087");


            }
        }
    }
}
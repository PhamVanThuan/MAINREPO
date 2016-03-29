using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Test;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class BankAccountQueriesTest : TestBase
    {
        /// <summary>
        /// Performs a basic search - this test just ensures that all the criteria searchs don't raise any errors.
        /// </summary>
        [Test]
        public void BasicSearch()
        {
            IBankAccountSearchCriteria searchCriteria = new BankAccountSearchCriteria();
            searchCriteria.ACBBranchKey = "0";
            searchCriteria.ACBTypeKey = 1;
            searchCriteria.AccountName = "A";
            searchCriteria.AccountNumber = "1";

            SAHL.Common.BusinessModel.Repositories.BankAccountLegalEntitySearchQuery query = new SAHL.Common.BusinessModel.Repositories.BankAccountLegalEntitySearchQuery(searchCriteria);
            IList<LegalEntityBankAccount_DAO> bankAccounts = LegalEntityBankAccount_DAO.ExecuteQuery(query) as IList<LegalEntityBankAccount_DAO>;
        }

        /// <summary>
        /// Checks that IsEmpty works for all criteria.
        /// </summary>
        [Test]
        public void IsEmptyCheck()
        {
            IBankAccountSearchCriteria searchCriteria = new BankAccountSearchCriteria();
            Assert.IsTrue(searchCriteria.IsEmpty);

            searchCriteria = new BankAccountSearchCriteria();
            searchCriteria.ACBBranchKey = "0";
            Assert.IsFalse(searchCriteria.IsEmpty);

            searchCriteria = new BankAccountSearchCriteria();
            searchCriteria.ACBTypeKey = 1;
            Assert.IsFalse(searchCriteria.IsEmpty);

            searchCriteria = new BankAccountSearchCriteria();
            searchCriteria.AccountName = "A";
            Assert.IsFalse(searchCriteria.IsEmpty);

            searchCriteria = new BankAccountSearchCriteria();
            searchCriteria.AccountNumber = "1";
            Assert.IsFalse(searchCriteria.IsEmpty);
        }
    }
}
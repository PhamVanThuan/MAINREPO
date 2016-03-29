using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using System.Data;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class LoanAgreementRepositoryTest : TestBase
    {
        private ILoanAgreementRepository _repo = RepositoryFactory.GetRepository<ILoanAgreementRepository>();

        [NUnit.Framework.Test]
        public void CreateLoanAgreementTest()
        {
            DateTime AgreementTime = DateTime.Now;
            double Amount = 100000.00;
            DateTime ChangeDate = DateTime.Now;

            string query = "select top 1 * from Bond where BondRegistrationNumber != '' and BondRegistrationNumber is not null";
            DataTable DT = base.GetQueryResults(query);
            Assert.That(DT.Rows.Count == 1);
            IBondRepository Brepo = RepositoryFactory.GetRepository<IBondRepository>();
            IReadOnlyEventList<IBond> bonds = Brepo.GetBondByRegistrationNumber(DT.Rows[0]["BondRegistrationNumber"].ToString());


            ILoanAgreement loanAgree = _repo.CreateLoanAgreement(AgreementTime, Amount, ChangeDate, bonds[0], "nUnit Test");

            Assert.IsNotNull(loanAgree);
        }
    }
}

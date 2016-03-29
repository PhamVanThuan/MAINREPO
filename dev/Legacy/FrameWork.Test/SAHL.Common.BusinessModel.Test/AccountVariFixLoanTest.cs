using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using System.Data;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class AccountVariFixLoanTest : TestBase
    {
        [Test]
        public void FixedSecuredMortgageLoanTest()
        {
            // get an item from the db that is an AccountVarifixLoan and has an AccountStatusKey = 3 (Application),
            // then get that object and use it to call the FixedSecuredMortgageLoan
            // create items for the test and try and save a new object -- 2168061

            using (new SessionScope())
            {
                string query = String.Format(@"SELECT   top 1  act.AccountKey
                    FROM fin.MortgageLoan AS mort (nolock)
                    INNER JOIN FinancialService AS fs (nolock) ON mort.FinancialServiceKey = fs.FinancialServiceKey 
                    INNER JOIN Account AS act (nolock) ON fs.AccountKey = act.AccountKey
                    WHERE     (act.RRR_ProductKey = 2) AND act.accountstatuskey =1
                    order by fs.AccountKey desc");
                    DataTable DT = base.GetQueryResults(query);
                             
                int accountKey = Convert.ToInt32(DT.Rows[0][0]); 
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAccountVariFixLoan acc = BMTM.GetMappedType<IAccountVariFixLoan, AccountVariFixLoan_DAO>(AccountVariFixLoan_DAO.Find(accountKey) as AccountVariFixLoan_DAO);
                IMortgageLoan mort = acc.FixedSecuredMortgageLoan;
                Console.WriteLine("OUTPUT " + mort);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Web.AJAX;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel;
using SAHL.Common.Web.UI.Controls;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;

namespace SAHL.Web.Test.AJAX
{
    [TestFixture]
    public class BankService : TestViewBase
    {

        private Bank _bankService;


        [SetUp]
        public void Setup()
        {
            _bankService = new Bank();
        }

        [TearDown]
        public void TearDown()
        {
            _bankService = null;
        }

        [NUnit.Framework.Test]       
        public void GetBranches()
        {
            bool found = false;
            ACBBank_DAO bank = ACBBank_DAO.FindFirst();

            SimpleQuery q = new SimpleQuery(typeof(ACBBranch_DAO), @"
                from ACBBranch_DAO b 
                where b.ACBBank = ?
                and b.ActiveIndicator = 0",
                bank
            );
            q.SetQueryRange(5);
            List<ACBBranch_DAO> lstDao = new List<ACBBranch_DAO>((ACBBranch_DAO[])ActiveRecordBase.ExecuteQuery(q));


            ACBBranch_DAO branch = lstDao[0];
            string desc = branch.ACBBranchDescription;  // use whole description so we can ensure the bank is within the limited number of branches returned
            SAHLAutoCompleteItem[] branches = _bankService.GetBranches(desc, bank.Key.ToString());
            foreach (SAHLAutoCompleteItem b in branches)
            {
                if (b.Value == branch.Key)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                Assert.Fail("The branch with prefix '{0}' could not be found for bank '{1}'", desc, bank.ACBBankDescription);
            }
        }
    }
}

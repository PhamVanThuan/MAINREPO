using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [Ignore("Need to fix this test")]
    [TestFixture]
    public class DebitOrderTransaction_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {
                DebitOrderTransactions_DAO dots = new DebitOrderTransactions_DAO();
                dots.DebitOrderDate = DateTime.Now;
                dots.LoanNumber = 1432899;
                dots.Installment = 70.76;
                dots.Status = 2;
                dots.CDExportNumber = 3364310;
                dots.ActionDate = DateTime.Now;

                dots.CreateAndFlush();

                dots.Refresh();

                dots.DeleteAndFlush();
            }
        }

    }
}

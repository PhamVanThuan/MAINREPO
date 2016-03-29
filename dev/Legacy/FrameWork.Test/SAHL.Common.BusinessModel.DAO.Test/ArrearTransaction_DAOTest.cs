using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class ArrearTransaction_DAOTest : TestBase
    {
        [Test]
		[Ignore("Needs to be fixed with Revamp")]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {
				//Needs to be fixed with Revamp
                ArrearTransaction_DAO arrTrans = new ArrearTransaction_DAO();
				//arrTrans.TransactionTypeNumber = 110;
				//arrTrans.ArrearTransactionInsertDate = DateTime.Now;
				//arrTrans.ArrearTransactionEffectiveDate = DateTime.Now;
				//arrTrans.ArrearTransactionActualEffectiveDate = DateTime.Now;
				//arrTrans.ArrearTransactionRate = 0.164999992f;
				//arrTrans.ArrearTransactionAmount = 430000;
				//arrTrans.ArrearTransactionNewBalance = 430000;
				//arrTrans.ArrearTransactionUserid = "Testing";
                arrTrans.FinancialService = FinancialService_DAO.FindFirst();

                arrTrans.CreateAndFlush();

                arrTrans.Refresh();

                arrTrans.DeleteAndFlush();
            }
        }

    }
}

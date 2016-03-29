using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class FinancialTransaction_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {
                FinancialTransaction_DAO LoanTran = new FinancialTransaction_DAO();
                //LoanTran.LoanNumber = 10023581;
                LoanTran.FinancialService = FinancialService_DAO.FindFirst();
                LoanTran.TransactionType = TransactionType_DAO.FindFirst();
                LoanTran.InsertDate = DateTime.Now;
                LoanTran.EffectiveDate = DateTime.Now;
                LoanTran.InterestRate = 0.2312f;
                LoanTran.Amount = 0.0;
                LoanTran.Balance = 12.0;
                LoanTran.UserID = "Testing";
                LoanTran.SPV = SPV_DAO.FindFirst();
                //LoanTran.SPVNumber = 4;

                LoanTran.CreateAndFlush();

                LoanTran.Refresh();
                
                LoanTran.DeleteAndFlush();


                
            }
        
        }
    }
}

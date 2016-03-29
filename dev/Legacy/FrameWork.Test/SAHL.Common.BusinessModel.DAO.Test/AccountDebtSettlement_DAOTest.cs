using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using NUnit.Framework;


namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="AccountDebtSettlement_DAO"/> entity.
    /// </summary>
//    [TestFixture]
    public class AccountDebtSettlement_DAOTest : TestBase
    {

        #region Static helper methods

        public static AccountDebtSettlement_DAO CreateAccountDebtSettlement()
        {
            AccountDebtSettlement_DAO DebtSettlement = new AccountDebtSettlement_DAO();

            DebtSettlement.AccountExpense = AccountExpense_DAOTest.CreateAccountExpense();
            DebtSettlement.AccountExpense.Create();
            DebtSettlement.CapitalAmount = 42000;
            DebtSettlement.SettlementAmount = 4200;
            DebtSettlement.DisbursementType = DisbursementType_DAO.FindFirst();
            DebtSettlement.DisbursementInterestApplied = DisbursementInterestApplied_DAO.FindFirst();
            return DebtSettlement;
        }

        public static void DeleteAccountDebtSettlement(AccountDebtSettlement_DAO ads)
        {
            AccountExpense_DAO Expense = ads.AccountExpense;
            ads.Delete();
            Expense.Delete();
        }


        #endregion

    }
}

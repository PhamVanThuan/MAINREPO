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
    /// Class for testing the <see cref="AccountInformation_DAO"/> entity.
    /// </summary>
//    [TestFixture]
    public class AccountInformation_DAOTest : TestBase
    {
        #region Static helper methods

        public static AccountInformation_DAO CreateAccountInformation()
        {
            AccountInformation_DAO Info = new AccountInformation_DAO();
            Info.AccountInformationType = AccountInformationType_DAO.FindFirst();
            Info.Amount = 100.0d;
            Info.Information = "Test information";
            return Info;
        }


        #endregion
    }
}

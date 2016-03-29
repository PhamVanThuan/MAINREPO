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
    /// Class for testing the <see cref="Disbursement_DAO"/> entity.
    /// </summary>
    //[TestFixture]
    public class Disbursement_DAOTest : TestBase
    {
        //[Test]
        //public void Save()
        //{
        //    using (new SessionScope())
        //    {
        //        int acckey = 1257118;
        //        Disbursement_DAO[] D1 = Disbursement_DAO.FindAllByProperty("Account.Key", acckey);


        //        Disbursement_DAO Disb = new Disbursement_DAO();
        //        //Disb.DisbursementStatus = DisbursementStatus_DAO.FindFirst();
        //        Disb.Account = Account_DAO.FindFirst();

        //        Disb.SaveAndFlush();

        //        Disb.DeleteAndFlush();
        //    }
        //}
    }
}
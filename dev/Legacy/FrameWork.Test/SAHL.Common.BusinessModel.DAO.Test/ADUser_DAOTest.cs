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
    /// Class for testing the <see cref="ADUser_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class ADUser_DAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoad()
        {
            using(new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ADUser_DAO aduser = new ADUser_DAO();
                aduser.ADUserName = "Test";
                aduser.GeneralStatusKey = GeneralStatus_DAO.FindFirst();
                aduser.Password = "TestPassword";
                aduser.PasswordQuestion = "TestQuestion";
                aduser.PasswordAnswer = "TestAnswer";

                aduser.LegalEntity = LegalEntityNaturalPerson_DAO.FindFirst();
                aduser.CreateAndFlush();

                // now try and load it
                ADUser_DAO aduser2 = ADUser_DAO.Find(aduser.Key);
                Assert.IsNotNull(aduser2);
            }
	
        }
    }
}

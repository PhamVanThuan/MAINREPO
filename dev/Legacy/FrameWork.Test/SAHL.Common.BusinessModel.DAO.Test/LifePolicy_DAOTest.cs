using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;

using Castle.ActiveRecord;
using NUnit.Framework;
using NHibernate;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="LifePolicy_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class LifePolicy_DAOTest : TestBase
    {
        [Test]
        [Ignore("DONT HARD CODE A KEY!!")]
        public void GetLifePolicyFromAccount()
        {
            using (SessionScope S = new SessionScope())
            { 
               // ISession Session = S.GetSession(null);

               // ICriteria Crit =  Session.CreateCriteria(typeof(Account_DAO));
               // Crit.Add(Expression.Eq("Product", 4);
               //// Account_DAO Acc = Account_DAO.FindOne(NHibernate.Expression.DetachedCriteria
                LifePolicy_DAO LP = base.TestFind<LifePolicy_DAO>("LifePolicy", "FinancialServiceKey");
                Account_DAO Acc = LP.Account;
                FinancialService_DAO FS = Acc.FinancialServices[0];
                FinancialServiceType_DAO FST = FS.FinancialServiceType;

                int PrimaryKey = 1715278;
                Acc = (Account_DAO) base.TestFind<Account_DAO>(PrimaryKey);
                int acckey = Acc.Key;
     
            }
        }
    }
}


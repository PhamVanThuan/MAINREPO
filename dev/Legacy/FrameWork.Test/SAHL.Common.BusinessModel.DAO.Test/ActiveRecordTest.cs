using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Castle.ActiveRecord;
using NHibernate.Engine.Query;
using NHibernate;
using Castle.ActiveRecord.Queries;
using SAHL.Test;
using Castle.ActiveRecord.Framework;
using SAHL.Common.DataAccess;
using System.Data;
using NHibernate.Criterion;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class ActiveRecordTest: TestBase
    {
        [Test]
        public void DetachObj()               
        {
            try
            {
                string refrr;
                SessionScope Scope = null;
                Application_DAO app;
                ISession Sess = null;

                using (Scope = new SessionScope())
                {
                    app = new ApplicationMortgageLoanNewPurchase_DAO();//Application_DAO.Find(136085);
                    //refrr = app.Reference;
                    app.Reference = "mieeu mieeu mieeu";
                    ApplicationExpense_DAO Expense = new ApplicationExpense_DAO();
                    Expense.ExpenseAccountName = "testing";
                    Expense.ExpenseType = new ExpenseType_DAO();
                    app.ApplicationExpenses = new List<ApplicationExpense_DAO>();
                    app.ApplicationExpenses.Add(Expense);
                    app.OriginationSource = OriginationSource_DAO.Find(1);
                    app.ApplicationStatus = ApplicationStatus_DAO.Find(1);
                    //app.ApplicationType = ApplicationType_DAO.Find(7);
                    ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();

                    Sess = sessionHolder.CreateSession(typeof(Application_DAO));
                    Sess.Evict(app);
                }

                using (Scope = new SessionScope())
                {
                    Sess = null;
                    ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();

                    Sess = sessionHolder.CreateSession(typeof(Application_DAO));
                    Sess.Update(app);

                    app.Reference = "karamba !! ai!!";

                    Sess.Evict(app);
                }

                using (Scope = new SessionScope())
                {
                    Sess = null;
                    ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();

                    Sess = sessionHolder.CreateSession(typeof(Application_DAO));
                    Sess.Update(app);

                    Sess.Evict(app);
                }
            }
            catch (Exception E)
            {
                string M = E.Message;
            }
            
        }
    }
}
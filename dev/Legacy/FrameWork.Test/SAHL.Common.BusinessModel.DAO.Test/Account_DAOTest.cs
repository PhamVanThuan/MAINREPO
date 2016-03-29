using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Internal;
using NHibernate.Criterion;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Test;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="Account"/> entity.
    /// </summary>
    [TestFixture]
    public class Account_DAOTest : TestBase
    {
        /// <summary>
        /// Tests the retrieval of applications off an Account.
        /// </summary>
        [Test]
        public void Applications()
        {
            using (new SessionScope())
            {
                //ActiveRecordStarter.GenerateCreationScripts("c:\\2am.sql");
                object key = base.GetPrimaryKey("Account", "AccountKey", " OriginationSourceProductKey = 8 ");  // we know RCS loan accounts have offers.

                //int key = 1294297;
                Account_DAO Acc = (Account_DAO)base.TestFind<Account_DAO>(key);
                IList<Application_DAO> Applications = Acc.Applications;
                foreach (Application_DAO app in Applications)
                {
                    int cnt = app.ApplicationInformations.Count;
                }
            }
        }

        //[Test]
        public void AccountRelationshipCreate()
        {
            ILookupRepository lookupRepo = new LookupRepository();
            DomainMessageCollection messages = new DomainMessageCollection();

            using (new SessionScope())
            {
                try
                {
                    Account_DAO MAcc = Account_DAO.Find(1240051);
                    // Reserve an account key
                    AccountSequence_DAO accountSequence = new AccountSequence_DAO();
                    accountSequence.SaveAndFlush();

                    OriginationSourceProduct_DAO OSPDAO = OriginationSourceProduct_DAO.Find(1);

                    AccountLifePolicy_DAO acc = new AccountLifePolicy_DAO();
                    acc.Key = accountSequence.Key;
                    acc.AccountStatus = AccountStatus_DAO.Find(6);
                    acc.FixedPayment = 0.0;
                    acc.OriginationSourceProduct = OSPDAO;
                    acc.UserID = "TestUser";
                    acc.ChangeDate = System.DateTime.Now;
                    acc.InsertedDate = System.DateTime.Now;
                    acc.ParentAccount = new Account_DAO();

                    acc.CreateAndFlush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        [Test]
        public void Blah()
        {
            ActiveRecordModel arm = ActiveRecordModel.GetModel(typeof(ApplicationRoleType_DAO));

            DetachedCriteria DC = DetachedCriteria.For<ApplicationRoleType_DAO>();

            foreach (BelongsToModel am in arm.BelongsTo)
            {
                DC.SetFetchMode(am.Property.Name, NHibernate.FetchMode.Eager);
            }

            foreach (HasAndBelongsToManyModel am in arm.HasAndBelongsToMany)
            {
                if (am.HasManyAtt.Lazy == false)
                    DC.SetFetchMode(am.Property.Name, NHibernate.FetchMode.Eager);
            }

            foreach (HasManyModel am in arm.HasMany)
            {
                if (am.HasManyAtt.Lazy == false)
                    DC.SetFetchMode(am.Property.Name, NHibernate.FetchMode.Eager);
            }

            foreach (OneToOneModel am in arm.OneToOnes)
            {
                DC.SetFetchMode(am.Property.Name, NHibernate.FetchMode.Eager);
            }

            ApplicationRoleType_DAO[] list = ApplicationRoleType_DAO.FindAll(DC);
        }
    }
}
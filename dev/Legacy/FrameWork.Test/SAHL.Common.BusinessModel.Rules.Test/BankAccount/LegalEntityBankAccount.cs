using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Rules.BankAccount;
using Rhino.Mocks;
using SAHL.Test;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.BankAccount.LegalEntityBankAccount;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Test.BankAccount
{
    [TestFixture]
    public class LegalEntityBankAccount : RuleBase
    {

        IBankAccount _ba = null;
        IACBBranch _br = null;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #region BankAccountDebitOrderDoNotDelete      
        [NUnit.Framework.Test]
        public void BankAccountDebitOrderDoNotDeleteSuccess()
        {
            BankAccountDebitOrderDoNotDelete rule = new BankAccountDebitOrderDoNotDelete();

            // create items for the test and try and save a new object
            using (new SessionScope())
            {
                ////////////////////////////////////////////////////////////////////
                // get a legalentitybankaccount object that is linked through to a financialservice object.
                // to this fs object we will now add the rest of what we need.
                string HQL = @"select leba from LegalEntityBankAccount_DAO leba inner join 
                        leba.LegalEntity le inner join le.Roles r inner join r.Account acc inner join acc.FinancialServices fs where leba.GeneralStatus.Key = 1";

                SimpleQuery<LegalEntityBankAccount_DAO> query1 = new SimpleQuery<LegalEntityBankAccount_DAO>(HQL);
                query1.SetQueryRange(1);
                LegalEntityBankAccount_DAO[] res1 = query1.Execute();

                // create the first item here...
                //leba.LegalEntity.Roles[0].Account.FinancialServices[0].FinancialServiceBankAccounts[0].BankAccount.Key == leba.BankAccount.Key;
                FinancialServiceBankAccount_DAO fsba = DAODataConsistancyChecker.GetDAO<FinancialServiceBankAccount_DAO>();
                fsba.FinancialService = res1[0].LegalEntity.Roles[0].Account.FinancialServices[0];
                fsba.ProviderKey = null;
                fsba.BankAccount = res1[0].BankAccount;
                fsba.SaveAndFlush();

                // now to create the other item
                //leba.LegalEntity.ApplicationRoles[0].Application.ApplicationDebitOrders[0].BankAccount.Key == leba.BankAccount.Key;
                // then the ApplicationDebitOrder object
                //ApplicationDebitOrder_DAO appDebitOrder = DAODataConsistancyChecker.GetDAO<ApplicationDebitOrder_DAO>();
                //appDebitOrder.BankAccount = res1[0].BankAccount;
                
                //// then the Application object
                //ApplicationUnknown_DAO app = DAODataConsistancyChecker.GetDAO<ApplicationUnknown_DAO>();
                
                //app.SaveAndFlush();
                //appDebitOrder.Application = app;
                //appDebitOrder.SaveAndFlush();

                //ApplicationRoleType_DAO appRoleType = DAODataConsistancyChecker.GetDAO<ApplicationRoleType_DAO>();
                //appRoleType.Key = 11;

                //// then the ApplicationRoles object (only client types)
                //ApplicationRole_DAO appRole = DAODataConsistancyChecker.GetDAO<ApplicationRole_DAO>();
                //appRole.ApplicationRoleType = appRoleType;
                //appRole.Application = app;
                
                //appRole.LegalEntity = res1[0].LegalEntity;
                //appRole.SaveAndFlush();

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntityBankAccount leba = BMTM.GetMappedType<ILegalEntityBankAccount>(res1[0]);
                ExecuteRule(rule, 0, leba);

                // delete the items we created here

                fsba.DeleteAndFlush();

                //appRole.DeleteAndFlush();
                //appDebitOrder.DeleteAndFlush();
            }

        }
       
        [NUnit.Framework.Test]
        public void BankAccountDebitOrderDoNotDeleteFail()
        {
            BankAccountDebitOrderDoNotDelete rule = new BankAccountDebitOrderDoNotDelete();

            // create items for the test and try and save a new object
            using (new SessionScope())
            {
                ////////////////////////////////////////////////////////////////////
                // get a legalentitybankaccount object that is linked through to a financialservice object.
                // to this fs object we will now add the rest of what we need.
                string HQL = @"select leba from LegalEntityBankAccount_DAO leba 
                                inner join leba.LegalEntity le 
                                inner join le.Roles r 
                                inner join r.Account acc 
                                inner join acc.FinancialServices fs 
                                where leba.GeneralStatus.Key = 2 order by leba.Key";

                SimpleQuery<LegalEntityBankAccount_DAO> query1 = new SimpleQuery<LegalEntityBankAccount_DAO>(HQL);
                query1.SetQueryRange(1);
                LegalEntityBankAccount_DAO[] res1 = query1.Execute();

                // create the first item here...
                //leba.LegalEntity.Roles[0].Account.FinancialServices[0].FinancialServiceBankAccounts[0].BankAccount.Key == leba.BankAccount.Key;
                FinancialServiceBankAccount_DAO fsba = DAODataConsistancyChecker.GetDAO<FinancialServiceBankAccount_DAO>();
                fsba.FinancialService = res1[0].LegalEntity.Roles[0].Account.FinancialServices[0];
                fsba.ProviderKey = null;

                Console.WriteLine("leba key: " + res1[0].Key.ToString());

                fsba.BankAccount = res1[0].BankAccount;
                fsba.SaveAndFlush();

                // now to create the other item
                //leba.LegalEntity.ApplicationRoles[0].Application.ApplicationDebitOrders[0].BankAccount.Key == leba.BankAccount.Key;
                // then the ApplicationDebitOrder object
                ApplicationDebitOrder_DAO appDebitOrder = DAODataConsistancyChecker.GetDAO<ApplicationDebitOrder_DAO>();
                appDebitOrder.BankAccount = res1[0].BankAccount;

                // then the Application object
                ApplicationUnknown_DAO app = DAODataConsistancyChecker.GetDAO<ApplicationUnknown_DAO>();

                app.SaveAndFlush();
                appDebitOrder.Application = app;
                appDebitOrder.SaveAndFlush();

                ApplicationRoleType_DAO appRoleType = DAODataConsistancyChecker.GetDAO<ApplicationRoleType_DAO>();
                appRoleType.Key = 11;

                // then the ApplicationRoles object (only client types)
                ApplicationRole_DAO appRole = DAODataConsistancyChecker.GetDAO<ApplicationRole_DAO>();
                appRole.ApplicationRoleType = appRoleType;
                appRole.ApplicationKey = app.Key;

                appRole.LegalEntityKey = res1[0].LegalEntity.Key;
                appRole.SaveAndFlush();

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntityBankAccount leba = BMTM.GetMappedType<ILegalEntityBankAccount>(res1[0]);
                ExecuteRule(rule, 1, leba);

                //Dont need to delete, as the transaction scope declared will rollback
                // delete the items we created here

                fsba.DeleteAndFlush();

                appRole.DeleteAndFlush();
                appDebitOrder.DeleteAndFlush();
            }

        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.BankAccount.LegalEntityBankAccount
{

    #region ApplicationConfirmIncome

    [RuleDBTag("BankAccountDebitOrderDoNotDelete",
    "Prevents a bank account from being set to inactive if its being used as a Debit Order Account",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.BankAccount.LegalEntityBankAccount.BankAccountDebitOrderDoNotDelete")]
    [RuleInfo]
    public class BankAccountDebitOrderDoNotDelete : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The BankAccountDebitOrderDoNotDelete rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ILegalEntityBankAccount))
                throw new ArgumentException("The BankAccountDebitOrderDoNotDelete rule expects the following objects to be passed: ILegalEntityBankAccount.");

            #endregion

            ILegalEntityBankAccount leba = Parameters[0] as ILegalEntityBankAccount;

            #region Application Debit Orders
            //foreach (LegalEntityBankAccount_DAO leba in leRes)
            //{

                if (leba.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                {
                   return 1;
                }
                
            string HQL = @"select leba 
                from LegalEntityBankAccount_DAO leba, ApplicationRole_DAO ar, Application_DAO app
                inner join app.ApplicationDebitOrders ado 
                inner join ado.BankAccount ba 
                where leba.LegalEntity.Key = ar.LegalEntityKey
                and leba.BankAccount.Key = ba.Key
                and app.Key = ar.ApplicationKey
                and leba.LegalEntity.Key = ?
                and leba.BankAccount.Key = ?";

                SimpleQuery<LegalEntityBankAccount_DAO> query = new SimpleQuery<LegalEntityBankAccount_DAO>(HQL, leba.LegalEntity.Key,leba.BankAccount.Key);
                query.SetQueryRange(50);
                LegalEntityBankAccount_DAO[] res = query.Execute();

                if (res.Length > 0)
                {
                    string errorMessage = "Cannot deactivate a bank account that is being used as a direct debit.";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }
            #endregion

            #region Financial Services                  

                string HQL2 = @"select fs from LegalEntity_DAO le
                                join le.Roles r
                                join r.Account a
                                join a.FinancialServices fs   
                                join fs.FinancialServiceBankAccounts fsba                                
                                where le.Key = ?
                                and a.AccountStatus.Key = 1
                                and r.RoleType.Key in (2,3)
                                and a.Product.Key not in (3,4)
                                and fsba.BankAccount.Key = ?
                                and fs.FinancialServiceType.Key not in (4,5)
                                and fsba.GeneralStatus.Key = 1";

                SimpleQuery<FinancialService_DAO> query2 = new SimpleQuery<FinancialService_DAO>(HQL2, leba.LegalEntity.Key,leba.BankAccount.Key);
                query2.SetQueryRange(50);
                FinancialService_DAO[] res2 = query2.Execute();

                if (res2.Length > 0)
                {
                            string errorMessage = "Cannot deactivate a bank account that is being used as a direct debit.";
                            AddMessage(errorMessage, errorMessage, Messages);
                            return 0;
                }


            #endregion

            #region  FutureDatedChanges

            string HQL3 = @"select fdc from FutureDatedChange_DAO fdc, FutureDatedChangeDetail_DAO fdcd, LegalEntity_DAO le
                            join le.Roles r
                            join r.Account a
                            join a.FinancialServices fs   
                            join fs.FinancialServiceBankAccounts fsba                                
                            where fdc.IdentifierReferenceKey = fsba.FinancialService.Key
                            and fdcd.FutureDatedChange.Key = fdc.Key
                            and fdcd.TableName = 'FinancialServiceBankAccount'
                            and fsba.BankAccount.Key = ?
                            and fdcd.Action = 'I'
                            and fdcd.ReferenceKey = fsba.Key
                            and le.Key = ?                                    
                            and a.AccountStatus.Key = 1
                            and r.RoleType.Key in (2,3)
                            and a.Product.Key not in (3,4)
                            and fs.FinancialServiceType.Key not in (4,5)";

                SimpleQuery<FutureDatedChange_DAO> query3 = new SimpleQuery<FutureDatedChange_DAO>(HQL3, leba.BankAccount.Key, leba.LegalEntity.Key);
                query3.SetQueryRange(50);
                FutureDatedChange_DAO[] res3 = query3.Execute();

                if (res3.Length > 0)
                {
                            string errorMessage = "Cannot deactivate a bank account that is being used as in a future dated change.";
                            AddMessage(errorMessage, errorMessage, Messages);
                            return 0;
                }

            #endregion

            #region AccountDebtSettlement

                string sql = string.Format(@"select distinct ads.*
                from [2am].[dbo].LegalEntity le (nolock)
                inner join [2am].[dbo].LegalEntityBankAccount leba
                    on leba.LegalEntityKey = le.LegalEntityKey
                inner join [2am].[dbo].role r (nolock)
                    on le.LegalEntityKey = r.LegalEntityKey
                inner join [2am].[dbo].Account acc (nolock)
                    on r.accountKey = acc.AccountKey
                inner join [2am].[rcs].AccountExpense ae  (nolock)
                    on acc.AccountKey = ae.AccountKey
                inner join[2am].[dbo].BankAccount ba (nolock)
                    on ae.ExpenseAccountNumber = ba.AccountNumber
                inner join [2am].[rcs].AccountDebtSettlement ads (nolock)
                    on ae.ExpenseKey = ads.ExpenseKey
                where leba.BankAccountKey = ads.BankAccountKey and ads.DisbursementKey is null and leba.LegalEntityKey = {0} ", leba.LegalEntity.Key);

                SimpleQuery<AccountDebtSettlement_DAO> adsQ = new SimpleQuery<AccountDebtSettlement_DAO>(QueryLanguage.Sql, sql);
                adsQ.AddSqlReturnDefinition(typeof(AccountDebtSettlement_DAO), "ads");
                AccountDebtSettlement_DAO[] ADSres = adsQ.Execute();

                if (ADSres != null && ADSres.Length > 0)
                {
                    string errorMessage = "Cannot deactivate a bank account that is being used as in AccountDebtSettlement.";
                    AddMessage(errorMessage, errorMessage, Messages);
                    return 0;
                }

            #endregion

            #region FinancialServiceRecurringTransactions

            string sqlMDO = @"select mdo.* from deb.ManualDebitOrder mdo (nolock) where GeneralStatusKey = 1 and BankAccountKey = ?";
            SimpleQuery<ManualDebitOrder_DAO> qMDO = new SimpleQuery<ManualDebitOrder_DAO>(QueryLanguage.Sql, sqlMDO, leba.BankAccount.Key);
            qMDO.AddSqlReturnDefinition(typeof(ManualDebitOrder_DAO), "mdo");
            ManualDebitOrder_DAO[] resMDO = qMDO.Execute();

            if (resMDO != null && resMDO.Length > 0)
            {
                string errorMessage = "Cannot deactivate a bank account that is being used in a ManualDebitOrder.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            #endregion

            return 1;
        }
    }

    #endregion


}


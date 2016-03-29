using Automation.DataAccess;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface ILoanTransactionService
    {
        void pProcessTran(int financialServiceKey, TransactionTypeEnum transactionType, decimal amount, string reference, string userID);

        string GetLoanTransactionColumn(string columnToGet, string columnToRetrieveBy, string columnValue);

        Dictionary<int, double> GetLatestArrearTransaction(int accountKey);

        int GetLatestArrearTransactionKey(int accountKey);

        int GetRemainingInstalmentsOnLoan(int financialServiceKey);

        void UpdateRemainingInstalmentsOnLoan(int financialServiceKey, int numberOfInstalments);

        void BackDateArrearTransaction(int arrearTransactionKey, int days);

        decimal GetLatestArrearBalanceAmount(int accountKey);

        bool IsTransactionAnArrearTransaction(TransactionTypeEnum transactionType);

        QueryResults pProcessAccountPaymentTran(int accountKey, TransactionTypeEnum transactionType, double amount, string reference, string userID, DateTime effectiveDate);

        IEnumerable<Automation.DataModels.TransactionType> GetTransactionDataAccess(string adcredentials, params int[] transactionTypes);

        QueryResults GetArrearTransactionByTypeDateAndAccountKey(int accountKey, string insertDate, int transactionTypeNumber);

        QueryResults GetLoanTransactionByTypeDateAndAccountKey(int accountKey, DateTime insertDate, int transactionTypeNumber);

        QueryResults GetArrearTransactionsByParentFinancialServiceKey(int financialServiceKey, DateTime insertDate, TransactionTypeEnum transactionType);

        QueryResults GetFinancialTransactions(int financialServiceKey, DateTime insertDate, TransactionTypeEnum transactionType);

        QueryResults GetFinancialTransactionForRollback(ProductEnum product);

        Automation.DataModels.TransactionType GetTransactionTypeDetails(int transactionTypeKey);

        QueryResults GetArrearTransactionForRollback(ProductEnum product);

        QueryResults GetAccountWithLoanTransactionAgainstParentFinancialService(TransactionTypeEnum transactionType);

        /// <summary>
        ///  Returns all the financialtransactions for a specific account.
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="financialServiceType"></param>
        ///  <returns>all the columns of dbo.financialtransaction</returns>
        QueryResults GetFinancialTransactions(int accountkey, FinancialServiceTypeEnum financialServiceType);

        /// <summary>
        ///  Returns all the arreartransactions for a specific account.
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="financialServiceType"></param>
        ///  <returns>all the columns of dbo.arreartransaction</returns>
        QueryResults GetArrearTransactions(int accountkey, FinancialServiceTypeEnum financialServiceType);

        bool UpdateArrearTransactionBalance(int arrearTransactionKey, double amount);
    }
}
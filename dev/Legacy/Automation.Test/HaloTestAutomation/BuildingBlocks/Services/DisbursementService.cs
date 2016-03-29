using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class DisbursementService : _2AMDataHelper, IDisbursementService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="disbursementTransactionTypeKey"></param>
        /// <param name="disbursementStatusKey"></param>
        /// <returns></returns>
        public Automation.DataModels.Disbursement GetRandomOpenAccountWithDisbursements(DisbursementTransactionTypeEnum disbursementTransactionTypeKey, DisbursementStatusEnum disbursementStatusKey)
        {
            var accountDisbursements = (from d in base.GetDisbursementRecordsForOpenAccounts()
                                        where d.DisbursementTransactionTypeKey == disbursementTransactionTypeKey &&
                                        d.DisbursementStatusKey == disbursementStatusKey
                                        select d).DefaultIfEmpty();

            Random r = new Random();
            int i = r.Next(0, accountDisbursements.Count());

            return accountDisbursements.ElementAt(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disbursementKey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.Disbursement> GetDisbursementByDisbursementKey(List<int> disbursementKey)
        {
            var disbursements = base.GetDisbursementRecordsForOpenAccounts();

            var disbursement = (from d in disbursements
                                join k in disbursementKey on d.DisbursementKey equals k
                                select d).DefaultIfEmpty();

            return disbursement;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disbursementStatus"></param>
        /// <returns></returns>
        public Automation.DataModels.Disbursement GetRandomOpenAccountWithNoDisbursementsInStatus(DisbursementStatusEnum disbursementStatus)
        {
            var accountsAll = base.GetDisbursementRecordsForOpenAccounts();

            var accounts = (from a in accountsAll
                            join aa in
                                (from b in accountsAll
                                 where b.DisbursementStatusKey == disbursementStatus
                                 select b)
                            on a.AccountKey equals aa.AccountKey into joined
                            from j in joined.DefaultIfEmpty()
                            where j == null
                            select a).DefaultIfEmpty();
            return accounts.SelectRandom();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disbursementTransactionType"></param>
        /// <param name="disbursementStatus"></param>
        /// <returns></returns>
        public Automation.DataModels.Disbursement GetRandomOpenAccountWithDisbursementLoanTransactions(DisbursementTransactionTypeEnum disbursementTransactionType, DisbursementStatusEnum disbursementStatus)
        {
            var accountDisbursements = (from d in base.GetDisbursementRecordsWithLoanTransactionsForOpenAccounts()
                                        where d.DisbursementTransactionTypeKey == disbursementTransactionType &&
                                        d.DisbursementStatusKey == disbursementStatus
                                        select d).DefaultIfEmpty();
            return accountDisbursements.SelectRandom();
        }

        /// <summary>
        /// Fetches the accounts disbursement records for a specified disbursement status and type.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="disbursementStatus">Disbursement Status</param>
        /// <param name="disbursementType">Disbursement Type</param>
        /// <param name="value">Total Value</param>
        public double GetDisbursementRecords(int accountKey, DisbursementStatusEnum disbursementStatus, DisbursementTransactionTypeEnum disbursementType)
        {
            double value = 0.00;
            var r = base.GetDisbursementRecords(accountKey, (int)disbursementStatus, (int)disbursementType);
            foreach (var row in r.RowList)
            {
                value += row.Column("Amount").GetValueAs<double>();
            }
            r.Dispose();
            return value;
        }

        public void UpdateReadyForDisbursementToDisbursed(int accountKey)
        {
            base.UpdateDisbursementStatusForAccount(accountKey, DisbursementStatusEnum.ReadyForDisbursement, DisbursementStatusEnum.Disbursed);
        }
    }
}
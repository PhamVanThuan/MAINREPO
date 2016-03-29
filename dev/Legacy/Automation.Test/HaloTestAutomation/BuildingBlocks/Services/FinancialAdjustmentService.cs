using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class FinancialAdjustmentService : _2AMDataHelper, IFinancialAdjustmentService
    {
        /// <summary>
        /// This will return a set of financial adjustments that exist on an account with the status provided.
        /// </summary>
        /// <param name="finAdjTypeSources">List of Financial Adjustment Type Sources</param>
        /// <param name="finAdjStatus">Financial Adjustment Status</param>
        /// <param name="accountKey">Account Number</param>
        /// <returns>QueryResults</returns>
        public List<FinancialAdjustmentTypeSourceEnum> GetAccountFinancialAdjustmentsByStatus(FinancialAdjustmentStatusEnum finAdjStatus, int accountKey, params FinancialAdjustmentTypeSourceEnum[] finAdjTypeSources)
        {
            var finAdjustments = new List<FinancialAdjustmentTypeSourceEnum>();
            foreach (FinancialAdjustmentTypeSourceEnum fin in finAdjTypeSources)
            {
                var r = base.GetFinAdjustmentByAccountFinAdjustmentTypeAndStatus(accountKey, fin, finAdjStatus);
                if (r.HasResults)
                    finAdjustments.Add(fin);
            }
            return finAdjustments;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public QueryResultsRow GetAccountWithFinancialAdjustmentNotUnderDebtCounselling(FinancialAdjustmentTypeSourceEnum fats,
            FinancialAdjustmentStatusEnum status)
        {
            // Get Test Data
            var results = base.GetFinancialAdjustmentsByTypeAndStatus(fats, status);
            var row = (from r in results
                       where fIsAccountUnderDebtCounselling(r.Column("accountKey").GetValueAs<int>()) == false
                       select r).FirstOrDefault();
            return row;
        }

        public QueryResultsRow GetAccountWithFutureDatedFinancialAdjustment(FinancialAdjustmentTypeSourceEnum fats, FinancialAdjustmentStatusEnum status)
        {
            // Get Test Data
            var results = base.GetFinancialAdjustmentsByTypeAndStatus(fats, status);
            var row = (from r in results
                       where fIsAccountUnderDebtCounselling(r.Column("accountKey").GetValueAs<int>()) == false
                       && r.Column("FromDate").GetValueAs<DateTime>() > DateTime.Now
                       select r).FirstOrDefault();
            return row;
        }
    }
}
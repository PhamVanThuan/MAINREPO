using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MonthEndStatsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public MonthEndStatsDataModel(DateTime? entryDate, int? totalOpenAccount, decimal? totalOpenAccountCurrentBalance, int? totalClosedAccounts, int? totalClosedthismonth, int? totalOpenedthisMonth, int? openSuperLowAccounts, int? openVariableAccounts, int? openVariFixAccounts, int? monthEnd910Count, decimal? totalOf910TranAmount, int? monthEnd210Count, decimal? totalOf210TranAmount, int? monthEnd211Count, int? monthEnd310Count, int? monthEnd465Count, int? monthEnd466Count, int? monthEnd470Count, int? monthEnd921Count, int? monthEnd922Count, int? monthEnd960Count, int? monthEnd965Count, int? monthEnd485Count, int? monthEnd265Count)
        {
            this.EntryDate = entryDate;
            this.TotalOpenAccount = totalOpenAccount;
            this.TotalOpenAccountCurrentBalance = totalOpenAccountCurrentBalance;
            this.TotalClosedAccounts = totalClosedAccounts;
            this.TotalClosedthismonth = totalClosedthismonth;
            this.TotalOpenedthisMonth = totalOpenedthisMonth;
            this.OpenSuperLowAccounts = openSuperLowAccounts;
            this.OpenVariableAccounts = openVariableAccounts;
            this.OpenVariFixAccounts = openVariFixAccounts;
            this.MonthEnd910Count = monthEnd910Count;
            this.TotalOf910TranAmount = totalOf910TranAmount;
            this.MonthEnd210Count = monthEnd210Count;
            this.TotalOf210TranAmount = totalOf210TranAmount;
            this.MonthEnd211Count = monthEnd211Count;
            this.MonthEnd310Count = monthEnd310Count;
            this.MonthEnd465Count = monthEnd465Count;
            this.MonthEnd466Count = monthEnd466Count;
            this.MonthEnd470Count = monthEnd470Count;
            this.MonthEnd921Count = monthEnd921Count;
            this.MonthEnd922Count = monthEnd922Count;
            this.MonthEnd960Count = monthEnd960Count;
            this.MonthEnd965Count = monthEnd965Count;
            this.MonthEnd485Count = monthEnd485Count;
            this.MonthEnd265Count = monthEnd265Count;
		
        }
		[JsonConstructor]
        public MonthEndStatsDataModel(int monthEndStatsKey, DateTime? entryDate, int? totalOpenAccount, decimal? totalOpenAccountCurrentBalance, int? totalClosedAccounts, int? totalClosedthismonth, int? totalOpenedthisMonth, int? openSuperLowAccounts, int? openVariableAccounts, int? openVariFixAccounts, int? monthEnd910Count, decimal? totalOf910TranAmount, int? monthEnd210Count, decimal? totalOf210TranAmount, int? monthEnd211Count, int? monthEnd310Count, int? monthEnd465Count, int? monthEnd466Count, int? monthEnd470Count, int? monthEnd921Count, int? monthEnd922Count, int? monthEnd960Count, int? monthEnd965Count, int? monthEnd485Count, int? monthEnd265Count)
        {
            this.MonthEndStatsKey = monthEndStatsKey;
            this.EntryDate = entryDate;
            this.TotalOpenAccount = totalOpenAccount;
            this.TotalOpenAccountCurrentBalance = totalOpenAccountCurrentBalance;
            this.TotalClosedAccounts = totalClosedAccounts;
            this.TotalClosedthismonth = totalClosedthismonth;
            this.TotalOpenedthisMonth = totalOpenedthisMonth;
            this.OpenSuperLowAccounts = openSuperLowAccounts;
            this.OpenVariableAccounts = openVariableAccounts;
            this.OpenVariFixAccounts = openVariFixAccounts;
            this.MonthEnd910Count = monthEnd910Count;
            this.TotalOf910TranAmount = totalOf910TranAmount;
            this.MonthEnd210Count = monthEnd210Count;
            this.TotalOf210TranAmount = totalOf210TranAmount;
            this.MonthEnd211Count = monthEnd211Count;
            this.MonthEnd310Count = monthEnd310Count;
            this.MonthEnd465Count = monthEnd465Count;
            this.MonthEnd466Count = monthEnd466Count;
            this.MonthEnd470Count = monthEnd470Count;
            this.MonthEnd921Count = monthEnd921Count;
            this.MonthEnd922Count = monthEnd922Count;
            this.MonthEnd960Count = monthEnd960Count;
            this.MonthEnd965Count = monthEnd965Count;
            this.MonthEnd485Count = monthEnd485Count;
            this.MonthEnd265Count = monthEnd265Count;
		
        }		

        public int MonthEndStatsKey { get; set; }

        public DateTime? EntryDate { get; set; }

        public int? TotalOpenAccount { get; set; }

        public decimal? TotalOpenAccountCurrentBalance { get; set; }

        public int? TotalClosedAccounts { get; set; }

        public int? TotalClosedthismonth { get; set; }

        public int? TotalOpenedthisMonth { get; set; }

        public int? OpenSuperLowAccounts { get; set; }

        public int? OpenVariableAccounts { get; set; }

        public int? OpenVariFixAccounts { get; set; }

        public int? MonthEnd910Count { get; set; }

        public decimal? TotalOf910TranAmount { get; set; }

        public int? MonthEnd210Count { get; set; }

        public decimal? TotalOf210TranAmount { get; set; }

        public int? MonthEnd211Count { get; set; }

        public int? MonthEnd310Count { get; set; }

        public int? MonthEnd465Count { get; set; }

        public int? MonthEnd466Count { get; set; }

        public int? MonthEnd470Count { get; set; }

        public int? MonthEnd921Count { get; set; }

        public int? MonthEnd922Count { get; set; }

        public int? MonthEnd960Count { get; set; }

        public int? MonthEnd965Count { get; set; }

        public int? MonthEnd485Count { get; set; }

        public int? MonthEnd265Count { get; set; }

        public void SetKey(int key)
        {
            this.MonthEndStatsKey =  key;
        }
    }
}
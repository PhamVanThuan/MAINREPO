using Automation.DataAccess.DataHelper;
using Automation.Framework.DataAccess;
using Common.Constants;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Automation.Framework
{
    public class WorkflowBase : _2AMDataHelper
    {
        /// <summary>
        /// Inserts a correspondence record. Use this when an activity uses the correspondence screen prior to submit
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="reportStatementKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        internal void InsertCorrespondence(int genericKey, int reportStatementKey, int genericKeyTypeKey)
        {
            var parameters = new Dictionary<string, string>
                {
                    {"@genericKey", genericKey.ToString()},
                    {"@reportStatementKey", reportStatementKey.ToString()},
                    {"@genericKeyTypeKey", genericKeyTypeKey.ToString()}
                };
            DataHelper.ExecuteProcedure("test.InsertCorrespondence", parameters);
        }

        /// <summary>
        /// Inserts a reason for a case. Use this when the activity uses a reason screen prior to submit.
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="reasonDefinitionKey"></param>
        internal void InsertReason(int genericKey, int reasonDefinitionKey)
        {
            string sql = string.Format(@"INSERT INTO [2am].dbo.Reason (ReasonDefinitionKey, GenericKey, Comment, StageTransitionKey)
                                        VALUES ({0}, {1}, 'TEST AUTOMATION', NULL)", reasonDefinitionKey, genericKey);
            DataHelper.ExecuteAdHocSQL(sql);
        }

        /// <summary>
        /// Inserts ITC's for the applicants on an application
        /// </summary>
        /// <param name="applicationKey"></param>
        internal void InsertITC(int applicationKey)
        {
            var parameters = new Dictionary<string, string> { { "@offerKey", applicationKey.ToString() } };
            DataHelper.ExecuteProcedure("test.insertITCv4", parameters);
        }

        /// <summary>
        /// Inserts an application debit order
        /// </summary>
        /// <param name="applicationKey"></param>
        internal void InsertOfferDebitOrder(int applicationKey)
        {
            var parameters = new Dictionary<string, string> { { "@offerKey", applicationKey.ToString() } };
            DataHelper.ExecuteProcedure("test.CleanUpOfferDebitOrder", parameters);
        }

        /// <summary>
        /// Inserts an application mailing address
        /// </summary>
        /// <param name="applicationKey"></param>
        internal void InsertOfferMailingAddress(int applicationKey)
        {
            var parameters = new Dictionary<string, string> { { "@offerKey", applicationKey.ToString() } };
            DataHelper.ExecuteProcedure("test.insertofferMailingAddress", parameters);
        }

        /// <summary>
        /// Inserts a single income and single expense record
        /// </summary>
        /// <param name="applicationKey"></param>
        internal void InsertLegalEntityAffordability(int applicationKey)
        {
            var parameters = new Dictionary<string, string> { { "@offerKey", applicationKey.ToString() } };
            DataHelper.ExecuteProcedure("test.InsertAffordabilityAssessment", parameters);
        }

        /// <summary>
        /// Gets the max offerInformationKey for an offer
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        internal int GetLatestOfferInformationKey(int keyValue)
        {
            return DataHelper.GetMaxOfferInformationKey(keyValue);
        }

        internal void InsertDetailTypeForAccount(int accountKey, DetailTypeEnum detailType)
        {
            string sql = string.Format(@"INSERT INTO [2am].dbo.Detail (DetailTypeKey, AccountKey, DetailDate, UserID, ChangeDate)
                                        VALUES ({0}, {1}, getdate(), 'SAHL\ClintonS', getdate())", (int)detailType, accountKey);
            DataHelper.ExecuteAdHocSQL(sql);
        }

        internal void UpdateValuationDateToDateLessThan12MonthsAgo(int offerKey)
        {
            var query = string.Format(@"update v
                                set valuationdate = dateadd(mm, -6, getdate())
                                from [2am].dbo.offermortgageloan oml
	                                join [2am].dbo.property p on oml.propertyKey = p.propertyKey
	                                join [2am].dbo.valuation v on p.propertyKey = v.propertyKey and isActive = 1
                                where oml.offerkey = {0}
	                                and v.valuationdate < dateadd(mm, -12, getdate())", offerKey);
            DataHelper.ExecuteAdHocSQL(query);
        }

        internal void InsertDomiciliumAddressForLegalEntity(int legalEntityKey)
        {
            base.CreateLegalEntityDomicilium(legalEntityKey, GeneralStatusEnum.Active);
        }

        internal void PutAccountIntoArrears(int accountKey)
        {
            var account = base.GetAccountByKeySQL(accountKey);
            account.FinancialServices = base.GetLoanFinancialServices(account.AccountKey);
            var result = base.GetLatestArrearTransaction(accountKey);
            decimal balance = (from r in result select r.Column("Balance").GetValueAs<decimal>()).FirstOrDefault();
            //post tran to go into arrears
            var financialService = (from fs in account.FinancialServices where fs.FinancialServiceTypeKey == 1 select fs).FirstOrDefault();
            base.pProcessTran(financialService.FinancialServiceKey, TransactionTypeEnum.RaiseInstalment, balance + Convert.ToDecimal(financialService.Payment),
                "Raise Instalment", @"SAHL\HaloUser");
            result = base.GetLatestArrearTransaction(accountKey);
            int arrearTransactionKey = (from r in result select r.Column("ArrearTransactionKey").GetValueAs<int>()).FirstOrDefault();
            base.BackDateArrearTransaction(arrearTransactionKey, -8);
        }

        internal void WaitForX2WorkflowHistoryActivity(string activityName, Int64 instanceID, int count, string dateFilter = "")
        {
            dateFilter = string.IsNullOrEmpty(dateFilter) ? dateFilter = new DateTime(1900, 1, 1).ToString(Formats.DateTimeFormatSQL) : dateFilter;
            var timer = new SimpleTimer(90);
            timer.StartTimer();
            bool b = false;
            while (!timer.timeElapsed)
            {
                //get the workflow history
                var r = GetWorkflowHistoryActivityCount(instanceID, activityName, dateFilter);
                //does the count equal to expected count
                if (r.Rows(0).Column(0).GetValueAs<int>() == count)
                {
                    b = true;
                    //we have found it
                    break;
                }
            }
            if (!b)
            {
                //throw exception if it isnt found.
                throw new Exception(string.Format(@"Activity {0} did not get run for instance {1} after waiting 90 seconds. Expected to
                    occur after date: {2}", activityName, instanceID, dateFilter));
            }
        }

        internal void UpdateX2Valuation(int applicationKey, int propertyKey, string requestingAdUser, string lightstonePropertyID, int valuationKey, int valuationDataProviderDataServiceKey)
        {
            DataHelper.UpdateX2Valuation(applicationKey, propertyKey, requestingAdUser, lightstonePropertyID, valuationKey, valuationDataProviderDataServiceKey);
        }

        internal void InsertXmlHistoryForOffer(int keyValue)
        {
            base.CreateXmlHistoryForOffer(keyValue);
        }

        internal void SetStageTransitionOnLatestReason(int keyValue, int genericKey, StageDefinitionStageDefinitionGroupEnum sdsdgkey)
        {
            string sql = string.Format(@"declare @stageTransitionKey int

                                        select top 1 @stageTransitionKey=st.StageTransitionKey from [2AM].dbo.StageTransition st
                                        where st.GenericKey={0} and st.StageDefinitionStageDefinitionGroupKey={2} order by st.StageTransitionKey desc

                                        declare @reasonKey int

                                        select top 1 @reasonKey=r.ReasonKey from [2AM].dbo.Reason r
                                        where r.GenericKey={1} order by r.ReasonKey desc

                                        update r
                                        set r.StageTransitionKey = @stageTransitionKey
                                        from [2AM].dbo.Reason r
                                        where r.ReasonKey=@reasonKey", keyValue, genericKey, (int)sdsdgkey);
            DataHelper.ExecuteAdHocSQL(sql);
        }
    }

    public class SimpleTimer
    {
        internal Timer timer;

        internal double interval { get; set; }

        public double hours { get; set; }

        public double minutes { get; set; }

        public double seconds { get; set; }

        public bool timeElapsed { get; protected set; }

        public SimpleTimer(double hours, double minutes, double seconds)
        {
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
        }

        public SimpleTimer(double seconds)
            : this(0, 0, seconds)
        {
        }

        internal void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            TimeElapsed();
        }

        public void StartTimer()
        {
            timeElapsed = false;
            interval = 0;
            interval += seconds * 1000;
            interval += minutes * (60 * 1000);
            interval += hours * (60 * 60 * 1000);
            timer = new Timer(interval) { Enabled = false, AutoReset = false };
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Start();
        }

        internal void TimeElapsed()
        {
            timeElapsed = true;
            timer.Stop();
            timer.Dispose();
        }
    }
}
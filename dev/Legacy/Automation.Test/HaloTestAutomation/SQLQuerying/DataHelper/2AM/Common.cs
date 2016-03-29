using Common.Constants;
using Common.Enums;
using Common.Extensions;
using System;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        ///   Calculate the future date n business days from todays date
        /// </summary>
        /// <param name = "Days">N business days in the the future</param>
        /// <returns>Date n business days form todays date</returns>
        public DateTime CalculateFutureDateInBusinessDays(int Days, DateTime? timerBaseValue = null)
        {
            DateTime timerDate = timerBaseValue == null ? DateTime.Now : timerBaseValue.Value;

            string query =
                    string.Format(@"Select c.CalendarDate
								    From (Select CalendarDate, Row_Number() Over(Order By CalendarDate) as 'RowNum'
										    from Calendar
										    where CalendarDate > '{0}' and IsSaturday <> 1 and IsSunday <> 1 and IsHoliday <> 1) as c
								    Where RowNum = {1}", timerDate.ToString(Formats.DateTimeFormatSQL), Days);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement).Rows(0).Column(0).GetValueAs<DateTime>();
        }

        /// <summary>
        ///   Will update every offerkey in the test.lifeleads
        /// </summary>
        public QueryResults UpdateLifeLeadsWithOffers()
        {
            SQLStoredProcedure Procedure = new SQLStoredProcedure { Name = "test.UpdatelifeleadWithOffers" };
            return dataContext.ExecuteStoredProcedureWithResults(Procedure);
        }

        /// <summary>
        /// Get Market Rate
        /// </summary>
        /// <param name="marketRateValue"></param>
        /// <param name="mRate"></param>
        /// <returns></returns>
        public double GetMarketRate(MarketRateEnum mRate)
        {
            string query =
                   string.Format(@"select value from [2am].dbo.MarketRate where MarketRateKey = {0}", (int)mRate);
            var statement = new SQLStatement { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r.Rows(0).Column(0).GetValueAs<double>();
        }

        /// <summary>
        /// Gets the next non business day after today.
        /// </summary>
        /// <returns></returns>
        public DateTime GetNextNonBusinessDay(DateTime? date = null)
        {
            string dateTime = date == null ? DateTime.Now.ToString(Formats.DateTimeFormatSQL) : date.Value.ToString(Formats.DateTimeFormatSQL);
            string query =
                string.Format(@"Select CalendarDate
                from [2am].dbo.Calendar
                where CalendarDate > '{0}' and
                (IsSaturday <> 0 or IsSunday <> 0 or IsHoliday <> 0)", dateTime);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).GetValueAs<DateTime>();
        }

        /// <summary>
        /// Gets the next business day after today.
        /// </summary>
        /// <returns></returns>
        public DateTime GetNextBusinessDay(DateTime? date = null, bool includeDate = false)
        {
            string dateTime = date == null ? DateTime.Now.ToString(Formats.DateTimeFormatSQL) : date.Value.ToString(Formats.DateTimeFormatSQL);
            string query = string.Empty;
            if (includeDate)
            {
                query =
                     string.Format(@"Select CalendarDate
                from [2am].dbo.Calendar
                where CalendarDate >= '{0}' and
                (IsSaturday <> 1 and IsSunday <> 1 and IsHoliday <> 1)", dateTime);
            }
            else
            {
                query =
                        string.Format(@"Select CalendarDate
                from [2am].dbo.Calendar
                where CalendarDate > '{0}' and
                (IsSaturday <> 1 and IsSunday <> 1 and IsHoliday <> 1)", dateTime);
            }
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).GetValueAs<DateTime>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DateTime GetNextBusinessDay(DateTime startDate, DateTime endDate)
        {
            string query = string.Format(@"Select CalendarDate
                from [2am].dbo.Calendar
                where(CalendarDate > '{0}' and CalendarDate < '{1}')
                and (IsSaturday <> 1 and IsSunday <> 1 and IsHoliday <> 1",
                    startDate.ToString(Formats.DateTimeFormatSQL), endDate.ToString(Formats.DateTimeFormatSQL));
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).GetValueAs<DateTime>();
        }

        public void SyncMarketRates()
        {
            string query = @"update mr
                            set value = mr_prod.Value
                            from [2am].dbo.MarketRate mr
                            join [sahls15].[2am].dbo.MarketRate mr_prod on mr.marketRateKey=mr_prod.marketRateKey";
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public Automation.DataModels.MarketingSource GetMarketingSourceByDescriptionAndStatus(string newMarketingSource, GeneralStatusEnum generalStatus)
        {
            string sql = string.Format(@"select * from [2am].dbo.OfferSource where description = '{0}' and generalStatusKey = {1}", newMarketingSource, (int)generalStatus);
            return dataContext.Query<Automation.DataModels.MarketingSource>(sql).First();
        }

        public Automation.DataModels.MarketingSource GetMarketingSourceByStatus(GeneralStatusEnum generalStatus)
        {
            string sql = string.Format(@"select * from [2am].dbo.OfferSource where generalStatusKey = {0}", (int)generalStatus);
            return dataContext.Query<Automation.DataModels.MarketingSource>(sql).SelectRandom();
        }
    }
}
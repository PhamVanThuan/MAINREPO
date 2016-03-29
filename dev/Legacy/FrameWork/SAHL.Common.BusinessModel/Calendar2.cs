using System;
using Castle.ActiveRecord.Queries;
using NHibernate.Transform;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class Calendar : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Calendar_DAO>, ICalendar
    {
        public static ICalendar GetCalendarItemsByDate(DateTime calendarDate)
        {
            string HQL = "from Calendar_DAO c where "
             + "c.CalendarDate = ?";

            SimpleQuery<Calendar_DAO> q = new SimpleQuery<Calendar_DAO>(HQL, calendarDate.Date);
            q.SetResultTransformer(new DistinctRootEntityResultTransformer());

            Calendar_DAO[] res = q.Execute();

            if (res == null || res.Length == 0)
                return null;
            return new Calendar(res[0]);
        }

        /// <summary>
        /// Emulates the function [2AM]..[fBusinessDayDiff]
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public static int BusinessDayDiff(DateTime StartDate, DateTime EndDate)
        {
            string HQL = "from Calendar_DAO c where c.CalendarDate between ? and ? and c.IsSaturday = 0 and c.IsSunday = 0 and c.IsHoliday = 0";
            SimpleQuery<Calendar_DAO> q = new SimpleQuery<Calendar_DAO>(HQL, StartDate.Date, EndDate.Date);
            q.SetResultTransformer(new DistinctRootEntityResultTransformer());
            Calendar_DAO[] res = q.Execute();

            return res.Length;
        }
    }
}
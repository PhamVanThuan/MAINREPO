using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Calendar_DAO
    /// </summary>
    public partial class Calendar : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Calendar_DAO>, ICalendar
    {
        public Calendar(SAHL.Common.BusinessModel.DAO.Calendar_DAO Calendar)
            : base(Calendar)
        {
            this._DAO = Calendar;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Calendar_DAO.CalendarDate
        /// </summary>
        public DateTime CalendarDate
        {
            get { return _DAO.CalendarDate; }
            set { _DAO.CalendarDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Calendar_DAO.IsSaturday
        /// </summary>
        public Boolean IsSaturday
        {
            get { return _DAO.IsSaturday; }
            set { _DAO.IsSaturday = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Calendar_DAO.IsSunday
        /// </summary>
        public Boolean IsSunday
        {
            get { return _DAO.IsSunday; }
            set { _DAO.IsSunday = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Calendar_DAO.IsHoliday
        /// </summary>
        public Boolean IsHoliday
        {
            get { return _DAO.IsHoliday; }
            set { _DAO.IsHoliday = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Calendar_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}
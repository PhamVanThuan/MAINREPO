using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Calendar_DAO
    /// </summary>
    public partial interface ICalendar : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Calendar_DAO.CalendarDate
        /// </summary>
        System.DateTime CalendarDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Calendar_DAO.IsSaturday
        /// </summary>
        System.Boolean IsSaturday
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Calendar_DAO.IsSunday
        /// </summary>
        System.Boolean IsSunday
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Calendar_DAO.IsHoliday
        /// </summary>
        System.Boolean IsHoliday
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Calendar_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}
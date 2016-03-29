using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO
    /// </summary>
    public partial interface ICourierAppointment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO.AppointmentDate
        /// </summary>
        DateTime? AppointmentDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO.Notes
        /// </summary>
        System.String Notes
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO.Courier
        /// </summary>
        ICourier Courier
        {
            get;
            set;
        }
    }
}
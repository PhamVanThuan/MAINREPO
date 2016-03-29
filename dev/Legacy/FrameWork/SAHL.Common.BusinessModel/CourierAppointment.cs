using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO
    /// </summary>
    public partial class CourierAppointment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO>, ICourierAppointment
    {
        public CourierAppointment(SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO CourierAppointment)
            : base(CourierAppointment)
        {
            this._DAO = CourierAppointment;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO.AppointmentDate
        /// </summary>
        public DateTime? AppointmentDate
        {
            get { return _DAO.AppointmentDate; }
            set { _DAO.AppointmentDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO.Notes
        /// </summary>
        public String Notes
        {
            get { return _DAO.Notes; }
            set { _DAO.Notes = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO.Account
        /// </summary>
        public IAccount Account
        {
            get
            {
                if (null == _DAO.Account) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Account = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Account = (Account_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CourierAppointment_DAO.Courier
        /// </summary>
        public ICourier Courier
        {
            get
            {
                if (null == _DAO.Courier) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICourier, Courier_DAO>(_DAO.Courier);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Courier = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Courier = (Courier_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}
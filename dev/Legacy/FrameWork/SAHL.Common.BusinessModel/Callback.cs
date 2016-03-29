using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Callback_DAO
    /// </summary>
    public partial class Callback : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Callback_DAO>, ICallback
    {
        public Callback(SAHL.Common.BusinessModel.DAO.Callback_DAO Callback)
            : base(Callback)
        {
            this._DAO = Callback;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Callback_DAO.GenericKeyType
        /// </summary>
        public IGenericKeyType GenericKeyType
        {
            get
            {
                if (null == _DAO.GenericKeyType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GenericKeyType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Callback_DAO.GenericKey
        /// </summary>
        public Int32 GenericKey
        {
            get { return _DAO.GenericKey; }
            set { _DAO.GenericKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Callback_DAO.EntryDate
        /// </summary>
        public DateTime EntryDate
        {
            get { return _DAO.EntryDate; }
            set { _DAO.EntryDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Callback_DAO.EntryUser
        /// </summary>
        public String EntryUser
        {
            get { return _DAO.EntryUser; }
            set { _DAO.EntryUser = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Callback_DAO.CallbackDate
        /// </summary>
        public DateTime CallbackDate
        {
            get { return _DAO.CallbackDate; }
            set { _DAO.CallbackDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Callback_DAO.CallbackUser
        /// </summary>
        public String CallbackUser
        {
            get { return _DAO.CallbackUser; }
            set { _DAO.CallbackUser = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Callback_DAO.CompletedDate
        /// </summary>
        public DateTime? CompletedDate
        {
            get { return _DAO.CompletedDate; }
            set { _DAO.CompletedDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Callback_DAO.CompletedUser
        /// </summary>
        public String CompletedUser
        {
            get { return _DAO.CompletedUser; }
            set { _DAO.CompletedUser = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Callback_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// The Reason for a callback, which will also provide a link to the object in question.
        /// </summary>
        public IReason Reason
        {
            get
            {
                if (null == _DAO.Reason) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IReason, Reason_DAO>(_DAO.Reason);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Reason = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Reason = (Reason_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}
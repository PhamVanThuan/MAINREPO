using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BankRange_DAO
    /// </summary>
    public partial class BankRange : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BankRange_DAO>, IBankRange
    {
        public BankRange(SAHL.Common.BusinessModel.DAO.BankRange_DAO BankRange)
            : base(BankRange)
        {
            this._DAO = BankRange;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.RangeStart
        /// </summary>
        public Int32 RangeStart
        {
            get { return _DAO.RangeStart; }
            set { _DAO.RangeStart = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.RangeEnd
        /// </summary>
        public Int32 RangeEnd
        {
            get { return _DAO.RangeEnd; }
            set { _DAO.RangeEnd = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.DateChange
        /// </summary>
        public DateTime DateChange
        {
            get { return _DAO.DateChange; }
            set { _DAO.DateChange = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankRange_DAO.ACBBank
        /// </summary>
        public IACBBank ACBBank
        {
            get
            {
                if (null == _DAO.ACBBank) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IACBBank, ACBBank_DAO>(_DAO.ACBBank);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ACBBank = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ACBBank = (ACBBank_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}
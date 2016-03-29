using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CallingContextType_DAO
    /// </summary>
    public partial class CallingContextType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CallingContextType_DAO>, ICallingContextType
    {
        public CallingContextType(SAHL.Common.BusinessModel.DAO.CallingContextType_DAO CallingContextType)
            : base(CallingContextType)
        {
            this._DAO = CallingContextType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContextType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CallingContextType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }
    }
}
using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CitizenType_DAO
    /// </summary>
    public partial class CitizenType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CitizenType_DAO>, ICitizenType
    {
        public CitizenType(SAHL.Common.BusinessModel.DAO.CitizenType_DAO CitizenType)
            : base(CitizenType)
        {
            this._DAO = CitizenType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CitizenType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CitizenType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}
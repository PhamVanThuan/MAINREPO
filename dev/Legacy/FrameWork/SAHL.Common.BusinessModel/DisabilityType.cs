using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using System;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DisabilityType_DAO
    /// </summary>
    public partial class DisabilityType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DisabilityType_DAO>, IDisabilityType
    {
        public DisabilityType(SAHL.Common.BusinessModel.DAO.DisabilityType_DAO DisabilityType)
            : base(DisabilityType)
        {
            this._DAO = DisabilityType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}
using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AreaClassification_DAO
    /// </summary>
    public partial class AreaClassification : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AreaClassification_DAO>, IAreaClassification
    {
        public AreaClassification(SAHL.Common.BusinessModel.DAO.AreaClassification_DAO AreaClassification)
            : base(AreaClassification)
        {
            this._DAO = AreaClassification;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AreaClassification_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AreaClassification_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}
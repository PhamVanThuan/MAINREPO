using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ConditionType_DAO
    /// </summary>
    public partial class ConditionType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ConditionType_DAO>, IConditionType
    {
        public ConditionType(SAHL.Common.BusinessModel.DAO.ConditionType_DAO ConditionType)
            : base(ConditionType)
        {
            this._DAO = ConditionType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}
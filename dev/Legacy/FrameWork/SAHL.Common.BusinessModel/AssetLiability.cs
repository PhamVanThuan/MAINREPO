using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AssetLiability_DAO
    /// </summary>
    public abstract partial class AssetLiability : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AssetLiability_DAO>, IAssetLiability
    {
        public AssetLiability(SAHL.Common.BusinessModel.DAO.AssetLiability_DAO AssetLiability)
            : base(AssetLiability)
        {
            this._DAO = AssetLiability;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiability_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}
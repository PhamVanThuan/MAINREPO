using System;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial class AssetLiabilityOther : AssetLiability, IAssetLiabilityOther
    {
        protected new SAHL.Common.BusinessModel.DAO.AssetLiabilityOther_DAO _DAO;

        public AssetLiabilityOther(SAHL.Common.BusinessModel.DAO.AssetLiabilityOther_DAO AssetLiabilityOther)
            : base(AssetLiabilityOther)
        {
            this._DAO = AssetLiabilityOther;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityOther_DAO.AssetValue
        /// </summary>
        public Double AssetValue
        {
            get { return _DAO.AssetValue; }
            set { _DAO.AssetValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityOther_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityOther_DAO.LiabilityValue
        /// </summary>
        public Double LiabilityValue
        {
            get { return _DAO.LiabilityValue; }
            set { _DAO.LiabilityValue = value; }
        }
    }
}
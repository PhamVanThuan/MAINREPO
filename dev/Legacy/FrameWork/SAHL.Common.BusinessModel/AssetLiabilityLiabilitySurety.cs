using System;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial class AssetLiabilityLiabilitySurety : AssetLiability, IAssetLiabilityLiabilitySurety
    {
        protected new SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilitySurety_DAO _DAO;

        public AssetLiabilityLiabilitySurety(SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilitySurety_DAO AssetLiabilityLiabilitySurety)
            : base(AssetLiabilityLiabilitySurety)
        {
            this._DAO = AssetLiabilityLiabilitySurety;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilitySurety_DAO.AssetValue
        /// </summary>
        public Double? AssetValue
        {
            get { return _DAO.AssetValue; }
            set { _DAO.AssetValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilitySurety_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilitySurety_DAO.LiabilityValue
        /// </summary>
        public Double? LiabilityValue
        {
            get { return _DAO.LiabilityValue; }
            set { _DAO.LiabilityValue = value; }
        }
    }
}
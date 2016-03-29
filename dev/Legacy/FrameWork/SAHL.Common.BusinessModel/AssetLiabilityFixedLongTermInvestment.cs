using System;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedLongTermInvestment_DAO
    /// </summary>
    public partial class AssetLiabilityFixedLongTermInvestment : AssetLiability, IAssetLiabilityFixedLongTermInvestment
    {
        protected new SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedLongTermInvestment_DAO _DAO;

        public AssetLiabilityFixedLongTermInvestment(SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedLongTermInvestment_DAO AssetLiabilityFixedLongTermInvestment)
            : base(AssetLiabilityFixedLongTermInvestment)
        {
            this._DAO = AssetLiabilityFixedLongTermInvestment;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedLongTermInvestment_DAO.LiabilityValue
        /// </summary>
        public Double LiabilityValue
        {
            get { return _DAO.LiabilityValue; }
            set { _DAO.LiabilityValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityFixedLongTermInvestment_DAO.CompanyName
        /// </summary>
        public String CompanyName
        {
            get { return _DAO.CompanyName; }
            set { _DAO.CompanyName = value; }
        }
    }
}
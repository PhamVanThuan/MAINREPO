using System;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial class AssetLiabilityInvestmentPublic : AssetLiability, IAssetLiabilityInvestmentPublic
    {
        protected new SAHL.Common.BusinessModel.DAO.AssetLiabilityInvestmentPublic_DAO _DAO;

        public AssetLiabilityInvestmentPublic(SAHL.Common.BusinessModel.DAO.AssetLiabilityInvestmentPublic_DAO AssetLiabilityInvestmentPublic)
            : base(AssetLiabilityInvestmentPublic)
        {
            this._DAO = AssetLiabilityInvestmentPublic;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityInvestmentPublic_DAO.AssetValue
        /// </summary>
        public Double AssetValue
        {
            get { return _DAO.AssetValue; }
            set { _DAO.AssetValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityInvestmentPublic_DAO.CompanyName
        /// </summary>
        public String CompanyName
        {
            get { return _DAO.CompanyName; }
            set { _DAO.CompanyName = value; }
        }
    }
}
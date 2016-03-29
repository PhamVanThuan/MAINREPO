using System;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial class AssetLiabilityInvestmentPrivate : AssetLiability, IAssetLiabilityInvestmentPrivate
    {
        protected new SAHL.Common.BusinessModel.DAO.AssetLiabilityInvestmentPrivate_DAO _DAO;

        public AssetLiabilityInvestmentPrivate(SAHL.Common.BusinessModel.DAO.AssetLiabilityInvestmentPrivate_DAO AssetLiabilityInvestmentPrivate)
            : base(AssetLiabilityInvestmentPrivate)
        {
            this._DAO = AssetLiabilityInvestmentPrivate;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityInvestmentPrivate_DAO.AssetValue
        /// </summary>
        public Double AssetValue
        {
            get { return _DAO.AssetValue; }
            set { _DAO.AssetValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityInvestmentPrivate_DAO.CompanyName
        /// </summary>
        public String CompanyName
        {
            get { return _DAO.CompanyName; }
            set { _DAO.CompanyName = value; }
        }
    }
}
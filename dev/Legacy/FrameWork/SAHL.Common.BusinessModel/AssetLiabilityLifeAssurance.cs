using System;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial class AssetLiabilityLifeAssurance : AssetLiability, IAssetLiabilityLifeAssurance
    {
        protected new SAHL.Common.BusinessModel.DAO.AssetLiabilityLifeAssurance_DAO _DAO;

        public AssetLiabilityLifeAssurance(SAHL.Common.BusinessModel.DAO.AssetLiabilityLifeAssurance_DAO AssetLiabilityLifeAssurance)
            : base(AssetLiabilityLifeAssurance)
        {
            this._DAO = AssetLiabilityLifeAssurance;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLifeAssurance_DAO.CompanyName
        /// </summary>
        public String CompanyName
        {
            get { return _DAO.CompanyName; }
            set { _DAO.CompanyName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLifeAssurance_DAO.SurrenderValue
        /// </summary>
        public Double SurrenderValue
        {
            get { return _DAO.SurrenderValue; }
            set { _DAO.SurrenderValue = value; }
        }
    }
}
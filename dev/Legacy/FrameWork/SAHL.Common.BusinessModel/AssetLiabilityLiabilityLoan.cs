using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    public partial class AssetLiabilityLiabilityLoan : AssetLiability, IAssetLiabilityLiabilityLoan
    {
        protected new SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO _DAO;

        public AssetLiabilityLiabilityLoan(SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO AssetLiabilityLiabilityLoan)
            : base(AssetLiabilityLiabilityLoan)
        {
            this._DAO = AssetLiabilityLiabilityLoan;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO.DateRepayable
        /// </summary>
        public DateTime? DateRepayable
        {
            get { return _DAO.DateRepayable; }
            set { _DAO.DateRepayable = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO.FinancialInstitution
        /// </summary>
        public String FinancialInstitution
        {
            get { return _DAO.FinancialInstitution; }
            set { _DAO.FinancialInstitution = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO.InstalmentValue
        /// </summary>
        public Double InstalmentValue
        {
            get { return _DAO.InstalmentValue; }
            set { _DAO.InstalmentValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO.LiabilityValue
        /// </summary>
        public Double LiabilityValue
        {
            get { return _DAO.LiabilityValue; }
            set { _DAO.LiabilityValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetLiabilityLiabilityLoan_DAO.LoanType
        /// </summary>
        public IAssetLiabilitySubType LoanType
        {
            get
            {
                if (null == _DAO.LoanType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAssetLiabilitySubType, AssetLiabilitySubType_DAO>(_DAO.LoanType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.LoanType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.LoanType = (AssetLiabilitySubType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}
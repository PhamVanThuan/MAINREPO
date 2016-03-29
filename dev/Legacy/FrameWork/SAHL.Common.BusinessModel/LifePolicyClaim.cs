using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO
	/// </summary>
    public partial class LifePolicyClaim : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO>, ILifePolicyClaim
	{
        public LifePolicyClaim(SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO LifePolicyClaim)
            : base(LifePolicyClaim)
		{
            this._DAO = LifePolicyClaim;
		}

		/// <summary>
		/// Used for Activerecord exclusively, please use Key.
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO.FinancialService
        /// </summary>
        public IFinancialService FinancialService
        {
            get
            {
                if (null == _DAO.FinancialService) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFinancialService, FinancialService_DAO>(_DAO.FinancialService);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.FinancialService = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.FinancialService = (FinancialService_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO.ClaimStatus
        /// </summary>
        public IClaimStatus ClaimStatus
        {
            get
            {
                if (null == _DAO.ClaimStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IClaimStatus, ClaimStatus_DAO>(_DAO.ClaimStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ClaimStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ClaimStatus = (ClaimStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO.ClaimType
        /// </summary>
        public IClaimType ClaimType
        {
            get
            {
                if (null == _DAO.ClaimType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IClaimType, ClaimType_DAO>(_DAO.ClaimType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ClaimType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ClaimType = (ClaimType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

		/// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifePolicyClaim_DAO.ClaimDate
		/// </summary>
		public DateTime ClaimDate
		{
			get { return _DAO.ClaimDate; }
			set { _DAO.ClaimDate = value;}
		}
    }
}



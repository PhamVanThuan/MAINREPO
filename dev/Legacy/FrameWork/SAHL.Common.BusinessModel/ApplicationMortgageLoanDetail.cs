using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class ApplicationMortgageLoanDetail : IApplicationMortgageLoanDetail, IDAOObject 
	{
				public ApplicationMortgageLoanDetail(SAHL.Common.BusinessModel.DAO.ApplicationMortgageLoanDetail_DAO ApplicationMortgageLoanDetail) : base(ApplicationMortgageLoanDetail)
		{
			this._DAO = ApplicationMortgageLoanDetail;
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 ApplicationKey 
		{
			get { return _DAO.ApplicationKey; }
			set { _DAO.ApplicationKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double? ApplicationAmount
		{
			get { return _DAO.ApplicationAmount; }
			set { _DAO.ApplicationAmount = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public IMortgageLoanPurpose MortgageLoanPurpose 
		{
			get
			{
				if (null == _DAO.MortgageLoanPurpose) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMortgageLoanPurpose, MortgageLoanPurpose_DAO>(_DAO.MortgageLoanPurpose);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.MortgageLoanPurpose = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.MortgageLoanPurpose = (MortgageLoanPurpose_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public IApplicantType ApplicantType 
		{
			get
			{
				if (null == _DAO.ApplicantType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicantType, ApplicantType_DAO>(_DAO.ApplicantType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicantType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicantType = (ApplicantType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32? NumApplicants
		{
			get { return _DAO.NumApplicants; }
			set { _DAO.NumApplicants = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Boolean? CreditVerification
		{
			get { return _DAO.CreditVerification; }
			set { _DAO.CreditVerification = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? HomePurchaseDate
		{
			get { return _DAO.HomePurchaseDate; }
			set { _DAO.HomePurchaseDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? BondRegistrationDate
		{
			get { return _DAO.BondRegistrationDate; }
			set { _DAO.BondRegistrationDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double? CurrentBondValue
		{
			get { return _DAO.CurrentBondValue; }
			set { _DAO.CurrentBondValue = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? DeedsOfficeDate
		{
			get { return _DAO.DeedsOfficeDate; }
			set { _DAO.DeedsOfficeDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String BondFinancialInstitution 
		{
			get { return _DAO.BondFinancialInstitution; }
			set { _DAO.BondFinancialInstitution = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double? PurchasePrice
		{
			get { return _DAO.PurchasePrice; }
			set { _DAO.PurchasePrice = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public IResetConfiguration ResetConfiguration 
		{
			get
			{
				if (null == _DAO.ResetConfiguration) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IResetConfiguration, ResetConfiguration_DAO>(_DAO.ResetConfiguration);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ResetConfiguration = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ResetConfiguration = (ResetConfiguration_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public IApplication Application 
		{
			get
			{
				if (null == _DAO.Application) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Application = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Application = (Application_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public String TransferringAttorney 
		{
			get { return _DAO.TransferringAttorney; }
			set { _DAO.TransferringAttorney = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double? ClientEstimatePropertyValuation
		{
			get { return _DAO.ClientEstimatePropertyValuation; }
			set { _DAO.ClientEstimatePropertyValuation = value;}
		}
        /// <summary>
        /// 
        /// </summary>
        Int32? DependentsPerHousehold
        {
            get { return _DAO.DependentsPerHousehold; }
            set { _DAO.DependentsPerHousehold = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        Int32? ContributingDependents
        {
            get { return _DAO.ContributingDependents; }
            set { _DAO.ContributingDependents = value; }
        }
	}
}



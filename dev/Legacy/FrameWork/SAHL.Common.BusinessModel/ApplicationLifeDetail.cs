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
	public partial class ApplicationLifeDetail : IApplicationLifeDetail, IDAOObject 
	{
				public ApplicationLifeDetail(SAHL.Common.BusinessModel.DAO.ApplicationLifeDetail_DAO ApplicationLifeDetail) : base(ApplicationLifeDetail)
		{
			this._DAO = ApplicationLifeDetail;
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double DeathBenefit 
		{
			get { return _DAO.DeathBenefit; }
			set { _DAO.DeathBenefit = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double InstallmentProtectionBenefit 
		{
			get { return _DAO.InstallmentProtectionBenefit; }
			set { _DAO.InstallmentProtectionBenefit = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double DeathBenefitPremium 
		{
			get { return _DAO.DeathBenefitPremium; }
			set { _DAO.DeathBenefitPremium = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double InstallmentProtectionPremium 
		{
			get { return _DAO.InstallmentProtectionPremium; }
			set { _DAO.InstallmentProtectionPremium = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime DateOfExpiry 
		{
			get { return _DAO.DateOfExpiry; }
			set { _DAO.DateOfExpiry = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? DateOfAcceptance
		{
			get { return _DAO.DateOfAcceptance; }
			set { _DAO.DateOfAcceptance = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Decimal UpliftFactor 
		{
			get { return _DAO.UpliftFactor; }
			set { _DAO.UpliftFactor = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Decimal JointDiscountFactor 
		{
			get { return _DAO.JointDiscountFactor; }
			set { _DAO.JointDiscountFactor = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double MonthlyPremium 
		{
			get { return _DAO.MonthlyPremium; }
			set { _DAO.MonthlyPremium = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double YearlyPremium 
		{
			get { return _DAO.YearlyPremium; }
			set { _DAO.YearlyPremium = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double? SumAssured
		{
			get { return _DAO.SumAssured; }
			set { _DAO.SumAssured = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? DateLastUpdated
		{
			get { return _DAO.DateLastUpdated; }
			set { _DAO.DateLastUpdated = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String Consultant 
		{
			get { return _DAO.Consultant; }
			set { _DAO.Consultant = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double? CurrentSumAssured
		{
			get { return _DAO.CurrentSumAssured; }
			set { _DAO.CurrentSumAssured = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double? PremiumShortfall
		{
			get { return _DAO.PremiumShortfall; }
			set { _DAO.PremiumShortfall = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public IInsurer Insurer 
		{
			get
			{
				if (null == _DAO.Insurer) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IInsurer, Insurer_DAO>(_DAO.Insurer);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Insurer = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Insurer = (Insurer_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public String ExternalPolicyNumber 
		{
			get { return _DAO.ExternalPolicyNumber; }
			set { _DAO.ExternalPolicyNumber = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? DateCeded
		{
			get { return _DAO.DateCeded; }
			set { _DAO.DateCeded = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public IPriority Priority 
		{
			get
			{
				if (null == _DAO.Priority) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IPriority, Priority_DAO>(_DAO.Priority);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Priority = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Priority = (Priority_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public ILegalEntity PolicyHolderLegalEntity 
		{
			get
			{
				if (null == _DAO.PolicyHolderLegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.PolicyHolderLegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.PolicyHolderLegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.PolicyHolderLegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public String RPARInsurer 
		{
			get { return _DAO.RPARInsurer; }
			set { _DAO.RPARInsurer = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String RPARPolicyNumber 
		{
			get { return _DAO.RPARPolicyNumber; }
			set { _DAO.RPARPolicyNumber = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public IBroker Broker 
		{
			get
			{
				if (null == _DAO.Broker) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IBroker, Broker_DAO>(_DAO.Broker);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Broker = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Broker = (Broker_DAO)obj.GetDAOObject();
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
	}
}



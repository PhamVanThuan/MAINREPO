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
	/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO
	/// </summary>
    public partial class LifePolicy : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifePolicy_DAO>, ILifePolicy
	{
		public LifePolicy(SAHL.Common.BusinessModel.DAO.LifePolicy_DAO LifePolicy) : base(LifePolicy)
		{
			this._DAO = LifePolicy;
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
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DeathBenefit
		/// </summary>
		public Double DeathBenefit 
		{
			get { return _DAO.DeathBenefit; }
			set { _DAO.DeathBenefit = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.InstallmentProtectionBenefit
		/// </summary>
		public Double InstallmentProtectionBenefit 
		{
			get { return _DAO.InstallmentProtectionBenefit; }
			set { _DAO.InstallmentProtectionBenefit = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DeathBenefitPremium
		/// </summary>
		public Double DeathBenefitPremium 
		{
			get { return _DAO.DeathBenefitPremium; }
			set { _DAO.DeathBenefitPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.InstallmentProtectionPremium
		/// </summary>
		public Double InstallmentProtectionPremium 
		{
			get { return _DAO.InstallmentProtectionPremium; }
			set { _DAO.InstallmentProtectionPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateOfCommencement
		/// </summary>
		public DateTime? DateOfCommencement
		{
			get { return _DAO.DateOfCommencement; }
			set { _DAO.DateOfCommencement = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateOfExpiry
		/// </summary>
		public DateTime DateOfExpiry 
		{
			get { return _DAO.DateOfExpiry; }
			set { _DAO.DateOfExpiry = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DeathRetentionLimit
		/// </summary>
		public Double DeathRetentionLimit 
		{
			get { return _DAO.DeathRetentionLimit; }
			set { _DAO.DeathRetentionLimit = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.InstallmentProtectionRetentionLimit
		/// </summary>
		public Double InstallmentProtectionRetentionLimit 
		{
			get { return _DAO.InstallmentProtectionRetentionLimit; }
			set { _DAO.InstallmentProtectionRetentionLimit = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.UpliftFactor
		/// </summary>
		public Decimal UpliftFactor 
		{
			get { return _DAO.UpliftFactor; }
			set { _DAO.UpliftFactor = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.JointDiscountFactor
		/// </summary>
		public Decimal JointDiscountFactor 
		{
			get { return _DAO.JointDiscountFactor; }
			set { _DAO.JointDiscountFactor = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateOfCancellation
		/// </summary>
		public DateTime? DateOfCancellation
		{
			get { return _DAO.DateOfCancellation; }
			set { _DAO.DateOfCancellation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DeathReassuranceRetention
		/// </summary>
		public Double? DeathReassuranceRetention
		{
			get { return _DAO.DeathReassuranceRetention; }
			set { _DAO.DeathReassuranceRetention = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.IPBReassuranceRetention
		/// </summary>
		public Double? IPBReassuranceRetention
		{
			get { return _DAO.IPBReassuranceRetention; }
			set { _DAO.IPBReassuranceRetention = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.YearlyPremium
		/// </summary>
		public Double YearlyPremium 
		{
			get { return _DAO.YearlyPremium; }
			set { _DAO.YearlyPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateOfAcceptance
		/// </summary>
		public DateTime? DateOfAcceptance
		{
			get { return _DAO.DateOfAcceptance; }
			set { _DAO.DateOfAcceptance = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.SumAssured
		/// </summary>
		public Double SumAssured 
		{
			get { return _DAO.SumAssured; }
			set { _DAO.SumAssured = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateLastUpdated
		/// </summary>
		public DateTime? DateLastUpdated
		{
			get { return _DAO.DateLastUpdated; }
			set { _DAO.DateLastUpdated = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.Consultant
		/// </summary>
		public String Consultant 
		{
			get { return _DAO.Consultant; }
			set { _DAO.Consultant = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.CurrentSumAssured
		/// </summary>
		public Double? CurrentSumAssured
		{
			get { return _DAO.CurrentSumAssured; }
			set { _DAO.CurrentSumAssured = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.PremiumShortfall
		/// </summary>
		public Double? PremiumShortfall
		{
			get { return _DAO.PremiumShortfall; }
			set { _DAO.PremiumShortfall = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.ExternalPolicyNumber
		/// </summary>
		public String ExternalPolicyNumber 
		{
			get { return _DAO.ExternalPolicyNumber; }
			set { _DAO.ExternalPolicyNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.DateCeded
		/// </summary>
		public DateTime? DateCeded
		{
			get { return _DAO.DateCeded; }
			set { _DAO.DateCeded = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.ClaimStatusDate
		/// </summary>
		public DateTime? ClaimStatusDate
		{
			get { return _DAO.ClaimStatusDate; }
			set { _DAO.ClaimStatusDate = value;}
		}
		/// <summary>
		/// The Primary Legal entity for this policy.
		/// </summary>
		public ILegalEntity PolicyHolderLE 
		{
			get
			{
				if (null == _DAO.PolicyHolderLE) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.PolicyHolderLE);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.PolicyHolderLE = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.PolicyHolderLE = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.RPARInsurer
		/// </summary>
		public String RPARInsurer 
		{
			get { return _DAO.RPARInsurer; }
			set { _DAO.RPARInsurer = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.RPARPolicyNumber
		/// </summary>
		public String RPARPolicyNumber 
		{
			get { return _DAO.RPARPolicyNumber; }
			set { _DAO.RPARPolicyNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.Broker
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
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.Insurer
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
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.ClaimStatus
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
        /// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.ClaimType
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
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.LifePolicyStatus
		/// </summary>
		public ILifePolicyStatus LifePolicyStatus 
		{
			get
			{
				if (null == _DAO.LifePolicyStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILifePolicyStatus, LifePolicyStatus_DAO>(_DAO.LifePolicyStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LifePolicyStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LifePolicyStatus = (LifePolicyStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.Priority
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
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.LifePolicyType
		/// </summary>
		public ILifePolicyType LifePolicyType 
		{
			get
			{
				if (null == _DAO.LifePolicyType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILifePolicyType, LifePolicyType_DAO>(_DAO.LifePolicyType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LifePolicyType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LifePolicyType = (LifePolicyType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePolicy_DAO.AnniversaryDate
		/// </summary>
		public DateTime? AnniversaryDate
		{
			get { return _DAO.AnniversaryDate; }
			set { _DAO.AnniversaryDate = value;}
		}

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.HOC_DAO.FinancialService
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
    }
}



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
	/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO
	/// </summary>
	public partial class SANTAMPolicyTracking : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO>, ISANTAMPolicyTracking
	{
				public SANTAMPolicyTracking(SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO SANTAMPolicyTracking) : base(SANTAMPolicyTracking)
		{
			this._DAO = SANTAMPolicyTracking;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.SANTAMPolicyTrackingKey
		/// </summary>
		public Int32 SANTAMPolicyTrackingKey 
		{
			get { return _DAO.SANTAMPolicyTrackingKey; }
			set { _DAO.SANTAMPolicyTrackingKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.PolicyNumber
		/// </summary>
		public String PolicyNumber 
		{
			get { return _DAO.PolicyNumber; }
			set { _DAO.PolicyNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.QuoteNumber
		/// </summary>
		public String QuoteNumber 
		{
			get { return _DAO.QuoteNumber; }
			set { _DAO.QuoteNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.CampaignTargetContactKey
		/// </summary>
		public Int32 CampaignTargetContactKey 
		{
			get { return _DAO.CampaignTargetContactKey; }
			set { _DAO.CampaignTargetContactKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.LegalEntityKey
		/// </summary>
		public Int32 LegalEntityKey 
		{
			get { return _DAO.LegalEntityKey; }
			set { _DAO.LegalEntityKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.AccountKey
		/// </summary>
		public Int32 AccountKey 
		{
			get { return _DAO.AccountKey; }
			set { _DAO.AccountKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.ActiveDate
		/// </summary>
		public DateTime ActiveDate 
		{
			get { return _DAO.ActiveDate; }
			set { _DAO.ActiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.Canceldate
		/// </summary>
		public DateTime Canceldate 
		{
			get { return _DAO.Canceldate; }
			set { _DAO.Canceldate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.MonthlyPremium
		/// </summary>
		public Double MonthlyPremium 
		{
			get { return _DAO.MonthlyPremium; }
			set { _DAO.MonthlyPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.CollectionDay
		/// </summary>
		public Int32 CollectionDay 
		{
			get { return _DAO.CollectionDay; }
			set { _DAO.CollectionDay = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyTracking_DAO.SANTAMPolicyStatus
		/// </summary>
		public ISANTAMPolicyStatus SANTAMPolicyStatus 
		{
			get
			{
				if (null == _DAO.SANTAMPolicyStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISANTAMPolicyStatus, SANTAMPolicyStatus_DAO>(_DAO.SANTAMPolicyStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SANTAMPolicyStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SANTAMPolicyStatus = (SANTAMPolicyStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



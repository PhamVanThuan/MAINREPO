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
	/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO
	/// </summary>
	public partial class DebtCounselling : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO>, IDebtCounselling
	{
				public DebtCounselling(SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO DebtCounselling) : base(DebtCounselling)
		{
			this._DAO = DebtCounselling;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.PaymentReceivedDate
		/// </summary>
		public DateTime? PaymentReceivedDate
		{
			get { return _DAO.PaymentReceivedDate; }
			set { _DAO.PaymentReceivedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.PaymentReceivedAmount
		/// </summary>
		public Double? PaymentReceivedAmount
		{
			get { return _DAO.PaymentReceivedAmount; }
			set { _DAO.PaymentReceivedAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.Account
		/// </summary>
		public IAccount Account 
		{
			get
			{
				if (null == _DAO.Account) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Account = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Account = (Account_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.DebtCounsellingStatus
		/// </summary>
		public IDebtCounsellingStatus DebtCounsellingStatus 
		{
			get
			{
				if (null == _DAO.DebtCounsellingStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDebtCounsellingStatus, DebtCounsellingStatus_DAO>(_DAO.DebtCounsellingStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DebtCounsellingStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DebtCounsellingStatus = (DebtCounsellingStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.DebtCounsellingGroup
		/// </summary>
		public IDebtCounsellingGroup DebtCounsellingGroup 
		{
			get
			{
				if (null == _DAO.DebtCounsellingGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDebtCounsellingGroup, DebtCounsellingGroup_DAO>(_DAO.DebtCounsellingGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DebtCounsellingGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DebtCounsellingGroup = (DebtCounsellingGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.DiaryDate
		/// </summary>
		public DateTime? DiaryDate
		{
			get { return _DAO.DiaryDate; }
			set { _DAO.DiaryDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.ReferenceNumber
		/// </summary>
		public String ReferenceNumber 
		{
			get { return _DAO.ReferenceNumber; }
			set { _DAO.ReferenceNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.HearingDetails
		/// </summary>
		private DAOEventList<HearingDetail_DAO, IHearingDetail, HearingDetail> _HearingDetails;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.HearingDetails
		/// </summary>
		public IEventList<IHearingDetail> HearingDetails
		{
			get
			{
				if (null == _HearingDetails) 
				{
					if(null == _DAO.HearingDetails)
						_DAO.HearingDetails = new List<HearingDetail_DAO>();
					_HearingDetails = new DAOEventList<HearingDetail_DAO, IHearingDetail, HearingDetail>(_DAO.HearingDetails);
					_HearingDetails.BeforeAdd += new EventListHandler(OnHearingDetails_BeforeAdd);					
					_HearingDetails.BeforeRemove += new EventListHandler(OnHearingDetails_BeforeRemove);					
					_HearingDetails.AfterAdd += new EventListHandler(OnHearingDetails_AfterAdd);					
					_HearingDetails.AfterRemove += new EventListHandler(OnHearingDetails_AfterRemove);					
				}
				return _HearingDetails;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.Proposals
		/// </summary>
		private DAOEventList<Proposal_DAO, IProposal, Proposal> _Proposals;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounselling_DAO.Proposals
		/// </summary>
		public IEventList<IProposal> Proposals
		{
			get
			{
				if (null == _Proposals) 
				{
					if(null == _DAO.Proposals)
						_DAO.Proposals = new List<Proposal_DAO>();
					_Proposals = new DAOEventList<Proposal_DAO, IProposal, Proposal>(_DAO.Proposals);
					_Proposals.BeforeAdd += new EventListHandler(OnProposals_BeforeAdd);					
					_Proposals.BeforeRemove += new EventListHandler(OnProposals_BeforeRemove);					
					_Proposals.AfterAdd += new EventListHandler(OnProposals_AfterAdd);					
					_Proposals.AfterRemove += new EventListHandler(OnProposals_AfterRemove);					
				}
				return _Proposals;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_HearingDetails = null;
			_Proposals = null;
			
		}
	}
}



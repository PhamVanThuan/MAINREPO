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
	/// SAHL.Common.BusinessModel.DAO.Proposal_DAO
	/// </summary>
	public partial class Proposal : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Proposal_DAO>, IProposal
	{
				public Proposal(SAHL.Common.BusinessModel.DAO.Proposal_DAO Proposal) : base(Proposal)
		{
			this._DAO = Proposal;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.DebtCounselling
		/// </summary>
		public IDebtCounselling DebtCounselling 
		{
			get
			{
				if (null == _DAO.DebtCounselling) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDebtCounselling, DebtCounselling_DAO>(_DAO.DebtCounselling);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DebtCounselling = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DebtCounselling = (DebtCounselling_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ADUser
		/// </summary>
		public IADUser ADUser 
		{
			get
			{
				if (null == _DAO.ADUser) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IADUser, ADUser_DAO>(_DAO.ADUser);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ADUser = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ADUser = (ADUser_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.CreateDate
		/// </summary>
		public DateTime CreateDate 
		{
			get { return _DAO.CreateDate; }
			set { _DAO.CreateDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ProposalItems
		/// </summary>
		private DAOEventList<ProposalItem_DAO, IProposalItem, ProposalItem> _ProposalItems;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ProposalItems
		/// </summary>
		public IEventList<IProposalItem> ProposalItems
		{
			get
			{
				if (null == _ProposalItems) 
				{
					if(null == _DAO.ProposalItems)
						_DAO.ProposalItems = new List<ProposalItem_DAO>();
					_ProposalItems = new DAOEventList<ProposalItem_DAO, IProposalItem, ProposalItem>(_DAO.ProposalItems);					
				}
				return _ProposalItems;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ProposalStatus
		/// </summary>
		public IProposalStatus ProposalStatus 
		{
			get
			{
				if (null == _DAO.ProposalStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProposalStatus, ProposalStatus_DAO>(_DAO.ProposalStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ProposalStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ProposalStatus = (ProposalStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ProposalType
		/// </summary>
		public IProposalType ProposalType 
		{
			get
			{
				if (null == _DAO.ProposalType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProposalType, ProposalType_DAO>(_DAO.ProposalType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ProposalType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ProposalType = (ProposalType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.HOCInclusive
		/// </summary>
		public Boolean? HOCInclusive
		{
			get { return _DAO.HOCInclusive; }
			set { _DAO.HOCInclusive = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.LifeInclusive
		/// </summary>
		public Boolean? LifeInclusive
		{
			get { return _DAO.LifeInclusive; }
			set { _DAO.LifeInclusive = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.Accepted
		/// </summary>
		public Boolean? Accepted
		{
			get { return _DAO.Accepted; }
			set { _DAO.Accepted = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ReviewDate
		/// </summary>
		public DateTime? ReviewDate
		{
			get { return _DAO.ReviewDate; }
			set { _DAO.ReviewDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Proposal_DAO.MonthlyServiceFeeInclusive
		/// </summary>
        public Boolean MonthlyServiceFeeInclusive
        {
            get { return _DAO.MonthlyServiceFeeInclusive; }
            set { _DAO.MonthlyServiceFeeInclusive = value; }
        }
        
        public override void Refresh()
		{
			base.Refresh();
			_ProposalItems = null;
			
		}
    }
}



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
	/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO
	/// </summary>
	public partial class DebtCounsellingProposal : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO>, IDebtCounsellingProposal
	{
				public DebtCounsellingProposal(SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO DebtCounsellingProposal) : base(DebtCounsellingProposal)
		{
			this._DAO = DebtCounsellingProposal;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.DebtCounsellingProposalKey
		/// </summary>
		public Int32 DebtCounsellingProposalKey 
		{
			get { return _DAO.DebtCounsellingProposalKey; }
			set { _DAO.DebtCounsellingProposalKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.ProposalStatusKey
		/// </summary>
		public Int32 ProposalStatusKey 
		{
			get { return _DAO.ProposalStatusKey; }
			set { _DAO.ProposalStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.HOCInclusive
		/// </summary>
		public Boolean HOCInclusive 
		{
			get { return _DAO.HOCInclusive; }
			set { _DAO.HOCInclusive = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.LifeInclusive
		/// </summary>
		public Boolean LifeInclusive 
		{
			get { return _DAO.LifeInclusive; }
			set { _DAO.LifeInclusive = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.AcceptedProposal
		/// </summary>
		public Boolean AcceptedProposal 
		{
			get { return _DAO.AcceptedProposal; }
			set { _DAO.AcceptedProposal = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.ProposalDate
		/// </summary>
		public DateTime ProposalDate 
		{
			get { return _DAO.ProposalDate; }
			set { _DAO.ProposalDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.DebtCounsellingProposalItems
		/// </summary>
		private DAOEventList<DebtCounsellingProposalItem_DAO, IDebtCounsellingProposalItem, DebtCounsellingProposalItem> _DebtCounsellingProposalItems;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.DebtCounsellingProposalItems
		/// </summary>
		public IEventList<IDebtCounsellingProposalItem> DebtCounsellingProposalItems
		{
			get
			{
				if (null == _DebtCounsellingProposalItems) 
				{
					if(null == _DAO.DebtCounsellingProposalItems)
						_DAO.DebtCounsellingProposalItems = new List<DebtCounsellingProposalItem_DAO>();
					_DebtCounsellingProposalItems = new DAOEventList<DebtCounsellingProposalItem_DAO, IDebtCounsellingProposalItem, DebtCounsellingProposalItem>(_DAO.DebtCounsellingProposalItems);
					_DebtCounsellingProposalItems.BeforeAdd += new EventListHandler(OnDebtCounsellingProposalItems_BeforeAdd);					
					_DebtCounsellingProposalItems.BeforeRemove += new EventListHandler(OnDebtCounsellingProposalItems_BeforeRemove);					
					_DebtCounsellingProposalItems.AfterAdd += new EventListHandler(OnDebtCounsellingProposalItems_AfterAdd);					
					_DebtCounsellingProposalItems.AfterRemove += new EventListHandler(OnDebtCounsellingProposalItems_AfterRemove);					
				}
				return _DebtCounsellingProposalItems;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.DebtCounselling
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
		public override void Refresh()
		{
			base.Refresh();
			_DebtCounsellingProposalItems = null;
			
		}
	}
}



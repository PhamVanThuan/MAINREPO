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
	/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO
	/// </summary>
	public partial class MigrationDebtCounsellingProposal : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO>, IMigrationDebtCounsellingProposal
	{
				public MigrationDebtCounsellingProposal(SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO MigrationDebtCounsellingProposal) : base(MigrationDebtCounsellingProposal)
		{
			this._DAO = MigrationDebtCounsellingProposal;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.ProposalStatusKey
		/// </summary>
		public Int32 ProposalStatusKey 
		{
			get { return _DAO.ProposalStatusKey; }
			set { _DAO.ProposalStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.HOCInclusive
		/// </summary>
		public Boolean HOCInclusive 
		{
			get { return _DAO.HOCInclusive; }
			set { _DAO.HOCInclusive = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.LifeInclusive
		/// </summary>
		public Boolean LifeInclusive 
		{
			get { return _DAO.LifeInclusive; }
			set { _DAO.LifeInclusive = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.AcceptedProposal
		/// </summary>
		public Boolean AcceptedProposal 
		{
			get { return _DAO.AcceptedProposal; }
			set { _DAO.AcceptedProposal = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.ProposalDate
		/// </summary>
		public DateTime ProposalDate 
		{
			get { return _DAO.ProposalDate; }
			set { _DAO.ProposalDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.DebtCounsellingProposalItems
		/// </summary>
		private DAOEventList<MigrationDebtCounsellingProposalItem_DAO, IMigrationDebtCounsellingProposalItem, MigrationDebtCounsellingProposalItem> _DebtCounsellingProposalItems;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.DebtCounsellingProposalItems
		/// </summary>
		public IEventList<IMigrationDebtCounsellingProposalItem> DebtCounsellingProposalItems
		{
			get
			{
				if (null == _DebtCounsellingProposalItems) 
				{
					if(null == _DAO.DebtCounsellingProposalItems)
						_DAO.DebtCounsellingProposalItems = new List<MigrationDebtCounsellingProposalItem_DAO>();
					_DebtCounsellingProposalItems = new DAOEventList<MigrationDebtCounsellingProposalItem_DAO, IMigrationDebtCounsellingProposalItem, MigrationDebtCounsellingProposalItem>(_DAO.DebtCounsellingProposalItems);
					_DebtCounsellingProposalItems.BeforeAdd += new EventListHandler(OnDebtCounsellingProposalItems_BeforeAdd);					
					_DebtCounsellingProposalItems.BeforeRemove += new EventListHandler(OnDebtCounsellingProposalItems_BeforeRemove);					
					_DebtCounsellingProposalItems.AfterAdd += new EventListHandler(OnDebtCounsellingProposalItems_AfterAdd);					
					_DebtCounsellingProposalItems.AfterRemove += new EventListHandler(OnDebtCounsellingProposalItems_AfterRemove);					
				}
				return _DebtCounsellingProposalItems;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.DebtCounselling
		/// </summary>
		public IMigrationDebtCounselling DebtCounselling 
		{
			get
			{
				if (null == _DAO.DebtCounselling) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMigrationDebtCounselling, MigrationDebtCounselling_DAO>(_DAO.DebtCounselling);
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
					_DAO.DebtCounselling = (MigrationDebtCounselling_DAO)obj.GetDAOObject();
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



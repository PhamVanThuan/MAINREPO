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
	/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO
	/// </summary>
	public partial class MigrationDebtCounselling : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO>, IMigrationDebtCounselling
	{
				public MigrationDebtCounselling(SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO MigrationDebtCounselling) : base(MigrationDebtCounselling)
		{
			this._DAO = MigrationDebtCounselling;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.AccountKey
		/// </summary>
		public Int32 AccountKey 
		{
			get { return _DAO.AccountKey; }
			set { _DAO.AccountKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DebtCounsellingConsultantKey
		/// </summary>
		public Int32? DebtCounsellingConsultantKey
		{
			get { return _DAO.DebtCounsellingConsultantKey; }
			set { _DAO.DebtCounsellingConsultantKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DebtCounsellorKey
		/// </summary>
		public Int32? DebtCounsellorKey
		{
			get { return _DAO.DebtCounsellorKey; }
			set { _DAO.DebtCounsellorKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.ProposalTypeKey
		/// </summary>
		public Int32 ProposalTypeKey 
		{
			get { return _DAO.ProposalTypeKey; }
			set { _DAO.ProposalTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DateOf171
		/// </summary>
		public DateTime? DateOf171
		{
			get { return _DAO.DateOf171; }
			set { _DAO.DateOf171 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.ReviewDate
		/// </summary>
		public DateTime? ReviewDate
		{
			get { return _DAO.ReviewDate; }
			set { _DAO.ReviewDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.CourtOrderDate
		/// </summary>
		public DateTime? CourtOrderDate
		{
			get { return _DAO.CourtOrderDate; }
			set { _DAO.CourtOrderDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.TerminateDate
		/// </summary>
		public DateTime? TerminateDate
		{
			get { return _DAO.TerminateDate; }
			set { _DAO.TerminateDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.SixtyDaysDate
		/// </summary>
		public DateTime? SixtyDaysDate
		{
			get { return _DAO.SixtyDaysDate; }
			set { _DAO.SixtyDaysDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.ApprovalDate
		/// </summary>
		public DateTime? ApprovalDate
		{
			get { return _DAO.ApprovalDate; }
			set { _DAO.ApprovalDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.ApprovalUserKey
		/// </summary>
		public Int32? ApprovalUserKey
		{
			get { return _DAO.ApprovalUserKey; }
			set { _DAO.ApprovalUserKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.PaymentReceivedDate
		/// </summary>
		public DateTime? PaymentReceivedDate
		{
			get { return _DAO.PaymentReceivedDate; }
			set { _DAO.PaymentReceivedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DebtCounsellingExternalRoles
		/// </summary>
		private DAOEventList<MigrationDebtCounsellingExternalRole_DAO, IMigrationDebtCounsellingExternalRole, MigrationDebtCounsellingExternalRole> _DebtCounsellingExternalRoles;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DebtCounsellingExternalRoles
		/// </summary>
		public IEventList<IMigrationDebtCounsellingExternalRole> DebtCounsellingExternalRoles
		{
			get
			{
				if (null == _DebtCounsellingExternalRoles) 
				{
					if(null == _DAO.DebtCounsellingExternalRoles)
						_DAO.DebtCounsellingExternalRoles = new List<MigrationDebtCounsellingExternalRole_DAO>();
					_DebtCounsellingExternalRoles = new DAOEventList<MigrationDebtCounsellingExternalRole_DAO, IMigrationDebtCounsellingExternalRole, MigrationDebtCounsellingExternalRole>(_DAO.DebtCounsellingExternalRoles);					
				}
				return _DebtCounsellingExternalRoles;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DebtCounsellingProposals
		/// </summary>
		private DAOEventList<MigrationDebtCounsellingProposal_DAO, IMigrationDebtCounsellingProposal, MigrationDebtCounsellingProposal> _DebtCounsellingProposals;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DebtCounsellingProposals
		/// </summary>
		public IEventList<IMigrationDebtCounsellingProposal> DebtCounsellingProposals
		{
			get
			{
				if (null == _DebtCounsellingProposals) 
				{
					if(null == _DAO.DebtCounsellingProposals)
						_DAO.DebtCounsellingProposals = new List<MigrationDebtCounsellingProposal_DAO>();
					_DebtCounsellingProposals = new DAOEventList<MigrationDebtCounsellingProposal_DAO, IMigrationDebtCounsellingProposal, MigrationDebtCounsellingProposal>(_DAO.DebtCounsellingProposals);
					_DebtCounsellingProposals.BeforeAdd += new EventListHandler(OnDebtCounsellingProposals_BeforeAdd);					
					_DebtCounsellingProposals.BeforeRemove += new EventListHandler(OnDebtCounsellingProposals_BeforeRemove);					
					_DebtCounsellingProposals.AfterAdd += new EventListHandler(OnDebtCounsellingProposals_AfterAdd);					
					_DebtCounsellingProposals.AfterRemove += new EventListHandler(OnDebtCounsellingProposals_AfterRemove);					
				}
				return _DebtCounsellingProposals;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_DebtCounsellingExternalRoles = null;
			_DebtCounsellingProposals = null;
			
		}
	}
}



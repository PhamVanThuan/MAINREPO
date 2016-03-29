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
	/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO
	/// </summary>
	public partial class ProposalItem : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ProposalItem_DAO>, IProposalItem
	{
				public ProposalItem(SAHL.Common.BusinessModel.DAO.ProposalItem_DAO ProposalItem) : base(ProposalItem)
		{
			this._DAO = ProposalItem;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.StartDate
		/// </summary>
		public DateTime StartDate 
		{
			get { return _DAO.StartDate; }
			set { _DAO.StartDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.EndDate
		/// </summary>
		public DateTime EndDate 
		{
			get { return _DAO.EndDate; }
			set { _DAO.EndDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.InterestRate
		/// </summary>
		public Double InterestRate 
		{
			get { return _DAO.InterestRate; }
			set { _DAO.InterestRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.Amount
		/// </summary>
		public Double Amount 
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.AdditionalAmount
		/// </summary>
		public Double AdditionalAmount 
		{
			get { return _DAO.AdditionalAmount; }
			set { _DAO.AdditionalAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.ADUser
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
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.CreateDate
		/// </summary>
		public DateTime CreateDate 
		{
			get { return _DAO.CreateDate; }
			set { _DAO.CreateDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.MarketRate
		/// </summary>
		public IMarketRate MarketRate 
		{
			get
			{
				if (null == _DAO.MarketRate) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMarketRate, MarketRate_DAO>(_DAO.MarketRate);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.MarketRate = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.MarketRate = (MarketRate_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.Proposal
		/// </summary>
		public IProposal Proposal 
		{
			get
			{
				if (null == _DAO.Proposal) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProposal, Proposal_DAO>(_DAO.Proposal);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Proposal = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Proposal = (Proposal_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.InstalmentPercent
		/// </summary>
		public Double? InstalmentPercent
		{
			get { return _DAO.InstalmentPercent; }
			set { _DAO.InstalmentPercent = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.AnnualEscalation
		/// </summary>
		public Double? AnnualEscalation
		{
			get { return _DAO.AnnualEscalation; }
			set { _DAO.AnnualEscalation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.StartPeriod
		/// </summary>
		public Int16 StartPeriod 
		{
			get { return _DAO.StartPeriod; }
			set { _DAO.StartPeriod = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProposalItem_DAO.EndPeriod
		/// </summary>
		public Int16 EndPeriod 
		{
			get { return _DAO.EndPeriod; }
			set { _DAO.EndPeriod = value;}
		}
	}
}



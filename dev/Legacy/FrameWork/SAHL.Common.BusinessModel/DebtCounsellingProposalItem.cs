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
	/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO
	/// </summary>
	public partial class DebtCounsellingProposalItem : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO>, IDebtCounsellingProposalItem
	{
				public DebtCounsellingProposalItem(SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO DebtCounsellingProposalItem) : base(DebtCounsellingProposalItem)
		{
			this._DAO = DebtCounsellingProposalItem;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.DebtCounsellingProposalItemKey
		/// </summary>
		public Int32 DebtCounsellingProposalItemKey 
		{
			get { return _DAO.DebtCounsellingProposalItemKey; }
			set { _DAO.DebtCounsellingProposalItemKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.StartDate
		/// </summary>
		public DateTime StartDate 
		{
			get { return _DAO.StartDate; }
			set { _DAO.StartDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.EndDate
		/// </summary>
		public DateTime EndDate 
		{
			get { return _DAO.EndDate; }
			set { _DAO.EndDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.MarketRateKey
		/// </summary>
		public Decimal MarketRateKey 
		{
			get { return _DAO.MarketRateKey; }
			set { _DAO.MarketRateKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.InterestRate
		/// </summary>
		public Decimal InterestRate 
		{
			get { return _DAO.InterestRate; }
			set { _DAO.InterestRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.Amount
		/// </summary>
		public Decimal Amount 
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.AdditionalAmount
		/// </summary>
		public Decimal AdditionalAmount 
		{
			get { return _DAO.AdditionalAmount; }
			set { _DAO.AdditionalAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.CreateDate
		/// </summary>
		public DateTime CreateDate 
		{
			get { return _DAO.CreateDate; }
			set { _DAO.CreateDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.InstalmentPercentage
		/// </summary>
		public Decimal InstalmentPercentage 
		{
			get { return _DAO.InstalmentPercentage; }
			set { _DAO.InstalmentPercentage = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.AnnualEscalation
		/// </summary>
		public Decimal AnnualEscalation 
		{
			get { return _DAO.AnnualEscalation; }
			set { _DAO.AnnualEscalation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.DebtCounsellingProposal
		/// </summary>
		public IDebtCounsellingProposal DebtCounsellingProposal 
		{
			get
			{
				if (null == _DAO.DebtCounsellingProposal) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDebtCounsellingProposal, DebtCounsellingProposal_DAO>(_DAO.DebtCounsellingProposal);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DebtCounsellingProposal = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DebtCounsellingProposal = (DebtCounsellingProposal_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



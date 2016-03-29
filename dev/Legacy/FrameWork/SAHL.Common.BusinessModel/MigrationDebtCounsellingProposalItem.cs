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
	/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO
	/// </summary>
	public partial class MigrationDebtCounsellingProposalItem : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO>, IMigrationDebtCounsellingProposalItem
	{
				public MigrationDebtCounsellingProposalItem(SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO MigrationDebtCounsellingProposalItem) : base(MigrationDebtCounsellingProposalItem)
		{
			this._DAO = MigrationDebtCounsellingProposalItem;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.StartDate
		/// </summary>
		public DateTime StartDate 
		{
			get { return _DAO.StartDate; }
			set { _DAO.StartDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.EndDate
		/// </summary>
		public DateTime EndDate 
		{
			get { return _DAO.EndDate; }
			set { _DAO.EndDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.MarketRateKey
		/// </summary>
		public Int32 MarketRateKey 
		{
			get { return _DAO.MarketRateKey; }
			set { _DAO.MarketRateKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.InterestRate
		/// </summary>
		public Decimal InterestRate 
		{
			get { return _DAO.InterestRate; }
			set { _DAO.InterestRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.Amount
		/// </summary>
		public Decimal Amount 
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.AdditionalAmount
		/// </summary>
		public Decimal AdditionalAmount 
		{
			get { return _DAO.AdditionalAmount; }
			set { _DAO.AdditionalAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.CreateDate
		/// </summary>
		public DateTime CreateDate 
		{
			get { return _DAO.CreateDate; }
			set { _DAO.CreateDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.InstalmentPercentage
		/// </summary>
		public Decimal InstalmentPercentage 
		{
			get { return _DAO.InstalmentPercentage; }
			set { _DAO.InstalmentPercentage = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.AnnualEscalation
		/// </summary>
		public Decimal AnnualEscalation 
		{
			get { return _DAO.AnnualEscalation; }
			set { _DAO.AnnualEscalation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.StartPeriod
		/// </summary>
		public Int32 StartPeriod 
		{
			get { return _DAO.StartPeriod; }
			set { _DAO.StartPeriod = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.EndPeriod
		/// </summary>
		public Int32 EndPeriod 
		{
			get { return _DAO.EndPeriod; }
			set { _DAO.EndPeriod = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.DebtCounsellingProposal
		/// </summary>
		public IMigrationDebtCounsellingProposal DebtCounsellingProposal 
		{
			get
			{
				if (null == _DAO.DebtCounsellingProposal) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMigrationDebtCounsellingProposal, MigrationDebtCounsellingProposal_DAO>(_DAO.DebtCounsellingProposal);
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
					_DAO.DebtCounsellingProposal = (MigrationDebtCounsellingProposal_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



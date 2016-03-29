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
	/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO
	/// </summary>
	public partial class LoanBalance : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LoanBalance_DAO>, ILoanBalance
	{
				public LoanBalance(SAHL.Common.BusinessModel.DAO.LoanBalance_DAO LoanBalance) : base(LoanBalance)
		{
			this._DAO = LoanBalance;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.Term
		/// </summary>
		public Int32 Term 
		{
			get { return _DAO.Term; }
			set { _DAO.Term = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.InitialBalance
		/// </summary>
		public Double InitialBalance 
		{
			get { return _DAO.InitialBalance; }
			set { _DAO.InitialBalance = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.RemainingInstalments
		/// </summary>
		public Int32 RemainingInstalments 
		{
			get { return _DAO.RemainingInstalments; }
			set { _DAO.RemainingInstalments = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.InterestRate
		/// </summary>
		public Double InterestRate 
		{
			get { return _DAO.InterestRate; }
			set { _DAO.InterestRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.RateAdjustment
		/// </summary>
		public Double RateAdjustment 
		{
			get { return _DAO.RateAdjustment; }
			set { _DAO.RateAdjustment = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.ActiveMarketRate
		/// </summary>
		public Double ActiveMarketRate 
		{
			get { return _DAO.ActiveMarketRate; }
			set { _DAO.ActiveMarketRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.MTDInterest
		/// </summary>
		public Double MTDInterest 
		{
			get { return _DAO.MTDInterest; }
			set { _DAO.MTDInterest = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.RateConfiguration
		/// </summary>
		public IRateConfiguration RateConfiguration 
		{
			get
			{
				if (null == _DAO.RateConfiguration) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRateConfiguration, RateConfiguration_DAO>(_DAO.RateConfiguration);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RateConfiguration = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RateConfiguration = (RateConfiguration_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.ResetConfiguration
		/// </summary>
		public IResetConfiguration ResetConfiguration 
		{
			get
			{
				if (null == _DAO.ResetConfiguration) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IResetConfiguration, ResetConfiguration_DAO>(_DAO.ResetConfiguration);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ResetConfiguration = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ResetConfiguration = (ResetConfiguration_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanBalance_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}



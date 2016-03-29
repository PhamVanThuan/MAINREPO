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
	/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO
	/// </summary>
	public partial class LifePremiumHistory : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO>, ILifePremiumHistory
	{
				public LifePremiumHistory(SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO LifePremiumHistory) : base(LifePremiumHistory)
		{
			this._DAO = LifePremiumHistory;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.DeathPremium
		/// </summary>
		public Double DeathPremium 
		{
			get { return _DAO.DeathPremium; }
			set { _DAO.DeathPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.IPBPremium
		/// </summary>
		public Double IPBPremium 
		{
			get { return _DAO.IPBPremium; }
			set { _DAO.IPBPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.SumAssured
		/// </summary>
		public Double SumAssured 
		{
			get { return _DAO.SumAssured; }
			set { _DAO.SumAssured = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.YearlyPremium
		/// </summary>
		public Double YearlyPremium 
		{
			get { return _DAO.YearlyPremium; }
			set { _DAO.YearlyPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.PolicyFactor
		/// </summary>
		public Double PolicyFactor 
		{
			get { return _DAO.PolicyFactor; }
			set { _DAO.PolicyFactor = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.DiscountFactor
		/// </summary>
		public Double DiscountFactor 
		{
			get { return _DAO.DiscountFactor; }
			set { _DAO.DiscountFactor = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.MonthlyPremium
		/// </summary>
		public Double MonthlyPremium 
		{
			get { return _DAO.MonthlyPremium; }
			set { _DAO.MonthlyPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.UserName
		/// </summary>
		public String UserName 
		{
			get { return _DAO.UserName; }
			set { _DAO.UserName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumHistory_DAO.Account
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
	}
}



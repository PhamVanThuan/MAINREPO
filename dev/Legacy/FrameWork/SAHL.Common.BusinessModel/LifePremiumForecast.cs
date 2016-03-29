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
	/// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO
	/// </summary>
	public partial class LifePremiumForecast : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO>, ILifePremiumForecast
	{
				public LifePremiumForecast(SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO LifePremiumForecast) : base(LifePremiumForecast)
		{
			this._DAO = LifePremiumForecast;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.LoanYear
		/// </summary>
		public Int16 LoanYear 
		{
			get { return _DAO.LoanYear; }
			set { _DAO.LoanYear = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.Age
		/// </summary>
		public Int16 Age 
		{
			get { return _DAO.Age; }
			set { _DAO.Age = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.SumAssured
		/// </summary>
		public Double SumAssured 
		{
			get { return _DAO.SumAssured; }
			set { _DAO.SumAssured = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.MonthlyPremium
		/// </summary>
		public Double MonthlyPremium 
		{
			get { return _DAO.MonthlyPremium; }
			set { _DAO.MonthlyPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.YearlyPremium
		/// </summary>
		public Double YearlyPremium 
		{
			get { return _DAO.YearlyPremium; }
			set { _DAO.YearlyPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.MonthlyComm
		/// </summary>
		public Double MonthlyComm 
		{
			get { return _DAO.MonthlyComm; }
			set { _DAO.MonthlyComm = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.EntryDate
		/// </summary>
		public DateTime EntryDate 
		{
			get { return _DAO.EntryDate; }
			set { _DAO.EntryDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifePremiumForecast_DAO.LifePolicy
		/// </summary>
		public IAccountLifePolicy LifePolicy 
		{
			get
			{
				if (null == _DAO.LifePolicy) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccountLifePolicy, AccountLifePolicy_DAO>(_DAO.LifePolicy);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LifePolicy = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LifePolicy = (AccountLifePolicy_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



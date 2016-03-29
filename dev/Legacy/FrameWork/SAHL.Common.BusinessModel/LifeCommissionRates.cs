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
	/// SAHL.Common.BusinessModel.DAO.LifeCommissionRates_DAO
	/// </summary>
	public partial class LifeCommissionRates : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifeCommissionRates_DAO>, ILifeCommissionRates
	{
				public LifeCommissionRates(SAHL.Common.BusinessModel.DAO.LifeCommissionRates_DAO LifeCommissionRates) : base(LifeCommissionRates)
		{
			this._DAO = LifeCommissionRates;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionRates_DAO.Entity
		/// </summary>
		public String Entity 
		{
			get { return _DAO.Entity; }
			set { _DAO.Entity = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionRates_DAO.Percentage
		/// </summary>
		public Double Percentage 
		{
			get { return _DAO.Percentage; }
			set { _DAO.Percentage = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionRates_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionRates_DAO.EffectiveDate
		/// </summary>
		public DateTime EffectiveDate 
		{
			get { return _DAO.EffectiveDate; }
			set { _DAO.EffectiveDate = value;}
		}
	}
}



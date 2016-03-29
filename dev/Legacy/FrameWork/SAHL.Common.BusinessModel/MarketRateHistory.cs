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
	/// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO
	/// </summary>
	public partial class MarketRateHistory : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO>, IMarketRateHistory
	{
				public MarketRateHistory(SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO MarketRateHistory) : base(MarketRateHistory)
		{
			this._DAO = MarketRateHistory;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.RateBefore
		/// </summary>
		public Double RateBefore 
		{
			get { return _DAO.RateBefore; }
			set { _DAO.RateBefore = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.RateAfter
		/// </summary>
		public Double RateAfter 
		{
			get { return _DAO.RateAfter; }
			set { _DAO.RateAfter = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.ChangedBy
		/// </summary>
		public String ChangedBy 
		{
			get { return _DAO.ChangedBy; }
			set { _DAO.ChangedBy = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.ChangedByHost
		/// </summary>
		public String ChangedByHost 
		{
			get { return _DAO.ChangedByHost; }
			set { _DAO.ChangedByHost = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.ChangedByApp
		/// </summary>
		public String ChangedByApp 
		{
			get { return _DAO.ChangedByApp; }
			set { _DAO.ChangedByApp = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketRateHistory_DAO.MarketRate
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
	}
}



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
	/// SAHL.Common.BusinessModel.DAO.MarketRate_DAO
	/// </summary>
	public partial class MarketRate : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MarketRate_DAO>, IMarketRate
	{
				public MarketRate(SAHL.Common.BusinessModel.DAO.MarketRate_DAO MarketRate) : base(MarketRate)
		{
			this._DAO = MarketRate;
		}
		/// <summary>
		/// The current value of this marketrate.  This is always the current marketrate, irrespective of resets.
		/// </summary>
		public Double Value 
		{
			get { return _DAO.Value; }
			set { _DAO.Value = value;}
		}
		/// <summary>
		/// The description of this marketrate.
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// This is the primary key, used to identify an instance of Marketrate.
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// A list that contains the history of this marketrate's changes through time.
		/// </summary>
		private DAOEventList<MarketRateHistory_DAO, IMarketRateHistory, MarketRateHistory> _MarketRateHistories;
		/// <summary>
		/// A list that contains the history of this marketrate's changes through time.
		/// </summary>
		public IEventList<IMarketRateHistory> MarketRateHistories
		{
			get
			{
				if (null == _MarketRateHistories) 
				{
					if(null == _DAO.MarketRateHistories)
						_DAO.MarketRateHistories = new List<MarketRateHistory_DAO>();
					_MarketRateHistories = new DAOEventList<MarketRateHistory_DAO, IMarketRateHistory, MarketRateHistory>(_DAO.MarketRateHistories);
					_MarketRateHistories.BeforeAdd += new EventListHandler(OnMarketRateHistories_BeforeAdd);					
					_MarketRateHistories.BeforeRemove += new EventListHandler(OnMarketRateHistories_BeforeRemove);					
					_MarketRateHistories.AfterAdd += new EventListHandler(OnMarketRateHistories_AfterAdd);					
					_MarketRateHistories.AfterRemove += new EventListHandler(OnMarketRateHistories_AfterRemove);					
				}
				return _MarketRateHistories;
			}
		}
		/// <summary>
		/// A collection of all the OriginationSourceProductConfigurtation entries that use this Marketrate.
		/// </summary>
		private DAOEventList<RateConfiguration_DAO, IRateConfiguration, RateConfiguration> _RateConfigurations;
		/// <summary>
		/// A collection of all the OriginationSourceProductConfigurtation entries that use this Marketrate.
		/// </summary>
		public IEventList<IRateConfiguration> RateConfigurations
		{
			get
			{
				if (null == _RateConfigurations) 
				{
					if(null == _DAO.RateConfigurations)
						_DAO.RateConfigurations = new List<RateConfiguration_DAO>();
					_RateConfigurations = new DAOEventList<RateConfiguration_DAO, IRateConfiguration, RateConfiguration>(_DAO.RateConfigurations);
					_RateConfigurations.BeforeAdd += new EventListHandler(OnRateConfigurations_BeforeAdd);					
					_RateConfigurations.BeforeRemove += new EventListHandler(OnRateConfigurations_BeforeRemove);					
					_RateConfigurations.AfterAdd += new EventListHandler(OnRateConfigurations_AfterAdd);					
					_RateConfigurations.AfterRemove += new EventListHandler(OnRateConfigurations_AfterRemove);					
				}
				return _RateConfigurations;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_MarketRateHistories = null;
			_RateConfigurations = null;
			
		}
	}
}



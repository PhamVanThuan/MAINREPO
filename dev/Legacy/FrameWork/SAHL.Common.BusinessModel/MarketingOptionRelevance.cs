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
	/// SAHL.Common.BusinessModel.DAO.MarketingOptionRelevance_DAO
	/// </summary>
	public partial class MarketingOptionRelevance : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MarketingOptionRelevance_DAO>, IMarketingOptionRelevance
	{
				public MarketingOptionRelevance(SAHL.Common.BusinessModel.DAO.MarketingOptionRelevance_DAO MarketingOptionRelevance) : base(MarketingOptionRelevance)
		{
			this._DAO = MarketingOptionRelevance;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketingOptionRelevance_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketingOptionRelevance_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketingOptionRelevance_DAO.CampaignDefinitions
		/// </summary>
		private DAOEventList<CampaignDefinition_DAO, ICampaignDefinition, CampaignDefinition> _CampaignDefinitions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MarketingOptionRelevance_DAO.CampaignDefinitions
		/// </summary>
		public IEventList<ICampaignDefinition> CampaignDefinitions
		{
			get
			{
				if (null == _CampaignDefinitions) 
				{
					if(null == _DAO.CampaignDefinitions)
						_DAO.CampaignDefinitions = new List<CampaignDefinition_DAO>();
					_CampaignDefinitions = new DAOEventList<CampaignDefinition_DAO, ICampaignDefinition, CampaignDefinition>(_DAO.CampaignDefinitions);
					_CampaignDefinitions.BeforeAdd += new EventListHandler(OnCampaignDefinitions_BeforeAdd);					
					_CampaignDefinitions.BeforeRemove += new EventListHandler(OnCampaignDefinitions_BeforeRemove);					
					_CampaignDefinitions.AfterAdd += new EventListHandler(OnCampaignDefinitions_AfterAdd);					
					_CampaignDefinitions.AfterRemove += new EventListHandler(OnCampaignDefinitions_AfterRemove);					
				}
				return _CampaignDefinitions;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_CampaignDefinitions = null;
			
		}
	}
}



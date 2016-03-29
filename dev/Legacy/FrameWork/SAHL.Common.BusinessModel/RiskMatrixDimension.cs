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
	/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO
	/// </summary>
	public partial class RiskMatrixDimension : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO>, IRiskMatrixDimension
	{
				public RiskMatrixDimension(SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO RiskMatrixDimension) : base(RiskMatrixDimension)
		{
			this._DAO = RiskMatrixDimension;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO.ScoreCards
		/// </summary>
		private DAOEventList<ScoreCard_DAO, IScoreCard, ScoreCard> _ScoreCards;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO.ScoreCards
		/// </summary>
		public IEventList<IScoreCard> ScoreCards
		{
			get
			{
				if (null == _ScoreCards) 
				{
					if(null == _DAO.ScoreCards)
						_DAO.ScoreCards = new List<ScoreCard_DAO>();
					_ScoreCards = new DAOEventList<ScoreCard_DAO, IScoreCard, ScoreCard>(_DAO.ScoreCards);
					_ScoreCards.BeforeAdd += new EventListHandler(OnScoreCards_BeforeAdd);					
					_ScoreCards.BeforeRemove += new EventListHandler(OnScoreCards_BeforeRemove);					
					_ScoreCards.AfterAdd += new EventListHandler(OnScoreCards_AfterAdd);					
					_ScoreCards.AfterRemove += new EventListHandler(OnScoreCards_AfterRemove);					
				}
				return _ScoreCards;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO.RiskMatrixRanges
		/// </summary>
		private DAOEventList<RiskMatrixRange_DAO, IRiskMatrixRange, RiskMatrixRange> _RiskMatrixRanges;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimension_DAO.RiskMatrixRanges
		/// </summary>
		public IEventList<IRiskMatrixRange> RiskMatrixRanges
		{
			get
			{
				if (null == _RiskMatrixRanges) 
				{
					if(null == _DAO.RiskMatrixRanges)
						_DAO.RiskMatrixRanges = new List<RiskMatrixRange_DAO>();
					_RiskMatrixRanges = new DAOEventList<RiskMatrixRange_DAO, IRiskMatrixRange, RiskMatrixRange>(_DAO.RiskMatrixRanges);
					_RiskMatrixRanges.BeforeAdd += new EventListHandler(OnRiskMatrixRanges_BeforeAdd);					
					_RiskMatrixRanges.BeforeRemove += new EventListHandler(OnRiskMatrixRanges_BeforeRemove);					
					_RiskMatrixRanges.AfterAdd += new EventListHandler(OnRiskMatrixRanges_AfterAdd);					
					_RiskMatrixRanges.AfterRemove += new EventListHandler(OnRiskMatrixRanges_AfterRemove);					
				}
				return _RiskMatrixRanges;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ScoreCards = null;
			_RiskMatrixRanges = null;
			
		}
	}
}



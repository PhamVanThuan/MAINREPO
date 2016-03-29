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
	/// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO
	/// </summary>
	public partial class ScoreCard : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ScoreCard_DAO>, IScoreCard
	{
				public ScoreCard(SAHL.Common.BusinessModel.DAO.ScoreCard_DAO ScoreCard) : base(ScoreCard)
		{
			this._DAO = ScoreCard;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.BasePoints
		/// </summary>
		public Double BasePoints 
		{
			get { return _DAO.BasePoints; }
			set { _DAO.BasePoints = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.RevisionDate
		/// </summary>
		public DateTime RevisionDate 
		{
			get { return _DAO.RevisionDate; }
			set { _DAO.RevisionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.ScoreCardAttributes
		/// </summary>
		private DAOEventList<ScoreCardAttribute_DAO, IScoreCardAttribute, ScoreCardAttribute> _ScoreCardAttributes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.ScoreCardAttributes
		/// </summary>
		public IEventList<IScoreCardAttribute> ScoreCardAttributes
		{
			get
			{
				if (null == _ScoreCardAttributes) 
				{
					if(null == _DAO.ScoreCardAttributes)
						_DAO.ScoreCardAttributes = new List<ScoreCardAttribute_DAO>();
					_ScoreCardAttributes = new DAOEventList<ScoreCardAttribute_DAO, IScoreCardAttribute, ScoreCardAttribute>(_DAO.ScoreCardAttributes);
					_ScoreCardAttributes.BeforeAdd += new EventListHandler(OnScoreCardAttributes_BeforeAdd);					
					_ScoreCardAttributes.BeforeRemove += new EventListHandler(OnScoreCardAttributes_BeforeRemove);					
					_ScoreCardAttributes.AfterAdd += new EventListHandler(OnScoreCardAttributes_AfterAdd);					
					_ScoreCardAttributes.AfterRemove += new EventListHandler(OnScoreCardAttributes_AfterRemove);					
				}
				return _ScoreCardAttributes;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.RiskMatrixDimensions
		/// </summary>
		private DAOEventList<RiskMatrixDimension_DAO, IRiskMatrixDimension, RiskMatrixDimension> _RiskMatrixDimensions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.RiskMatrixDimensions
		/// </summary>
		public IEventList<IRiskMatrixDimension> RiskMatrixDimensions
		{
			get
			{
				if (null == _RiskMatrixDimensions) 
				{
					if(null == _DAO.RiskMatrixDimensions)
						_DAO.RiskMatrixDimensions = new List<RiskMatrixDimension_DAO>();
					_RiskMatrixDimensions = new DAOEventList<RiskMatrixDimension_DAO, IRiskMatrixDimension, RiskMatrixDimension>(_DAO.RiskMatrixDimensions);
					_RiskMatrixDimensions.BeforeAdd += new EventListHandler(OnRiskMatrixDimensions_BeforeAdd);					
					_RiskMatrixDimensions.BeforeRemove += new EventListHandler(OnRiskMatrixDimensions_BeforeRemove);					
					_RiskMatrixDimensions.AfterAdd += new EventListHandler(OnRiskMatrixDimensions_AfterAdd);					
					_RiskMatrixDimensions.AfterRemove += new EventListHandler(OnRiskMatrixDimensions_AfterRemove);					
				}
				return _RiskMatrixDimensions;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCard_DAO.GeneralStatus
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ScoreCardAttributes = null;
			_RiskMatrixDimensions = null;
			
		}
	}
}



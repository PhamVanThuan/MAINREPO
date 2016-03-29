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
	/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimensionScoreCard_DAO
	/// </summary>
	public partial class RiskMatrixDimensionScoreCard : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RiskMatrixDimensionScoreCard_DAO>, IRiskMatrixDimensionScoreCard
	{
				public RiskMatrixDimensionScoreCard(SAHL.Common.BusinessModel.DAO.RiskMatrixDimensionScoreCard_DAO RiskMatrixDimensionScoreCard) : base(RiskMatrixDimensionScoreCard)
		{
			this._DAO = RiskMatrixDimensionScoreCard;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimensionScoreCard_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimensionScoreCard_DAO.RiskMatrixDimension
		/// </summary>
		public IRiskMatrixDimension RiskMatrixDimension 
		{
			get
			{
				if (null == _DAO.RiskMatrixDimension) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRiskMatrixDimension, RiskMatrixDimension_DAO>(_DAO.RiskMatrixDimension);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RiskMatrixDimension = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RiskMatrixDimension = (RiskMatrixDimension_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixDimensionScoreCard_DAO.ScoreCard
		/// </summary>
		public IScoreCard ScoreCard 
		{
			get
			{
				if (null == _DAO.ScoreCard) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IScoreCard, ScoreCard_DAO>(_DAO.ScoreCard);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ScoreCard = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ScoreCard = (ScoreCard_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



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
	/// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO
	/// </summary>
	public partial class ScoreCardAttributeRange : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO>, IScoreCardAttributeRange
	{
				public ScoreCardAttributeRange(SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO ScoreCardAttributeRange) : base(ScoreCardAttributeRange)
		{
			this._DAO = ScoreCardAttributeRange;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO.Min
		/// </summary>
		public Double? Min
		{
			get { return _DAO.Min; }
			set { _DAO.Min = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO.Max
		/// </summary>
		public Double? Max
		{
			get { return _DAO.Max; }
			set { _DAO.Max = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO.Points
		/// </summary>
		public Double Points 
		{
			get { return _DAO.Points; }
			set { _DAO.Points = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttributeRange_DAO.ScoreCardAttribute
		/// </summary>
		public IScoreCardAttribute ScoreCardAttribute 
		{
			get
			{
				if (null == _DAO.ScoreCardAttribute) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IScoreCardAttribute, ScoreCardAttribute_DAO>(_DAO.ScoreCardAttribute);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ScoreCardAttribute = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ScoreCardAttribute = (ScoreCardAttribute_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



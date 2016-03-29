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
	/// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO
	/// </summary>
	public partial class ScoreCardAttribute : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO>, IScoreCardAttribute
	{
				public ScoreCardAttribute(SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO ScoreCardAttribute) : base(ScoreCardAttribute)
		{
			this._DAO = ScoreCardAttribute;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.ScoreCardKey
		/// </summary>
		public IScoreCard ScoreCardKey 
		{
			get
			{
				if (null == _DAO.ScoreCardKey) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IScoreCard, ScoreCard_DAO>(_DAO.ScoreCardKey);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ScoreCardKey = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ScoreCardKey = (ScoreCard_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.Code
		/// </summary>
		public String Code 
		{
			get { return _DAO.Code; }
			set { _DAO.Code = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.ScoreCardAttributeRanges
		/// </summary>
		private DAOEventList<ScoreCardAttributeRange_DAO, IScoreCardAttributeRange, ScoreCardAttributeRange> _ScoreCardAttributeRanges;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ScoreCardAttribute_DAO.ScoreCardAttributeRanges
		/// </summary>
		public IEventList<IScoreCardAttributeRange> ScoreCardAttributeRanges
		{
			get
			{
				if (null == _ScoreCardAttributeRanges) 
				{
					if(null == _DAO.ScoreCardAttributeRanges)
						_DAO.ScoreCardAttributeRanges = new List<ScoreCardAttributeRange_DAO>();
					_ScoreCardAttributeRanges = new DAOEventList<ScoreCardAttributeRange_DAO, IScoreCardAttributeRange, ScoreCardAttributeRange>(_DAO.ScoreCardAttributeRanges);
					_ScoreCardAttributeRanges.BeforeAdd += new EventListHandler(OnScoreCardAttributeRanges_BeforeAdd);					
					_ScoreCardAttributeRanges.BeforeRemove += new EventListHandler(OnScoreCardAttributeRanges_BeforeRemove);					
					_ScoreCardAttributeRanges.AfterAdd += new EventListHandler(OnScoreCardAttributeRanges_AfterAdd);					
					_ScoreCardAttributeRanges.AfterRemove += new EventListHandler(OnScoreCardAttributeRanges_AfterRemove);					
				}
				return _ScoreCardAttributeRanges;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ScoreCardAttributeRanges = null;
			
		}
	}
}



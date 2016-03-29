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
	/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO
	/// </summary>
	public partial class ITCCreditScore : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO>, IITCCreditScore
	{
				public ITCCreditScore(SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO ITCCreditScore) : base(ITCCreditScore)
		{
			this._DAO = ITCCreditScore;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.EmpiricaScore
		/// </summary>
		public Double? EmpiricaScore
		{
			get { return _DAO.EmpiricaScore; }
			set { _DAO.EmpiricaScore = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.SBCScore
		/// </summary>
		public Double? SBCScore
		{
			get { return _DAO.SBCScore; }
			set { _DAO.SBCScore = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ADUserName
		/// </summary>
		public String ADUserName 
		{
			get { return _DAO.ADUserName; }
			set { _DAO.ADUserName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ScoreDate
		/// </summary>
		public DateTime ScoreDate 
		{
			get { return _DAO.ScoreDate; }
			set { _DAO.ScoreDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ITCDecisionReasons
		/// </summary>
		private DAOEventList<ITCDecisionReason_DAO, IITCDecisionReason, ITCDecisionReason> _ITCDecisionReasons;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ITCDecisionReasons
		/// </summary>
		public IEventList<IITCDecisionReason> ITCDecisionReasons
		{
			get
			{
				if (null == _ITCDecisionReasons) 
				{
					if(null == _DAO.ITCDecisionReasons)
						_DAO.ITCDecisionReasons = new List<ITCDecisionReason_DAO>();
					_ITCDecisionReasons = new DAOEventList<ITCDecisionReason_DAO, IITCDecisionReason, ITCDecisionReason>(_DAO.ITCDecisionReasons);
					_ITCDecisionReasons.BeforeAdd += new EventListHandler(OnITCDecisionReasons_BeforeAdd);					
					_ITCDecisionReasons.BeforeRemove += new EventListHandler(OnITCDecisionReasons_BeforeRemove);					
					_ITCDecisionReasons.AfterAdd += new EventListHandler(OnITCDecisionReasons_AfterAdd);					
					_ITCDecisionReasons.AfterRemove += new EventListHandler(OnITCDecisionReasons_AfterRemove);					
				}
				return _ITCDecisionReasons;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.CreditScoreDecision
		/// </summary>
		public ICreditScoreDecision CreditScoreDecision 
		{
			get
			{
				if (null == _DAO.CreditScoreDecision) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICreditScoreDecision, CreditScoreDecision_DAO>(_DAO.CreditScoreDecision);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.CreditScoreDecision = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.CreditScoreDecision = (CreditScoreDecision_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.GeneralStatus
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.RiskMatrixRevision
		/// </summary>
		public IRiskMatrixRevision RiskMatrixRevision 
		{
			get
			{
				if (null == _DAO.RiskMatrixRevision) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRiskMatrixRevision, RiskMatrixRevision_DAO>(_DAO.RiskMatrixRevision);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RiskMatrixRevision = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RiskMatrixRevision = (RiskMatrixRevision_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.RiskMatrixCell
		/// </summary>
		public IRiskMatrixCell RiskMatrixCell 
		{
			get
			{
				if (null == _DAO.RiskMatrixCell) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRiskMatrixCell, RiskMatrixCell_DAO>(_DAO.RiskMatrixCell);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RiskMatrixCell = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RiskMatrixCell = (RiskMatrixCell_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ITCKey
		/// </summary>
		public Int32 ITCKey 
		{
			get { return _DAO.ITCKey; }
			set { _DAO.ITCKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.ScoreCard
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCCreditScore_DAO.LegalEntity
		/// </summary>
		public ILegalEntity LegalEntity 
		{
			get
			{
				if (null == _DAO.LegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ITCDecisionReasons = null;
			
		}
	}
}



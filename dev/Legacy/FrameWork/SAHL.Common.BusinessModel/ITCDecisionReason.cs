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
	/// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO
	/// </summary>
	public partial class ITCDecisionReason : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO>, IITCDecisionReason
	{
				public ITCDecisionReason(SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO ITCDecisionReason) : base(ITCDecisionReason)
		{
			this._DAO = ITCDecisionReason;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO.CreditScoreDecision
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
		/// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO.ITCCreditScore
		/// </summary>
		public IITCCreditScore ITCCreditScore 
		{
			get
			{
				if (null == _DAO.ITCCreditScore) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IITCCreditScore, ITCCreditScore_DAO>(_DAO.ITCCreditScore);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ITCCreditScore = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ITCCreditScore = (ITCCreditScore_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO.ApplicationCreditScore
		/// </summary>
		public IApplicationCreditScore ApplicationCreditScore 
		{
			get
			{
				if (null == _DAO.ApplicationCreditScore) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationCreditScore, ApplicationCreditScore_DAO>(_DAO.ApplicationCreditScore);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationCreditScore = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicationCreditScore = (ApplicationCreditScore_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCDecisionReason_DAO.Reason
		/// </summary>
		public IReason Reason 
		{
			get
			{
				if (null == _DAO.Reason) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IReason, Reason_DAO>(_DAO.Reason);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Reason = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Reason = (Reason_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



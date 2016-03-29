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
	/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO
	/// </summary>
	public partial class RiskMatrixCell : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO>, IRiskMatrixCell
	{
				public RiskMatrixCell(SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO RiskMatrixCell) : base(RiskMatrixCell)
		{
			this._DAO = RiskMatrixCell;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.RiskMatrixDimensions
		/// </summary>
		private DAOEventList<RiskMatrixDimension_DAO, IRiskMatrixDimension, RiskMatrixDimension> _RiskMatrixDimensions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.RiskMatrixDimensions
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
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.RiskMatrixRanges
		/// </summary>
		private DAOEventList<RiskMatrixRange_DAO, IRiskMatrixRange, RiskMatrixRange> _RiskMatrixRanges;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.RiskMatrixRanges
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.CreditScoreDecision
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
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.RiskMatrixRevision
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
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.RuleItem
		/// </summary>
		public IRuleItem RuleItem 
		{
			get
			{
				if (null == _DAO.RuleItem) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRuleItem, RuleItem_DAO>(_DAO.RuleItem);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RuleItem = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RuleItem = (RuleItem_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCell_DAO.Designation
		/// </summary>
		public String Designation 
		{
			get { return _DAO.Designation; }
			set { _DAO.Designation = value;}
		}
		public override void Refresh()
		{
			base.Refresh();
			_RiskMatrixDimensions = null;
			_RiskMatrixRanges = null;
			
		}
	}
}



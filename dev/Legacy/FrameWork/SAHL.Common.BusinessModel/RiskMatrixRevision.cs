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
	/// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO
	/// </summary>
	public partial class RiskMatrixRevision : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO>, IRiskMatrixRevision
	{
				public RiskMatrixRevision(SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO RiskMatrixRevision) : base(RiskMatrixRevision)
		{
			this._DAO = RiskMatrixRevision;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.RevisionDate
		/// </summary>
		public DateTime RevisionDate 
		{
			get { return _DAO.RevisionDate; }
			set { _DAO.RevisionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.RiskMatrixCells
		/// </summary>
		private DAOEventList<RiskMatrixCell_DAO, IRiskMatrixCell, RiskMatrixCell> _RiskMatrixCells;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.RiskMatrixCells
		/// </summary>
		public IEventList<IRiskMatrixCell> RiskMatrixCells
		{
			get
			{
				if (null == _RiskMatrixCells) 
				{
					if(null == _DAO.RiskMatrixCells)
						_DAO.RiskMatrixCells = new List<RiskMatrixCell_DAO>();
					_RiskMatrixCells = new DAOEventList<RiskMatrixCell_DAO, IRiskMatrixCell, RiskMatrixCell>(_DAO.RiskMatrixCells);
					_RiskMatrixCells.BeforeAdd += new EventListHandler(OnRiskMatrixCells_BeforeAdd);					
					_RiskMatrixCells.BeforeRemove += new EventListHandler(OnRiskMatrixCells_BeforeRemove);					
					_RiskMatrixCells.AfterAdd += new EventListHandler(OnRiskMatrixCells_AfterAdd);					
					_RiskMatrixCells.AfterRemove += new EventListHandler(OnRiskMatrixCells_AfterRemove);					
				}
				return _RiskMatrixCells;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixRevision_DAO.RiskMatrix
		/// </summary>
		public IRiskMatrix RiskMatrix 
		{
			get
			{
				if (null == _DAO.RiskMatrix) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRiskMatrix, RiskMatrix_DAO>(_DAO.RiskMatrix);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RiskMatrix = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RiskMatrix = (RiskMatrix_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_RiskMatrixCells = null;
			
		}
	}
}



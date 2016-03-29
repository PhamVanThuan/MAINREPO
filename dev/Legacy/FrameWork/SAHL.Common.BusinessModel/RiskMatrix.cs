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
	/// SAHL.Common.BusinessModel.DAO.RiskMatrix_DAO
	/// </summary>
	public partial class RiskMatrix : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RiskMatrix_DAO>, IRiskMatrix
	{
				public RiskMatrix(SAHL.Common.BusinessModel.DAO.RiskMatrix_DAO RiskMatrix) : base(RiskMatrix)
		{
			this._DAO = RiskMatrix;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrix_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrix_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrix_DAO.RiskMatrixRevisions
		/// </summary>
		private DAOEventList<RiskMatrixRevision_DAO, IRiskMatrixRevision, RiskMatrixRevision> _RiskMatrixRevisions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrix_DAO.RiskMatrixRevisions
		/// </summary>
		public IEventList<IRiskMatrixRevision> RiskMatrixRevisions
		{
			get
			{
				if (null == _RiskMatrixRevisions) 
				{
					if(null == _DAO.RiskMatrixRevisions)
						_DAO.RiskMatrixRevisions = new List<RiskMatrixRevision_DAO>();
					_RiskMatrixRevisions = new DAOEventList<RiskMatrixRevision_DAO, IRiskMatrixRevision, RiskMatrixRevision>(_DAO.RiskMatrixRevisions);
					_RiskMatrixRevisions.BeforeAdd += new EventListHandler(OnRiskMatrixRevisions_BeforeAdd);					
					_RiskMatrixRevisions.BeforeRemove += new EventListHandler(OnRiskMatrixRevisions_BeforeRemove);					
					_RiskMatrixRevisions.AfterAdd += new EventListHandler(OnRiskMatrixRevisions_AfterAdd);					
					_RiskMatrixRevisions.AfterRemove += new EventListHandler(OnRiskMatrixRevisions_AfterRemove);					
				}
				return _RiskMatrixRevisions;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_RiskMatrixRevisions = null;
			
		}
	}
}



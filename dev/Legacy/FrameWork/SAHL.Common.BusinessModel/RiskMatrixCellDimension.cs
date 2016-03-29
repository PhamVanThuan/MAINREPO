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
	/// SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO
	/// </summary>
	public partial class RiskMatrixCellDimension : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO>, IRiskMatrixCellDimension
	{
				public RiskMatrixCellDimension(SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO RiskMatrixCellDimension) : base(RiskMatrixCellDimension)
		{
			this._DAO = RiskMatrixCellDimension;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO.RiskMatrixCell
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
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO.RiskMatrixDimension
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
		/// SAHL.Common.BusinessModel.DAO.RiskMatrixCellDimension_DAO.RiskMatrixRange
		/// </summary>
		public IRiskMatrixRange RiskMatrixRange 
		{
			get
			{
				if (null == _DAO.RiskMatrixRange) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRiskMatrixRange, RiskMatrixRange_DAO>(_DAO.RiskMatrixRange);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RiskMatrixRange = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RiskMatrixRange = (RiskMatrixRange_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



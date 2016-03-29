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
	/// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentTypeSource_DAO
	/// </summary>
	public partial class FinancialAdjustmentTypeSource : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialAdjustmentTypeSource_DAO>, IFinancialAdjustmentTypeSource
	{
				public FinancialAdjustmentTypeSource(SAHL.Common.BusinessModel.DAO.FinancialAdjustmentTypeSource_DAO FinancialAdjustmentTypeSource) : base(FinancialAdjustmentTypeSource)
		{
			this._DAO = FinancialAdjustmentTypeSource;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentTypeSource_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentTypeSource_DAO.FinancialAdjustmentSource
		/// </summary>
		public IFinancialAdjustmentSource FinancialAdjustmentSource 
		{
			get
			{
				if (null == _DAO.FinancialAdjustmentSource) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialAdjustmentSource, FinancialAdjustmentSource_DAO>(_DAO.FinancialAdjustmentSource);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialAdjustmentSource = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialAdjustmentSource = (FinancialAdjustmentSource_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialAdjustmentTypeSource_DAO.FinancialAdjustmentType
		/// </summary>
		public IFinancialAdjustmentType FinancialAdjustmentType 
		{
			get
			{
				if (null == _DAO.FinancialAdjustmentType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialAdjustmentType, FinancialAdjustmentType_DAO>(_DAO.FinancialAdjustmentType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialAdjustmentType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialAdjustmentType = (FinancialAdjustmentType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



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
	/// SAHL.Common.BusinessModel.DAO.OSPRateOverrideType_DAO
	/// </summary>
	public partial class OSPRateOverrideType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.OSPRateOverrideType_DAO>, IOSPRateOverrideType
	{
				public OSPRateOverrideType(SAHL.Common.BusinessModel.DAO.OSPRateOverrideType_DAO OSPRateOverrideType) : base(OSPRateOverrideType)
		{
			this._DAO = OSPRateOverrideType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OSPRateOverrideType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OSPRateOverrideType_DAO.OriginationSourceProduct
		/// </summary>
		public IOriginationSourceProduct OriginationSourceProduct 
		{
			get
			{
				if (null == _DAO.OriginationSourceProduct) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOriginationSourceProduct, OriginationSourceProduct_DAO>(_DAO.OriginationSourceProduct);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OriginationSourceProduct = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OriginationSourceProduct = (OriginationSourceProduct_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OSPRateOverrideType_DAO.FinancialAdjustmentTypeSource
		/// </summary>
		public IFinancialAdjustmentTypeSource FinancialAdjustmentTypeSource 
		{
			get
			{
				if (null == _DAO.FinancialAdjustmentTypeSource) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialAdjustmentTypeSource, FinancialAdjustmentTypeSource_DAO>(_DAO.FinancialAdjustmentTypeSource);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialAdjustmentTypeSource = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialAdjustmentTypeSource = (FinancialAdjustmentTypeSource_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



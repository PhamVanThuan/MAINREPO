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
	/// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO
	/// </summary>
	public partial class ProductCondition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ProductCondition_DAO>, IProductCondition
	{
				public ProductCondition(SAHL.Common.BusinessModel.DAO.ProductCondition_DAO ProductCondition) : base(ProductCondition)
		{
			this._DAO = ProductCondition;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.PurposeKey
		/// </summary>
		public Int32 PurposeKey 
		{
			get { return _DAO.PurposeKey; }
			set { _DAO.PurposeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.ApplicationName
		/// </summary>
		public String ApplicationName 
		{
			get { return _DAO.ApplicationName; }
			set { _DAO.ApplicationName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.Condition
		/// </summary>
		public ICondition Condition 
		{
			get
			{
				if (null == _DAO.Condition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICondition, Condition_DAO>(_DAO.Condition);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Condition = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Condition = (Condition_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.FinancialServiceType
		/// </summary>
		public IFinancialServiceType FinancialServiceType 
		{
			get
			{
				if (null == _DAO.FinancialServiceType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialServiceType, FinancialServiceType_DAO>(_DAO.FinancialServiceType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialServiceType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialServiceType = (FinancialServiceType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.OriginationSourceProduct
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
		/// SAHL.Common.BusinessModel.DAO.ProductCondition_DAO.ProductConditionType
		/// </summary>
		public IProductConditionType ProductConditionType 
		{
			get
			{
				if (null == _DAO.ProductConditionType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProductConditionType, ProductConditionType_DAO>(_DAO.ProductConditionType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ProductConditionType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ProductConditionType = (ProductConditionType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



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
	/// This class links a OriginationSourceProduct (OSP) for a given Category to a margin.
	/// </summary>
	public partial class ProductCategory : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ProductCategory_DAO>, IProductCategory
	{
				public ProductCategory(SAHL.Common.BusinessModel.DAO.ProductCategory_DAO ProductCategory) : base(ProductCategory)
		{
			this._DAO = ProductCategory;
		}
		/// <summary>
		/// This is the primary key, used to identify an instance of ProductCategory.
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// The category the OSP is linked to.
		/// </summary>
		public ICategory Category 
		{
			get
			{
				if (null == _DAO.Category) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICategory, Category_DAO>(_DAO.Category);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Category = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Category = (Category_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The margin associated with this OSP for given Category.
		/// </summary>
		public IMargin Margin 
		{
			get
			{
				if (null == _DAO.Margin) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMargin, Margin_DAO>(_DAO.Margin);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Margin = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Margin = (Margin_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The OSP.
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
	}
}



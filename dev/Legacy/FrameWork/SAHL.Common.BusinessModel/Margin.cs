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
	/// SAHL.Common.BusinessModel.DAO.Margin_DAO
	/// </summary>
	public partial class Margin : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Margin_DAO>, IMargin
	{
				public Margin(SAHL.Common.BusinessModel.DAO.Margin_DAO Margin) : base(Margin)
		{
			this._DAO = Margin;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Margin_DAO.Value
		/// </summary>
		public Double Value 
		{
			get { return _DAO.Value; }
			set { _DAO.Value = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Margin_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Margin_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Margin_DAO.MarginProducts
		/// </summary>
		private DAOEventList<MarginProduct_DAO, IMarginProduct, MarginProduct> _MarginProducts;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Margin_DAO.MarginProducts
		/// </summary>
		public IEventList<IMarginProduct> MarginProducts
		{
			get
			{
				if (null == _MarginProducts) 
				{
					if(null == _DAO.MarginProducts)
						_DAO.MarginProducts = new List<MarginProduct_DAO>();
					_MarginProducts = new DAOEventList<MarginProduct_DAO, IMarginProduct, MarginProduct>(_DAO.MarginProducts);
					_MarginProducts.BeforeAdd += new EventListHandler(OnMarginProducts_BeforeAdd);					
					_MarginProducts.BeforeRemove += new EventListHandler(OnMarginProducts_BeforeRemove);					
					_MarginProducts.AfterAdd += new EventListHandler(OnMarginProducts_AfterAdd);					
					_MarginProducts.AfterRemove += new EventListHandler(OnMarginProducts_AfterRemove);					
				}
				return _MarginProducts;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Margin_DAO.ProductCategories
		/// </summary>
		private DAOEventList<ProductCategory_DAO, IProductCategory, ProductCategory> _ProductCategories;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Margin_DAO.ProductCategories
		/// </summary>
		public IEventList<IProductCategory> ProductCategories
		{
			get
			{
				if (null == _ProductCategories) 
				{
					if(null == _DAO.ProductCategories)
						_DAO.ProductCategories = new List<ProductCategory_DAO>();
					_ProductCategories = new DAOEventList<ProductCategory_DAO, IProductCategory, ProductCategory>(_DAO.ProductCategories);
					_ProductCategories.BeforeAdd += new EventListHandler(OnProductCategories_BeforeAdd);					
					_ProductCategories.BeforeRemove += new EventListHandler(OnProductCategories_BeforeRemove);					
					_ProductCategories.AfterAdd += new EventListHandler(OnProductCategories_AfterAdd);					
					_ProductCategories.AfterRemove += new EventListHandler(OnProductCategories_AfterRemove);					
				}
				return _ProductCategories;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Margin_DAO.RateConfigurations
		/// </summary>
		private DAOEventList<RateConfiguration_DAO, IRateConfiguration, RateConfiguration> _RateConfigurations;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Margin_DAO.RateConfigurations
		/// </summary>
		public IEventList<IRateConfiguration> RateConfigurations
		{
			get
			{
				if (null == _RateConfigurations) 
				{
					if(null == _DAO.RateConfigurations)
						_DAO.RateConfigurations = new List<RateConfiguration_DAO>();
					_RateConfigurations = new DAOEventList<RateConfiguration_DAO, IRateConfiguration, RateConfiguration>(_DAO.RateConfigurations);
					_RateConfigurations.BeforeAdd += new EventListHandler(OnRateConfigurations_BeforeAdd);					
					_RateConfigurations.BeforeRemove += new EventListHandler(OnRateConfigurations_BeforeRemove);					
					_RateConfigurations.AfterAdd += new EventListHandler(OnRateConfigurations_AfterAdd);					
					_RateConfigurations.AfterRemove += new EventListHandler(OnRateConfigurations_AfterRemove);					
				}
				return _RateConfigurations;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_MarginProducts = null;
			_ProductCategories = null;
			_RateConfigurations = null;
			
		}
	}
}



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
	/// SAHL.Common.BusinessModel.DAO.ProductConditionType_DAO
	/// </summary>
	public partial class ProductConditionType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ProductConditionType_DAO>, IProductConditionType
	{
				public ProductConditionType(SAHL.Common.BusinessModel.DAO.ProductConditionType_DAO ProductConditionType) : base(ProductConditionType)
		{
			this._DAO = ProductConditionType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProductConditionType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProductConditionType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProductConditionType_DAO.ProductConditions
		/// </summary>
		private DAOEventList<ProductCondition_DAO, IProductCondition, ProductCondition> _ProductConditions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ProductConditionType_DAO.ProductConditions
		/// </summary>
		public IEventList<IProductCondition> ProductConditions
		{
			get
			{
				if (null == _ProductConditions) 
				{
					if(null == _DAO.ProductConditions)
						_DAO.ProductConditions = new List<ProductCondition_DAO>();
					_ProductConditions = new DAOEventList<ProductCondition_DAO, IProductCondition, ProductCondition>(_DAO.ProductConditions);
					_ProductConditions.BeforeAdd += new EventListHandler(OnProductConditions_BeforeAdd);					
					_ProductConditions.BeforeRemove += new EventListHandler(OnProductConditions_BeforeRemove);					
					_ProductConditions.AfterAdd += new EventListHandler(OnProductConditions_AfterAdd);					
					_ProductConditions.AfterRemove += new EventListHandler(OnProductConditions_AfterRemove);					
				}
				return _ProductConditions;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ProductConditions = null;
			
		}
	}
}



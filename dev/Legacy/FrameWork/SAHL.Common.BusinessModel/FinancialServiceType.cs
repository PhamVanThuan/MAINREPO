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
	/// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO
	/// </summary>
	public partial class FinancialServiceType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO>, IFinancialServiceType
	{
				public FinancialServiceType(SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO FinancialServiceType) : base(FinancialServiceType)
		{
			this._DAO = FinancialServiceType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.FinancialServiceGroup
		/// </summary>
		public IFinancialServiceGroup FinancialServiceGroup 
		{
			get
			{
				if (null == _DAO.FinancialServiceGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialServiceGroup, FinancialServiceGroup_DAO>(_DAO.FinancialServiceGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialServiceGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialServiceGroup = (FinancialServiceGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.OriginationSourceProductConfigurations
		/// </summary>
		private DAOEventList<OriginationSourceProductConfiguration_DAO, IOriginationSourceProductConfiguration, OriginationSourceProductConfiguration> _OriginationSourceProductConfigurations;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.OriginationSourceProductConfigurations
		/// </summary>
		public IEventList<IOriginationSourceProductConfiguration> OriginationSourceProductConfigurations
		{
			get
			{
				if (null == _OriginationSourceProductConfigurations) 
				{
					if(null == _DAO.OriginationSourceProductConfigurations)
						_DAO.OriginationSourceProductConfigurations = new List<OriginationSourceProductConfiguration_DAO>();
					_OriginationSourceProductConfigurations = new DAOEventList<OriginationSourceProductConfiguration_DAO, IOriginationSourceProductConfiguration, OriginationSourceProductConfiguration>(_DAO.OriginationSourceProductConfigurations);
					_OriginationSourceProductConfigurations.BeforeAdd += new EventListHandler(OnOriginationSourceProductConfigurations_BeforeAdd);					
					_OriginationSourceProductConfigurations.BeforeRemove += new EventListHandler(OnOriginationSourceProductConfigurations_BeforeRemove);					
					_OriginationSourceProductConfigurations.AfterAdd += new EventListHandler(OnOriginationSourceProductConfigurations_AfterAdd);					
					_OriginationSourceProductConfigurations.AfterRemove += new EventListHandler(OnOriginationSourceProductConfigurations_AfterRemove);					
				}
				return _OriginationSourceProductConfigurations;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.ProductConditions
		/// </summary>
		private DAOEventList<ProductCondition_DAO, IProductCondition, ProductCondition> _ProductConditions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.ProductConditions
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceType_DAO.ResetConfiguration
		/// </summary>
		public IResetConfiguration ResetConfiguration 
		{
			get
			{
				if (null == _DAO.ResetConfiguration) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IResetConfiguration, ResetConfiguration_DAO>(_DAO.ResetConfiguration);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ResetConfiguration = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ResetConfiguration = (ResetConfiguration_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_OriginationSourceProductConfigurations = null;
			_ProductConditions = null;
			
		}
	}
}



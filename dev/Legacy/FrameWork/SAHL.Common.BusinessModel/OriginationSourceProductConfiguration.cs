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
	/// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO
	/// </summary>
	public partial class OriginationSourceProductConfiguration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO>, IOriginationSourceProductConfiguration
	{
				public OriginationSourceProductConfiguration(SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO OriginationSourceProductConfiguration) : base(OriginationSourceProductConfiguration)
		{
			this._DAO = OriginationSourceProductConfiguration;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO.FinancialServiceType
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
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO.MarketRate
		/// </summary>
		public IMarketRate MarketRate 
		{
			get
			{
				if (null == _DAO.MarketRate) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMarketRate, MarketRate_DAO>(_DAO.MarketRate);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.MarketRate = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.MarketRate = (MarketRate_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO.OriginationSourceProduct
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
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProductConfiguration_DAO.ResetConfiguration
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
	}
}



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
	/// SAHL.Common.BusinessModel.DAO.PropertyData_DAO
	/// </summary>
	public partial class PropertyData : BusinessModelBase<SAHL.Common.BusinessModel.DAO.PropertyData_DAO>, IPropertyData
	{
				public PropertyData(SAHL.Common.BusinessModel.DAO.PropertyData_DAO PropertyData) : base(PropertyData)
		{
			this._DAO = PropertyData;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.Property
		/// </summary>
		public IProperty Property 
		{
			get
			{
				if (null == _DAO.Property) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProperty, Property_DAO>(_DAO.Property);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Property = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Property = (Property_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.PropertyDataProviderDataService
		/// </summary>
		public IPropertyDataProviderDataService PropertyDataProviderDataService 
		{
			get
			{
				if (null == _DAO.PropertyDataProviderDataService) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IPropertyDataProviderDataService, PropertyDataProviderDataService_DAO>(_DAO.PropertyDataProviderDataService);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.PropertyDataProviderDataService = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.PropertyDataProviderDataService = (PropertyDataProviderDataService_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.PropertyID
		/// </summary>
		public String PropertyID 
		{
			get { return _DAO.PropertyID; }
			set { _DAO.PropertyID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.Data
		/// </summary>
		public String Data 
		{
			get { return _DAO.Data; }
			set { _DAO.Data = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.InsertDate
		/// </summary>
		public DateTime InsertDate 
		{
			get { return _DAO.InsertDate; }
			set { _DAO.InsertDate = value;}
		}
	}
}



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
	/// SAHL.Common.BusinessModel.DAO.PropertyDataProviderDataService_DAO
	/// </summary>
	public partial class PropertyDataProviderDataService : BusinessModelBase<SAHL.Common.BusinessModel.DAO.PropertyDataProviderDataService_DAO>, IPropertyDataProviderDataService
	{
				public PropertyDataProviderDataService(SAHL.Common.BusinessModel.DAO.PropertyDataProviderDataService_DAO PropertyDataProviderDataService) : base(PropertyDataProviderDataService)
		{
			this._DAO = PropertyDataProviderDataService;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyDataProviderDataService_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyDataProviderDataService_DAO.DataProviderDataService
		/// </summary>
		public IDataProviderDataService DataProviderDataService 
		{
			get
			{
				if (null == _DAO.DataProviderDataService) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDataProviderDataService, DataProviderDataService_DAO>(_DAO.DataProviderDataService);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DataProviderDataService = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DataProviderDataService = (DataProviderDataService_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



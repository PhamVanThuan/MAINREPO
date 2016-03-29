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
	/// SAHL.Common.BusinessModel.DAO.DataProviderDataService_DAO
	/// </summary>
	public partial class DataProviderDataService : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DataProviderDataService_DAO>, IDataProviderDataService
	{
				public DataProviderDataService(SAHL.Common.BusinessModel.DAO.DataProviderDataService_DAO DataProviderDataService) : base(DataProviderDataService)
		{
			this._DAO = DataProviderDataService;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataProviderDataService_DAO.DataProvider
		/// </summary>
		public IDataProvider DataProvider 
		{
			get
			{
				if (null == _DAO.DataProvider) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDataProvider, DataProvider_DAO>(_DAO.DataProvider);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DataProvider = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DataProvider = (DataProvider_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataProviderDataService_DAO.DataService
		/// </summary>
		public IDataService DataService 
		{
			get
			{
				if (null == _DAO.DataService) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDataService, DataService_DAO>(_DAO.DataService);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DataService = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DataService = (DataService_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataProviderDataService_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}



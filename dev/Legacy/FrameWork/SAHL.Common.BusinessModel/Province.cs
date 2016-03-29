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
	/// SAHL.Common.BusinessModel.DAO.Province_DAO
	/// </summary>
	public partial class Province : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Province_DAO>, IProvince
	{
				public Province(SAHL.Common.BusinessModel.DAO.Province_DAO Province) : base(Province)
		{
			this._DAO = Province;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Province_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Province_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Province_DAO.Cities
		/// </summary>
		private DAOEventList<City_DAO, ICity, City> _Cities;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Province_DAO.Cities
		/// </summary>
		public IEventList<ICity> Cities
		{
			get
			{
				if (null == _Cities) 
				{
					if(null == _DAO.Cities)
						_DAO.Cities = new List<City_DAO>();
					_Cities = new DAOEventList<City_DAO, ICity, City>(_DAO.Cities);
					_Cities.BeforeAdd += new EventListHandler(OnCities_BeforeAdd);					
					_Cities.BeforeRemove += new EventListHandler(OnCities_BeforeRemove);					
					_Cities.AfterAdd += new EventListHandler(OnCities_AfterAdd);					
					_Cities.AfterRemove += new EventListHandler(OnCities_AfterRemove);					
				}
				return _Cities;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Province_DAO.Country
		/// </summary>
		public ICountry Country 
		{
			get
			{
				if (null == _DAO.Country) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICountry, Country_DAO>(_DAO.Country);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Country = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Country = (Country_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_Cities = null;
			
		}
	}
}



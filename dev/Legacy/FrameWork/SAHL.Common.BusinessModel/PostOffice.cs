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
	/// SAHL.Common.BusinessModel.DAO.PostOffice_DAO
	/// </summary>
	public partial class PostOffice : BusinessModelBase<SAHL.Common.BusinessModel.DAO.PostOffice_DAO>, IPostOffice
	{
				public PostOffice(SAHL.Common.BusinessModel.DAO.PostOffice_DAO PostOffice) : base(PostOffice)
		{
			this._DAO = PostOffice;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PostOffice_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PostOffice_DAO.PostalCode
		/// </summary>
		public String PostalCode 
		{
			get { return _DAO.PostalCode; }
			set { _DAO.PostalCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PostOffice_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PostOffice_DAO.City
		/// </summary>
		public ICity City 
		{
			get
			{
				if (null == _DAO.City) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICity, City_DAO>(_DAO.City);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.City = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.City = (City_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



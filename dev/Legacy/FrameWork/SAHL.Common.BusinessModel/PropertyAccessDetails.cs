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
	/// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO
	/// </summary>
	public partial class PropertyAccessDetails : BusinessModelBase<SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO>, IPropertyAccessDetails
	{
				public PropertyAccessDetails(SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO PropertyAccessDetails) : base(PropertyAccessDetails)
		{
			this._DAO = PropertyAccessDetails;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Property
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
		/// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact1
		/// </summary>
		public String Contact1 
		{
			get { return _DAO.Contact1; }
			set { _DAO.Contact1 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact1Phone
		/// </summary>
		public String Contact1Phone 
		{
			get { return _DAO.Contact1Phone; }
			set { _DAO.Contact1Phone = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact1WorkPhone
		/// </summary>
		public String Contact1WorkPhone 
		{
			get { return _DAO.Contact1WorkPhone; }
			set { _DAO.Contact1WorkPhone = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact1MobilePhone
		/// </summary>
		public String Contact1MobilePhone 
		{
			get { return _DAO.Contact1MobilePhone; }
			set { _DAO.Contact1MobilePhone = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact2
		/// </summary>
		public String Contact2 
		{
			get { return _DAO.Contact2; }
			set { _DAO.Contact2 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact2Phone
		/// </summary>
		public String Contact2Phone 
		{
			get { return _DAO.Contact2Phone; }
			set { _DAO.Contact2Phone = value;}
		}
	}
}



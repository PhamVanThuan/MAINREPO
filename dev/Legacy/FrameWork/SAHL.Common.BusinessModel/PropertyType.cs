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
	/// SAHL.Common.BusinessModel.DAO.PropertyType_DAO
	/// </summary>
	public partial class PropertyType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.PropertyType_DAO>, IPropertyType
	{
				public PropertyType(SAHL.Common.BusinessModel.DAO.PropertyType_DAO PropertyType) : base(PropertyType)
		{
			this._DAO = PropertyType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PropertyType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}



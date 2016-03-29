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
	/// SAHL.Common.BusinessModel.DAO.DomainField_DAO
	/// </summary>
	public partial class DomainField : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DomainField_DAO>, IDomainField
	{
				public DomainField(SAHL.Common.BusinessModel.DAO.DomainField_DAO DomainField) : base(DomainField)
		{
			this._DAO = DomainField;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DomainField_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DomainField_DAO.DisplayDescription
		/// </summary>
		public String DisplayDescription 
		{
			get { return _DAO.DisplayDescription; }
			set { _DAO.DisplayDescription = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DomainField_DAO.Key
		/// </summary>
		public String Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DomainField_DAO.FormatType
		/// </summary>
		public IFormatType FormatType 
		{
			get
			{
				if (null == _DAO.FormatType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFormatType, FormatType_DAO>(_DAO.FormatType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FormatType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FormatType = (FormatType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



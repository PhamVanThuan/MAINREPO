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
	/// SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO
	/// </summary>
	public partial class SPVAttribute : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO>, ISPVAttribute
	{
				public SPVAttribute(SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO SPVAttribute) : base(SPVAttribute)
		{
			this._DAO = SPVAttribute;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO.SPV
		/// </summary>
		public ISPV SPV 
		{
			get
			{
				if (null == _DAO.SPV) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISPV, SPV_DAO>(_DAO.SPV);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SPV = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SPV = (SPV_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO.SPVAttributeType
		/// </summary>
		public ISPVAttributeType SPVAttributeType 
		{
			get
			{
				if (null == _DAO.SPVAttributeType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISPVAttributeType, SPVAttributeType_DAO>(_DAO.SPVAttributeType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SPVAttributeType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SPVAttributeType = (SPVAttributeType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVAttribute_DAO.Value
		/// </summary>
		public String Value 
		{
			get { return _DAO.Value; }
			set { _DAO.Value = value;}
		}
	}
}



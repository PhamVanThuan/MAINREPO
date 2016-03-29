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
	/// SAHL.Common.BusinessModel.DAO.XSLTransformation_DAO
	/// </summary>
	public partial class XSLTransformation : BusinessModelBase<SAHL.Common.BusinessModel.DAO.XSLTransformation_DAO>, IXSLTransformation
	{
				public XSLTransformation(SAHL.Common.BusinessModel.DAO.XSLTransformation_DAO XSLTransformation) : base(XSLTransformation)
		{
			this._DAO = XSLTransformation;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.XSLTransformation_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.XSLTransformation_DAO.StyleSheet
		/// </summary>
		public String StyleSheet 
		{
			get { return _DAO.StyleSheet; }
			set { _DAO.StyleSheet = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.XSLTransformation_DAO.Version
		/// </summary>
		public Int32 Version 
		{
			get { return _DAO.Version; }
			set { _DAO.Version = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.XSLTransformation_DAO.GenericKeyType
		/// </summary>
		public IGenericKeyType GenericKeyType 
		{
			get
			{
				if (null == _DAO.GenericKeyType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GenericKeyType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



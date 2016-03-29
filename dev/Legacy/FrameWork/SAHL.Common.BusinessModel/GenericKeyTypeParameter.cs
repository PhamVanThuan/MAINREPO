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
	/// SAHL.Common.BusinessModel.DAO.GenericKeyTypeParameter_DAO
	/// </summary>
	public partial class GenericKeyTypeParameter : BusinessModelBase<SAHL.Common.BusinessModel.DAO.GenericKeyTypeParameter_DAO>, IGenericKeyTypeParameter
	{
				public GenericKeyTypeParameter(SAHL.Common.BusinessModel.DAO.GenericKeyTypeParameter_DAO GenericKeyTypeParameter) : base(GenericKeyTypeParameter)
		{
			this._DAO = GenericKeyTypeParameter;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyTypeParameter_DAO.ParameterName
		/// </summary>
		public String ParameterName 
		{
			get { return _DAO.ParameterName; }
			set { _DAO.ParameterName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyTypeParameter_DAO.ParameterTypeKey
		/// </summary>
		public IParameterType ParameterTypeKey 
		{
			get
			{
				if (null == _DAO.ParameterTypeKey) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IParameterType, ParameterType_DAO>(_DAO.ParameterTypeKey);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ParameterTypeKey = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ParameterTypeKey = (ParameterType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyTypeParameter_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyTypeParameter_DAO.GenericKeyType
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



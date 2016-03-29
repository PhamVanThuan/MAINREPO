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
	/// SAHL.Common.BusinessModel.DAO.InputGenericType_DAO
	/// </summary>
	public partial class InputGenericType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.InputGenericType_DAO>, IInputGenericType
	{
				public InputGenericType(SAHL.Common.BusinessModel.DAO.InputGenericType_DAO InputGenericType) : base(InputGenericType)
		{
			this._DAO = InputGenericType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InputGenericType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InputGenericType_DAO.CoreBusinessObjectMenu
		/// </summary>
		public ICBOMenu CoreBusinessObjectMenu 
		{
			get
			{
				if (null == _DAO.CoreBusinessObjectMenu) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICBOMenu, CBOMenu_DAO>(_DAO.CoreBusinessObjectMenu);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.CoreBusinessObjectMenu = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.CoreBusinessObjectMenu = (CBOMenu_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InputGenericType_DAO.GenericKeyTypeParameter
		/// </summary>
		public IGenericKeyTypeParameter GenericKeyTypeParameter 
		{
			get
			{
				if (null == _DAO.GenericKeyTypeParameter) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGenericKeyTypeParameter, GenericKeyTypeParameter_DAO>(_DAO.GenericKeyTypeParameter);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GenericKeyTypeParameter = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GenericKeyTypeParameter = (GenericKeyTypeParameter_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



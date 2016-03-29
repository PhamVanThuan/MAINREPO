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
	/// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO
	/// </summary>
	public partial class RoundRobinPointerDefinition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO>, IRoundRobinPointerDefinition
	{
				public RoundRobinPointerDefinition(SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO RoundRobinPointerDefinition) : base(RoundRobinPointerDefinition)
		{
			this._DAO = RoundRobinPointerDefinition;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.GenericKey
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.ApplicationName
		/// </summary>
		public String ApplicationName 
		{
			get { return _DAO.ApplicationName; }
			set { _DAO.ApplicationName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.StatementName
		/// </summary>
		public String StatementName 
		{
			get { return _DAO.StatementName; }
			set { _DAO.StatementName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.RoundRobinPointer
		/// </summary>
		public IRoundRobinPointer RoundRobinPointer 
		{
			get
			{
				if (null == _DAO.RoundRobinPointer) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRoundRobinPointer, RoundRobinPointer_DAO>(_DAO.RoundRobinPointer);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RoundRobinPointer = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RoundRobinPointer = (RoundRobinPointer_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.GeneralStatus
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointerDefinition_DAO.GenericKeyType
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



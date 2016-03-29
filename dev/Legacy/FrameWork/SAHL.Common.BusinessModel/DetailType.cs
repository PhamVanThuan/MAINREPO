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
	/// SAHL.Common.BusinessModel.DAO.DetailType_DAO
	/// </summary>
	public partial class DetailType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DetailType_DAO>, IDetailType
	{
				public DetailType(SAHL.Common.BusinessModel.DAO.DetailType_DAO DetailType) : base(DetailType)
		{
			this._DAO = DetailType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DetailType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DetailType_DAO.AllowUpdateDelete
		/// </summary>
		public Boolean AllowUpdateDelete 
		{
			get { return _DAO.AllowUpdateDelete; }
			set { _DAO.AllowUpdateDelete = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DetailType_DAO.AllowUpdate
		/// </summary>
		public Boolean AllowUpdate 
		{
			get { return _DAO.AllowUpdate; }
			set { _DAO.AllowUpdate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DetailType_DAO.AllowScreen
		/// </summary>
		public Boolean AllowScreen 
		{
			get { return _DAO.AllowScreen; }
			set { _DAO.AllowScreen = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DetailType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DetailType_DAO.GeneralStatus
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
	}
}



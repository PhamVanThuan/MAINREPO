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
	/// SAHL.Common.BusinessModel.DAO.ExternalRoleType_DAO
	/// </summary>
	public partial class ExternalRoleType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ExternalRoleType_DAO>, IExternalRoleType
	{
				public ExternalRoleType(SAHL.Common.BusinessModel.DAO.ExternalRoleType_DAO ExternalRoleType) : base(ExternalRoleType)
		{
			this._DAO = ExternalRoleType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalRoleType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalRoleType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalRoleType_DAO.ExternalRoleTypeGroup
		/// </summary>
		public IExternalRoleTypeGroup ExternalRoleTypeGroup 
		{
			get
			{
				if (null == _DAO.ExternalRoleTypeGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IExternalRoleTypeGroup, ExternalRoleTypeGroup_DAO>(_DAO.ExternalRoleTypeGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ExternalRoleTypeGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ExternalRoleTypeGroup = (ExternalRoleTypeGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
		}
	}
}



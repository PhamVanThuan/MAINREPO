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
	/// SAHL.Common.BusinessModel.DAO.ExternalRoleTypeGroup_DAO
	/// </summary>
	public partial class ExternalRoleTypeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ExternalRoleTypeGroup_DAO>, IExternalRoleTypeGroup
	{
				public ExternalRoleTypeGroup(SAHL.Common.BusinessModel.DAO.ExternalRoleTypeGroup_DAO ExternalRoleTypeGroup) : base(ExternalRoleTypeGroup)
		{
			this._DAO = ExternalRoleTypeGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalRoleTypeGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalRoleTypeGroup_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalRoleTypeGroup_DAO.ExternalRoleTypes
		/// </summary>
		private DAOEventList<ExternalRoleType_DAO, IExternalRoleType, ExternalRoleType> _ExternalRoleTypes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalRoleTypeGroup_DAO.ExternalRoleTypes
		/// </summary>
		public IEventList<IExternalRoleType> ExternalRoleTypes
		{
			get
			{
				if (null == _ExternalRoleTypes) 
				{
					if(null == _DAO.ExternalRoleTypes)
						_DAO.ExternalRoleTypes = new List<ExternalRoleType_DAO>();
					_ExternalRoleTypes = new DAOEventList<ExternalRoleType_DAO, IExternalRoleType, ExternalRoleType>(_DAO.ExternalRoleTypes);
					_ExternalRoleTypes.BeforeAdd += new EventListHandler(OnExternalRoleTypes_BeforeAdd);					
					_ExternalRoleTypes.BeforeRemove += new EventListHandler(OnExternalRoleTypes_BeforeRemove);					
					_ExternalRoleTypes.AfterAdd += new EventListHandler(OnExternalRoleTypes_AfterAdd);					
					_ExternalRoleTypes.AfterRemove += new EventListHandler(OnExternalRoleTypes_AfterRemove);					
				}
				return _ExternalRoleTypes;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ExternalRoleTypes = null;
			
		}
	}
}



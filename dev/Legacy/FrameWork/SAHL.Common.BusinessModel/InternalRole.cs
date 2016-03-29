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
	/// SAHL.Common.BusinessModel.DAO.InternalRole_DAO
	/// </summary>
	public partial class InternalRole : BusinessModelBase<SAHL.Common.BusinessModel.DAO.InternalRole_DAO>, IInternalRole
	{
				public InternalRole(SAHL.Common.BusinessModel.DAO.InternalRole_DAO InternalRole) : base(InternalRole)
		{
			this._DAO = InternalRole;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.TableName
		/// </summary>
		public String TableName 
		{
			get { return _DAO.TableName; }
			set { _DAO.TableName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.Alias
		/// </summary>
		public String Alias 
		{
			get { return _DAO.Alias; }
			set { _DAO.Alias = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.PrimaryKeyColumn
		/// </summary>
		public String PrimaryKeyColumn 
		{
			get { return _DAO.PrimaryKeyColumn; }
			set { _DAO.PrimaryKeyColumn = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.ContextTableKey
		/// </summary>
		public Int32 ContextTableKey 
		{
			get { return _DAO.ContextTableKey; }
			set { _DAO.ContextTableKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternalRole_DAO.ContextTableJoinKey
		/// </summary>
		public String ContextTableJoinKey 
		{
			get { return _DAO.ContextTableJoinKey; }
			set { _DAO.ContextTableJoinKey = value;}
		}
	}
}



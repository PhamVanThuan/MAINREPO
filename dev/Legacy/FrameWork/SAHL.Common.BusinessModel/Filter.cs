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
	/// SAHL.Common.BusinessModel.DAO.Filter_DAO
	/// </summary>
	public partial class Filter : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Filter_DAO>, IFilter
	{
				public Filter(SAHL.Common.BusinessModel.DAO.Filter_DAO Filter) : base(Filter)
		{
			this._DAO = Filter;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Filter_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Filter_DAO.Name
		/// </summary>
		public String Name 
		{
			get { return _DAO.Name; }
			set { _DAO.Name = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Filter_DAO.ContextKey
		/// </summary>
		public Int32 ContextKey 
		{
			get { return _DAO.ContextKey; }
			set { _DAO.ContextKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Filter_DAO.RoleKey
		/// </summary>
		public Int32 RoleKey 
		{
			get { return _DAO.RoleKey; }
			set { _DAO.RoleKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Filter_DAO.WorkflowContextKey
		/// </summary>
		public Int32 WorkflowContextKey 
		{
			get { return _DAO.WorkflowContextKey; }
			set { _DAO.WorkflowContextKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Filter_DAO.Query
		/// </summary>
		public String Query 
		{
			get { return _DAO.Query; }
			set { _DAO.Query = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Filter_DAO.Parameters
		/// </summary>
		public String Parameters 
		{
			get { return _DAO.Parameters; }
			set { _DAO.Parameters = value;}
		}
	}
}



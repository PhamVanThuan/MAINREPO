using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Base;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.Select_DAO
	/// </summary>
	public partial class SearchSelect : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SearchSelect_DAO>, ISearchSelect
	{
		public SearchSelect(SAHL.Common.BusinessModel.DAO.SearchSelect_DAO SearchSelect)
			: base(SearchSelect)
		{
			this._DAO = SearchSelect;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Select_DAO.Key
		/// </summary>
		public Int32 Key
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value; }
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Select_DAO.Name
		/// </summary>
		public String Name
		{
			get { return _DAO.Name; }
			set { _DAO.Name = value; }
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Select_DAO.Query
		/// </summary>
		public String Query
		{
			get { return _DAO.Query; }
			set { _DAO.Query = value; }
		}
	}
}

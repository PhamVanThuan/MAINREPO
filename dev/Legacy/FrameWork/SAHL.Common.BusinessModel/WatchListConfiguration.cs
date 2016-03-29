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
	/// SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO
	/// </summary>
	public partial class WatchListConfiguration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO>, IWatchListConfiguration
	{
				public WatchListConfiguration(SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO WatchListConfiguration) : base(WatchListConfiguration)
		{
			this._DAO = WatchListConfiguration;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO.ProcessName
		/// </summary>
		public String ProcessName 
		{
			get { return _DAO.ProcessName; }
			set { _DAO.ProcessName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO.WorkFlowName
		/// </summary>
		public String WorkFlowName 
		{
			get { return _DAO.WorkFlowName; }
			set { _DAO.WorkFlowName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WatchListConfiguration_DAO.StatementName
		/// </summary>
		public String StatementName 
		{
			get { return _DAO.StatementName; }
			set { _DAO.StatementName = value;}
		}
	}
}



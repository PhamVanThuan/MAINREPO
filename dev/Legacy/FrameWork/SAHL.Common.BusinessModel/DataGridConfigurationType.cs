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
	/// SAHL.Common.BusinessModel.DAO.DataGridConfigurationType_DAO
	/// </summary>
	public partial class DataGridConfigurationType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DataGridConfigurationType_DAO>, IDataGridConfigurationType
	{
				public DataGridConfigurationType(SAHL.Common.BusinessModel.DAO.DataGridConfigurationType_DAO DataGridConfigurationType) : base(DataGridConfigurationType)
		{
			this._DAO = DataGridConfigurationType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfigurationType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfigurationType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfigurationType_DAO.DataGridConfigurations
		/// </summary>
		private DAOEventList<DataGridConfiguration_DAO, IDataGridConfiguration, DataGridConfiguration> _DataGridConfigurations;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfigurationType_DAO.DataGridConfigurations
		/// </summary>
		public IEventList<IDataGridConfiguration> DataGridConfigurations
		{
			get
			{
				if (null == _DataGridConfigurations) 
				{
					if(null == _DAO.DataGridConfigurations)
						_DAO.DataGridConfigurations = new List<DataGridConfiguration_DAO>();
					_DataGridConfigurations = new DAOEventList<DataGridConfiguration_DAO, IDataGridConfiguration, DataGridConfiguration>(_DAO.DataGridConfigurations);
					_DataGridConfigurations.BeforeAdd += new EventListHandler(OnDataGridConfigurations_BeforeAdd);					
					_DataGridConfigurations.BeforeRemove += new EventListHandler(OnDataGridConfigurations_BeforeRemove);					
					_DataGridConfigurations.AfterAdd += new EventListHandler(OnDataGridConfigurations_AfterAdd);					
					_DataGridConfigurations.AfterRemove += new EventListHandler(OnDataGridConfigurations_AfterRemove);					
				}
				return _DataGridConfigurations;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_DataGridConfigurations = null;
			
		}
	}
}



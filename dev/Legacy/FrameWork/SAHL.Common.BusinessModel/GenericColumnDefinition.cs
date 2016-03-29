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
	/// SAHL.Common.BusinessModel.DAO.GenericColumnDefinition_DAO
	/// </summary>
	public partial class GenericColumnDefinition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.GenericColumnDefinition_DAO>, IGenericColumnDefinition
	{
				public GenericColumnDefinition(SAHL.Common.BusinessModel.DAO.GenericColumnDefinition_DAO GenericColumnDefinition) : base(GenericColumnDefinition)
		{
			this._DAO = GenericColumnDefinition;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericColumnDefinition_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericColumnDefinition_DAO.TableName
		/// </summary>
		public String TableName 
		{
			get { return _DAO.TableName; }
			set { _DAO.TableName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericColumnDefinition_DAO.ColumnName
		/// </summary>
		public String ColumnName 
		{
			get { return _DAO.ColumnName; }
			set { _DAO.ColumnName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericColumnDefinition_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericColumnDefinition_DAO.ConditionConfigurations
		/// </summary>
		private DAOEventList<ConditionConfiguration_DAO, IConditionConfiguration, ConditionConfiguration> _ConditionConfigurations;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericColumnDefinition_DAO.ConditionConfigurations
		/// </summary>
		public IEventList<IConditionConfiguration> ConditionConfigurations
		{
			get
			{
				if (null == _ConditionConfigurations) 
				{
					if(null == _DAO.ConditionConfigurations)
						_DAO.ConditionConfigurations = new List<ConditionConfiguration_DAO>();
					_ConditionConfigurations = new DAOEventList<ConditionConfiguration_DAO, IConditionConfiguration, ConditionConfiguration>(_DAO.ConditionConfigurations);
					_ConditionConfigurations.BeforeAdd += new EventListHandler(OnConditionConfigurations_BeforeAdd);					
					_ConditionConfigurations.BeforeRemove += new EventListHandler(OnConditionConfigurations_BeforeRemove);					
					_ConditionConfigurations.AfterAdd += new EventListHandler(OnConditionConfigurations_AfterAdd);					
					_ConditionConfigurations.AfterRemove += new EventListHandler(OnConditionConfigurations_AfterRemove);					
				}
				return _ConditionConfigurations;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ConditionConfigurations = null;
			
		}
	}
}



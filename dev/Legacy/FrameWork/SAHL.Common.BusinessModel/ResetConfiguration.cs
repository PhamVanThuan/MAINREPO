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
	/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO
	/// </summary>
	public partial class ResetConfiguration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO>, IResetConfiguration
	{
				public ResetConfiguration(SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO ResetConfiguration) : base(ResetConfiguration)
		{
			this._DAO = ResetConfiguration;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.IntervalType
		/// </summary>
		public String IntervalType 
		{
			get { return _DAO.IntervalType; }
			set { _DAO.IntervalType = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.IntervalDuration
		/// </summary>
		public Int32 IntervalDuration 
		{
			get { return _DAO.IntervalDuration; }
			set { _DAO.IntervalDuration = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.ResetDate
		/// </summary>
		public DateTime ResetDate 
		{
			get { return _DAO.ResetDate; }
			set { _DAO.ResetDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.ActionDate
		/// </summary>
		public DateTime ActionDate 
		{
			get { return _DAO.ActionDate; }
			set { _DAO.ActionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.BusinessDayIndicator
		/// </summary>
		public Char BusinessDayIndicator 
		{
			get { return _DAO.BusinessDayIndicator; }
			set { _DAO.BusinessDayIndicator = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.FinancialServiceTypes
		/// </summary>
		private DAOEventList<FinancialServiceType_DAO, IFinancialServiceType, FinancialServiceType> _FinancialServiceTypes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.FinancialServiceTypes
		/// </summary>
		public IEventList<IFinancialServiceType> FinancialServiceTypes
		{
			get
			{
				if (null == _FinancialServiceTypes) 
				{
					if(null == _DAO.FinancialServiceTypes)
						_DAO.FinancialServiceTypes = new List<FinancialServiceType_DAO>();
					_FinancialServiceTypes = new DAOEventList<FinancialServiceType_DAO, IFinancialServiceType, FinancialServiceType>(_DAO.FinancialServiceTypes);
					_FinancialServiceTypes.BeforeAdd += new EventListHandler(OnFinancialServiceTypes_BeforeAdd);					
					_FinancialServiceTypes.BeforeRemove += new EventListHandler(OnFinancialServiceTypes_BeforeRemove);					
					_FinancialServiceTypes.AfterAdd += new EventListHandler(OnFinancialServiceTypes_AfterAdd);					
					_FinancialServiceTypes.AfterRemove += new EventListHandler(OnFinancialServiceTypes_AfterRemove);					
				}
				return _FinancialServiceTypes;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.OriginationSourceProductConfigurations
		/// </summary>
		private DAOEventList<OriginationSourceProductConfiguration_DAO, IOriginationSourceProductConfiguration, OriginationSourceProductConfiguration> _OriginationSourceProductConfigurations;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.OriginationSourceProductConfigurations
		/// </summary>
		public IEventList<IOriginationSourceProductConfiguration> OriginationSourceProductConfigurations
		{
			get
			{
				if (null == _OriginationSourceProductConfigurations) 
				{
					if(null == _DAO.OriginationSourceProductConfigurations)
						_DAO.OriginationSourceProductConfigurations = new List<OriginationSourceProductConfiguration_DAO>();
					_OriginationSourceProductConfigurations = new DAOEventList<OriginationSourceProductConfiguration_DAO, IOriginationSourceProductConfiguration, OriginationSourceProductConfiguration>(_DAO.OriginationSourceProductConfigurations);
					_OriginationSourceProductConfigurations.BeforeAdd += new EventListHandler(OnOriginationSourceProductConfigurations_BeforeAdd);					
					_OriginationSourceProductConfigurations.BeforeRemove += new EventListHandler(OnOriginationSourceProductConfigurations_BeforeRemove);					
					_OriginationSourceProductConfigurations.AfterAdd += new EventListHandler(OnOriginationSourceProductConfigurations_AfterAdd);					
					_OriginationSourceProductConfigurations.AfterRemove += new EventListHandler(OnOriginationSourceProductConfigurations_AfterRemove);					
				}
				return _OriginationSourceProductConfigurations;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_FinancialServiceTypes = null;
			_OriginationSourceProductConfigurations = null;
			
		}
	}
}



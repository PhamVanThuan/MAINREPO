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
	/// SAHL.Common.BusinessModel.DAO.RateOverrideTypeGroup_DAO
	/// </summary>
	public partial class RateOverrideTypeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RateOverrideTypeGroup_DAO>, IRateOverrideTypeGroup
	{
				public RateOverrideTypeGroup(SAHL.Common.BusinessModel.DAO.RateOverrideTypeGroup_DAO RateOverrideTypeGroup) : base(RateOverrideTypeGroup)
		{
			this._DAO = RateOverrideTypeGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateOverrideTypeGroup_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateOverrideTypeGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateOverrideTypeGroup_DAO.RateOverrideTypes
		/// </summary>
		private DAOEventList<RateOverrideType_DAO, IRateOverrideType, RateOverrideType> _RateOverrideTypes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateOverrideTypeGroup_DAO.RateOverrideTypes
		/// </summary>
		public IEventList<IRateOverrideType> RateOverrideTypes
		{
			get
			{
				if (null == _RateOverrideTypes) 
				{
					if(null == _DAO.RateOverrideTypes)
						_DAO.RateOverrideTypes = new List<RateOverrideType_DAO>();
					_RateOverrideTypes = new DAOEventList<RateOverrideType_DAO, IRateOverrideType, RateOverrideType>(_DAO.RateOverrideTypes);
					_RateOverrideTypes.BeforeAdd += new EventListHandler(OnRateOverrideTypes_BeforeAdd);					
					_RateOverrideTypes.BeforeRemove += new EventListHandler(OnRateOverrideTypes_BeforeRemove);					
					_RateOverrideTypes.AfterAdd += new EventListHandler(OnRateOverrideTypes_AfterAdd);					
					_RateOverrideTypes.AfterRemove += new EventListHandler(OnRateOverrideTypes_AfterRemove);					
				}
				return _RateOverrideTypes;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_RateOverrideTypes = null;
			
		}
	}
}



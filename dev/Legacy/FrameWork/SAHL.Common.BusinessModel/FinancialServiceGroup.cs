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
	/// SAHL.Common.BusinessModel.DAO.FinancialServiceGroup_DAO
	/// </summary>
	public partial class FinancialServiceGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialServiceGroup_DAO>, IFinancialServiceGroup
	{
				public FinancialServiceGroup(SAHL.Common.BusinessModel.DAO.FinancialServiceGroup_DAO FinancialServiceGroup) : base(FinancialServiceGroup)
		{
			this._DAO = FinancialServiceGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceGroup_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceGroup_DAO.FinancialServiceTypes
		/// </summary>
		private DAOEventList<FinancialServiceType_DAO, IFinancialServiceType, FinancialServiceType> _FinancialServiceTypes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceGroup_DAO.FinancialServiceTypes
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
		public override void Refresh()
		{
			base.Refresh();
			_FinancialServiceTypes = null;
			
		}
	}
}



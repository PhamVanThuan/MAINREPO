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
	/// SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO
	/// </summary>
	public partial class DebtCounsellingGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO>, IDebtCounsellingGroup
	{
				public DebtCounsellingGroup(SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO DebtCounsellingGroup) : base(DebtCounsellingGroup)
		{
			this._DAO = DebtCounsellingGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO.CreatedDate
		/// </summary>
		public DateTime CreatedDate 
		{
			get { return _DAO.CreatedDate; }
			set { _DAO.CreatedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO.DebtCounsellingCases
		/// </summary>
		private DAOEventList<DebtCounselling_DAO, IDebtCounselling, DebtCounselling> _DebtCounsellingCases;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO.DebtCounsellingCases
		/// </summary>
		public IEventList<IDebtCounselling> DebtCounsellingCases
		{
			get
			{
				if (null == _DebtCounsellingCases) 
				{
					if(null == _DAO.DebtCounsellingCases)
						_DAO.DebtCounsellingCases = new List<DebtCounselling_DAO>();
					_DebtCounsellingCases = new DAOEventList<DebtCounselling_DAO, IDebtCounselling, DebtCounselling>(_DAO.DebtCounsellingCases);
					_DebtCounsellingCases.BeforeAdd += new EventListHandler(OnDebtCounsellingCases_BeforeAdd);					
					_DebtCounsellingCases.BeforeRemove += new EventListHandler(OnDebtCounsellingCases_BeforeRemove);					
					_DebtCounsellingCases.AfterAdd += new EventListHandler(OnDebtCounsellingCases_AfterAdd);					
					_DebtCounsellingCases.AfterRemove += new EventListHandler(OnDebtCounsellingCases_AfterRemove);					
				}
				return _DebtCounsellingCases;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_DebtCounsellingCases = null;
			
		}
	}
}



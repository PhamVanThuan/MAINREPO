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
	/// SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO
	/// </summary>
	public partial class RateAdjustmentGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO>, IRateAdjustmentGroup
	{
				public RateAdjustmentGroup(SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO RateAdjustmentGroup) : base(RateAdjustmentGroup)
		{
			this._DAO = RateAdjustmentGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO.RateAdjustmentElements
		/// </summary>
		private DAOEventList<RateAdjustmentElement_DAO, IRateAdjustmentElement, RateAdjustmentElement> _RateAdjustmentElements;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO.RateAdjustmentElements
		/// </summary>
		public IEventList<IRateAdjustmentElement> RateAdjustmentElements
		{
			get
			{
				if (null == _RateAdjustmentElements) 
				{
					if(null == _DAO.RateAdjustmentElements)
						_DAO.RateAdjustmentElements = new List<RateAdjustmentElement_DAO>();
					_RateAdjustmentElements = new DAOEventList<RateAdjustmentElement_DAO, IRateAdjustmentElement, RateAdjustmentElement>(_DAO.RateAdjustmentElements);
					_RateAdjustmentElements.BeforeAdd += new EventListHandler(OnRateAdjustmentElements_BeforeAdd);					
					_RateAdjustmentElements.BeforeRemove += new EventListHandler(OnRateAdjustmentElements_BeforeRemove);					
					_RateAdjustmentElements.AfterAdd += new EventListHandler(OnRateAdjustmentElements_AfterAdd);					
					_RateAdjustmentElements.AfterRemove += new EventListHandler(OnRateAdjustmentElements_AfterRemove);					
				}
				return _RateAdjustmentElements;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_RateAdjustmentElements = null;
			
		}
	}
}



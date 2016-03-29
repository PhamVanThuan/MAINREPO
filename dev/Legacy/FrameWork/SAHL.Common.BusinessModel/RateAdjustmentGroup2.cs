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
using System.Linq;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO
	/// </summary>
	public partial class RateAdjustmentGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO>, IRateAdjustmentGroup
	{

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO.RateAdjustmentElements
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnRateAdjustmentElements_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO.RateAdjustmentElements
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnRateAdjustmentElements_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO.RateAdjustmentElements
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnRateAdjustmentElements_AfterAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentGroup_DAO.RateAdjustmentElements
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnRateAdjustmentElements_AfterRemove(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// Returns a List of the Active Rate Adjustment Elements
		/// </summary>
		public IEventList<IRateAdjustmentElement> ActiveRateAdjustmentElements
		{
			get
			{
				IEventList<IRateAdjustmentElement> rateAdjustmentElements = new EventList<IRateAdjustmentElement>();
				var query = from rateAdjustmentElement in RateAdjustmentElements
							 where rateAdjustmentElement.GeneralStatus.Key == (int)GeneralStatusKey.Active
							 select rateAdjustmentElement;
				if (query.Count() > 0)
				{
					rateAdjustmentElements = new EventList<IRateAdjustmentElement>(query);
				}
				return rateAdjustmentElements;
			}
		}
	}
}



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
	/// 
	/// </summary>
	public partial class Operator : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Operator_DAO>, IOperator
	{
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnAllocationMandateOperators_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="Item"></param>
        protected void OnAllocationMandateOperators_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="Item"></param>
        protected void OnAllocationMandateOperators_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnAllocationMandateOperators_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}
	}
}



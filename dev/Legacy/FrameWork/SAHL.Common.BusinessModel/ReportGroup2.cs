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
	public partial class ReportGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReportGroup_DAO>, IReportGroup,IComparable
	{
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnReportStatements_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnReportStatements_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnReportStatements_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnReportStatements_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            ReportGroup g = (ReportGroup) obj;
            return String.Compare(this.Description, g.Description);
        }

        #endregion
    }
}



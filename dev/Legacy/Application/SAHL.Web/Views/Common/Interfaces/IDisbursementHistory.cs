using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IDisbursementHistory : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        int SelectedStatus { get;set;}
        /// <summary>
        /// 
        /// </summary>
        double TotalDisbursementsValue { set;}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="historyStatuses"></param>
        void BindDisbursementStatuses(IList<IDisbursementStatus> historyStatuses);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disbursements"></param>
        void BindGrid(IReadOnlyEventList<IDisbursement> disbursements);
    }
}

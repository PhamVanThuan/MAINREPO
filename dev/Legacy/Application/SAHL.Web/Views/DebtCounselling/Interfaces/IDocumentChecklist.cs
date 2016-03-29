using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    public interface IDocumentChecklist : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dates"></param>
        void BindDateGrid(List<BindableDCItem> dates);

        /// <summary>
        /// 
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateTime? NewDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DCItemType ItemType { get; set; }
    }


    public class BindableDCItem
    {
        public BindableDCItem(string description, DateTime? date, string detail, int type, bool canSave)
        {
            Description = description;
            Date = date.HasValue ? date.Value.ToString(SAHL.Common.Constants.DateFormat) : String.Empty;
            Detail = detail;
            ItemType = type;
            CanSave = canSave;
        }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Detail { get; set; }
        public int ItemType { get; set; }
        public bool CanSave { get; set; }
    }

    public enum DCItemType
    {
        dte171 = 0,
        dte172,
        dte173,
        dte60Days,
        dteHearing,
        dteReview,
        NONE
    }
}

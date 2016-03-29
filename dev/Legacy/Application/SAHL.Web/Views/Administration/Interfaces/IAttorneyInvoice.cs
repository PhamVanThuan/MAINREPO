using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IAttorneyInvoice : IViewBase
    {
        /// <summary>
        /// Property to switch edit/view modes
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        bool ReadOnly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int AccountKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int SelectedAccountAttorneyInvoiceKey { get; set; }

        /// <summary>
        /// OnAccountItemSelect Event
        /// </summary>
        event EventHandler OnAccountItemSelect;

        /// <summary>
        /// OnAccountItemSelect Event
        /// </summary>
        event EventHandler OnAddClick;

        /// <summary>
        /// OnAccountItemSelect Event
        /// </summary>
        event EventHandler OnCancelClick;

        /// <summary>
        /// OnAccountItemSelect Event
        /// </summary>
        event EventHandler OnDeleteClick;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invList"></param>
        void BindGrid(IEventList<IAccountAttorneyInvoice> invList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attorneys"></param>
        void BindAttorneys(Dictionary<int, string> attorneys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accAttInv"></param>
        void PopulateDetail(IAccountAttorneyInvoice accAttInv);

        /// <summary>
        /// 
        /// </summary>
        void ResetInputs();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Data;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    public interface IConsultantDecline : IViewBase
    {
        /// <summary>
        /// An event for handling when the SubmitButton is clicked
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// An event for handling when the Cancel is clicked
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proposals"></param>
        void BindProposals(List<IProposal> proposals);

        /// <summary>
        /// Set whether the cancel button should be visible
        /// </summary>
        bool CancelButtonVisible { set; }

        /// <summary>
        /// Set whether the submit button should be visible
        /// </summary>
        bool SubmitButtonVisible { set; }

        /// <summary>
        /// 
        /// </summary>
        int SelectedProposalKey { get; }
    }
}

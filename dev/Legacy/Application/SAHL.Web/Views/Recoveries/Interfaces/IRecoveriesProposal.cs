using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Recoveries.Interfaces
{
    public interface IRecoveriesProposal : IViewBase
    {
        event EventHandler OnAddButtonClicked;
        event EventHandler OnCancelButtonClicked;

        void BindRecoveriesProposals(List<SAHL.Common.BusinessModel.Interfaces.IRecoveriesProposal> proposalList);

        double ShortfallAmount { get; }
        double RepaymentAmount { get; }
        DateTime? StartDate { get; }
        bool AOD { get; }
        bool AddPanelVisible { set; }
        bool AddButtonVisible { set; }
        bool CancelButtonVisible { set; }
    }
}

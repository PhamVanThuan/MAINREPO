using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ICreditScoringSummary : IViewBase
    {
        event KeyChangedEventHandler OnScoreGridSelectedIndexChanged;
        void BindScoreGrid(IEventList<IApplicationCreditScore> scores);
        void BindApplicantGrid(IApplicationCreditScore acs);
        void BindRuleGrid(IApplicationCreditScore acs);
        void ShowGrids();
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IWorkflowRuleSetMaint : IViewBase
    {
        event EventHandler OnSubmitClick;
        event EventHandler OnRuleSetChanged;

        void BindRuleSetList(IList<IWorkflowRuleSet> Sets);
        void BindRuleList(IList<IRuleItem> AllRules);
        void DoSelection(IList<int> Keys);
        int GetSelectedRuleSet {get;}

        ListItemCollection CheckListBoxItems { get;}
    }
}

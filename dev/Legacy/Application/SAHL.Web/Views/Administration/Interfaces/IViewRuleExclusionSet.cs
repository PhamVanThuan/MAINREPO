using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public class BindRuleExclusionSet
    {
        int _Key;
        public int Key { get { return _Key; } }
        string _Desc;
        public string Desc { get { return _Desc; } }
        string _Name;
        public string Name { get { return _Name; } }
        public BindRuleExclusionSet() { }
    }

    public interface IViewRuleExclusionSet : IViewBase
    {
        /// <summary>
        /// Shows / hides the Search pane
        /// </summary>
        bool VisibleSearch { set; }
        /// <summary>
        /// Shows / hides the RuleSet Grid
        /// </summary>
        bool VisibleRuleSetGrid { set; }
        /// <summary>
        /// Shows / hides the Add / Edit pane for a single rule
        /// </summary>
        bool VisibleRuleMaint { set; }
        /// <summary>
        /// Sets whether the submit button is visible
        /// </summary>
        bool VisibleSubmit { set; }

        /// <summary>
        /// Fires when the search button is clicked
        /// </summary>
        event EventHandler OnSearchClicked;
        /// <summary>
        /// Fires when a Rule is added / updated and the submit button is clicked
        /// </summary>
        event EventHandler OnSubmiClick;
        /// <summary>
        /// Fires when a ruleset is selected in the grid
        /// </summary>
        event EventHandler OnRuleSetSelected;

        int RuleSetKey { get; }
        string RuleSetDesc { get; }
        string RuleSetName { get; }
    }
}

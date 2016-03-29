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
using System.Collections.Generic;


namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IViewRuleExclusionSetRule : IViewBase
    {
        /// <summary>
        /// Shows / hides the list of rulesets
        /// </summary>
        bool VisibleRuleSetGrid { set; }
        /// <summary>
        /// 
        /// </summary>
        bool VisibleMap { set; }
        /// <summary>
        /// 
        /// </summary>
        bool VisibleSubmit { set; }

        /// <summary>
        /// Gets whether the EnforceRuleCheckbox has been checked.
        /// </summary>
        bool Enforce { get; }
        bool Disable { get; }

        /// <summary>
        /// Fires when the search button is clicked
        /// </summary>
        event EventHandler OnSearchClicked;
        /// <summary>
        /// Fires when a Rule membership is added / updated and the submit button is clicked
        /// </summary>
        event EventHandler OnSubmiClick;
        /// <summary>
        /// Fires when a ruleset is selected in the grid
        /// </summary>
        event EventHandler OnRuleSetSelected;

        event EventHandler OnRuleAdded;
        event EventHandler OnRuleRemoved;
        void BindMapping(List<RuleItemBind> InRuleSet, List<RuleItemBind> NotInRuleSet);
    }
}

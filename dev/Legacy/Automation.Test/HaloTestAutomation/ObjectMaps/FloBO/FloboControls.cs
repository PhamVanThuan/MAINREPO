using ObjectMaps.NavigationControls;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.FloboControls
{
    public abstract class MenuControls : BaseNavigation
    {
        //Tabs Menu
        [FindBy(Id = "__tab_ctl00_tabsMenu_pnlTasks")]
        protected Span tabTasks { get; set; }

        //flobo
        [FindBy(Text = "WorkFlows")]
        protected Link linkWorkFlows { get; set; }

        protected Link linkWorklist(string worklist)
        {
            Regex expression = new Regex("^" + worklist + @" \([0-9]*\)$");
            return base.Document.Link(Find.ByText(regex: expression));
        }

        protected ImageCollection imageCollectionTreeImage
        {
            get
            {
                return base.Document.Images.Filter(Find.By("onmouseover", "this.src='../../Images/delete.png'"));
            }
        }

        [FindBy(Id = "__tab_ctl00_tabsMenu_pnlMenu")]
        protected Span tabMenu { get; set; }

        [FindBy(Text = "Workflow Search")]
        protected Link linkWorkflowSearch { get; set; }

        [FindBy(Text = "Batch Reassign")]
        protected Link linkBatchReassign { get; set; }

        [FindBy(Title = "Application Calculator")]
        protected Link linkApplicationCalculator { get; set; }

        [FindBy(Title = "Lead Capture")]
        protected Link linkLeadCapture { get; set; }

        [FindBy(Title = "Application Wizard")]
        protected Link linkApplicationWizard { get; set; }

        [FindBy(Title = "Create Application")]
        protected Link linkCreateApplication { get; set; }
    }
}
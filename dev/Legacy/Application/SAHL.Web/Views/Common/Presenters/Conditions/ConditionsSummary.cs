using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters.Conditions
{
    /// <summary>
    /// 
    /// </summary>
    public class ConditionsSummary : SAHLCommonBasePresenter<IConditionsSummary>
    {
        private IConditionsRepository conditionsRepository;
        private CBONode cboNode; // = null;
        private List<string> loanConditions = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ConditionsSummary(IConditionsSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;
            //Get the CBO Node  
            cboNode = CBOManager.GetCurrentCBONode(View.CurrentPrincipal) as CBOMenuNode;
            if (cboNode == null) throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
            conditionsRepository = RepositoryFactory.GetRepository<IConditionsRepository>();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;
            loanConditions = conditionsRepository.GetLoanConditions(cboNode.GenericKey);
                BindData();
                RegisterClientScripts();
                PrivateCacheData.Remove("ConditionsExist");

        }

        void BindData()
        {
            for (int x = 0; x < loanConditions.Count; x++)
            {
                ListItem li = new ListItem();
                li.Text = conditionsRepository.ConvertStringForHTMLDisplay(loanConditions[x]);
                li.Value = Convert.ToString(x);
                View.AddListBoxItem(li);
            }

        }

        void RegisterClientScripts()
        {

            System.Text.StringBuilder mBuilder = new System.Text.StringBuilder();
            // Clear and set the text display box with the selected text 
            mBuilder.AppendLine("function cleartext(TextToView)");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("  document.getElementById('" + View.GettxtDisplayClientID + "').value = TextToView;");
            mBuilder.AppendLine("}");

            View.RegisterClientScripts(mBuilder);


        }
    }
}

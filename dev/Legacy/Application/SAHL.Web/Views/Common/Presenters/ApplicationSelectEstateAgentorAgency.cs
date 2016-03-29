using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using NHibernate.Mapping;
using System.Collections;
using System.Collections.Generic;
using SAHL.Web.Views.Administration.Presenters.EstateAgent;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Not duplicating functionality and code so rather inherit, override and extend
    /// </summary>
    public class ApplicationSelectEstateAgentorAgency : EstateAgentBase
    {
        int _applicationKey;
        CBOMenuNode _node;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicationSelectEstateAgentorAgency(SAHL.Web.Views.Administration.Interfaces.IEstateAgent view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node != null)
                _applicationKey = Convert.ToInt32(_node.GenericKey);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.AdminButtonRowVisible = false;
            _view.ApplicationButtonRowVisible = true;
            _view.SubmitButtonClicked +=new EventHandler(_view_SubmitButtonClicked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_SubmitButtonClicked(object sender, EventArgs e)
        {
            // Add exclusion set
            this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

            int leKey;
            Int32.TryParse(GetSelectedItemValue(DataTableColumn.LegalEntityKey), out leKey);
            IX2Service svc = ServiceFactory.GetService<IX2Service>();

            if (leKey > 0)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    OSRepo.GenerateApplicationRole((int)OfferRoleTypes.EstateAgent, _applicationKey, leKey, true);
                    txn.VoteCommit();
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                    {
                        svc.CancelActivity(_view.CurrentPrincipal);
                        throw;
                    }

                }
                finally
                {
                    txn.Dispose();
                }
            }
            else
            {
                _view.Messages.Add(new Error("No item selected to update.", "No item selected to update."));
            }

            if (_view.IsValid)
            {
                svc.CompleteActivity(_view.CurrentPrincipal, null, false);
                svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }

            this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityLeadApplicants);
        }
    }
}

using System;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters.EstateAgent
{
	/// <summary>
	/// EstateAgent Select
	/// </summary>
	public class EstateAgentSelect : EstateAgentBase
	{

        int _applicationKey;
        CBOMenuNode _node;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public EstateAgentSelect(IExternalOrganisationStructure view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node != null && _node.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
                _applicationKey = Convert.ToInt32(_node.GenericKey);
            else
                throw new NotImplementedException("Only defined for Generic Key Type - Offer");
		}

		#region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            //need to set the node to expand to before the base init
            //need the LEOS key, from OSKey and LEKey
            //only do this for the initial page load, not post backs

            if (!_view.IsPostBack)
            {
                int leOSKey = OSRepo.GetLegalEntityOrganisationKeyByApplicationAndRoleType(_applicationKey, (int)OfferRoleTypes.EstateAgent);

                if (leOSKey > -1)
                    _view.SelectedNodeKey = leOSKey.ToString();
            }
            base.OnViewInitialised(sender, e);
            _view.SelectButtonClicked += new EventHandler(SelectClick);

            // Set Button Visibility
            _view.AddButtonVisible = false;
            _view.RemoveButtonVisible = false;
            _view.UpdateButtonVisible = false;
            _view.ViewButtonVisible = false;
            _view.SelectButtonVisible = true;
            _view.CancelButtonVisible = true;

            // Set DragDrop Functionality
            _view.AllowNodeDragging = false;

            // Set "Add to CBO" button visibility
            _view.AllowAddToCBO = false;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        protected virtual void SelectClick(object sender, EventArgs e)
        {
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
        }

		#endregion

		#region Methods

		#endregion
	}
}

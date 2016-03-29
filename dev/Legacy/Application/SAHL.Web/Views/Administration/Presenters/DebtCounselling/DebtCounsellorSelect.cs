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

namespace SAHL.Web.Views.Administration.Presenters.DebtCounselling
{
    /// <summary>
    /// 
    /// </summary>
    public class DebtCounsellorSelect : DebtCounsellorBase
    {
        List<ICacheObjectLifeTime> _lifeTimes;
        //int _applicationKey;
        //CBOMenuNode _node;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebtCounsellorSelect(IExternalOrganisationStructure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            //_node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            //if (_node != null && _node.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
            //    _applicationKey = Convert.ToInt32(_node.GenericKey);
            //else
            //    throw new NotImplementedException("Only defined for Generic Key Type - Offer");
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

            //if (!_view.IsPostBack)
            //{
            //    int leOSKey = OSRepo.GetLegalEntityOrganisationKeyByApplicationAndRoleType(_applicationKey, (int)OfferRoleTypes.EstateAgent);

            //    if (leOSKey > -1)
            //        _view.SelectedNodeKey = leOSKey.ToString();
            //}
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

            _view.SearchServiceMethod = SAHL.Web.AJAX.WebServiceUrls.GetDebtCounsellorByNCRRegistrationNumber;
            _view.AllowSearch = true;
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
        protected override void CancelClick(object sender, EventArgs e)
        {
            //if the return view has been specified, then navigate there
            //other wise use the default as per the web config, this was to simplify navigation for the temp 
            //DC capture screens. This entire event handler can be removed
            if (GlobalCacheData.ContainsKey(ViewConstants.CancelView))
                Navigator.Navigate(GlobalCacheData[ViewConstants.CancelView].ToString());
            else
                base.CancelClick(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification="New rule added to FxCop, avoiding refactor")]
        protected virtual void SelectClick(object sender, EventArgs e)
        {
            int leKey;
            Int32.TryParse(GetSelectedItemValue(DataTableColumn.LegalEntityKey), out leKey);

            if (GlobalCacheData.ContainsKey(ViewConstants.DebtCounsellorLegalEntityKey))
                GlobalCacheData.Remove(ViewConstants.DebtCounsellorLegalEntityKey);

            if (leKey > 0)
            {
                GlobalCacheData.Add(ViewConstants.DebtCounsellorLegalEntityKey, leKey, LifeTimes);
            }
            else
            {
                _view.Messages.Add(new Error("No item selected to update.", "No item selected to update."));
            }

            if (_view.IsValid)
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.SelectView))
                    Navigator.Navigate(GlobalCacheData[ViewConstants.SelectView].ToString());
                else
                    Navigator.Navigate("Select");
            }
        }

        #endregion

        #region Methods

        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("AdminDebtCounsellorSelect");
                    views.Add("DebtCounsellorUpdate");
                    views.Add("DebtCounsellingInitiatorReason");
                    views.Add("DebtCounsellingCreateCase");

                    views.Add("CreateMigrateDCCase");
                    views.Add("CaseLegalEntities");
                    views.Add("CaseDetail");
                    views.Add("CreateMigrateDCProposal");

                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return _lifeTimes;
            }
        }

        #endregion
    }
}

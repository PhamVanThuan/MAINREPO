using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.UI;

using SAHL.Common.DomainMessages;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationWizardSummary : SAHLCommonBasePresenter<IApplicationWizardSummary>
    {
        IApplication _app;
        IApplicationRepository _appRepo;
        IList<ILegalEntity> _lstLegalEntities;
        public ApplicationWizardSummary(IApplicationWizardSummary view, SAHLCommonBaseController controller)
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
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnAddButtonClicked += new EventHandler(_view_OnAddButtonClicked);
            _view.OnCalculateButtonClicked += new EventHandler(_view_OnCalculateButtonClicked);
            _view.OnFinishedButtonClicked += new EventHandler(_view_OnFinishedButtonClicked);
            _view.OnUpdateButtonClicked += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnUpdateButtonClicked);

            _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            int key = -1;
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
            {
                key = int.Parse(GlobalCacheData[ViewConstants.ApplicationKey].ToString());
            }
            if (key != -1)
            {
                _app = _appRepo.GetApplicationByKey(key);
            }

            _lstLegalEntities = new List<ILegalEntity>();
            if (_app != null)
            {
                IReadOnlyEventList<IApplicationRole> lstRoles = _app.GetApplicationRolesByType(SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant);
                foreach (IApplicationRole role in lstRoles)
                {
                    _lstLegalEntities.Add(role.LegalEntity);
                }
                _view.BindLoanGrid(_app);
                _view.BindLegalEntities(_lstLegalEntities, _app);
            }

            if (GlobalCacheData.ContainsKey("SKIPCALCULATOR"))
            {
                GlobalCacheData.Remove("SKIPCALCULATOR");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnUpdateButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key.ToString(), lifeTimes);

            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                GlobalCacheData.Remove(ViewConstants.ApplicationKey);
            if (_app != null)
            {
                GlobalCacheData.Add(ViewConstants.ApplicationKey, _app.Key, lifeTimes);
            }

            if (GlobalCacheData.ContainsKey("SKIPCALCULATOR"))
                GlobalCacheData.Remove("SKIPCALCULATOR");

            if (_app.ApplicationType.Key != (int)SAHL.Common.Globals.OfferTypes.Unknown)
            {
                GlobalCacheData.Add("SKIPCALCULATOR", true, lifeTimes);
            }
            _view.Navigator.Navigate("Update");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnFinishedButtonClicked(object sender, EventArgs e)
        {
            List<CBONode> nodes = CBOManager.GetMenuNodes(_view.CurrentPrincipal, CBONodeSetType.X2);

            TaskListNode taskListNode = null;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is TaskListNode)
                {
                    taskListNode = nodes[i] as TaskListNode;
                    break;
                }
            }

            if (taskListNode == null)
            {
                taskListNode = new TaskListNode(null);
                CBOManager.AddCBOMenuNode(_view.CurrentPrincipal, null, taskListNode, CBONodeSetType.X2);
            }


            //had to do this because switching between cbo tabs does not set IsMenuPostback to true
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
            {
                GlobalCacheData.Remove(ViewConstants.ApplicationKey);
            }

            if (GlobalCacheData.ContainsKey("SKIPCALCULATOR"))
            {
                GlobalCacheData.Remove("SKIPCALCULATOR");
            }

            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
            {
                GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            }

            // navigate to the workflow redirect view

            Navigator.Navigate("X2InstanceRedirect");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCalculateButtonClicked(object sender, EventArgs e)
        {
            IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();

            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                GlobalCacheData.Remove(ViewConstants.ApplicationKey);
            if (_app != null)
            {
                GlobalCacheData.Add(ViewConstants.ApplicationKey, _app.Key, lifeTimes);
            }

            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, _lstLegalEntities[0].Key.ToString(), lifeTimes);
            GlobalCacheData.Add("MUSTNAVIGATE", false, lifeTimes);
            _view.Navigator.Navigate("Calculator");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnAddButtonClicked(object sender, EventArgs e)
        {
            IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();

            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
            {
                GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            }

            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                GlobalCacheData.Remove(ViewConstants.ApplicationKey);
            if (_app != null)
            {
                GlobalCacheData.Add(ViewConstants.ApplicationKey, _app.Key, lifeTimes);
            }

            if (GlobalCacheData.ContainsKey("SKIPCALCULATOR"))
                GlobalCacheData.Remove("SKIPCALCULATOR");

            if (_app.ApplicationType.Key != (int)SAHL.Common.Globals.OfferTypes.Unknown)
            {
                GlobalCacheData.Add("SKIPCALCULATOR", true, lifeTimes);
            }
            _view.Navigator.Navigate("Add");
        }
    }
}

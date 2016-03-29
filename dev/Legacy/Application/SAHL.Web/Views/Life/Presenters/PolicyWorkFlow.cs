using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

using SAHL.Common.CacheData;
using SAHL.Common;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class PolicyWorkFlow : PolicyBase
    {
        private InstanceNode _node;
        private IApplicationLife _applicationLife;
        private long _instanceID = -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PolicyWorkFlow(IPolicy view, SAHLCommonBaseController controller)
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
            if (!_view.ShouldRunPage)
                return;

            // Get the CBO Node    
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            // Get values from the LifeOrigination Data 
            _instanceID = Convert.ToInt64(_node.X2Data["InstanceID"]);
            base.ContactNumber = _node.X2Data["ContactNumber"].ToString();

            _applicationLife = ApplicationRepo.GetApplicationLifeByKey((int)_node.GenericKey);
            
            base.AccountKey = _applicationLife.Account.Key;
 
            _view.OnRecalculatePremiumsButtonClicked += new EventHandler(OnRecalculatePremiumsButtonClicked);
            _view.OnAcceptPlanButtonClicked += new EventHandler(OnAcceptPlanButtonClicked);
            _view.OnDeclinePlanButtonClicked += new EventHandler(OnDeclinePlanButtonClicked);
            _view.OnQuoteRequiredButtonClicked += new EventHandler(OnQuoteRequiredButtonClicked);
            _view.OnConsideringButtonClicked += new EventHandler(OnConsideringButtonClicked);
            _view.OnContactPersonSelectedIndexChanged += new KeyChangedEventHandler(OnContactPersonSelectedIndexChanged);

            _view.ShowReassuranceFields = false;

            // Check the premiums on the OfferLife table,
            // if they are zero then recalculate the premiums
            // this will eventually move back into the workflow helper(lifehelper) once the premium calcs are moved into the domain.
            if (_applicationLife.MonthlyPremium == 0 && _applicationLife.YearlyPremium == 0)
            {
                using (new TransactionScope())
                {
                    // peform the premium recalculation
                    RecalculatePremiums();
                }
                // reget the application to reflect the new premiums
                _applicationLife.Refresh();
            }

            // Get the data and bind controls
            base.OnViewInitialised(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;


            // Show the WorkFlowHeader
            _view.ShowWorkFlowHeader = true;
            _view.WorkFlowButtonsVisible = true;

            _view.ShowAddLifeButton = true;
            _view.ShowRemoveLifeButton = true;

            _view.ShowRecalculatePremiumsButton = true;
            _view.ShowPremiumCalculatorButton = true;

            // Setup other fields
            _view.ContactPersonEnabled = true;
        }

        void OnAcceptPlanButtonClicked(object sender, EventArgs e)
        {
            if (_view.Messages.ErrorMessages.Count == 0)
            {
                // Navigate to the next State
                X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                svc.WorkFlowWizardNext(_view.CurrentPrincipal, _view.ViewName, _view.Navigator);
            }
        }

        void OnConsideringButtonClicked(object sender, EventArgs e)
        {
            // Start the Create Callback activity and Navigate to callback screen
            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
            svc.StartActivity(_view.CurrentPrincipal, _instanceID, "Create Callback", new Dictionary<string, string>(), false);
            _view.Navigator.Navigate("Considering");
        }

        void OnQuoteRequiredButtonClicked(object sender, EventArgs e)
        {
            // Start the Send Quote activity and Navigate to correspondence screen
            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
            svc.StartActivity(_view.CurrentPrincipal, _instanceID, "Send Quote", new Dictionary<string, string>(), false);

            _view.Navigator.Navigate("Quote");
        }

        void OnDeclinePlanButtonClicked(object sender, EventArgs e)
        {
            if (_view.Messages.ErrorMessages.Count == 0)
            {
                // Navigate to the next State
                X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                svc.StartActivity(_view.CurrentPrincipal, _instanceID, "Decline Quote", new Dictionary<string,string>(), false);
                _view.Navigator.Navigate("Decline");
            }
        }

        void OnRecalculatePremiumsButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();

            try
            {
                // Recalculate Premiums
                RecalculatePremiums();

                // Insert Stage Transition
                StageDefinitionRepo.SaveStageTransition(_applicationLife.Key, Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination), SAHL.Common.Constants.StageDefinitionConstants.RecalculatePremiums, "Premiums Recalculated", CurrentADUser);

                txn.VoteCommit();

                // Refresh Screen - this is not ideal and should prob be changed at a later stage time permitting
                // We need to refresh the screen so that the new data is reteived and re-bound
                _view.Navigator.Navigate(_view.ViewName);
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
        }

        private void RecalculatePremiums()
        {
            IAccountLifePolicy accountLifePolicy = _applicationLife.Account as IAccountLifePolicy;
            LifeRepo.RecalculateSALifePremium(accountLifePolicy, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnContactPersonSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            // Get the Contact Persons' LegalEntity object
            int policyHolderLEKey = Convert.ToInt32(e.Key);

            string oldContactName = "";
            if (AccountLifePolicy.CurrentLifeApplication.PolicyHolderLegalEntity != null)
                oldContactName = AccountLifePolicy.CurrentLifeApplication.PolicyHolderLegalEntity.GetLegalName(LegalNameFormat.InitialsOnly);

            // update the offerLife with the new contact person
            if (AccountLifePolicy.CurrentLifeApplication.PolicyHolderLegalEntity == null || policyHolderLEKey != AccountLifePolicy.CurrentLifeApplication.PolicyHolderLegalEntity.Key)
            {
                foreach (ILegalEntity le in lstLegalEntities)
                {
                    if (le.Key == policyHolderLEKey)
                    {
                        using (new TransactionScope(TransactionMode.New))
                        {
                            // update the offerLife with the new contact person
                            AccountLifePolicy.CurrentLifeApplication.PolicyHolderLegalEntity = le;
                            ApplicationRepo.SaveApplication(AccountLifePolicy.CurrentLifeApplication);

                            // write the stage transition  record
                            string comments = "From " + oldContactName + " to " + le.GetLegalName(LegalNameFormat.InitialsOnly);
                            StageDefinitionRepo.SaveStageTransition(AccountLifePolicy.CurrentLifeApplication.Key, Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination), SAHL.Common.Constants.StageDefinitionConstants.ContactPersonChanged, comments, CurrentADUser);
                        }
                        break;
                    }
                }
            }            
        }
    }
}

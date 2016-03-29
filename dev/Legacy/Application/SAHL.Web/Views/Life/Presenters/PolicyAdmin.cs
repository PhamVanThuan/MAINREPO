using System;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class PolicyAdmin : PolicyBase
    {
        private CBOMenuNode _node;
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PolicyAdmin(IPolicy view, SAHLCommonBaseController controller)
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
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _view.ConfirmMode = false;

            base.AccountKey = Convert.ToInt32(_node.GenericKey);

            //_view.OnRemoveLifeButtonClicked += new KeyChangedEventHandler(OnRemoveLifeButtonClicked);
            _view.OnRecalculatePremiumsButtonClicked += new EventHandler(OnRecalculatePremiumsButtonClicked);

            _view.ShowReassuranceFields = true;

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


            // Hide the WorkFlowHeader
            _view.ShowWorkFlowHeader = false;

            // Disable contact person dropdown
            _view.ContactPersonEnabled = false;

            // Setup Buttons
            _view.WorkFlowButtonsVisible = false;
            _view.ShowAddLifeButton = false;
            _view.ShowRemoveLifeButton = false;
            _view.ShowRecalculatePremiumsButton = false;
            _view.ShowPremiumCalculatorButton = false;

            // check if the user is an admin user
            bool adminUser = LifeRepo.IsAdminUser(_view.CurrentPrincipal);

            // if there is a life Policy 
            if (AccountLifePolicy.LifePolicy != null)
            {
                if (adminUser == true)
                {
                    // the user is an "Admin" user then show the buttons
                    switch (AccountLifePolicy.LifePolicy.LifePolicyStatus.Key)
                    {
                        case (int)SAHL.Common.Globals.LifePolicyStatuses.Accepted:
                        case (int)SAHL.Common.Globals.LifePolicyStatuses.Acceptedtocommenceon1st:
                        case (int)SAHL.Common.Globals.LifePolicyStatuses.Inforce:
                        case (int)SAHL.Common.Globals.LifePolicyStatuses.Prospect:
                            _view.ShowAddLifeButton = true;
                            _view.ShowRemoveLifeButton = true;
                            _view.ShowRecalculatePremiumsButton = true;
                            _view.ShowPremiumCalculatorButton = true;
                            break;
                        default:
                            break;
                    }
                }

                // Setup to show Claim Status information
                if (AccountLifePolicy.LifePolicy.ClaimStatus != null && AccountLifePolicy.LifePolicy.ClaimType != null)
                    _view.ShowClaimStatusInformation = true;
            }
        }

        void OnRecalculatePremiumsButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                // Recalculate Premiums
                LifeRepo.RecalculateSALifePremium(AccountLifePolicy, true);

                // Insert Stage Transition
                StageDefinitionRepo.SaveStageTransition(AccountLifePolicy.Key, Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination), SAHL.Common.Constants.StageDefinitionConstants.RecalculatePremiums, "Premiums Recalculated", CurrentADUser);

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
    }
}

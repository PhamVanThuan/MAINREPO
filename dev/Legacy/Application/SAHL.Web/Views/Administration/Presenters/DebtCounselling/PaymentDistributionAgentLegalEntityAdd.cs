using System;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters.DebtCounselling
{
	/// <summary>
	/// PaymentDistributionAgentLegalEntity Add
	/// </summary>
	public class PaymentDistributionAgentLegalEntityAdd : PaymentDistributionAgentLegalEntityBase
	{
        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public PaymentDistributionAgentLegalEntityAdd(IExternalOrganisationStructureLegalEntity view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		#region Events

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            if (!_osParentKeySet || _osKeyParent < 1)
            {
                _view.Messages.Add(new Error("No Organisation Structure Key Set to Add to...", "No Organisation Structure Key Set to Add to..."));
                _view.ShouldRunPage = false;
                return;
            }

            _view.onSubmitButtonClicked += new EventHandler(_view_onAddButtonClicked);
            _view.OnOrganisationTypeSelectedIndexChanged += new KeyChangedEventHandler(_view_OnOrganisationTypeSelectedIndexChanged);
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            SetUpControlsForAdd();
        }       

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            SetUpViewForAdd();

            base.SetAddressCaptureEnabled();
        }

		#endregion

        #region Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected void _view_onAddButtonClicked(object sender, EventArgs e)
        {
            if (!InputValidation())
                return;
            
            //get the le from the cache if exists
            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntityKey))
            {
                _legalEntity = LERepo.GetLegalEntityByKey(_leKey);
            }
            //else add a new guy
            else
            {
                // Create a blank LE populate it and save it
                _legalEntity = LERepo.GetEmptyLegalEntity((LegalEntityTypes)View.SelectedLegalEntityType);
                _legalEntity.IntroductionDate = DateTime.Now;
            }

            // Get the details from the screen
            _view.PopulateLegalEntityDetails(_legalEntity);
			IPaymentDistributionAgentOrganisationNode pdaNode = SelectedParentOrgNode.AddChildNode(_legalEntity, _view.OrganisationType, _view.OrganisationStructureDescription);
			
			//Check if the legal entity is not being added a second time to the same Org Structure		
			
			if (pdaNode != null)
			{
				IRuleService svc = ServiceFactory.GetService<IRuleService>();
				if (svc.ExecuteRule(_view.Messages, "UserOrganisationStructureAlreadyHasLegalEntity", _legalEntity.Key, pdaNode.Key) == 1)
				{
					_view.ShouldRunPage = false;
					SetUpViewForAdd();
					return;
				}

			}
			


            // Save
            TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
            try
            {
                // the default rule exclusionset (LegalEntityPaymentDistributionAgent) will be applied here - configured in web.config
                // add additional exclusionset based on type
                if (_view.OrganisationType.Key==(int)SAHL.Common.Globals.OrganisationTypes.Branch_Originator)
                    this.ExclusionSets.Add(RuleExclusionSets.LegalEntityPaymentDistributionAgentBranch);
                else if (_view.OrganisationType.Key == (int)SAHL.Common.Globals.OrganisationTypes.Department)
                    this.ExclusionSets.Add(RuleExclusionSets.LegalEntityPaymentDistributionAgentDept);

                LERepo.SaveLegalEntityPaymentDistributionAgent(_legalEntity, false);

                // Get & Save Address Details
                if (_view.OrganisationType.Key != (int)SAHL.Common.Globals.OrganisationTypes.Department
                 && _view.OrganisationType.Key != (int)SAHL.Common.Globals.OrganisationTypes.Designation)           
                    CaptureAddress(_legalEntity, false);
       
                //do the OrgStructure and LE OrgStruct stuff with these
                
                if (_view.IsValid && pdaNode != null)
                {
                    pdaNode.ValidateEntity();
                    DebtCounsellingRepo.SavePaymentDistributionAgentOrganisationStructure(pdaNode);
                }

                if (_view.IsValid)
                    txn.VoteCommit();
                else
                    txn.VoteRollBack();

                this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityPaymentDistributionAgentBranch);
                this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityPaymentDistributionAgentDept);
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {             
                try
                {
                    txn.Dispose();
                }
                catch (Exception)
                {
                    if (_view.IsValid)
                        throw;
                }
            }

            if (_view.IsValid)
            {
                //set an item in the global cache so that the view can expand to the selected item
                Navigator.Navigate("Submit");
            }
        }

        #endregion
    }
}

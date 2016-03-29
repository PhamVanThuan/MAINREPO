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

namespace SAHL.Web.Views.Administration.Presenters.EstateAgentLegalEntity
{
	/// <summary>
	/// EstateAgentLegalEntity Add
	/// </summary>
	public class EstateAgentLegalEntityAdd : EstateAgentLegalEntityBase
	{
        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public EstateAgentLegalEntityAdd(IExternalOrganisationStructureLegalEntity view, SAHLCommonBaseController controller)
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
        }

		#endregion

        #region Methods

        protected void _view_OnOrganisationTypeSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
        }

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

            //run rules
            //RuleSvc.ExecuteRule(_view.Messages, "EstateAgentMultipleAgencies", _legalEntity);

            //save the le
            // Save
            TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
            try
            {
                //need to get a proper list of rules to exclude, use all for now
                //this.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);

                LERepo.SaveLegalEntityEstateAgent(_legalEntity, false);

                // Get & Save Address Details
                if (_view.AddressCaptureEnabled)
                    CaptureAddress(_legalEntity, false);
        
                //do the OrgStructure and LE OrgStruct stuff with these
                IEstateAgentOrganisationNode eaN = SelectedParentOrgNode.AddChildNode(_legalEntity, _view.OrganisationType, _view.OrganisationStructureDescription);
                if (_view.IsValid && eaN != null)
                {
                    eaN.ValidateEntity();
                    EARepo.SaveEstateAgentOrganisationStructure(eaN);
                }

                #region OLD CODE
                //if (SelectedParentOrgNode.OrganisationType.Key == _view.OrganisationType.Key)
                //{

                //    if (_view.IsValid)
                //        //SelectedParentOrgStructure.Description = _view.OrganisationStructureDescription;
                //        throw new NotImplementedException("TODO");
                //        //EARepo.LinkEstateAgentOrganisationStructure(SelectedParentOrgNode, _legalEntity);

                //}
                //else
                //{
                //    IOrganisationStructure eaOrgStructure = EARepo.CreateEstateAgentOrganisationStructure(); 
                //    eaOrgStructure.OrganisationType = _view.OrganisationType; 
                //    eaOrgStructure.Description = _view.OrganisationStructureDescription;
                //    eaOrgStructure.Parent = SelectedParentOrgNode;
                //    eaOrgStructure.GeneralStatus = activeGeneralStatus;
                //    EARepo.UpdateEstateAgentOrganisationStructure(eaOrgStructure);
                //    if (_legalEntity.LegalEntityType.Key == (int)LegalEntityTypes.NaturalPerson)
                //    {
                //        svc.ExecuteRule(_view.Messages, "EstateAgentMultipleAgencies", _legalEntity);
                //    }
                //    else
                //    {
                //        //svc.ExecuteRule(_view.Messages, "OneLegalEntityInstanceInOrgStructure", _legalEntity);
                //        svc.ExecuteRule(_view.Messages, "LegalEntityEstateAgencyMandatoryAddress", _legalEntity);
                //    }
                    
                //    if (_view.IsValid)
                //        EARepo.LinkEstateAgentOrganisationStructure(eaOrgStructure, _legalEntity);
                    

                //}

                //if (_legalEntity.LegalEntityType.Key == (int)LegalEntityTypes.NaturalPerson)
                //{
                //    svc.ExecuteRule(_view.Messages, "EstateAgentMultipleAgencies", _legalEntity);
                //}
                //else
                //{
                //    //svc.ExecuteRule(_view.Messages, "OneLegalEntityInstanceInOrgStructure", _legalEntity);
                //    svc.ExecuteRule(_view.Messages, "LegalEntityEstateAgencyMandatoryAddress", _legalEntity);
                //}

                #endregion

                if (_view.IsValid)
                    txn.VoteCommit();
                else
                    txn.VoteRollBack();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                //this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityExcludeAll);
                //db can rollback txn when rules fail, need to not throw ex 
                //if view is valid
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

            //And Navigate
            if (_view.IsValid)
            {
                //set an item in the global cache so that the view can expand to the selected item
                Navigator.Navigate("Submit");
            }


        }

        #endregion
    }
}

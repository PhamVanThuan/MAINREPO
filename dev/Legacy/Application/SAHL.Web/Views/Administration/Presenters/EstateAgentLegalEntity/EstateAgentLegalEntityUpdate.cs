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
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters.EstateAgentLegalEntity
{
	/// <summary>
	/// EstateAgentLegalEntity Update
	/// </summary>
	public class EstateAgentLegalEntityUpdate : EstateAgentLegalEntityBase
	{
        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public EstateAgentLegalEntityUpdate(IExternalOrganisationStructureLegalEntity view, SAHLCommonBaseController controller)
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

            _view.DisableAjaxFunctionality = true;
            if (!_leKeySet || _leKey < 1 || !_osKeySet || _osKey < 1)
            {
                _view.Messages.Add(new Error("No Legal Entity Key set", "No Legal Entity Key set"));
                _view.ShouldRunPage = false;
                return;
            }
            
            _view.onSubmitButtonClicked += new EventHandler(_view_onUpdateButtonClicked);

            _view.AddressCaptureEnabled = false;
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            SetUpControlsForUpdate();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            SetUpViewForUpdate();

            //set the ajax controls hidden so that the LE can not be swapped out
        }

		#endregion

		#region Methods

        protected void ReloadLegalEntityIfTypeChanged()
        {
            // do this check below so we can pick up if the user has changed the legalentity type (company types only)
            switch (_legalEntity.LegalEntityType.Key)
            {
                case (int)LegalEntityTypes.Company:
                case (int)LegalEntityTypes.CloseCorporation:
                case (int)LegalEntityTypes.Trust:
                    int legalEntityTypeKey = _view.SelectedLegalEntityType;
                    if (legalEntityTypeKey > 0 && legalEntityTypeKey != _legalEntity.LegalEntityType.Key)
                    {
                        // if we are here then the legalentity type has changed

                        // get the legalentitykey
                        int legalEntityKey = _legalEntity.Key;

                        // clear from the nhibernate session
                        CRepo.ClearFromNHibernateSession(_legalEntity);

                        // update the legalentity type via sql
                        LERepo.UpdateLegalEntityType(legalEntityKey, legalEntityTypeKey);
                        // re-get the legal entity
                        _legalEntity = LERepo.GetLegalEntityByKey(legalEntityKey);
                    }
                    break;
                default:
                    break;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected void _view_onUpdateButtonClicked(object sender, EventArgs e)
        {
            if (!InputValidation())
                return;

            //get the le from the cache if exists
            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntityKey))
            {
                _legalEntity = LERepo.GetLegalEntityByKey(_leKey);
            }
            else
            {
                _view.Messages.Add(new Error("No LegalEntity found to update...", "No LegalEntity found to update..."));
                return;
            }

            //update the LE Type if it has changed
            ReloadLegalEntityIfTypeChanged();

            // Get the details from the screen
            _view.PopulateLegalEntityDetails(_legalEntity);

            //save the le
            // Save
            TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
            try
            {
                //this.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);

                LERepo.SaveLegalEntityEstateAgent(_legalEntity, false);
                //if we are updating 
                SelectedOrgNode.Update(_legalEntity, _view.OrganisationType, _view.OrganisationStructureDescription);
                SelectedOrgNode.ValidateEntity();

                EARepo.SaveEstateAgentOrganisationStructure(SelectedOrgNode);

                // Get & Save Address Details
                //CaptureAddress(_legalEntity, true);

                if (_view.IsValid)
                    txn.VoteCommit();
                else
                    txn.VoteRollBack();

                //this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityExcludeAll);
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
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

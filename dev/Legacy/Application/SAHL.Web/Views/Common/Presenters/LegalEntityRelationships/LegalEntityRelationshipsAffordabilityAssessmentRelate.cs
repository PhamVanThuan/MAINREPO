using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using System;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityRelationships
{
    public class LegalEntityRelationshipsAffordabilityAssessmentRelate : SAHLCommonBasePresenter<ILegalEntityRelationships>
    {
        private ILookupRepository lookupRepository;
        private ILegalEntityRepository legalEntityRepository;
        private ILegalEntity relatedLegalEntity;
        private ILegalEntity legalEntity;
        private int legalEntityKey;

        public LegalEntityRelationshipsAffordabilityAssessmentRelate(ILegalEntityRelationships view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnSubmitButtonClick += new KeyChangedEventHandler(OnSubmitButtonClick);
            _view.OnCancelButtonClick += new EventHandler(OnCancelButtonClick);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.AddToCBOButtonVisible = false;
            _view.CancelButtonVisible = true;
            _view.SubmitButtonVisible = true;
            _view.ActionTableVisible = true;
            _view.LegalEntityInfoTableVisible = true;
            _view.SubmitButtonText = "Add";
            _view.SubmitButtonEnabled = true;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.GridPostBackType = GridPostBackType.None;

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();

            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
            {
                relatedLegalEntity = (ILegalEntity)GlobalCacheData[ViewConstants.LegalEntity];
            }

            if (GlobalCacheData.ContainsKey("AffordabilityAssessmentContributorLegalEntityKey"))
            {
                int.TryParse(GlobalCacheData["AffordabilityAssessmentContributorLegalEntityKey"].ToString(), out legalEntityKey);
            }

            if (legalEntityKey > 0)
            {
                legalEntity = legalEntityRepository.GetLegalEntityByKey(legalEntityKey);
            }

            if (relatedLegalEntity != null &&
                legalEntity != null)
            {
                _view.BindRelationshipTypes(lookupRepository.LegalEntityRelationshipTypes.BindableDictionary, String.Empty);
                _view.BindLabelMessage(relatedLegalEntity.GetLegalName(LegalNameFormat.Full));
                _view.BindRelationshipGrid(legalEntity.LegalEntityRelationships);
            }
        }

        protected virtual void OnCancelButtonClick(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
            {
                GlobalCacheData.Remove(ViewConstants.LegalEntity);
            }

            if (GlobalCacheData.ContainsKey("AffordabilityAssessmentContributorLegalEntityKey"))
            {
                GlobalCacheData.Remove("AffordabilityAssessmentContributorLegalEntityKey");
            }

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            // int keyType = cboNode.GenericKeyTypeKey;
            // int applicationKey = cboNode.GenericKey;
            if (cboNode.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
            {
                Navigator.Navigate("Cancel");
            }
            if (cboNode.GenericKeyTypeKey == (int)GenericKeyTypes.AffordabilityAssessment)
            {
                Navigator.Navigate("LinkIncomeContributors");
            }
        }

        protected void OnSubmitButtonClick(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            // Get the selected relationship
            int selectedLegalEntityRelationshipTypeKey = _view.SelectedLegalEntityRelationshipTypeKey;
            ILegalEntityRelationshipType legalEntityRelationshipType = lookupRepository.LegalEntityRelationshipTypes.ObjectDictionary[selectedLegalEntityRelationshipTypeKey.ToString()];

            if (relatedLegalEntity != null &&
                legalEntity != null &&
                legalEntityRelationshipType != null)
            {
                ILegalEntityRelationship legalEntityRelationship = legalEntityRepository.CreateLegalEntityRelationship();

                // Attempt to save
                TransactionScope ts = new TransactionScope();
                try
                {
                    legalEntityRelationship.LegalEntity = legalEntity;
                    legalEntityRelationship.RelatedLegalEntity = relatedLegalEntity;
                    legalEntityRelationship.LegalEntityRelationshipType = legalEntityRelationshipType;

                    // Save the Relationship...
                    legalEntityRepository.SaveLegalEntityRelationship(legalEntityRelationship);

                    ts.VoteCommit();
                }
                catch (Exception)
                {
                    ts.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    ts.Dispose();
                }

                if (_view.Messages.Count == 0)
                {
                    if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
                    {
                        GlobalCacheData.Remove(ViewConstants.LegalEntity);
                    }

                    if (GlobalCacheData.ContainsKey("AffordabilityAssessmentContributorLegalEntityKey"))
                    {
                        GlobalCacheData.Remove("AffordabilityAssessmentContributorLegalEntityKey");
                    }

                    CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
                    // int keyType = cboNode.GenericKeyTypeKey;
                    // int applicationKey = cboNode.GenericKey;
                    if (cboNode.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
                    {
                        Navigator.Navigate("Cancel");
                    }
                    if (cboNode.GenericKeyTypeKey == (int)GenericKeyTypes.AffordabilityAssessment)
                    {
                        Navigator.Navigate("LinkIncomeContributors");
                    }
                }
            }
        }
    }
}
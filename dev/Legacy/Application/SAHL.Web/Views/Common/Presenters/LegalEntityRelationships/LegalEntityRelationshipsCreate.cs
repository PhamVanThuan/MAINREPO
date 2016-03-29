THIS MUST BE DELETE AS WE NO LONGER NEED IT

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using NHibernate;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityRelationships
{
    public class LegalEntityRelationshipsCreate : LegalEntityRelationshipsBase
    {
        public LegalEntityRelationshipsCreate(ILegalEntityRelationships view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

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

            // Override this to always enable.
            _view.SubmitButtonEnabled = true;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.GridPostBackType = GridPostBackType.None;
            
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            base.LoadRelatedLEFromCache();

            _view.BindRelationshipTypes(LookupRepository.LegalEntityRelationshipTypes.BindableDictionary, String.Empty);

            _view.BindLabelMessage(base.LabelString);
        }

        protected override void OnSubmitButtonClick(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            base.OnSubmitButtonClick(sender, e);

            // Get the selected relationship
            int selectedLegalEntityRelationshipTypeKey = _view.SelectedLegalEntityRelationshipTypeKey;

            LoadRelatedLEFromCache();

            if (selectedLegalEntityRelationshipTypeKey > -1
                && RelatedLegalEntity != null)
            {
                ILegalEntityRelationship legalEntityRelationship = LegalEntityRepository.CreateLegalEntityRelationship();

                // Attempt to save
                TransactionScope ts = new TransactionScope();
                try
                {
                    if (base.RelatedLegalEntity.Key != base.LegalEntity.Key)
                    {
                        // if the LE we are getting from cache is an existing (persisted one) then update the current session with thsi object
                        if (base.RelatedLegalEntity.Key > 0)
                        {
                            ICommonRepository commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();
                            commonRepo.UpdateInCurrentNHibernateSession(base.RelatedLegalEntity);
                        }

                        // save the related legalentity
                        LegalEntityRepository.SaveLegalEntity(base.RelatedLegalEntity, false);
                    }

                    // setup the relationship record
                    legalEntityRelationship.LegalEntity = base.LegalEntity;

                    if (base.RelatedLegalEntity.Key != base.LegalEntity.Key)
                        legalEntityRelationship.RelatedLegalEntity = base.RelatedLegalEntity;
                    else
                        legalEntityRelationship.RelatedLegalEntity = base.LegalEntity;

                    legalEntityRelationship.LegalEntityRelationshipType = LookupRepository.LegalEntityRelationshipTypes.ObjectDictionary[selectedLegalEntityRelationshipTypeKey.ToString()];

                    LegalEntity.LegalEntityRelationships.Add(_view.Messages, legalEntityRelationship);

                    // save the main legal entity
                    LegalEntityRepository.SaveLegalEntity(LegalEntity, false);

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
                    Navigator.Navigate("Submit");
            }
        }
    }
}

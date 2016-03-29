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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityRelationships
{
    public class LegalEntityRelationshipsUpdate : LegalEntityRelationshipsBase
    {
        public LegalEntityRelationshipsUpdate(ILegalEntityRelationships view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            _view.AddToCBOButtonVisible = false;
            _view.CancelButtonVisible = true;
            _view.SubmitButtonVisible = true;
            _view.ActionTableVisible = true;
            _view.LegalEntityInfoTableVisible = false;

            _view.SubmitButtonText = "Update";

            if (RelationshipsExists)
                _view.SubmitButtonEnabled = true;
            else
                _view.SubmitButtonEnabled = false;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.GridPostBackType = GridPostBackType.SingleClick;

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            // We should be setting this to the grid selected index. However on the first load, this will be the first item(provided the list has items)
            string defaultItem = String.Empty;

            if (LegalEntity.LegalEntityRelationships.Count > 0)
                defaultItem = LegalEntity.LegalEntityRelationships[0].LegalEntityRelationshipType.Key.ToString();

            _view.BindRelationshipTypes(LookupRepository.LegalEntityRelationshipTypes.BindableDictionary, defaultItem);

        }

        protected override void OnGridItemSelected(object sender, KeyChangedEventArgs e)
        {
            base.OnGridItemSelected(sender, e);

            string selectedRelationshipType = (string)e.Key;
            _view.BindRelationshipTypes(LookupRepository.LegalEntityRelationshipTypes.BindableDictionary, selectedRelationshipType);

        }

        protected override void OnSubmitButtonClick(object sender, KeyChangedEventArgs e)
        {
            base.OnSubmitButtonClick(sender, e);

            // Get the selected relationship
            int selectedLegalEntityRelationshipTypeKey = _view.SelectedLegalEntityRelationshipTypeKey;
            int selectedLegalEntityRelationshipIndex = _view.SelectedLegalEntityRelationshipIndex;

            if (selectedLegalEntityRelationshipIndex > -1 && LegalEntity.LegalEntityRelationships.Count > 0)
            {
                if (selectedLegalEntityRelationshipTypeKey > 0)
                    LegalEntity.LegalEntityRelationships[selectedLegalEntityRelationshipIndex].LegalEntityRelationshipType = LookupRepository.LegalEntityRelationshipTypes.ObjectDictionary[selectedLegalEntityRelationshipTypeKey.ToString()];
                else
                    LegalEntity.LegalEntityRelationships[selectedLegalEntityRelationshipIndex].LegalEntityRelationshipType = null;

                // Attempt to save
                TransactionScope ts = new TransactionScope();

                try
                {
                    // Save ...
                    LegalEntityRepository.SaveLegalEntity(LegalEntity, false);

                    ts.VoteCommit();

                    // Rebind the grid ...
                    _view.BindRelationshipGrid(LegalEntity.LegalEntityRelationships);
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
            }
        }
    }
}

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
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityRelationships
{
    public class LegalEntityRelationshipsDelete : LegalEntityRelationshipsBase
    {
        public LegalEntityRelationshipsDelete(ILegalEntityRelationships view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // this must happen before base call because the databind to the grid happens in the base call - without 
            // this the grid will not render correctly
            _view.GridPostBackType = GridPostBackType.NoneWithClientSelect;

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            _view.AddToCBOButtonVisible = false;
            _view.CancelButtonVisible = true;
            _view.SubmitButtonVisible = true;
            _view.ActionTableVisible = false;
            _view.LegalEntityInfoTableVisible = false;

            _view.SubmitButtonText = "Delete";
            _view.DeleteConfirmationVisible = true;

        }

        protected override void OnSubmitButtonClick(object sender, KeyChangedEventArgs e)
        {
            base.OnSubmitButtonClick(sender, e);

            // Get the selected relationship
            int selectedLegalEntityRelationshipIndex = _view.SelectedLegalEntityRelationshipIndex;

            if (selectedLegalEntityRelationshipIndex > -1
                && LegalEntity.LegalEntityRelationships.Count > 0)
            {
                LegalEntity.LegalEntityRelationships.RemoveAt(_view.Messages, selectedLegalEntityRelationshipIndex);

                // Attempt to save
                TransactionScope ts = new TransactionScope(TransactionMode.Inherits);

                try
                {
                    // Save ...
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

                // Rebind the grid ...
                if (LegalEntity.LegalEntityRelationships.Count > 0)
                    _view.SelectedLegalEntityRelationshipIndex = 0;
                _view.BindRelationshipGrid(LegalEntity.LegalEntityRelationships);
            }
        }
    }
}

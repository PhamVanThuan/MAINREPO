using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

using SAHL.Common.Authentication;
using SAHL.Common.Security;

namespace SAHL.Web.Views.Common.Presenters
{
    public class LegalEntityEnableUpdateUpdate : LegalEntityEnableUpdateBase
    {

        public LegalEntityEnableUpdateUpdate(ILegalEntityEnableUpdate view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        public ILegalEntity LegalEntityMock
        {
            set { LegalEntity = value; }
            get { return LegalEntity; }
        }
        
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            CBOMenuNode cboCurrentMenuNodeLegalEntity = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (cboCurrentMenuNodeLegalEntity != null)
                LegalEntity = LegalEntityRepository.GetLegalEntityByKey((int)cboCurrentMenuNodeLegalEntity.GenericKey);
            else
                throw new CBONodeNotFoundException(ResourceService.GetString(ResourceConstants.NodeNotFoundException));
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            _view.BindLabelMessage(ResourceService.GetString(ResourceConstants.LegalEntityEnableUpdateMessage));
            _view.BindLabelQuestion(ResourceService.GetString(ResourceConstants.LegalEntityEnableUpdateQuestion));

            _view.BindCancelButtonText("No");
            _view.BindSubmitButtonText("Yes");
        }

        protected override void OnSubmitButtonClick(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                LegalEntity.UserID = SAHLPrincipal.GetCurrent().Identity.Name;
                LegalEntity.ChangeDate = System.DateTime.Now;

                LegalEntityRepository.SaveLegalEntityWithExceptionStatus(LegalEntity, LegalEntityExceptionStatuses.InvalidIDNumber);

                ts.VoteCommit();

                // navigate 
                base.OnSubmitButtonClick(sender, e);
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

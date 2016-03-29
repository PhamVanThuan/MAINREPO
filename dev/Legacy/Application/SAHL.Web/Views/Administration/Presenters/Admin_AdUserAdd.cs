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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_AdUserAdd : Admin_AdUserBase
    {

        public Admin_AdUserAdd(IAduser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) 
                return;

            _view.OnSubmitClick += new EventHandler(_view_OnSubmitClick);
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) 
                return;

            _view.VisibleSubmit = true;
            //_view.SubmitText = "Add ADUser Record";
        }

        void _view_OnSubmitClick(object sender, EventArgs e)
        {
            if (base.ValidateInput() == false)
                return;

            TransactionScope txn = new TransactionScope(OnDispose.Rollback);
            try
            {
                if (_view.IsValid)
                {
                    IADUser user = base.OrganisationStructureRepo.CreateEmptyAdUser();

                    user.ADUserName = _view.AdUserName;
                    user.LegalEntity.FirstNames = _view.FirstName;
                    user.LegalEntity.Surname = _view.Surname;
                    user.LegalEntity.EmailAddress = _view.EMail;
                    user.LegalEntity.CellPhoneNumber = _view.CellPhoneNumber;
                    user.GeneralStatusKey = base.LookupRepo.GeneralStatuses[GeneralStatuses.Active];

                    base.OrganisationStructureRepo.SaveAdUser(user);

                    txn.VoteCommit();
                }
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

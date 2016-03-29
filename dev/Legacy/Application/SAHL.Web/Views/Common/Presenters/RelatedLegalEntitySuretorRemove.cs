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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters
{
    public class RelatedLegalEntitySuretorRemove : RelatedLegalEntity
    {
        private ILookupRepository _lookupReo;

        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RelatedLegalEntitySuretorRemove(IRelatedLegalEntity view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnRemoveButtonClicked += new KeyChangedEventHandler(OnRemoveButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            _view.AllowGridSelect = true;
            _view.AllowGridDoubleClick = false;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            base.OnViewInitialised(sender, e);

            _lookupReo = RepositoryFactory.GetRepository<ILookupRepository>();
        }

        void OnRemoveButtonClicked(object sender, KeyChangedEventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                // get the selected role record
                int roleKey = Convert.ToInt32(e.Key);
                IRole role = AccountRepo.GetRoleByKey(roleKey);

                // update the role to be inactive
                role.GeneralStatus = _lookupReo.GeneralStatuses[GeneralStatuses.Inactive];

                // save the role record
                AccountRepo.SaveRole(role);

                ts.VoteCommit();

                _view.Navigator.Navigate("Submit");

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

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            base.OnViewPreRender(sender, e);

            _view.RemoveButtonEnabled = true;
            _view.AddToMenuButtonEnabled = false;
            _view.CancelButtonEnabled = true;
        }
    }
}

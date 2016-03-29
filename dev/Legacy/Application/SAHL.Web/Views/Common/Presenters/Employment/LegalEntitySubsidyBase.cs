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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters.Employment
{
    public class LegalEntitySubsidyBase : LegalEntityEmploymentBase<ISubsidyDetails>
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntitySubsidyBase(ISubsidyDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;
            View.BackButtonClicked += new EventHandler(View_BackButtonClicked);
            View.CancelButtonClicked += new EventHandler(View_CancelButtonClicked);
        }

        void View_BackButtonClicked(object sender, EventArgs e)
        {
            View.Navigator.Navigate(PreviousView);
        }

        void View_CancelButtonClicked(object sender, EventArgs e)
        {
            ClearCachedData();
            View.ShouldRunPage = false;
            View.Navigator.Navigate("Cancel");
        }

        /// <summary>
        /// Gets all applications and accounts that the user is related to and binds them to the view.
        /// </summary>
        public void BindSubsidyAccounts()
        {
            // reload the legal entity as lazy items won't be available from the dead session
            ILegalEntity le = GetLegalEntity(View.CurrentPrincipal);

            IEventList<IAccount> accounts = new EventList<IAccount>();
            IEventList<IApplication> applications = new EventList<IApplication>();

            foreach (IRole role in le.Roles)
            {
                if (role.GeneralStatus.Key == (int)GeneralStatuses.Active && role.Account.AccountType == AccountTypes.MortgageLoan && role.Account.AccountStatus.Key == (int)AccountStatuses.Open)
                    accounts.Add(_view.Messages, role.Account);
            }
            foreach (IApplicationRole role in le.ApplicationRoles)
            {
                if (role.GeneralStatus.Key == (int)GeneralStatuses.Active && role.Application.ApplicationStatus.Key == (int)OfferStatuses.Open)
                    applications.Add(_view.Messages, role.Application);
            }
            View.BindAccounts(accounts, applications);
        }


    }
}

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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.UI;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters
{

    public class SellersRemove : ApplicantsOfferBase
    {
        /// <summary>
        /// Consructor. Gets the View and controller pairs.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SellersRemove(IApplicants view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Add any additional event hooks
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.OnRemoveButtonClicked += new KeyChangedEventHandler(OnRemoveButtonClicked);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage) return;

            // set the applicationroletypes to display
            base.ApplicationRoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.Seller);
            base.ApplicationRoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.Seller]));

            _view.GridHeading = "Sellers";

            // call the base initialise to handle the binding etc
            base.OnViewInitialised(sender, e);
        }

        void OnRemoveButtonClicked(object sender, KeyChangedEventArgs e)
        {
            TransactionScope txn = new TransactionScope();

            try
            {
                // get the selected LegalEntity       
                ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                ILegalEntity le = legalEntityRepo.GetLegalEntityByKey(_view.SelectedLegalEntityKey);

                // remove the role from the application
                base.Application.RemoveRolesForLegalEntity(_view.Messages, le, new int[] { _view.SelectedApplicationRoleTypeKey });

                // save the application
                ApplicationRepository.SaveApplication(base.Application);

                txn.VoteCommit();

                //remove the node here
                CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);

                _view.Navigator.Navigate("Submit");
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

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage) return;

            base.OnViewPreRender(sender, e);

            _view.ButtonsVisible = true;
        }
    }
}

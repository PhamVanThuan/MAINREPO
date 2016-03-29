using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Presenters;
using System;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    /// <summary>
    /// Legal Entity Domicilium Address - Display Presenter
    /// </summary>
    public class PersonalLoanLegalEntityDomiciliumAddressDisplayWorkflow : LegalEntityDomiciliumAddressBase
    {
        /// <summary>
        /// Constructor - Legal Entity Domicilium Address - Display
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PersonalLoanLegalEntityDomiciliumAddressDisplayWorkflow(ILegalEntityDomiciliumAddress view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// OnView Initialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            // walk up tree till we get a node with application key
            CBONode parentNode = node.GetParentNodeByType(SAHL.Common.Globals.GenericKeyTypes.Offer);

            // get the external role for this legalentity
            IExternalRole externalRole = base.legalEntity.GetActiveClientExternalRoleForOffer(parentNode.GenericKey);
            IExternalRoleDomicilium pendingExternalRoleDomicilium = null;

            if (externalRole != null)
            {
                if (externalRole.LegalEntity != null && externalRole.LegalEntity.ActiveDomicilium != null)
                {
                    _view.BindActiveDomiciliumAddress(externalRole.LegalEntity.ActiveDomicilium.LegalEntityAddress.Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.Comma));
                    _view.ShowActiveDomiciliumAddressRow = true;
                }

                pendingExternalRoleDomicilium = externalRole.PendingExternalRoleDomicilium;

                // we in workflow mode, show the ExternalRoleDomicilium
                if (pendingExternalRoleDomicilium != null)
                {
                    _view.BindDomiciliumAddress(pendingExternalRoleDomicilium.LegalEntityDomicilium.LegalEntityAddress.Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.Comma));
                }
            }
            _view.SetControlsForDisplay("Proposed Pending Domicilium Address");
        }
    }
}
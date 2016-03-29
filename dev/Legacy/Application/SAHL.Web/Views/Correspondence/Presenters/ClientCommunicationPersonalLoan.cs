using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Correspondence.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.Correspondence.Presenters
{
    public class ClientCommunicationPersonalLoan : ClientCommunicationBase
    {
        public ClientCommunicationPersonalLoan(IClientCommunication view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (base.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
            {
                base.GenericKey = base.Node.ParentNode.GenericKey;
            }

            var externalRoles = LegalEntityRepo.GetLegalEntityInformationByOfferKey(base.GenericKey);

            // setup bank details - this is based on the origination source of the account
            ICorrespondenceTemplate template = null;

            template = CRepo.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.SAHLBankDetails);
            _view.BankDetails = String.Format(template.Template, ApplicationRepository.GetApplicationByKey(base.GenericKey).ReservedAccount.Key);

            foreach (var externalRole in externalRoles)
            {
                base.RecipientsList.Add(new BindableRecipient(externalRole.LegalEntity, externalRole.ExternalRoleType.Description));
            }

            _view.SelectFirstItem = true;

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }
    }
}
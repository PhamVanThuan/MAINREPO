using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Correspondence.Interfaces;

namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
    public class CorrespondenceProcessingDebtCounsellingTerminationLetter : CorrespondenceProcessingMultipleWorkflowDebtCounsellor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CorrespondenceProcessingDebtCounsellingTerminationLetter(ICorrespondenceProcessing view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.DisplayClientsAndNCR = true;
            _view.CCDebtCounsellor = true;
            // setting the property below to TRUE will print one copy of the report for legalentities that share a domicilium address - applies to 'post' option only
            _view.PostSingleCopyForSharedDomiciliums = true;
            _view.EmailSingleCopyForSharedDomiciliums = true;

            base.OnViewInitialised(sender, e);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        protected override void OnSendButtonClicked(object sender, EventArgs e)
        {
            // do validation      
            if (ValidateInput() == false)
                return;

            base.OnSendButtonClicked(sender, e);
        }

        bool ValidateInput()
        {
            // 1. Ensure that at least one client is selected
            // 2. Ensure that in the case where the clients have different domicilium addresses that we are sending the docs to each client.

            string errorMessage = "";
            IList<int> uniqueSelectedDomiciliumAddressKeys = new List<int>();
            IList<ILegalEntity> unSelectedClients = new List<ILegalEntity>();

            IList<ILegalEntity> clientsToSelect = new List<ILegalEntity>();

            // loop through all the people
            foreach (var cmi in _view.SelectedCorrespondenceMediumInfo)
            {
                int externalRoleTypeKey = base._debtCounsellingRepo.GetExternalRoleTypeKeyForDebtCounsellingKeyAndLegalEntityKey(base._genericKey, cmi.LegalEntityKey);

                // we are only interested in clients
                if (externalRoleTypeKey == (int)SAHL.Common.Globals.ExternalRoleTypes.Client)
                {
                    // if this guy is a client, add him to the all Clients list
                    ILegalEntity legalEntity = base._legalEntityRepo.GetLegalEntityByKey(cmi.LegalEntityKey);

                    // if the client is selected, add him to the selected Clients list
                    if (cmi.CorrespondenceMediumsSelected.Count > 0)
                    {
                        if (!uniqueSelectedDomiciliumAddressKeys.Contains(legalEntity.ActiveDomicilium.LegalEntityAddress.Address.Key))
                            uniqueSelectedDomiciliumAddressKeys.Add(legalEntity.ActiveDomicilium.LegalEntityAddress.Address.Key);
                    }
                    else
                    {
                        unSelectedClients.Add(legalEntity);
                    }
                }               
            }

            if (uniqueSelectedDomiciliumAddressKeys.Count == 0)
            {
                errorMessage = "Must select at least one Client.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }

            foreach (var unSelectedClient in unSelectedClients)
            {
                if (!uniqueSelectedDomiciliumAddressKeys.Contains(unSelectedClient.ActiveDomicilium.LegalEntityAddress.Address.Key))
                {
                    clientsToSelect.Add(unSelectedClient);
                }
            }

            if (clientsToSelect.Count > 0)
            {
                errorMessage = "Must also select the following Clients as they have different Domicilium Addresses.";
                foreach (var client in clientsToSelect)
                {
                    errorMessage += "<br/>" + client.GetLegalName(LegalNameFormat.Full);
                }
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }


            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Logging;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Workflow.DebtCounselling
{
    public class SendLitigationReminderInternalEmailCommandHandler : IHandlesDomainServiceCommand<SendLitigationReminderInternalEmailCommand>
    {
        private IDebtCounsellingRepository debtcounsellingRepository;
        private IControlRepository controlRepository;
        private IMessageService messageService;
        private ILogger logger;

        public SendLitigationReminderInternalEmailCommandHandler(IDebtCounsellingRepository debtcounsellingRepository, IControlRepository controlRepository, IMessageService messageService, ILogger logger)
        {
            this.debtcounsellingRepository = debtcounsellingRepository;
            this.controlRepository = controlRepository;
            this.messageService = messageService;
            this.logger = logger;
        }

        public void Handle(IDomainMessageCollection messages, SendLitigationReminderInternalEmailCommand command)
        {
            IADUser adUser = null;

            // get the debt counselling record
            IDebtCounselling debtCounselling = debtcounsellingRepository.GetDebtCounsellingByKey(command.DebtCounsellingKey);

            if (debtCounselling != null)
            {
                // get the debt counsellor
                ILegalEntity debtCounsellor = debtCounselling.DebtCounsellor;

                // get the ework consultant
                string eStageName = "", eFolderId = "";
                debtcounsellingRepository.GetEworkDataForLossControlCase(debtCounselling.Account.Key, out eStageName, out eFolderId, out adUser);

                // get the list of clients
                IList<ILegalEntity> clients = debtCounselling.Clients;

                // only send email if there is an aduser record with a legalentity with an email address
                if (adUser != null && adUser.LegalEntity != null && !String.IsNullOrEmpty(adUser.LegalEntity.EmailAddress))
                {
                    // set the email details
                    string from = controlRepository.GetControlByDescription(SAHL.Common.Constants.ControlTable.HALOEmailAddress).ControlText;
                    string to = adUser.LegalEntity.EmailAddress;
                    string subject = "Continue Litigation Reminder";

                    // build the email body
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<br/>This is reminder to communicate with the Debt Counsellor about the decision to continue with Litigation");
                    sb.AppendLine("<br/>");
                    sb.AppendLine("<br/>Loan Number : " + debtCounselling.Account.Key);
                    sb.AppendLine("<br/>");
                    sb.AppendLine("<br/>Debt Counsellor Details");
                    sb.AppendLine("<br/>-----------------------");

                    string name = "", ncr = "", phone = "";
                    name = debtCounsellor.GetLegalName(SAHL.Common.BusinessModel.Interfaces.LegalNameFormat.Full);

                    // lets get the NCR number off top company node for this dc
                    IDebtCounsellorOrganisationNode dcNode = debtcounsellingRepository.GetTopDebtCounsellorCompanyNodeForDebtCounselling(command.DebtCounsellingKey);

                    if (dcNode.LegalEntities != null && dcNode.LegalEntities.Count == 1 && dcNode.LegalEntities[0].DebtCounsellorDetail != null)
                        ncr = dcNode.LegalEntities[0].DebtCounsellorDetail.NCRDCRegistrationNumber;

                    sb.AppendLine("<br/>Name       : " + name);
                    sb.AppendLine("<br/>NCR Number : " + ncr);
                    sb.AppendLine("<br/>Email      : " + debtCounsellor.EmailAddress);
                    if (!String.IsNullOrEmpty(debtCounsellor.HomePhoneNumber))
                    {
                        phone = debtCounsellor.HomePhoneNumber;
                        if (!String.IsNullOrEmpty(debtCounsellor.HomePhoneCode))
                            phone = "(" + debtCounsellor.HomePhoneCode + ") " + phone;

                        sb.AppendLine("<br/>Home Phone : " + phone);
                    }

                    if (!String.IsNullOrEmpty(debtCounsellor.WorkPhoneNumber))
                    {
                        phone = debtCounsellor.WorkPhoneNumber;
                        if (!String.IsNullOrEmpty(debtCounsellor.WorkPhoneCode))
                            phone = "(" + debtCounsellor.WorkPhoneCode + ") " + phone;

                        sb.AppendLine("<br/>Work Phone : " + phone);
                    }

                    if (!String.IsNullOrEmpty(debtCounsellor.CellPhoneNumber))
                        sb.AppendLine("<br/>Cell Phone : " + debtCounsellor.CellPhoneNumber);

                    sb.AppendLine("<br/>");
                    sb.AppendLine("<br/>Client Details");
                    sb.AppendLine("<br/>--------------");

                    foreach (ILegalEntity le in clients)
                    {
                        string num = "";
                        if (le.LegalEntityType.Key == (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson)
                        {
                            ILegalEntityNaturalPerson np = le as ILegalEntityNaturalPerson;
                            num = np.IDNumber;
                        }
                        else if (le.LegalEntityType.Key == (int)SAHL.Common.Globals.LegalEntityTypes.Company)
                        {
                            ILegalEntityCompany co = le as ILegalEntityCompany;
                            num = co.RegistrationNumber;
                        }
                        sb.AppendLine("<br/>" + le.GetLegalName(LegalNameFormat.Full) + " (" + num + ")");
                    }

                    //Send the email to the litigation consultant
                    messageService.SendEmailInternal(from, to, null, null, subject, sb.ToString(), true);
                }
                else
                {
                    Dictionary<string, object> methodParams = new Dictionary<string, object>();
                    methodParams.Add("command", command);
                    logger.LogFormattedWarning("SendLitigationReminderInternalEmailCommandHandler.Handle", "Unable to SendLitigationReminderInternalEmail({0})({1}){2}{3}", new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParams } }, command.DebtCounsellingKey, adUser.ADUserName, Environment.NewLine, "ADUser / LegalEntity not found or has no email address");
                }
            }
            else
            {
                Dictionary<string, object> methodParams = new Dictionary<string, object>();
                methodParams.Add("command", command);
                logger.LogFormattedWarning("SendLitigationReminderInternalEmailCommandHandler.Handle", "Unable to SendLitigationReminderInternalEmail({0})({1}){2}{3}", new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParams } }, command.DebtCounsellingKey, "", Environment.NewLine, "No Debt Counselling record found");
            }
        }
    }
}
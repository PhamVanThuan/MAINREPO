using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class SendAlphaHousingSurveyEmailCommandHandler : IHandlesDomainServiceCommand<SendAlphaHousingSurveyEmailCommand>
    {
        private IApplicationRepository applicationRepository;
        private ICommonRepository commonRepository;
        private IMessageService messageService;
        private IControlRepository controlRepository;

        public SendAlphaHousingSurveyEmailCommandHandler(IApplicationRepository applicationRepository, ICommonRepository commonRepository, IMessageService messageService,IControlRepository controlRepository)
        {
            this.applicationRepository = applicationRepository;
            this.commonRepository = commonRepository;
            this.messageService = messageService;
            this.controlRepository = controlRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SendAlphaHousingSurveyEmailCommand command)
        {
            IControl IsActivated = controlRepository.GetControlByDescription(SAHL.Common.Constants.ControlTable.AlphaHousingSurvey.IsActivated);
            if (IsActivated.ControlNumeric == 1)
            {
                IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
                if (application.IsAlphaHousing() &&
                    (application.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan ||
                    application.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.RefinanceLoan ||
                    application.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.SwitchLoan))
                {
                    ICorrespondenceTemplate template = commonRepository.GetCorrespondenceTemplateByKey(SAHL.Common.Globals.CorrespondenceTemplates.AlphaHousingSurvey);

                    string password = applicationRepository.GeneratePasswordFromAccountNumber(application.Account.Key);

                    foreach (var applicationMailingAddress in application.ApplicationMailingAddresses)
                    {
                        if (applicationMailingAddress.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Email && !string.IsNullOrEmpty(applicationMailingAddress.LegalEntity.EmailAddress))
                        {
                            string fullName = applicationMailingAddress.LegalEntity.GetLegalName(LegalNameFormat.Full);
                            string message = string.Format(template.Template, fullName, application.Account.Key, password);
                            messageService.SendEmailExternal(application.Key, template.DefaultEmail, applicationMailingAddress.LegalEntity.EmailAddress, "", "", template.Subject, message, string.Empty, string.Empty, string.Empty, (SAHL.Common.Globals.ContentTypes)template.ContentType.Key);
                            command.AlphaHousingEmailSent = true;
                        }
                    }

                    if (!command.AlphaHousingEmailSent)
                    {
                        IEnumerable<ILegalEntity> mainLegalEntities = application.GetApplicationRolesByType(SAHL.Common.Globals.OfferRoleTypes.MainApplicant).Select(x => x.LegalEntity);
                        foreach (ILegalEntity legalEntity in mainLegalEntities)
                        {
                            if (!string.IsNullOrEmpty(legalEntity.EmailAddress))
                            {
                                string fullName = legalEntity.GetLegalName(LegalNameFormat.Full);
                                string message = string.Format(template.Template, fullName, application.Account.Key, password);
                                messageService.SendEmailExternal(application.Key, template.DefaultEmail, legalEntity.EmailAddress, "", "", template.Subject, message, string.Empty, string.Empty, string.Empty, (SAHL.Common.Globals.ContentTypes)template.ContentType.Key);
                                command.AlphaHousingEmailSent = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
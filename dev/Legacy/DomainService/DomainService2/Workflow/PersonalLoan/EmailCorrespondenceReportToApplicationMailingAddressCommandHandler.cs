using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.PersonalLoan
{
    public class EmailCorrespondenceReportToApplicationMailingAddressCommandHandler : IHandlesDomainServiceCommand<EmailCorrespondenceReportToApplicationMailingAddressCommand>
    {
        ICorrespondenceRepository correspondenceRepository;
        IApplicationRepository applicationRepository;

        public EmailCorrespondenceReportToApplicationMailingAddressCommandHandler(ICorrespondenceRepository correspondenceRepository, IApplicationRepository applicationRepository)
        {
            this.correspondenceRepository = correspondenceRepository;
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, EmailCorrespondenceReportToApplicationMailingAddressCommand command)
        {

            //int genericKeyTypeKey = (int)SAHL.Common.Globals.GenericKeyTypes.Offer;

            // Given this is in the context of a Personal Loan implementation of the SendCorrespondence
            // we can make the assumption we dealing with an IApplication object
            IApplication application = applicationRepository.GetApplicationByKey(command.GenericKey);
            int accountKey = application.ReservedAccount.Key;

            // Set up the correspondence request
            foreach (var applicationMailingAddress in application.ApplicationMailingAddresses)
            {
                if (applicationMailingAddress.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Email && !string.IsNullOrEmpty(applicationMailingAddress.LegalEntity.EmailAddress))
                {
                    ISendCorrespondenceRequest sendCorrespondenceRequest = new SendCorrespondenceRequest(accountKey, command.GenericKey, (int)SAHL.Common.Globals.GenericKeyTypes.Offer, accountKey.ToString(), SAHL.Common.Constants.WorkFlowName.PersonalLoans, command.ReportName, command.ADUserName, SAHL.Common.Constants.DataSTOR.PersonalLoans, command.CorrespondenceTemplate, applicationMailingAddress.LegalEntity.Key, SAHL.Common.Globals.CorrespondenceMediums.Email);
                    correspondenceRepository.SendCorrespondenceReportToLegalEntity(sendCorrespondenceRequest);
                }
            }
        }
    }
}
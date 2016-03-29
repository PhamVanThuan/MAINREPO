using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.PersonalLoan
{
    public class SendSMSToApplicantUponDisbursementCommandHandler : IHandlesDomainServiceCommand<SendSMSToApplicantUponDisbursementCommand>
    {
        private IApplicationRepository applicationRepository;
        private ICommonRepository commonRepository;
        private IMessageService messageService;

        public SendSMSToApplicantUponDisbursementCommandHandler(IApplicationRepository applicationRepository, IMessageService messageService, ICommonRepository commonRepository)
        {
            this.applicationRepository = applicationRepository;
            this.messageService = messageService;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, SendSMSToApplicantUponDisbursementCommand command)
        {
            var applicationPersonalLoan = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            var latestAcceptedOfferInformation = applicationPersonalLoan.ApplicationInformations.Where(x => x.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                                                                            .OrderByDescending(x => x.ApplicationInsertDate)
                                                                            .FirstOrDefault();
            
            var applicationProductPersonalLoan = latestAcceptedOfferInformation.ApplicationProduct as IApplicationProductPersonalLoan;


            var loanAmount = applicationProductPersonalLoan.ApplicationInformationPersonalLoan.LoanAmount;

            var template = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.PersonalLoanDisbursementSMS);

            foreach (var applicant in applicationPersonalLoan.GetNaturalPersonsByExternalRoleType(ExternalRoleTypes.Client, GeneralStatuses.Active))
            {
                if (!string.IsNullOrEmpty(applicant.CellPhoneNumber))
                {
                    messageService.SendSMS(command.ApplicationKey, string.Format(template.Template, loanAmount), applicant.CellPhoneNumber);
                }
            }
        }
    }
}

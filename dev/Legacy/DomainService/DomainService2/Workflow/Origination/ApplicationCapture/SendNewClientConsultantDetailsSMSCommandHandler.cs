using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Linq;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
	public class SendNewClientConsultantDetailsSMSCommandHandler : IHandlesDomainServiceCommand<SendNewClientConsultantDetailsSMSCommand>
	{
		private IMessageService messageService;
		private IApplicationRepository applicationRepository;
		private ICommonRepository commonRepository;
		private ILookupRepository lookupRepository;

		public SendNewClientConsultantDetailsSMSCommandHandler(IMessageService messageService, IApplicationRepository applicationRepository, ICommonRepository commonRepository, ILookupRepository lookupRepository)
		{
			this.messageService = messageService;
			this.applicationRepository = applicationRepository;
			this.commonRepository = commonRepository;
			this.lookupRepository = lookupRepository;
		}

		public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, SendNewClientConsultantDetailsSMSCommand command)
		{
			ICorrespondenceTemplate template = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.NewClientConsultantDetailsSMS);

			var application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
			var mainApplicants = application.GetLegalEntitiesByRoleType(new OfferRoleTypes[] { OfferRoleTypes.MainApplicant }, GeneralStatusKey.Active);
			var consultants = application.GetLegalEntitiesByRoleType(new OfferRoleTypes[] { OfferRoleTypes.BranchConsultantD }, GeneralStatusKey.Active);

			var consultant = consultants.FirstOrDefault() as ILegalEntityNaturalPerson;
			string contactNumber = String.Empty;
			if (consultant != null && !String.IsNullOrEmpty(consultant.CellPhoneNumber))
			{
				contactNumber = consultant.CellPhoneNumber;
			}
			else
			{
				contactNumber = lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.HelpDeskContactNumber].ControlText;
			}
            string consultantName = consultant.FirstNames;
			foreach (var mainApplicant in mainApplicants)
			{
				if (!String.IsNullOrEmpty(mainApplicant.CellPhoneNumber))
				{
					string message = String.Format(template.Template, consultantName, contactNumber, application.Key);
					messageService.SendSMS(command.ApplicationKey, message, mainApplicant.CellPhoneNumber);
				}
			}
		}
	}
}
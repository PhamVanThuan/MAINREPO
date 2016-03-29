using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        public void AddDeclarations(IApplicationStateMachine applicationStateMachine, IEnumerable<ApplicantModel> applicants)
        {
            foreach (var applicant in applicants)
            {
                var commandCorrelationId = combGuidGenerator.Generate();
                try
                {
                    AddApplicantDeclarations(applicant, commandCorrelationId);
                }
                catch (Exception runtimeException)
                {
                    string friendlyErrorMessage = String.Format("Declarations could not be added for applicant with ID number: {0}", applicant.IDNumber);
                    HandleNonCriticalException(runtimeException, friendlyErrorMessage, commandCorrelationId, applicationStateMachine);
                }
            }
        }

        private void AddApplicantDeclarations(ApplicantModel applicant, Guid commandCorrelationId)
        {
            var clientKey = applicationStateMachine.ClientCollection[applicant.IDNumber];
            var applicationNumber = applicationStateMachine.ApplicationNumber;

            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, commandCorrelationId);

            OfferDeclarationAnswer declaredInsolventAnswer = OfferDeclarationAnswer.No;
            DateTime? insolventRehabilitationDate = null;
            OfferDeclarationAnswer underAdministrationOrderAnswer = OfferDeclarationAnswer.No;
            DateTime? administrationOrderDateRescinded = null;
            OfferDeclarationAnswer currentlyUnderDebtCounsellingAnswer = OfferDeclarationAnswer.No;
            bool? hasCurrentDebtArrangement = null;
            OfferDeclarationAnswer permissionToConductCreditCheck = OfferDeclarationAnswer.No;

            foreach (var declaration in applicant.ApplicantDeclarations)
            {
                switch (declaration.DeclarationQuestion)
                {
                    case OfferDeclarationQuestionEnum.DeclaredInsolventQuestionKey:
                        declaredInsolventAnswer = declaration.DeclarationAnswer;
                        break;

                    case OfferDeclarationQuestionEnum.InsolventRehabilitationDateQuestionKey:
                        insolventRehabilitationDate = declaration.DeclarationDate;
                        break;

                    case OfferDeclarationQuestionEnum.UnderAdministrationOrderQuestionKey:
                        underAdministrationOrderAnswer = declaration.DeclarationAnswer;
                        break;

                    case OfferDeclarationQuestionEnum.AdministrationOrderDateRescindedQuestionKey:
                        administrationOrderDateRescinded = declaration.DeclarationDate;
                        break;

                    case OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey:
                        currentlyUnderDebtCounsellingAnswer = declaration.DeclarationAnswer;
                        break;

                    case OfferDeclarationQuestionEnum.HasCurrentDebtArrangementQuestionKey:
                        hasCurrentDebtArrangement = declaration.DeclarationAnswer == OfferDeclarationAnswer.Yes;
                        break;

                    case OfferDeclarationQuestionEnum.PermissionToConductCreditCheckQuestionKey:
                        permissionToConductCreditCheck = declaration.DeclarationAnswer;
                        break;

                    default:
                        break;
                }
            }

            var applicantDeclarationsModel = new Services.Interfaces.ApplicationDomain.Models.ApplicantDeclarationsModel(clientKey, applicationNumber, DateTime.Now,
            new Services.Interfaces.ApplicationDomain.Models.DeclaredInsolventDeclarationModel(declaredInsolventAnswer, insolventRehabilitationDate),
            new Services.Interfaces.ApplicationDomain.Models.UnderAdministrationOrderDeclarationModel(underAdministrationOrderAnswer, administrationOrderDateRescinded),
            new Services.Interfaces.ApplicationDomain.Models.CurrentlyUnderDebtCounsellingReviewDeclarationModel(currentlyUnderDebtCounsellingAnswer, hasCurrentDebtArrangement),
            new Services.Interfaces.ApplicationDomain.Models.PermissionToConductCreditCheckDeclarationModel(permissionToConductCreditCheck));

            var addApplicantDeclarationsCommand = new AddApplicantDeclarationsCommand(applicantDeclarationsModel);
            var serviceMessages = this.applicationDomainService.PerformCommand(addApplicantDeclarationsCommand, serviceRequestMetadata);
            CheckForNonCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages, ApplicationState.DeclarationsCaptured);
        }
    }
}
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.PropertyDomain;
using SAHL.Services.Interfaces.PropertyDomain.Commands;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Behaviors
{
    [Behaviors]
    public class DomainProcessThatHasCompletedNonCriticalSteps
    {
        protected static IPropertyDomainServiceClient propertyDomainService;
        protected static IApplicationDomainServiceClient applicationDomainService;
        protected static int applicationNumber, clientKey;
        protected static ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail;
        protected static ApplicantModel applicant;
        protected static string vendorCode;

        private It should_add_the_comcorp_property = () =>
        {
            propertyDomainService.WasToldTo(x => x.PerformCommand(Param<AddComcorpOfferPropertyDetailsCommand>.Matches(m =>
                m.ApplicationNumber == applicationNumber &&
                m.ComcorpOfferPropertyDetails.City == comcorpApplicationPropertyDetail.City &&
                m.ComcorpOfferPropertyDetails.ComplexName == comcorpApplicationPropertyDetail.ComplexName &&
                m.ComcorpOfferPropertyDetails.ContactCellphone == comcorpApplicationPropertyDetail.ContactCellphone &&
                m.ComcorpOfferPropertyDetails.PortionNo == comcorpApplicationPropertyDetail.PortionNo &&
                m.ComcorpOfferPropertyDetails.PostalCode == comcorpApplicationPropertyDetail.PostalCode &&
                m.ComcorpOfferPropertyDetails.Province == comcorpApplicationPropertyDetail.Province &&
                m.ComcorpOfferPropertyDetails.SAHLOccupancyType == comcorpApplicationPropertyDetail.SAHLOccupancyType
            ), Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_affordabilities = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(Param<AddApplicantAffordabilitiesCommand>.Matches(m =>
                m.ApplicantAffordabilityModel.ApplicationNumber == applicationNumber &&
                m.ApplicantAffordabilityModel.ClientAffordabilityAssessment.Count() == applicant.Affordabilities.Count()
            ), Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_declarations = () =>
        {
            var insolventAnswer = applicant.ApplicantDeclarations.First(d => d.DeclarationQuestion == OfferDeclarationQuestionEnum.DeclaredInsolventQuestionKey).DeclarationAnswer;
            var debtReviewAnswer = applicant.ApplicantDeclarations.First(d => d.DeclarationQuestion == OfferDeclarationQuestionEnum.CurrentlyUnderDebtCounsellingQuestionKey).DeclarationAnswer;
            var creditCheckAnswer = applicant.ApplicantDeclarations.First(d => d.DeclarationQuestion == OfferDeclarationQuestionEnum.PermissionToConductCreditCheckQuestionKey).DeclarationAnswer;
            var underAdminOrderAnswer = applicant.ApplicantDeclarations.First(d => d.DeclarationQuestion == OfferDeclarationQuestionEnum.UnderAdministrationOrderQuestionKey).DeclarationAnswer;
            applicationDomainService.WasToldTo(x => x.PerformCommand(Param<AddApplicantDeclarationsCommand>.Matches(m =>
                m.ApplicantDeclarations.ApplicationNumber == applicationNumber &&
                m.ApplicantDeclarations.ClientKey == clientKey &&
                m.ApplicantDeclarations.DeclaredInsolventDeclaration.Answer == insolventAnswer &&
                m.ApplicantDeclarations.CurrentlyUnderDebtReviewDeclaration.Answer == debtReviewAnswer &&
                m.ApplicantDeclarations.PermissiontoConductCreditCheckDeclaration.Answer == creditCheckAnswer &&
                m.ApplicantDeclarations.UnderAdministrationOrderDeclaration.Answer == underAdminOrderAnswer
                ), Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_the_external_vendor = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(Param<LinkExternalVendorToApplicationCommand>.Matches(m =>
                m.ApplicationNumber == applicationNumber &&
                m.VendorCode == vendorCode),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}
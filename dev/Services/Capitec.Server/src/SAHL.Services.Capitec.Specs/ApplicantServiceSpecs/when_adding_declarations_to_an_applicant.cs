using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Capitec.Managers.Address;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ContactDetail;
using SAHL.Services.Capitec.Managers.Declaration;
using SAHL.Services.Capitec.Managers.Employment;
using SAHL.Services.Capitec.Managers.Lookup;

using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantServiceSpecs
{
    public class when_adding_declarations_to_an_applicant : WithFakes
    {
        private static ApplicantManager applicantService;
        private static Guid applicantID;
        private static ILookupManager lookupService;

        private static ApplicantDeclarations declarations;
        private static string allowCreditBureauCheck;
        private static IDeclarationDataManager declarationDataService;
        private static Guid yesDeclarationTypeEnumId;
        private static Guid allowCreditBureauCheck_declarationDefinitionID;
        private static IApplicantDataManager applicantDataService;
        private static Guid declarationID;
        private static DateTime declarationDate;

        private Establish context = () =>
        {
            declarationDate = DateTime.Now;
            declarationID = Guid.NewGuid();
            applicantID = Guid.NewGuid();
            yesDeclarationTypeEnumId = Guid.Parse(DeclarationTypeEnumDataModel.YES);
            allowCreditBureauCheck_declarationDefinitionID = Guid.NewGuid();
            declarations = new ApplicantDeclarations(yesDeclarationTypeEnumId, yesDeclarationTypeEnumId, yesDeclarationTypeEnumId, yesDeclarationTypeEnumId);
            declarations.DeclarationsDate = declarationDate;
            //
            declarationDataService = An<IDeclarationDataManager>();
            lookupService = An<ILookupManager>();
            applicantDataService = An<IApplicantDataManager>();
            applicantService = new ApplicantManager(applicantDataService, An<IContactDetailDataManager>(), lookupService, declarationDataService, An<IEmploymentDataManager>(), An<IAddressDataManager>());
            //
            allowCreditBureauCheck = DeclarationDefinitions.AllowCreditBureauCheck;
            declarationDataService.WhenToldTo(x => x.GetDeclarationDefinition(yesDeclarationTypeEnumId, allowCreditBureauCheck)).Return(allowCreditBureauCheck_declarationDefinitionID);
            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(declarationID);
        };

        private Because of = () =>
        {
            applicantService.AddDeclarationsForApplicant(applicantID, declarations);
        };

        private It should_get_declaration_definition = () =>
        {
            declarationDataService.WasToldTo(x => x.GetDeclarationDefinition(yesDeclarationTypeEnumId, allowCreditBureauCheck));
        };

        private It should_add_the_declaration_definition = () =>
        {
            declarationDataService.WasToldTo(x => x.AddDeclaration(declarationID, allowCreditBureauCheck_declarationDefinitionID, declarationDate));
        };

        private It should_link_the_declaration_definition_to_the_applicant = () =>
        {
            applicantDataService.WasToldTo(x => x.AddDeclarationToApplicant(applicantID, declarationID));
        };
    }
}
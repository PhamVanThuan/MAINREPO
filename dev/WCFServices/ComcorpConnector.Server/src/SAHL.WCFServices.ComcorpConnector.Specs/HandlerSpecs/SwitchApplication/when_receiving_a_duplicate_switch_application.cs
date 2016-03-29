using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Services.DomainProcessManager.Client;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Handlers;
using SAHL.WCFServices.ComcorpConnector.Managers.ComcorpApplication;
using SAHL.WCFServices.ComcorpConnector.Managers.MacAuthentication;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;

namespace SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.SwitchApplication
{
    public class when_receiving_a_duplicate_switch_application : WithCoreFakes
    {
        private static SAHLHandler handler;
        private static IMacAuthenticationManager macAuthenticationManager;
        private static ISupportingDocumentManager supportingDocumentManager;
        private static IDomainProcessManagerClientApiFactory domainProcessManagerClientApiFactory;
        private static IApplicationModelManager modelCreationManager;
        private static IComcorpApplicationDataManager comcorpApplicationDataManager;
        private static IValidationUtils validationUtils;
        private static SAHLRequest request;
        private static int applicationNumber;
        private static SAHLResponse sahlResponse;

        private Establish context = () =>
        {
            validationUtils = An<IValidationUtils>();
            macAuthenticationManager = An<IMacAuthenticationManager>();
            supportingDocumentManager = An<ISupportingDocumentManager>();
            domainProcessManagerClientApiFactory = An<IDomainProcessManagerClientApiFactory>();
            modelCreationManager = An<IApplicationModelManager>();
            comcorpApplicationDataManager = An<IComcorpApplicationDataManager>();
            handler = new SAHLHandler(supportingDocumentManager, macAuthenticationManager, domainProcessManagerClientApiFactory,
                validationUtils, modelCreationManager, comcorpApplicationDataManager);

            Application comcorpApplication = IntegrationServiceTestHelper.SetupBasicApplication(MortgageLoanPurpose.Switchloan);
            request = new SAHLRequest { Application = comcorpApplication };

            applicationNumber = 1234567;

            comcorpApplicationDataManager.WhenToldTo(x => x.GetApplicationNumberForApplicationCode(comcorpApplication.ApplicationCode))
                                        .Return(applicationNumber);
        };

        private Because of = () =>
        {
            sahlResponse = handler.Handle(request);
        };

        private It should_check_if_an_application_already_exists_for_the_application_code = () =>
        {
            comcorpApplicationDataManager.WasToldTo(x => x.GetApplicationNumberForApplicationCode(request.Application.ApplicationCode));
        };

        private It should_return_an_application_number_in_the_response = () =>
        {
            sahlResponse.SAHLReference.ShouldEqual(applicationNumber.ToString());
        };
    }
}
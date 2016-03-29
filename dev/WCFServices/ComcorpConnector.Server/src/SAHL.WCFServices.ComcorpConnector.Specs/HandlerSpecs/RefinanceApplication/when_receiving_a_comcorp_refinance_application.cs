using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Config.Services.DomainProcessManager.Client;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.WCFServices.ComcorpConnector.Handlers;
using SAHL.WCFServices.ComcorpConnector.Managers.ComcorpApplication;
using SAHL.WCFServices.ComcorpConnector.Managers.MacAuthentication;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;

namespace SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.RefinanceApplication
{
    public class when_receiving_a_comcorp_refinance_application : WithCoreFakes
    {
        private static SAHLHandler handler;
        private static IMacAuthenticationManager macAuthenticationManager;
        private static ISupportingDocumentManager supportingDocumentManager;
        private static IDomainProcessManagerClientApiFactory domainProcessManagerClientApiFactory;
        private static IApplicationModelManager modelCreationManager;
        private static IComcorpApplicationDataManager comcorpApplicationDataManager;
        private static IDomainProcessManagerClient domainProcessManagerService;
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

            Application comcorpApplication = IntegrationServiceTestHelper.SetupBasicApplication(MortgageLoanPurpose.Refinance);
            request = new SAHLRequest { Application = comcorpApplication };

            applicationNumber = 22235;

            validationUtils.WhenToldTo(x => x.ParseEnum<OfferType>("Refinance")).Return(OfferType.RefinanceLoan);

            domainProcessManagerService = An<IDomainProcessManagerClient>();
            domainProcessManagerService.WhenToldTo(x => x.StartDomainProcess(Arg.Any<StartDomainProcessCommand>())).Return(() =>
            {
                var taskCompletionSource = new TaskCompletionSource<IStartDomainProcessResponse>();
                var response = new StartDomainProcessResponse(true, new ApplicationCreationReturnDataModel(applicationNumber));
                taskCompletionSource.SetResult(response);
                return taskCompletionSource.Task;
            });

            domainProcessManagerClientApiFactory.WhenToldTo(x => x.Create()).Return(new DomainProcessManagerClientApi(domainProcessManagerService));
            modelCreationManager.WhenToldTo(x => x.PopulateRefinanceApplicationCreationModel(Param.IsAny<Application>()))
                                .Return(ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.RefinanceLoan) as RefinanceApplicationCreationModel);
        };

        private Because of = () =>
        {
            sahlResponse = handler.Handle(request);
        };

        private It should_check_if_an_application_already_exists_for_the_application_code = () =>
        {
            comcorpApplicationDataManager.WasToldTo(x => x.GetApplicationNumberForApplicationCode(request.Application.ApplicationCode));
        };

        private It should_populate_the_application_creation_model = () =>
        {
            modelCreationManager.WasToldTo(x => x.PopulateRefinanceApplicationCreationModel(request.Application));
        };

        private It should_start_the_Refinance_Application_Domain_Process = () =>
        {
            domainProcessManagerService.WasToldTo(x => x.StartDomainProcess(Param<StartDomainProcessCommand>.Matches(m =>
                m.StartEventToWaitFor == typeof(ExternalVendorLinkedToApplicationEvent).Name &&
                m.DataModel is RefinanceApplicationCreationModel)));
        };

        private It should_return_an_application_number_in_the_response = () =>
        {
            sahlResponse.SAHLReference.ShouldEqual(applicationNumber.ToString());
        };
    }
}
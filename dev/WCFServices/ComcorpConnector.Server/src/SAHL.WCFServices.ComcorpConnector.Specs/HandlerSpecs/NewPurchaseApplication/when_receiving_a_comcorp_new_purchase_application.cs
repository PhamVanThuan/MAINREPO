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

namespace SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.NewPurchaseApplication
{
    public class when_receiving_a_comcorp_new_purchase_application : WithCoreFakes
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

            Application comcorpApplication = IntegrationServiceTestHelper.SetupBasicApplication(MortgageLoanPurpose.Newpurchase);
            request = new SAHLRequest { Application = comcorpApplication };

            applicationNumber = 1234567;

            domainProcessManagerService = An<IDomainProcessManagerClient>();
            domainProcessManagerService.WhenToldTo(x => x.StartDomainProcess(Arg.Any<StartDomainProcessCommand>())).Return(() =>
            {
                var taskCompletionSource = new TaskCompletionSource<IStartDomainProcessResponse>();
                var response = new StartDomainProcessResponse(true, new ApplicationCreationReturnDataModel(applicationNumber));
                taskCompletionSource.SetResult(response);
                return taskCompletionSource.Task;
            });

            validationUtils.WhenToldTo(x => x.ParseEnum<OfferType>("New Purchase Loan")).Return(OfferType.NewPurchaseLoan);
            domainProcessManagerClientApiFactory.WhenToldTo(x => x.Create()).Return(new DomainProcessManagerClientApi(domainProcessManagerService));
            modelCreationManager.WhenToldTo(x => x.PopulateNewPurchaseApplicationCreationModel(Param.IsAny<Application>()))
                                .Return(ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel);
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
            modelCreationManager.WasToldTo(x => x.PopulateNewPurchaseApplicationCreationModel(request.Application));
        };

        private It should_start_the_New_Purchase_Application_Domain_Process = () =>
        {
            domainProcessManagerService.WasToldTo(x => x.StartDomainProcess(Param<StartDomainProcessCommand>.Matches(m =>
                m.StartEventToWaitFor == typeof(ExternalVendorLinkedToApplicationEvent).Name &&
                m.DataModel is NewPurchaseApplicationCreationModel)));
        };

        private It should_return_an_application_number_in_the_response = () =>
        {
            sahlResponse.SAHLReference.ShouldEqual(applicationNumber.ToString());
        };
    }
}
using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Config.Services.DomainProcessManager.Client;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Handlers;
using SAHL.WCFServices.ComcorpConnector.Managers.ComcorpApplication;
using SAHL.WCFServices.ComcorpConnector.Managers.MacAuthentication;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument.Models;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;

namespace SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.ImagingHandler
{
    public class when_authentication_fails_for_a_mainapplicant_request : WithCoreFakes
    {
        private static SAHLHandler handler;
        private static ISupportingDocumentManager supportingDocumentManager;
        private static IMacAuthenticationManager macAuthenticationManager;
        private static IComcorpApplicationDataManager comcorpApplicationDataManager;
        private static ImagingMainApplicantRequest request;
        private static mainApplicantHeaderType requestHeader;
        private static IDomainProcessManagerClientApiFactory domainProcessManagerClientApiFactory;
        private static IApplicationModelManager modelCreationManager;
        private static SupportingDocumentsReply result;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            supportingDocumentManager = An<ISupportingDocumentManager>();
            macAuthenticationManager = An<IMacAuthenticationManager>();
            macAuthenticationManager.WhenToldTo(x => x.AuthenticateMessage(Arg.Any<ImagingApplicationRequest>(), Arg.Any<string>()))
                .Return(false);
            modelCreationManager = An<IApplicationModelManager>();
            domainProcessManagerClientApiFactory = An<IDomainProcessManagerClientApiFactory>();
            comcorpApplicationDataManager = An<IComcorpApplicationDataManager>();
            validationUtils = new ValidationUtils();
            handler = new SAHLHandler(supportingDocumentManager, macAuthenticationManager, domainProcessManagerClientApiFactory, validationUtils, modelCreationManager, comcorpApplicationDataManager);
            requestHeader = new mainApplicantHeaderType
            {
                ApplicationReference = "1234",
                BankReference = "3425",
                RequestAction = headerTypeRequestAction.New,
                RequestDateTime = new DateTime(2014, 1, 1),
                RequestMac = "1nt12h3oneuhonchu,.rcho",
                ServiceVersion = 1
            };
            request = new ImagingMainApplicantRequest
            {
                RequestHeader = requestHeader,
                SupportingDocuments = new MainApplicantSupportingDocuments
                {
                }
            };
        };

        private Because of = () =>
        {
            result = handler.Handle(request);
        };

        private It should_return_an_invalid_request_mac_status = () =>
        {
            result.ReplyHeader.RequestStatus.ShouldEqual((int)DocumentReplyStatus.InvalidRequestMac);
        };

        private It should_not_save_the_documents = () =>
        {
            supportingDocumentManager.WasNotToldTo(x => x.SaveClientFileDocuments(Arg.Any<List<ApplicationDocumentsModel>>(), requestHeader));
        };
    }
}
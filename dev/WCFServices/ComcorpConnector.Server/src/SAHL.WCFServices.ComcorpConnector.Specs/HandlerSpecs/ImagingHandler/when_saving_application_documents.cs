using Machine.Fakes;
using Machine.Specifications;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;

namespace SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs
{
    public class when_saving_application_documents : WithCoreFakes
    {
        private static SAHLHandler handler;
        private static IMacAuthenticationManager macAuthenticationManager;
        private static ISupportingDocumentManager supportingDocumentManager;
        private static ImagingApplicationRequest request;
        private static applicationHeaderType requestHeader;
        private static decimal documentReference;
        private static IDomainProcessManagerClientApiFactory domainProcessManagerClientApiFactory;
        private static IApplicationModelManager modelCreationManager;
        private static IComcorpApplicationDataManager comcorpApplicationDataManager;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = An<IValidationUtils>();
            macAuthenticationManager = An<IMacAuthenticationManager>();
            supportingDocumentManager = An<ISupportingDocumentManager>();
            modelCreationManager = An<IApplicationModelManager>();
            domainProcessManagerClientApiFactory = An<IDomainProcessManagerClientApiFactory>();
            comcorpApplicationDataManager = An<IComcorpApplicationDataManager>();
            macAuthenticationManager.WhenToldTo(x => x.AuthenticateMessage(Param.IsAny<object>(), Param.IsAny<string>())).Return(true);
            handler = new SAHLHandler(supportingDocumentManager, macAuthenticationManager, domainProcessManagerClientApiFactory, validationUtils, modelCreationManager, comcorpApplicationDataManager);

            documentReference = 1234;
            requestHeader = new applicationHeaderType
            {
                ApplicationReference = "1234",
                BankReference = "3425",
                RequestAction = headerTypeRequestAction.New,
                RequestDateTime = new DateTime(2014, 1, 1),
                RequestMac = "1nt12h3oneuhonchu,.rcho",
                ServiceVersion = 1
            };
            request = new ImagingApplicationRequest
            {
                RequestHeader = requestHeader,
                SupportingDocuments = new ApplicationSupportingDocuments
                {
                    ApplicationDocuments = new documentType[1]
                    {
                        new documentType
                        {
                            DocumentType = "ID Documents",
                            DocumentDescription = "Description",
                            DocumentImage = "oeu5oe61u6o5e4u16oe5u1",
                            DocumentReference = documentReference
                        }
                    }
                }
            };
        };

        private Because of = () =>
        {
            handler.Handle(request);
        };

        private It should_save_the_application_files = () =>
        {
            supportingDocumentManager.WasToldTo(x => x.SaveClientFileDocuments(Param<List<ApplicationDocumentsModel>>.Matches(y =>
                y.Count == 1 &&
                y.First().SupportingDocuments.Count == 1 &&
                y.First().SupportingDocuments.First().DocumentReference == documentReference)
                , requestHeader));
        };
    }
}
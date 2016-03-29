using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs
{
    public class when_saving_main_applicant_documents : WithCoreFakes
    {
        private static SAHLHandler handler;
        private static ISupportingDocumentManager supportingDocumentManager;
        private static IMacAuthenticationManager macAuthenticationManager;
        private static ImagingMainApplicantRequest request;
        private static mainApplicantHeaderType requestHeader;
        private static string identityNumber;
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
            domainProcessManagerClientApiFactory = An<IDomainProcessManagerClientApiFactory>();
            modelCreationManager = An<IApplicationModelManager>();
            comcorpApplicationDataManager = An<IComcorpApplicationDataManager>();
            macAuthenticationManager.WhenToldTo(x => x.AuthenticateMessage(Param.IsAny<object>(), Param.IsAny<string>())).Return(true);
            handler = new SAHLHandler(supportingDocumentManager, macAuthenticationManager, domainProcessManagerClientApiFactory, validationUtils, modelCreationManager, comcorpApplicationDataManager);

            identityNumber = "1234567890123";
            documentReference = 1234;
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
                    MainApplicantDocuments = new mainApplicantType
                    {
                        ApplicantFirstName = "Bob",
                        ApplicantIdentityNumber = identityNumber,
                        ApplicantPassportNumber = "",
                        ApplicantReference = 1,
                        ApplicantSurname = "Smith",
                        SupportingDocument = new mainApplicantDocumentType[1]
                        {
                            new mainApplicantDocumentType
                            {
                                DocumentType = "ID Documents",
                                DocumentDescription = "Description",
                                DocumentImage = "oeu5oe61u6o5e4u16oe5u1",
                                DocumentReference = documentReference
                            }
                        }
                    }
                }
            };
        };

        private Because of = () =>
        {
            handler.Handle(request);
        };

        private It should_save_the_client_files = () =>
        {
            supportingDocumentManager.WasToldTo(x => x.SaveClientFileDocuments(Param<List<ApplicationDocumentsModel>>.Matches(y =>
                y.Count == 1 &&
                y.First().IdentityNumber == identityNumber &&
                y.First().SupportingDocuments.Count == 1 &&
                y.First().SupportingDocuments.First().DocumentReference == documentReference)
                , requestHeader));
        };
    }
}
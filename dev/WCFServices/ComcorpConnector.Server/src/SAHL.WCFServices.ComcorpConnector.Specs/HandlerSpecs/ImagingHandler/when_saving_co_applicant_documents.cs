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
    public class when_saving_co_applicant_documents : WithCoreFakes
    {
        private static SAHLHandler handler;
        private static ISupportingDocumentManager supportingDocumentManager;
        private static IMacAuthenticationManager macAuthenticationManager;
        private static ImagingCoApplicantRequest request;
        private static coApplicantHeaderType requestHeader;
        private static string identityNumber;
        private static decimal documentReference;
        private static IDomainProcessManagerClientApiFactory domainProcessManagerClientApiFactory;
        private static IApplicationModelManager modelCreationManager;
        private static IValidationUtils validationUtils;
        private static IComcorpApplicationDataManager comcorpApplicationDataManager;

        private Establish context = () =>
        {
            validationUtils = An<IValidationUtils>();
            macAuthenticationManager = An<IMacAuthenticationManager>();
            modelCreationManager = An<IApplicationModelManager>();
            supportingDocumentManager = An<ISupportingDocumentManager>();
            domainProcessManagerClientApiFactory = An<IDomainProcessManagerClientApiFactory>();
            comcorpApplicationDataManager = An<IComcorpApplicationDataManager>();
            macAuthenticationManager.WhenToldTo(x => x.AuthenticateMessage(Param.IsAny<object>(), Param.IsAny<string>())).Return(true);
            handler = new SAHLHandler(supportingDocumentManager, macAuthenticationManager, domainProcessManagerClientApiFactory, validationUtils, modelCreationManager, comcorpApplicationDataManager);
            identityNumber = "1234567890123";
            documentReference = 1234;
            requestHeader = new coApplicantHeaderType
            {
                ApplicationReference = "1234",
                BankReference = "3425",
                RequestAction = headerTypeRequestAction.New,
                RequestDateTime = new DateTime(2014, 1, 1),
                RequestMac = "1nt12h3oneuhonchu,.rcho",
                ServiceVersion = 1
            };
            request = new ImagingCoApplicantRequest
            {
                RequestHeader = requestHeader,
                SupportingDocuments = new CoApplicantSupportingDocuments
                {
                    CoApplicantDocuments = new coApplicantType
                    {
                        ApplicantFirstName = "Bob",
                        ApplicantIdentityNumber = identityNumber,
                        ApplicantPassportNumber = "",
                        ApplicantReference = 1,
                        ApplicantSurname = "Smith",
                        SupportingDocument = new coApplicantDocumentType[1]
                        {
                            new coApplicantDocumentType
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
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Config.Services.DomainProcessManager.Client;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager;
using SAHL.WCFServices.ComcorpConnector.Interfaces;
using SAHL.WCFServices.ComcorpConnector.Managers.ComcorpApplication;
using SAHL.WCFServices.ComcorpConnector.Managers.MacAuthentication;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;

namespace SAHL.WCFServices.ComcorpConnector.Handlers
{
    public class SAHLHandler : ISAHLHandler
    {
        private IMacAuthenticationManager macAuthenticationManager;
        private ISupportingDocumentManager supportingDocumentManager;
        private IComcorpApplicationDataManager comcorpApplicationDataManager;
        private IDomainProcessManagerClientApiFactory domainProcessManagerClientApiFactory;
        private IValidationUtils validationUtils;
        private IApplicationModelManager applicationModelManager;

        public SAHLHandler(ISupportingDocumentManager supportingDocumentManager, IMacAuthenticationManager macAuthenticationManager,
            IDomainProcessManagerClientApiFactory domainProcessManagerClientApiFactory, IValidationUtils validationUtils, IApplicationModelManager applicationModelManager,
            IComcorpApplicationDataManager comcorpApplicationDataManager)
        {
            this.supportingDocumentManager = supportingDocumentManager;
            this.macAuthenticationManager = macAuthenticationManager;
            this.domainProcessManagerClientApiFactory = domainProcessManagerClientApiFactory;
            this.validationUtils = validationUtils;
            this.applicationModelManager = applicationModelManager;
            this.comcorpApplicationDataManager = comcorpApplicationDataManager;
        }

        public SAHLResponse Handle(SAHLRequest request)
        {
            string eventToWaitFor = String.Empty;
            IDataModel dataModel = null;

            int applicationNumber = comcorpApplicationDataManager.GetApplicationNumberForApplicationCode(request.Application.ApplicationCode) ?? 0;
            if (applicationNumber > 0)
            {
                return new SAHLResponse { SAHLReference = applicationNumber.ToString() };
            }

            OfferType applicationType = validationUtils.ParseEnum<OfferType>(request.Application.SahlLoanPurpose);

            // setup the event & populate the domain process model
            switch (applicationType)
            {
                case OfferType.NewPurchaseLoan:
                    dataModel = applicationModelManager.PopulateNewPurchaseApplicationCreationModel(request.Application);
                    break;

                case OfferType.SwitchLoan:
                    dataModel = applicationModelManager.PopulateSwitchApplicationCreationModel(request.Application);
                    break;

                case OfferType.RefinanceLoan:
                    dataModel = applicationModelManager.PopulateRefinanceApplicationCreationModel(request.Application);
                    break;

                default:
                    break;
            }

            // this event will fire once the vendor is linked to the application
            // this is the last 'critical' step in the application creation process which must succeed before we can send back an application key to comcorp
            eventToWaitFor = typeof(ExternalVendorLinkedToApplicationEvent).Name;

            // start the required domain process
            IStartDomainProcessResponse startResponse = this.domainProcessManagerClientApiFactory.Create()
                .DataModel(dataModel)
                .EventToWaitFor(eventToWaitFor)
                .StartProcess()
                .Result;

            if (startResponse.Result)
            {
                ApplicationCreationReturnDataModel returnData = startResponse.Data as ApplicationCreationReturnDataModel;
                applicationNumber = returnData.ApplicationNumber;
            }

            return new SAHLResponse { SAHLReference = applicationNumber.ToString() };
        }

        public SupportingDocumentsReply Handle(ImagingCoApplicantRequest request)
        {
            string requestMac = request.RequestHeader.RequestMac;
            var requestMessage = request;
            requestMessage.RequestHeader.RequestMac = String.Empty;
            var authenticated = IsRequestMacAuthenticated(requestMessage, requestMac);

            if (!authenticated)
            {
                return CreateInvalidRequestMacReply(request.RequestHeader);
            }

            var applicant = request.SupportingDocuments.CoApplicantDocuments;
            var requestHeader = request.RequestHeader;

            var applicationDocuments = new List<ApplicationDocumentsModel>();
            applicationDocuments.Add(new ApplicationDocumentsModel(applicant.ApplicantIdentityNumber, applicant.SupportingDocument.ToList<documentType>(), requestHeader.RequestDateTime));

            if (applicant.SpouseDocuments != null)
            {
                applicationDocuments.Add(new ApplicationDocumentsModel(applicant.SpouseDocuments.ApplicantIdentityNumber, applicant.SpouseDocuments.SupportingDocument.ToList<documentType>(),
                    requestHeader.RequestDateTime));
            }
            return supportingDocumentManager.SaveClientFileDocuments(applicationDocuments, requestHeader);
        }

        public SupportingDocumentsReply Handle(ImagingMainApplicantRequest request)
        {
            string requestMac = request.RequestHeader.RequestMac;
            var requestMessage = request;
            requestMessage.RequestHeader.RequestMac = String.Empty;
            var authenticated = IsRequestMacAuthenticated(requestMessage, requestMac);

            if (!authenticated)
            {
                return CreateInvalidRequestMacReply(request.RequestHeader);
            }

            var applicant = request.SupportingDocuments.MainApplicantDocuments;
            var requestHeader = request.RequestHeader;

            var applicationDocuments = new List<ApplicationDocumentsModel>();
            applicationDocuments.Add(new ApplicationDocumentsModel(applicant.ApplicantIdentityNumber, applicant.SupportingDocument.ToList<documentType>(), requestHeader.RequestDateTime));
            if (applicant.SpouseDocuments != null)
            {
                applicationDocuments.Add(new ApplicationDocumentsModel(applicant.SpouseDocuments.ApplicantIdentityNumber, applicant.SpouseDocuments.SupportingDocument.ToList<documentType>(),
                    requestHeader.RequestDateTime));
            }
            return supportingDocumentManager.SaveClientFileDocuments(applicationDocuments, requestHeader);
        }

        public SupportingDocumentsReply Handle(ImagingApplicationRequest request)
        {
            string requestMac = request.RequestHeader.RequestMac;
            var requestMessage = request;
            requestMessage.RequestHeader.RequestMac = String.Empty;
            var authenticated = IsRequestMacAuthenticated(requestMessage, requestMac);
            if (!authenticated)
            {
                return CreateInvalidRequestMacReply(request.RequestHeader);
            }

            var applicationDocuments = new List<ApplicationDocumentsModel>();
            applicationDocuments.Add(new ApplicationDocumentsModel("", request.SupportingDocuments.ApplicationDocuments.ToList(), request.RequestHeader.RequestDateTime));
            return supportingDocumentManager.SaveClientFileDocuments(applicationDocuments, request.RequestHeader);
        }

        private bool IsRequestMacAuthenticated<T>(T request, string requestMac)
        {
            try
            {
                return macAuthenticationManager.AuthenticateMessage(request, requestMac);
            }
            catch
            {
                return false;
            }
        }

        private SupportingDocumentsReply CreateInvalidRequestMacReply(headerType requestHeader)
        {
            var reply = new SupportingDocumentsReply();
            reply.ReplyHeader = new ReplyHeaderType
            {
                ApplicationReference = requestHeader.ApplicationReference,
                ImagingReference = requestHeader.ImagingReference,
                ReplyDateTime = DateTime.Now,
                RequestStatus = (int)DocumentReplyStatus.InvalidRequestMac,
                ServiceVersion = requestHeader.ServiceVersion
            };
            return reply;
        }
    }
}
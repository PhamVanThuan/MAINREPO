using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument.Models;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument
{
    public class SupportingDocumentManager : ISupportingDocumentManager
    {
        private const FileExtension IMAGE_EXTENSION = FileExtension.Tiff;
        private IDocumentManagerServiceClient documentManagerService;
        private IDomainQueryServiceClient domainQueryService;
        private IImagingRequestDataManager dataManager;

        public SupportingDocumentManager(IDocumentManagerServiceClient documentManagerService, IDomainQueryServiceClient domainQueryService, IImagingRequestDataManager dataManager, 
            IUnitOfWorkFactory uowFactory)
        {
            this.documentManagerService = documentManagerService;
            this.domainQueryService = domainQueryService;
            this.dataManager = dataManager;
            this.uowFactory = uowFactory;
        }

        private List<errorType> errors;
        private IUnitOfWorkFactory uowFactory;

        public SupportingDocumentsReply SaveClientFileDocuments(List<ApplicationDocumentsModel> applicationDocuments, headerType requestHeader)
        {
            errors = new List<errorType>();
            var reply = CreateSupportingDocumentsReply(requestHeader, DocumentReplyStatus.RequestSuccessful);
            var replyHeader = reply.ReplyHeader;

            if (!ValidateBankReference(requestHeader.BankReference))
            {
                replyHeader.RequestStatus = (int)DocumentReplyStatus.BankReferenceUnknown;
                return reply;
            }

            var offerKey = Int32.Parse(requestHeader.BankReference);

            using (var uow = uowFactory.Build())
            {
                if (String.IsNullOrEmpty(requestHeader.ImagingReference))
                {
                    replyHeader.ImagingReference = CreateNewImagingReference(offerKey, (int)requestHeader.RequestMessages);
                }
                else
                {
                    if (!ValidateImagingReference(requestHeader.ImagingReference))
                    {
                        replyHeader.RequestStatus = (int)DocumentReplyStatus.ImagingReferenceUnknown;
                        return reply;
                    }
                }

                var applicantDetails = GetApplicantDetails(Int32.Parse(requestHeader.BankReference));
                if (applicantDetails.Count() < 1)
                {
                    replyHeader.RequestStatus = (int)DocumentReplyStatus.DocumentValidationFailed;
                    return reply;
                }

                var branchConsultantUsername = GetBranchConsultantUsername(offerKey);
                var clientFileDocuments = CreateClientFilesFromApplicationDocuments(applicationDocuments, applicantDetails, branchConsultantUsername);

                var command = new StoreClientFileDocumentsCommand(clientFileDocuments);
                var messages = documentManagerService.PerformCommand(command, new ServiceRequestMetadata());
                if (messages.ErrorMessages().Any())
                {
                    foreach (var message in messages.ErrorMessages())
                    {
                        errors.Add(new errorType
                        {
                            ErrorDescription = message.Message
                        });
                    }
                    replyHeader.RequestStatus = (int)DocumentReplyStatus.UnknownError;
                    return reply;
                }
                else
                {
                    dataManager.IncrementMessagesReceived(Guid.Parse(replyHeader.ImagingReference));
                }
                uow.Complete();
            }
            reply.RequestErrors = errors.ToArray();
            return reply;
        }

        private bool ValidateBankReference(string bankReference)
        {
            int offerKey = -1;
            if (Int32.TryParse(bankReference, out offerKey))
            {
                var offerExistsQuery = new DoesOfferExistQuery(offerKey);
                domainQueryService.PerformQuery(offerExistsQuery);
                return offerExistsQuery.Result.Results.First().OfferExist;
            }
            else
            {
                return false;
            }
        }

        private string CreateNewImagingReference(int offerKey, int expectedMessages)
        {
            var imagingReferenceGuid = Guid.NewGuid();
            dataManager.SaveNewImagingReference(imagingReferenceGuid, offerKey, expectedMessages);
            return imagingReferenceGuid.ToString();
        }

        private bool ValidateImagingReference(string reference)
        {
            Guid imagingReferenceGuid;
            if ((!Guid.TryParse(reference, out imagingReferenceGuid)) || !dataManager.DoesImagingReferenecExist(imagingReferenceGuid))
            {
                return false;
            }
            return true;
        }

        private List<GetApplicantDetailsForOfferQueryResult> GetApplicantDetails(int offerKey)
        {
            var applicantsQuery = new GetApplicantDetailsForOfferQuery(offerKey);
            domainQueryService.PerformQuery(applicantsQuery);
            return applicantsQuery.Result.Results.ToList();
        }

        private string GetBranchConsultantUsername(int offerKey)
        {
            var consultantUsernameQuery = new GetOfferBranchConsultantUsernameQuery(offerKey);
            domainQueryService.PerformQuery(consultantUsernameQuery);
            return consultantUsernameQuery.Result.Results.First().BranchConsultantUsername;
        }

        private List<ClientFileDocumentModel> CreateClientFilesFromApplicationDocuments(List<ApplicationDocumentsModel> applicationDocuments, 
            List<GetApplicantDetailsForOfferQueryResult> applicantDetails, string username)
        {
            var documents = new List<ClientFileDocumentModel>();
            foreach (var applicationDocument in applicationDocuments)
            {
                var applicantDetailModel = applicantDetails.FirstOrDefault(x => x.IdentityNumber == applicationDocument.IdentityNumber);
                if (applicantDetailModel == null)
                {
                    applicantDetailModel = applicantDetails.First(x => x.IsMainApplicant);
                }

                foreach (var supportingDocument in applicationDocument.SupportingDocuments)
                {
                    var document = new ClientFileDocumentModel(supportingDocument.DocumentImage, supportingDocument.DocumentDescription, applicantDetailModel.IdentityNumber,
                        applicantDetailModel.FirstNames, applicantDetailModel.Surname, username, applicationDocument.RequestDateTime, IMAGE_EXTENSION);
                    documents.Add(document);
                }
            }
            return documents;
        }

        private SupportingDocumentsReply CreateSupportingDocumentsReply(headerType requestHeader, DocumentReplyStatus status)
        {
            var reply = new SupportingDocumentsReply();
            reply.ReplyHeader = new ReplyHeaderType
            {
                ApplicationReference = requestHeader.ApplicationReference,
                ImagingReference = requestHeader.ImagingReference,
                ReplyDateTime = DateTime.Now,
                RequestStatus = (int)status,
                ServiceVersion = requestHeader.ServiceVersion
            };
            return reply;
        }
    }
}
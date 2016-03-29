using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument.Models;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.SupportingDocument
{
    public class when_saving_docs_for_an_applicant_and_spouse : WithCoreFakes
    {
        private static IDomainQueryServiceClient domainQueryService;
        private static IDocumentManagerServiceClient documentManagerService;
        private static SupportingDocumentManager supportingDocumentManager;
        private static SupportingDocumentsReply reply;
        private static headerType requestHeader;

        private static List<ApplicationDocumentsModel> applicationDocuments;

        private static DateTime requestDateTime;
        private static int bankReference;
        private static GetApplicantDetailsForOfferQueryResult applicantDetails;
        private static GetApplicantDetailsForOfferQueryResult spouseDetails;
        private static documentType spouseSupportingDocument;
        private static documentType applicantSupportingDocument;

        private static string spouseIdentityNumber;
        private static string applicantIdentityNumber;

        private Establish context = () =>
        {
            var dataManager = An<IImagingRequestDataManager>();
            documentManagerService = An<IDocumentManagerServiceClient>();
            domainQueryService = An<IDomainQueryServiceClient>();
            bankReference = 50000;
            requestDateTime = new DateTime(2014, 09, 11);

            spouseIdentityNumber = "25468971230123";
            applicantIdentityNumber = "1234567890123";

            applicantSupportingDocument = SupportingDocumentFactory.GetSupportingDocument(11, "ID Documents");
            spouseSupportingDocument = SupportingDocumentFactory.GetSupportingDocument(12, "Driver's license");

            applicationDocuments = new List<ApplicationDocumentsModel> {
                new ApplicationDocumentsModel(spouseIdentityNumber, new List<documentType> { spouseSupportingDocument }, requestDateTime),
                new ApplicationDocumentsModel(applicantIdentityNumber, new List<documentType> { applicantSupportingDocument }, requestDateTime)
            };
            requestHeader = SupportingDocumentFactory.GetDocumentRequestHeader(bankReference.ToString(), "", requestDateTime);

            applicantDetails = new GetApplicantDetailsForOfferQueryResult { FirstNames = "Adam", Surname = "Smith", IdentityNumber = applicantIdentityNumber, IsMainApplicant = true };
            spouseDetails = new GetApplicantDetailsForOfferQueryResult { FirstNames = "Eve", Surname = "Smith", IdentityNumber = spouseIdentityNumber, IsMainApplicant = false };

            supportingDocumentManager = new SupportingDocumentManager(documentManagerService, domainQueryService, dataManager, unitOfWorkFactory);

            dataManager.WhenToldTo(x => x.DoesImagingReferenecExist(Param.IsAny<Guid>())).Return(true);
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<DoesOfferExistQuery>
                .Matches(y => y.OfferKey == bankReference)))
                .Callback<DoesOfferExistQuery>(q =>
                    {
                        q.Result = new ServiceQueryResult<DoesOfferExistQueryResult>(new List<DoesOfferExistQueryResult> { new DoesOfferExistQueryResult { OfferExist = true } });
                    });
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<GetApplicantDetailsForOfferQuery>
               .Matches(y => y.OfferKey == bankReference)))
               .Callback<GetApplicantDetailsForOfferQuery>(q =>
                    {
                        q.Result = new ServiceQueryResult<GetApplicantDetailsForOfferQueryResult>(new List<GetApplicantDetailsForOfferQueryResult> { applicantDetails, spouseDetails });
                    });
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<GetOfferBranchConsultantUsernameQuery>
                .Matches(m => m.OfferKey == bankReference)))
                .Callback<GetOfferBranchConsultantUsernameQuery>(q =>
                    {
                        q.Result = new ServiceQueryResult<GetOfferBranchConsultantUsernameQueryResult>(new List<GetOfferBranchConsultantUsernameQueryResult>
                        {
                            new GetOfferBranchConsultantUsernameQueryResult {BranchConsultantUsername = "Hilda" }
                        });
                    });
        };

        private Because of = () =>
        {
            reply = supportingDocumentManager.SaveClientFileDocuments(applicationDocuments, requestHeader);
        };

        private It should_query_if_the_offer_exists = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Param<DoesOfferExistQuery>.Matches(y => y.OfferKey == bankReference)));
        };

        private It should_get_the_details_of_the_applicants_on_the_offer = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Param<GetApplicantDetailsForOfferQuery>.Matches(y => y.OfferKey == bankReference)));
        };

        private It should_call_the_document_manager_service_with_the_applicant_document = () =>
        {
            documentManagerService.WasToldTo(x => x.PerformCommand(Param<StoreClientFileDocumentsCommand>.Matches(y =>
                y.ClientFileDocuments.Any(d =>
                    d.Category == applicantSupportingDocument.DocumentDescription &&
                    d.Document == applicantSupportingDocument.DocumentImage &&
                    d.DocumentSubmitDate == requestDateTime &&
                    d.OriginalExtension == FileExtension.Tiff &&
                    d.IdentityNumber == applicantDetails.IdentityNumber &&
                    d.FirstName == applicantDetails.FirstNames &&
                    d.Surname == applicantDetails.Surname)), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_call_the_document_manager_service_with_the_spouse_document = () =>
        {
            documentManagerService.WasToldTo(x => x.PerformCommand(Param<StoreClientFileDocumentsCommand>.Matches(y =>
                y.ClientFileDocuments.Any(d =>
                    d.Category == spouseSupportingDocument.DocumentDescription &&
                    d.Document == spouseSupportingDocument.DocumentImage &&
                    d.DocumentSubmitDate == requestDateTime &&
                    d.OriginalExtension == FileExtension.Tiff &&
                    d.IdentityNumber == spouseIdentityNumber &&
                    d.FirstName == spouseDetails.FirstNames &&
                    d.Surname == spouseDetails.Surname)), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_a_successful_reply_status = () =>
        {
            reply.ReplyHeader.ShouldMatch(x => x.RequestStatus == (int)DocumentReplyStatus.RequestSuccessful);
        };
    }
}
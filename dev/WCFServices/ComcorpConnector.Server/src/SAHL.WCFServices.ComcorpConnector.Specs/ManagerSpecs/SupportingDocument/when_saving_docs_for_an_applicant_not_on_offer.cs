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
    public class when_saving_docs_for_an_applicant_not_on_offer : WithCoreFakes
    {
        private static IDomainQueryServiceClient domainQueryService;
        private static IDocumentManagerServiceClient documentManagerService;
        private static SupportingDocumentManager supportingDocumentManager;
        private static SupportingDocumentsReply reply;
        private static headerType requestHeader;
        private static DateTime requestDateTime;
        private static int bankReference;
        private static string identityNumber;
        private static GetApplicantDetailsForOfferQueryResult applicantDetails;
        private static List<ApplicationDocumentsModel> applicationDocuments;
        private static documentType applicantSupportingDocument;

        private Establish context = () =>
        {
            var dataManager = An<IImagingRequestDataManager>();
            documentManagerService = An<IDocumentManagerServiceClient>();
            domainQueryService = An<IDomainQueryServiceClient>();
            bankReference = 50000;
            requestDateTime = new DateTime(2014, 09, 11);
            identityNumber = "1234567890123";

            applicantSupportingDocument = SupportingDocumentFactory.GetSupportingDocument(12, "ID Documents");
            applicationDocuments = new List<ApplicationDocumentsModel>
            {
                new ApplicationDocumentsModel(identityNumber, new List<documentType> {applicantSupportingDocument }, requestDateTime)
            };
            requestHeader = SupportingDocumentFactory.GetDocumentRequestHeader(bankReference.ToString(), "", requestDateTime);

            applicantDetails = new GetApplicantDetailsForOfferQueryResult { FirstNames = "Henry", Surname = "Jekyll", IdentityNumber = "1234567890123", IsMainApplicant = true };

            supportingDocumentManager = new SupportingDocumentManager(documentManagerService, domainQueryService, dataManager, unitOfWorkFactory);

            dataManager.WhenToldTo(x => x.DoesImagingReferenecExist(Param.IsAny<Guid>())).Return(true);
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery<DoesOfferExistQuery>(Param<DoesOfferExistQuery>
                .Matches(y => y.OfferKey == bankReference)))
                            .Callback<DoesOfferExistQuery>(q =>
                    {
                        q.Result = new ServiceQueryResult<DoesOfferExistQueryResult>(new List<DoesOfferExistQueryResult> { new DoesOfferExistQueryResult { OfferExist = true } });
                    });
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery<GetApplicantDetailsForOfferQuery>(Param<GetApplicantDetailsForOfferQuery>
               .Matches(y => y.OfferKey == bankReference)))
               .Callback<GetApplicantDetailsForOfferQuery>(q =>
                    {
                        q.Result = new ServiceQueryResult<GetApplicantDetailsForOfferQueryResult>(new List<GetApplicantDetailsForOfferQueryResult> { applicantDetails });
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
            domainQueryService.WasToldTo(x => x.PerformQuery<DoesOfferExistQuery>(Param<DoesOfferExistQuery>.Matches(y => y.OfferKey == bankReference)));
        };

        private It should_get_the_details_of_the_applicants_on_the_offer = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery<GetApplicantDetailsForOfferQuery>(Param<GetApplicantDetailsForOfferQuery>.Matches(y => y.OfferKey == bankReference)));
        };

        private It should_call_the_document_manager_service_and_use_the_main_applicant_id = () =>
        {
            documentManagerService.WasToldTo(x => x.PerformCommand<StoreClientFileDocumentsCommand>(Param<StoreClientFileDocumentsCommand>.Matches(y =>
                y.ClientFileDocuments.Any(d =>
                    d.Category == applicantSupportingDocument.DocumentDescription &&
                    d.Document == applicantSupportingDocument.DocumentImage &&
                    d.DocumentSubmitDate == requestDateTime &&
                    d.OriginalExtension == FileExtension.Tiff &&
                    d.IdentityNumber == applicantDetails.IdentityNumber &&
                    d.FirstName == applicantDetails.FirstNames &&
                    d.Surname == applicantDetails.Surname)), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_a_successful_reply_status = () =>
        {
            reply.ReplyHeader.ShouldMatch(x => x.RequestStatus == (int)DocumentReplyStatus.RequestSuccessful);
        };
    }
}
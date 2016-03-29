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
    public class when_saving_documents_for_an_application : WithCoreFakes
    {
        private static IDomainQueryServiceClient domainQueryService;
        private static IDocumentManagerServiceClient documentManagerService;
        private static SupportingDocumentManager supportingDocumentManager;
        private static List<ApplicationDocumentsModel> applicationDocuments;
        private static headerType requestHeader;
        private static SupportingDocumentsReply reply;
        private static DateTime requestDateTime;
        private static int bankReference;
        private static string identityNumber;
        private static GetApplicantDetailsForOfferQueryResult applicantDetails;
        private static List<documentType> supportingDocuments;

        private Establish context = () =>
        {
            var dataManager = An<IImagingRequestDataManager>();
            documentManagerService = An<IDocumentManagerServiceClient>();
            domainQueryService = An<IDomainQueryServiceClient>();
            bankReference = 50000;
            identityNumber = "";
            requestDateTime = new DateTime(2014, 09, 11);
            supportingDocuments = new List<documentType>
            {
                SupportingDocumentFactory.GetSupportingDocument(12, "ID Documents")
            };
            applicationDocuments = new List<ApplicationDocumentsModel>
            {
                new ApplicationDocumentsModel(identityNumber, supportingDocuments, requestDateTime)
            };
            requestHeader = SupportingDocumentFactory.GetDocumentRequestHeader(bankReference.ToString(), "", requestDateTime);

            applicantDetails = new GetApplicantDetailsForOfferQueryResult { FirstNames = "Bob", Surname = "Dylan", IdentityNumber = "1234567890123", IsMainApplicant = true };

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

        private It should_get_the_username_of_the_branch_consultant_for_the_offer = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Param<GetOfferBranchConsultantUsernameQuery>.Matches(y => y.OfferKey == bankReference)));
        };

        private It should_call_the_document_manager_service_with_the_supporting_documents = () =>
        {
            documentManagerService.WasToldTo(x => x.PerformCommand<StoreClientFileDocumentsCommand>(Param<StoreClientFileDocumentsCommand>.Matches(y =>
                y.ClientFileDocuments.Any(d =>
                    d.Category == supportingDocuments.First().DocumentDescription &&
                    d.Document == supportingDocuments.First().DocumentImage &&
                    d.DocumentSubmitDate == requestDateTime &&
                    d.OriginalExtension == FileExtension.Tiff)), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_use_the_main_applicant_id_number_and_names_when_saving_the_documents = () =>
        {
            documentManagerService.WasToldTo(x => x.PerformCommand<StoreClientFileDocumentsCommand>(Param<StoreClientFileDocumentsCommand>.Matches(y =>
                y.ClientFileDocuments.Any(d =>
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
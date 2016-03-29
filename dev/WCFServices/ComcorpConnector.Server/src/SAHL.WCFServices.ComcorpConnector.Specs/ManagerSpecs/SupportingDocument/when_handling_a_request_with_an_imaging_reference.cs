using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument.Models;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using System;
using System.Collections.Generic;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.SupportingDocument
{
    public class when_handling_a_request_with_an_imaging_reference : WithCoreFakes
    {
        private static SupportingDocumentManager manager;
        private static List<ApplicationDocumentsModel> applicationDocuments;
        private static headerType requestHeader;
        private static DateTime requestDateTime;
        private static IImagingRequestDataManager dataManager;
        private static int offerKey;
        private static int requestMessages;
        private static Guid imagingReference;
        private static SupportingDocumentsReply reply;

        private Establish context = () =>
        {
            requestDateTime = DateTime.Now;
            requestMessages = 1;
            var domainQueryService = An<IDomainQueryServiceClient>();
            dataManager = An<IImagingRequestDataManager>();
            manager = new SupportingDocumentManager(An<IDocumentManagerServiceClient>(), domainQueryService, dataManager, unitOfWorkFactory);
            offerKey = 50000;
            imagingReference = Guid.Parse("21B01EA4-EA8C-41D9-AD71-A31F008D7F43");

            requestHeader = SupportingDocumentFactory.GetDocumentRequestHeader(offerKey.ToString(), imagingReference.ToString(), requestDateTime, requestMessages);
            applicationDocuments = new List<ApplicationDocumentsModel>();

            dataManager.WhenToldTo(x => x.DoesImagingReferenecExist(imagingReference)).Return(true);
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<DoesOfferExistQuery>.Matches(y => y.OfferKey == offerKey)))
                .Callback<DoesOfferExistQuery>(y =>
                    {
                        y.Result = new ServiceQueryResult<DoesOfferExistQueryResult>(new List<DoesOfferExistQueryResult> { new DoesOfferExistQueryResult { OfferExist = true } });
                    });
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<GetApplicantDetailsForOfferQuery>.Matches(y => y.OfferKey == offerKey)))
                .Callback<GetApplicantDetailsForOfferQuery>(y =>
                    {
                        y.Result = new ServiceQueryResult<GetApplicantDetailsForOfferQueryResult>(new List<GetApplicantDetailsForOfferQueryResult> {
                            new GetApplicantDetailsForOfferQueryResult
                            {
                                IsMainApplicant = true,
                                FirstNames = "Henry",
                                Surname = "Black",
                                IdentityNumber = "1234567890123"
                            }
                        });
                    });
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<GetOfferBranchConsultantUsernameQuery>
                .Matches(m => m.OfferKey == offerKey)))
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
            reply = manager.SaveClientFileDocuments(applicationDocuments, requestHeader);
        };

        private It should_not_save_a_new_imaging_reference = () =>
        {
            dataManager.WasNotToldTo(x => x.SaveNewImagingReference(Param.IsAny<Guid>(), offerKey, requestMessages));
        };

        private It should_verify_the_imaging_reference_exists = () =>
        {
            dataManager.WasToldTo(x => x.DoesImagingReferenecExist(imagingReference));
        };

        private It should_update_the_number_of_messages_received = () =>
        {
            dataManager.WasToldTo(x => x.IncrementMessagesReceived(imagingReference));
        };

        private It should_have_the_imaging_reference_in_the_reply_header = () =>
        {
            reply.ReplyHeader.ImagingReference.ShouldEqual(imagingReference.ToString());
        };
    }
}
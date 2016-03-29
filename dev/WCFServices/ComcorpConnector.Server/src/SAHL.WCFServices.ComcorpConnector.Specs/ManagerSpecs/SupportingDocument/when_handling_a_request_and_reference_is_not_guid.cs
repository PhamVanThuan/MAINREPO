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
    public class when_handling_a_request_and_reference_is_not_guid : WithCoreFakes
    {
        private static SupportingDocumentManager manager;
        private static List<ApplicationDocumentsModel> applicationDocuments;
        private static headerType requestHeader;
        private static DateTime requestDateTime;
        private static IImagingRequestDataManager dataManager;
        private static int offerKey;
        private static int requestMessages;
        private static SupportingDocumentsReply reply;

        private Establish context = () =>
        {
            requestDateTime = DateTime.Now;
            requestMessages = 1;
            var domainQueryService = An<IDomainQueryServiceClient>();
            dataManager = An<IImagingRequestDataManager>();
            manager = new SupportingDocumentManager(An<IDocumentManagerServiceClient>(), domainQueryService, dataManager, unitOfWorkFactory);
            offerKey = 50000;

            requestHeader = SupportingDocumentFactory.GetDocumentRequestHeader(offerKey.ToString(), "Alice", requestDateTime);

            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<DoesOfferExistQuery>.Matches(y => y.OfferKey == offerKey)))
                .Callback<DoesOfferExistQuery>(y =>
                    {
                        y.Result = new ServiceQueryResult<DoesOfferExistQueryResult>(new List<DoesOfferExistQueryResult> { new DoesOfferExistQueryResult { OfferExist = true } });
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

        private It should_not_verify_the_imaging_reference_exists = () =>
        {
            dataManager.WasNotToldTo(x => x.DoesImagingReferenecExist(Param.IsAny<Guid>()));
        };

        private It should_return_an_invalid_reference_reply = () =>
        {
            reply.ReplyHeader.RequestStatus.ShouldEqual((int)DocumentReplyStatus.ImagingReferenceUnknown);
        };
    }
}
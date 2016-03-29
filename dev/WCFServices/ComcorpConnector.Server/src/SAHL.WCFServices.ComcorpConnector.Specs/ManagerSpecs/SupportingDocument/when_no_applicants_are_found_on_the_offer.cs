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
    public class when_no_applicants_are_found_on_the_offer : WithCoreFakes
    {
        private static IDomainQueryServiceClient domainQueryService;
        private static SupportingDocumentsReply reply;
        private static SupportingDocumentManager supportingDocumentManager;
        private static List<ApplicationDocumentsModel> documents;
        private static headerType requestHeader;

        private Establish context = () =>
        {
            domainQueryService = An<IDomainQueryServiceClient>();
            documents = new List<ApplicationDocumentsModel>();

            requestHeader = SupportingDocumentFactory.GetDocumentRequestHeader("2000", "", DateTime.Now);

            var dataManager = An<IImagingRequestDataManager>();
            supportingDocumentManager = new SupportingDocumentManager(An<IDocumentManagerServiceClient>(), domainQueryService, dataManager, unitOfWorkFactory);
            dataManager.WhenToldTo(x => x.DoesImagingReferenecExist(Param.IsAny<Guid>())).Return(true);
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery<DoesOfferExistQuery>(Param<DoesOfferExistQuery>.Matches(y => y.OfferKey == 2000)))
                .Callback<DoesOfferExistQuery>(y =>
                    {
                        y.Result = new ServiceQueryResult<DoesOfferExistQueryResult>(new List<DoesOfferExistQueryResult> { new DoesOfferExistQueryResult { OfferExist = true } });
                    });
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<GetApplicantDetailsForOfferQuery>.Matches(y => y.OfferKey == 2000)))
                .Callback<GetApplicantDetailsForOfferQuery>(y =>
                    {
                        y.Result = new ServiceQueryResult<GetApplicantDetailsForOfferQueryResult>(new List<GetApplicantDetailsForOfferQueryResult> { });
                    });
        };

        private Because of = () =>
        {
            reply = supportingDocumentManager.SaveClientFileDocuments(documents, requestHeader);
        };

        private It should_query_for_the_applicant_details = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Param<GetApplicantDetailsForOfferQuery>.Matches(y => y.OfferKey == 2000)));
        };

        private It should_return_a_validation_failed_status = () =>
        {
            reply.ReplyHeader.RequestStatus.ShouldEqual((int)DocumentReplyStatus.DocumentValidationFailed);
        };
    }
}
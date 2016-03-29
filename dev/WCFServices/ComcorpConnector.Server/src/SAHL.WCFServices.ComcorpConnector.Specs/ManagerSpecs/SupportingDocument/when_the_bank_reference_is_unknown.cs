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
using System.Collections.Generic;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.SupportingDocument
{
    public class when_the_bank_reference_is_unknown : WithCoreFakes
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
            requestHeader = new headerType
            {
                BankReference = "2000"
            };
            supportingDocumentManager = new SupportingDocumentManager(An<IDocumentManagerServiceClient>(), domainQueryService, An<IImagingRequestDataManager>(), unitOfWorkFactory);
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<DoesOfferExistQuery>.Matches(y => y.OfferKey == 2000)))
                .Callback<DoesOfferExistQuery>(y =>
                    {
                        y.Result = new ServiceQueryResult<DoesOfferExistQueryResult>(new List<DoesOfferExistQueryResult> { new DoesOfferExistQueryResult { OfferExist = false } });
                    });
        };

        private Because of = () =>
        {
            reply = supportingDocumentManager.SaveClientFileDocuments(documents, requestHeader);
        };

        private It should_query_if_the_offer_exists = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Param<DoesOfferExistQuery>.Matches(y => y.OfferKey == 2000)));
        };

        private It should_return_a_bank_reference_unknown_status = () =>
        {
            reply.ReplyHeader.RequestStatus.ShouldEqual((int)DocumentReplyStatus.BankReferenceUnknown);
        };
    }
}
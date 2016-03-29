using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DocumentManager.Commands;
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
    public class when_error_occurs_while_saving_documents : WithCoreFakes
    {
        private static IDomainQueryServiceClient domainQueryService;
        private static IDocumentManagerServiceClient documentManagerService;
        private static SupportingDocumentManager supportingDocumentManager;
        private static List<ApplicationDocumentsModel> applicationDocuments;
        private static headerType requestHeader;
        private static SupportingDocumentsReply reply;

        private Establish context = () =>
        {
            documentManagerService = An<IDocumentManagerServiceClient>();
            domainQueryService = An<IDomainQueryServiceClient>();
            var bankReference = 50000;
            var requestDateTime = new DateTime(2014, 09, 11);
            var supportingDocument = SupportingDocumentFactory.GetSupportingDocument(12, "ID Documents");

            applicationDocuments = new List<ApplicationDocumentsModel>
            {
                new ApplicationDocumentsModel("", new List<documentType> { supportingDocument }, requestDateTime)
            };
            requestHeader = SupportingDocumentFactory.GetDocumentRequestHeader(bankReference.ToString(), "", requestDateTime);

            var applicantDetails = new GetApplicantDetailsForOfferQueryResult { FirstNames = "Bob", Surname = "Dylan", IdentityNumber = "1234567890123", IsMainApplicant = true };
            var dataManager = An<IImagingRequestDataManager>();

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
            documentManagerService.WhenToldTo(x => x.PerformCommand(Param.IsAny<StoreClientFileDocumentsCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                    .Return(new SystemMessageCollection(new List<ISystemMessage> { new SystemMessage("Something went wrong", SystemMessageSeverityEnum.Error) }));
        };

        private Because of = () =>
        {
            reply = supportingDocumentManager.SaveClientFileDocuments(applicationDocuments, requestHeader);
        };

        private It should_try_to_save_the_documents = () =>
        {
            documentManagerService.WasToldTo(x => x.PerformCommand(Param.IsAny<StoreClientFileDocumentsCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_an_unknown_error_status = () =>
        {
            reply.ReplyHeader.RequestStatus.ShouldEqual((int)DocumentReplyStatus.UnknownError);
        };
    }
}
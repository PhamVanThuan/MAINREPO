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
    public class when_handling_a_new_imaging_request : WithCoreFakes
    {
        private static SupportingDocumentManager manager;
        private static List<ApplicationDocumentsModel> applicationDocuments;
        private static headerType requestHeader;
        private static DateTime requestDateTime;
        private static IImagingRequestDataManager dataManager;
        private static int offerKey;
        private static int requestMessages;
        private static SupportingDocumentsReply reply;
        private static Guid imagingReference;

        private Establish context = () =>
        {
            var domainQueryService = An<IDomainQueryServiceClient>();
            dataManager = An<IImagingRequestDataManager>();
            manager = new SupportingDocumentManager(An<IDocumentManagerServiceClient>(), domainQueryService, dataManager, unitOfWorkFactory);
            offerKey = 50000;
            requestMessages = 1;
            requestDateTime = DateTime.Now;
            imagingReference = Guid.NewGuid();
            applicationDocuments = new List<ApplicationDocumentsModel>();
            requestHeader = SupportingDocumentFactory.GetDocumentRequestHeader(offerKey.ToString(), "", requestDateTime, requestMessages);

            var applicantDetails = new GetApplicantDetailsForOfferQueryResult { FirstNames = "Bob", Surname = "Dylan", IdentityNumber = "1234567890123", IsMainApplicant = true };

            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<DoesOfferExistQuery>.Matches(y => y.OfferKey == offerKey)))
                .Callback<DoesOfferExistQuery>(y =>
                    {
                        y.Result = new ServiceQueryResult<DoesOfferExistQueryResult>(new List<DoesOfferExistQueryResult> { new DoesOfferExistQueryResult { OfferExist = true } });
                    });
            domainQueryService.WhenToldTo<IDomainQueryServiceClient>(x => x.PerformQuery(Param<GetApplicantDetailsForOfferQuery>.Matches(y => y.OfferKey == offerKey)))
                .Callback<GetApplicantDetailsForOfferQuery>(y =>
                    {
                        y.Result = new ServiceQueryResult<GetApplicantDetailsForOfferQueryResult>(new List<GetApplicantDetailsForOfferQueryResult> { applicantDetails });
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

        private It should_save_a_new_imaging_reference = () =>
        {
            dataManager.WasToldTo(x => x.SaveNewImagingReference(Param.IsAny<Guid>(), offerKey, requestMessages));
        };

        private It should_return_the_new_imaging_reference_with_the_reply = () =>
        {
            Guid result;
            Guid.TryParse(reply.ReplyHeader.ImagingReference, out result).ShouldBeTrue();
        };
    }
}
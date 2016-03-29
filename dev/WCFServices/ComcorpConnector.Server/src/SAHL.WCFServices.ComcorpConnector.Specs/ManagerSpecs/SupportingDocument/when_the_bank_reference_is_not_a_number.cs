using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DocumentManager;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument;
using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument.Models;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using System.Collections.Generic;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.SupportingDocument
{
    public class when_the_bank_reference_is_not_a_number : WithCoreFakes
    {
        private static SupportingDocumentsReply reply;
        private static SupportingDocumentManager supportingDocumentManager;
        private static List<ApplicationDocumentsModel> documents;
        private static headerType requestHeader;

        private Establish context = () =>
        {
            documents = new List<ApplicationDocumentsModel>();
            requestHeader = new headerType
            {
                BankReference = "ABC"
            };
            supportingDocumentManager = new SupportingDocumentManager(An<IDocumentManagerServiceClient>(), An<IDomainQueryServiceClient>(), An<IImagingRequestDataManager>(), unitOfWorkFactory);
        };

        private Because of = () =>
        {
            reply = supportingDocumentManager.SaveClientFileDocuments(documents, requestHeader);
        };

        private It should_return_a_bank_reference_unknown_status = () =>
        {
            reply.ReplyHeader.RequestStatus.ShouldEqual((int)DocumentReplyStatus.BankReferenceUnknown);
        };
    }
}
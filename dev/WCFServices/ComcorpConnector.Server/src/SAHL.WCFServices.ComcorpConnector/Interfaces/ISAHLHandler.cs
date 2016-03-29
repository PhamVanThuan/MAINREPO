using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;

namespace SAHL.WCFServices.ComcorpConnector.Interfaces
{
    public interface ISAHLHandler
    {
        SAHLResponse Handle(SAHLRequest request);

        SupportingDocumentsReply Handle(ImagingApplicationRequest request);

        SupportingDocumentsReply Handle(ImagingCoApplicantRequest request);

        SupportingDocumentsReply Handle(ImagingMainApplicantRequest request);
    }
}
using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using System.Net.Security;
using System.ServiceModel;

namespace SAHL.WCFServices.ComcorpConnector.Interfaces
{
    [ServiceContract]
    public interface ISAHLDocumentService
    {
        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
        [FaultContract(typeof(SAHLDocumentFault), ProtectionLevel = ProtectionLevel.None)]
        SupportingDocumentsReply ProcessApplicationMessage(ImagingApplicationRequest ImagingApplicationRequest);

        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
        [FaultContract(typeof(SAHLDocumentFault), ProtectionLevel = ProtectionLevel.None)]
        SupportingDocumentsReply ProcessMainApplicantMessage(ImagingMainApplicantRequest ImagingMainApplicantRequest);

        [OperationContract]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
        [FaultContract(typeof(SAHLDocumentFault), ProtectionLevel = ProtectionLevel.None)]
        SupportingDocumentsReply ProcessCoApplicantMessage(ImagingCoApplicantRequest ImagingCoApplicantRequest);
    }
}
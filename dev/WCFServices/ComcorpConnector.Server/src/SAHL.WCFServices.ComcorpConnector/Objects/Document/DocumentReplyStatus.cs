namespace SAHL.WCFServices.ComcorpConnector.Objects.Document
{
    public enum DocumentReplyStatus
    {
        RequestSuccessful = 1,
        UnknownError = 2,
        UnableToLoadXmlRequestDocument = 3,
        InvalidRequestMac = 4,
        InvalidServiceKey = 5,
        BankReferenceUnknown = 6,
        ImagingReferenceUnknown = 7,
        FailedToAuthenticate = 8,
        FailedToParse = 9,
        ApplicationAlreadyReceived = 10,
        DocumentValidationFailed = 11,
        DocumentTypeUnknown = 12
    }
}
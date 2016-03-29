using SAHL.WCFServices.ComcorpConnector.Managers.SupportingDocument.Models;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using System;
using System.Collections.Generic;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.SupportingDocument
{
    public class SupportingDocumentFactory
    {
        public static documentType GetSupportingDocument(decimal reference, string type)
        {
            return new documentType
            {
                DocumentImage = "onetuhonechuroecidoenpt,.ncekheneuidontehunosecuh29384g9.cob234",
                DocumentDescription = "description",
                DocumentComments = "No comment",
                DocumentReference = reference,
                DocumentType = type
            };
        }

        public static headerType GetDocumentRequestHeader(string bankReference, string imagingReference, DateTime requestDateTime, int expectedMessages = 1)
        {
            return new headerType
            {
                BankReference = bankReference,
                ImagingReference = imagingReference,
                RequestAction = headerTypeRequestAction.New,
                RequestDateTime = requestDateTime,
                ApplicationReference = "1234",
                RequestMac = "noethur,c.h39g4h,.pb23bp<>Po",
                RequestMessages = expectedMessages,
                ServiceVersion = 1
            };
        }
    }
}
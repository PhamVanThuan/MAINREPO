using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest.Statements
{
    public class IncrementMessagesReceived : ISqlStatement<ComcorpImagingRequestDataModel>
    {
        public Guid ImagingReference { get; protected set; }

        public IncrementMessagesReceived(Guid imagingReference)
        {
            this.ImagingReference = imagingReference;
        }

        public string GetStatement()
        {
            return @"UPDATE [2AM].[dbo].[ComcorpImagingRequest]
                     SET ReceivedDocuments = ReceivedDocuments + 1
                     WHERE ImagingReference = @ImagingReference";
        }
    }
}
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest.Statements
{
    public class DoesImagingRequestExistForReference : ISqlStatement<int>
    {
        public Guid ImagingReference { get; private set; }

        public DoesImagingRequestExistForReference(Guid imagingReference)
        {
            this.ImagingReference = imagingReference;
        }

        public string GetStatement()
        {
            return @"SELECT COUNT(1)
                                FROM[2AM].[dbo].[ComcorpImagingRequest]
                                WHERE ImagingReference = @ImagingReference";
        }
    }
}
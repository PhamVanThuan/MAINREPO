using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest
{
    public interface IImagingRequestDataManager
    {
        void SaveNewImagingReference(Guid imagingReference, int offerKey, int expectedDocuments);

        bool DoesImagingReferenecExist(Guid imagingReference);

        void IncrementMessagesReceived(Guid imagingReferenceGuid);
    }
}
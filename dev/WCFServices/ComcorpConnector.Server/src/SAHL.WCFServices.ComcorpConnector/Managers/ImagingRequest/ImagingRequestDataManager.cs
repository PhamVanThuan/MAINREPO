using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest.Statements;
using System;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest
{
    public class ImagingRequestDataManager : IImagingRequestDataManager
    {
        private IDbFactory dbFactory;

        public ImagingRequestDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool DoesImagingReferenecExist(Guid imagingReference)
        {
            var query = new DoesImagingRequestExistForReference(imagingReference);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                var count = db.SelectOne<int>(query);
                return count == 1;
            }
        }

        public void IncrementMessagesReceived(Guid imagingReferenceGuid)
        {
            var statement = new IncrementMessagesReceived(imagingReferenceGuid);
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.Update<ComcorpImagingRequestDataModel>(statement);
                db.Complete();
            }
        }

        public void SaveNewImagingReference(Guid imagingReference, int offerKey, int expectedDocuments)
        {
            var imagingRequest = new ComcorpImagingRequestDataModel(offerKey, imagingReference, expectedDocuments, 0);
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.Insert(imagingRequest);
                db.Complete();
            }
        }
    }
}
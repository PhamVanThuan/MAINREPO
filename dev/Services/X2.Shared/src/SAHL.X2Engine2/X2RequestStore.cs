using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Providers;
using System;

namespace SAHL.X2Engine2
{
    public class X2RequestStore : IX2RequestStore
    {
        private ISerializationProvider serializationProvider;

        public X2RequestStore(ISerializationProvider serializationProvider)
        {
            this.serializationProvider = serializationProvider;
        }

        public void StoreReceivedRequest(IX2Request request)
        {
            string serializedRequest = this.serializationProvider.Serialize<IX2Request>(request);
            using (var db = new Db().InWorkflowContext())
            {
                RequestDataModel requestToStore = new RequestDataModel(request.CorrelationId, serializedRequest, (int)SAHL.X2Engine2.Enumerations.RequestStatus.Received, DateTime.Now, DateTime.Now, 0);
                db.Insert<RequestDataModel>(requestToStore);
                db.Complete();
            }
        }

        public void UpdateReceivedRequestAsRouted(IX2Request request)
        {
            using (var db = new Db().InWorkflowContext())
            {
                RequestDataModel storedRequest = db.SelectOne<RequestDataModel>("select * from x2.x2.request where requestID=@RequestID", new { RequestID = request.CorrelationId });
                storedRequest.RequestStatusID = (int)SAHL.X2Engine2.Enumerations.RequestStatus.Routed;
                storedRequest.RequestUpdatedDate = DateTime.Now;
                db.Update<RequestDataModel>(storedRequest);
                db.Complete();
            }
        }

        public void UpdateReceivedRequestAsTimedoutAndWaiting(IX2Request request)
        {
            //TODO
            try
            {
                using (var db = new Db().InWorkflowContext())
                {
                    RequestDataModel storedRequest = db.SelectOne<RequestDataModel>("select * from x2.x2.request where requestID=@RequestID", new { RequestID = request.CorrelationId });
                    storedRequest.RequestStatusID = (int)SAHL.X2Engine2.Enumerations.RequestStatus.TimedOut;
                    storedRequest.RequestUpdatedDate = DateTime.Now;
                    storedRequest.RequestTimeoutRetries = (storedRequest.RequestTimeoutRetries + 1);
                    db.Update<RequestDataModel>(storedRequest);
                    db.Complete();
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void UpdateReceivedRequestAsTimedoutAndNotServicable(IX2Request request)
        {
            using (var db = new Db().InWorkflowContext())
            {
                try
                {
                    RequestDataModel storedRequest = db.SelectOne<RequestDataModel>("select * from x2.x2.request where requestID=@RequestID", new { RequestID = request.CorrelationId });
                    storedRequest.RequestStatusID = (int)SAHL.X2Engine2.Enumerations.RequestStatus.TimedOutAndNotServicable;
                    storedRequest.RequestUpdatedDate = DateTime.Now;
                    storedRequest.RequestTimeoutRetries = (storedRequest.RequestTimeoutRetries + 1);
                    db.Update<RequestDataModel>(storedRequest);
                }
                catch (Exception ex)
                {
                    // still cant work out why the request is no longer in the DB. Cant see whose removing it!
                    string err = ex.ToString();
                    Console.WriteLine(err);
                }
                db.Complete();
            }
        }

        public int GetNumberOfTimeouts(IX2Request request)
        {
            using (var db = new Db().InWorkflowContext())
            {
                try
                {
                    RequestDataModel storedRequest = db.SelectOne<RequestDataModel>("select * from x2.x2.request where requestID=@RequestID", new { RequestID = request.CorrelationId });
                    return storedRequest.RequestTimeoutRetries;
                }
                catch
                { }
                return 0;
            }
        }

        public void RemoveCompletedRequest(Guid requestID)
        {
            using (var db = new Db().InWorkflowContext())
            {
                db.DeleteByKey<RequestDataModel, Guid>(requestID);
                db.Complete();
            }
        }

        public void StoreReceivedRequestAsUnserviceableDueToNoAvailableRoute(IX2Request request)
        {
            using (var db = new Db().InWorkflowContext())
            {
                RequestDataModel storedRequest = db.SelectOne<RequestDataModel>("select * from x2.x2.request where requestID=@RequestID", new { RequestID = request.CorrelationId });
                storedRequest.RequestStatusID = (int)SAHL.X2Engine2.Enumerations.RequestStatus.NoRouteAvailable;
                storedRequest.RequestUpdatedDate = DateTime.Now;
                storedRequest.RequestTimeoutRetries = (storedRequest.RequestTimeoutRetries + 1);
                db.Update<RequestDataModel>(storedRequest);
                db.Complete();
            }
        }
    }
}
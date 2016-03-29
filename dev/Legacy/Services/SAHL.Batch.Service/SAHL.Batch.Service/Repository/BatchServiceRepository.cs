using SAHL.Batch.Common;
using SAHL.Common.Logging;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Core.Data.Models.Cuttlefish.SqlStatements;
using SAHL.Shared.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Batch.Service.Repository
{
    public class BatchServiceRepository : IRepository
    {
        private ILogger logger;
        private IDbFactory dbFactory;

        public BatchServiceRepository(ILogger logger, IDbFactory dbFactory)
        {
            this.logger = logger;
            this.dbFactory = dbFactory;
        }

        public void Save<TMessage>(TMessage message) where TMessage : IBatchMessage
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                try
                {
                    var serializedMessage = Newtonsoft.Json.JsonConvert.SerializeObject(message, JsonConversion.GetSerializerSettings());
                    var model = new GenericMessageDataModel(DateTime.Now, serializedMessage, message.GetType().ToString(), (int)GenericStatuses.Pending, message.BatchID,message.FailureCount);
                    db.Insert<GenericMessageDataModel>(model);
                    message.Id = model.ID;
                    db.Complete();
                }
                catch (Exception ex)
                {
                    this.logger.LogErrorMessageWithException(string.Format("Save : {0}", this.GetType()), "Save", ex);
                    throw ex;
                }
            }
        }

        public void Update<TMessage>(TMessage message, GenericStatuses genericStatus) where TMessage : IBatchMessage
        {
            using (var db = dbFactory.NewDb().InAppContext())
            {
                try
                {
                    var serializedMessage = Newtonsoft.Json.JsonConvert.SerializeObject(message, JsonConversion.GetSerializerSettings());
                    var model = new GenericMessageDataModel(message.Id, DateTime.Now, serializedMessage, message.GetType().ToString(), (int)genericStatus, message.BatchID,message.FailureCount);
                    db.Update<GenericMessageDataModel>(model);
                    db.Complete();
                }
                catch (Exception ex)
                {
                    this.logger.LogErrorMessageWithException(string.Format("Update : {0}", this.GetType()), "Update", ex);
                    throw ex;
                }
            }
        }

        public IEnumerable<TMessage> Load<TMessage>(GenericStatuses genericStatus, int numberOfAttemptsLimit) where TMessage : IBatchMessage
        {
            List<TMessage> messages = new List<TMessage>();
            try
            {
                using (var db = dbFactory.NewDb().InReadOnlyAppContext())
                {
                    var models = db.Select<GenericMessageDataModel>("select * from [Cuttlefish].[dbo].[GenericMessage] where GenericStatusID = @GenericStatusID and FailureCount < @FailureCount and MessageType = @MessageType ",
                        new { FailureCount = numberOfAttemptsLimit, GenericStatusID = (int)genericStatus, MessageType = typeof(TMessage).ToString() });
                    foreach (var model in models)
                    {
                        try
                        {
                            TMessage deserializedMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<TMessage>(model.MessageContent, JsonConversion.GetSerializerSettings());
                            deserializedMessage.Id = model.ID;
                            messages.Add(deserializedMessage);
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogErrorMessageWithException(string.Format("Load : {0}", this.GetType()), "DeserializeObject", ex);
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogErrorMessageWithException(string.Format("Update : {0}", this.GetType()), "Error while loading failed messages.", ex);
                throw ex;
            }
            return messages;
        }
    }
}
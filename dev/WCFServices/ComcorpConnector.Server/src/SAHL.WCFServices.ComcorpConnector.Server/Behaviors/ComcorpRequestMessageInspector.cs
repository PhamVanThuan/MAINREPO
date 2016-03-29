using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Models._2AM;
using StructureMap;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace SAHL.WCFServices.ComcorpConnector.Server.Behaviors
{
    public class ComcorpRequestMessageInspector : IDispatchMessageInspector
    {
        private Dictionary<Guid, ComcorpRequestLogMessage> comcorpRequests = new Dictionary<Guid, ComcorpRequestLogMessage>();
        
        private IDbFactory _dbFactory;

        private IDbFactory dbFactory
        {
            get
            {
                if (_dbFactory == null)
                {
                    _dbFactory = ObjectFactory.GetInstance<IDbFactory>();
                }
                return _dbFactory;
            }
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var doc = new XDocument();
            using (var xmlWriter = doc.CreateWriter())
            {
                request.WriteMessage(xmlWriter);
            }
            var messageId = Guid.NewGuid();
            var requestLogMessage = new ComcorpRequestLogMessage(doc.ToString(), DateTime.Now);
            comcorpRequests.Add(messageId, requestLogMessage);
            OperationContext.Current.IncomingMessageProperties.Add("messageId", messageId);
            request = Message.CreateMessage(doc.CreateReader(), int.MaxValue, request.Version);
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var doc = new XDocument();
            using (var xmlWriter = doc.CreateWriter())
            {
                reply.WriteMessage(xmlWriter);
            }
            string replyContent = doc.ToString();

            if (OperationContext.Current.IncomingMessageProperties.ContainsKey("messageId"))
            {
                var correlationKey = Guid.Parse(OperationContext.Current.IncomingMessageProperties["messageId"].ToString());
                if (comcorpRequests.ContainsKey(correlationKey))
                {
                    var requestLogMessage = comcorpRequests[correlationKey];

                    ComcorpRequestDataModel model = new ComcorpRequestDataModel(requestLogMessage.XmlRequest, requestLogMessage.RequestDate, replyContent, DateTime.Now);

                    using (IDbContext db = dbFactory.NewDb().InAppContext())
                    {
                        db.Insert<ComcorpRequestDataModel>(model);
                        db.Complete();
                    }
                    reply = Message.CreateMessage(doc.CreateReader(), int.MaxValue, reply.Version);
                }
            }
        }
    }

    internal class ComcorpRequestLogMessage
    {
        public string XmlRequest{ get; set; }

        public DateTime RequestDate{ get; set; }

        public ComcorpRequestLogMessage(string xmlRequest, DateTime requestDate)
        {
            this.RequestDate = requestDate;
            this.XmlRequest = xmlRequest;
        }
    }
}
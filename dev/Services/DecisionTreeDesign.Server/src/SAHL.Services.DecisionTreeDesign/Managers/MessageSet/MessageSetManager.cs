using Newtonsoft.Json.Linq;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.Identity;
using SAHL.Core.Strings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.DecisionTreeDesign.Managers.MessageSet
{
    public class MessageSetManager : IMessageSetManager
    {
        private IMessageSetDataManager messageDataService;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public MessageSetManager(IMessageSetDataManager messageDataService, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.messageDataService = messageDataService;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void SaveMessageSet(Guid id, int version, string data)
        {
            if (messageDataService.IsMessageSetVersionPublished(version) || !messageDataService.DoesMessageSetExist(id))
            {
                id = CombGuid.Instance.Generate();
                version += 1;
                messageDataService.InsertMessageSet(id, version, data);
            }
            else
            {
                messageDataService.UpdateMessageSet(id, version, data);
            }
        }

        public void SaveAndPublishMessageSet(Guid id, int version, string data, string publisher)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.Build())
            {
                if (messageDataService.IsMessageSetVersionPublished(version) || !messageDataService.DoesMessageSetExist(id))
                {
                    id = CombGuid.Instance.Generate();
                    version += 1;
                    messageDataService.InsertMessageSet(id, version, data);
                }
                else
                {
                    messageDataService.UpdateMessageSet(id, version, data);
                }

                messageDataService.InsertPublishedMessageSet(CombGuid.Instance.Generate(), id, Guid.Parse(PublishStatusEnumDataModel.IN_PROGRESS), DateTime.Now, publisher);

                uow.Complete();
            }
        }

        public System.Collections.Generic.IEnumerable<string> GetLatestMessageSetWords()
        {
            MessageSetDataModel dataModel = messageDataService.GetLatestMessageSet();
            JObject jObject = JObject.Parse(dataModel.Data);
            IList<string> words = new List<string>();
            foreach (JObject groupItem in jObject["groups"])
            {
                Flatten(groupItem, "Messages", ref words);
            }
            return words;
        }

        private void Flatten(JObject jObject, string prefix, ref IList<string> words)
        {

            string moduleName = jObject["name"].ToObject<string>();
            moduleName = string.IsNullOrEmpty(prefix) ? StringExtensions.PascalCase(moduleName) : string.Format("{0}::{1}", prefix, StringExtensions.CamelCase(moduleName));
            if (jObject["messages"] != null)
            {
                foreach (JObject enumItem in jObject["messages"])
                {
                    words.Add(string.Format("{0}.{1}", moduleName, StringExtensions.PascalCase(enumItem["name"].ToObject<string>())));
                }
            }
            foreach (JObject groupItem in jObject["groups"])
            {
                Flatten(groupItem, moduleName, ref words);
            }
        }
    }
}
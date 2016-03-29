using Newtonsoft.Json.Linq;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.Identity;
using SAHL.Core.Strings;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace SAHL.Services.DecisionTreeDesign.Managers.EnumerationSet
{
    public class EnumerationSetManager : IEnumerationSetManager
    {
        private IEnumerationSetDataManager enumerationSetDataService;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public EnumerationSetManager(IEnumerationSetDataManager enumerationSetDataService, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.enumerationSetDataService = enumerationSetDataService;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void SaveEnumerationSet(Guid id, int version, string data)
        {
            if (enumerationSetDataService.IsEnumerationSetVersionPublished(version) || !enumerationSetDataService.DoesEnumerationSetExist(id))
            {
                id = CombGuid.Instance.Generate();
                version += 1;
                enumerationSetDataService.InsertEnumerationSet(id, version, data);
            }
            else
            {
                enumerationSetDataService.UpdateEnumerationSet(id, version, data);
            }
        }

        public void SaveAndPublishEnumerationSet(Guid id, int version, string data, string publisher)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.Build())
            {
                if (enumerationSetDataService.IsEnumerationSetVersionPublished(version) || !enumerationSetDataService.DoesEnumerationSetExist(id))
                {
                    id = CombGuid.Instance.Generate();
                    version += 1;
                    enumerationSetDataService.InsertEnumerationSet(id, version, data);
                }
                else
                {
                    enumerationSetDataService.UpdateEnumerationSet(id, version, data);
                }

                enumerationSetDataService.InsertPublishedEnumerationSet(CombGuid.Instance.Generate(), id, Guid.Parse(PublishStatusEnumDataModel.IN_PROGRESS), DateTime.Now, publisher);

                uow.Complete();
            }
        }


        public System.Collections.Generic.IEnumerable<string> GetLatestEnumerationSetWords()
        {
            EnumerationSetDataModel dataModel = enumerationSetDataService.GetLatestEnumerationSet();
            JObject jObject = JObject.Parse(dataModel.Data);
            IList<string> words = new List<string>();
            foreach(JObject groupItem in jObject["groups"])
            {
                Flatten(groupItem, "Enumerations",ref words);
            }
            return words;
        }

        private void Flatten(JObject jObject,string prefix, ref IList<string> words)
        {

            string moduleName = jObject["name"].ToObject<string>();
            moduleName = string.IsNullOrEmpty(prefix) ? StringExtensions.PascalCase(moduleName) : string.Format("{0}::{1}", prefix, StringExtensions.CamelCase(moduleName));
            if (jObject["enumerations"] != null)
            {
                foreach (JObject enumItem in jObject["enumerations"])
                {
                    words.Add(string.Format("{0}.{1}", moduleName, StringExtensions.PascalCase(enumItem["name"].ToObject<string>())));
                }
            }
            foreach(JObject groupItem in jObject["groups"])
            {
                Flatten(groupItem, moduleName,ref words);
            }
        }

        
    }
}
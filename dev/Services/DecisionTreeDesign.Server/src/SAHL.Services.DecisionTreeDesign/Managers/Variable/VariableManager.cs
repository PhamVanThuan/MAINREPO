using Newtonsoft.Json.Linq;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.Identity;
using SAHL.Core.Strings;
using System;
using System.Collections.Generic;

namespace SAHL.Services.DecisionTreeDesign.Managers.Variable
{
    public class VariableManager : IVariableManager
    {
        private IVariableDataManager variableManager;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public VariableManager(IVariableDataManager variableService, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.variableManager = variableService;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void SaveVariableSet(Guid id, int version, string data)
        {
            if (variableManager.IsVariableSetVersionPublished(version) || !variableManager.DoesVariableSetExist(id))
            {
                id = CombGuid.Instance.Generate();
                version += 1;
                variableManager.SaveVariableSet(id, version, data);
            }
            else
            {
                variableManager.UpdateVariableSet(id, version, data);
            }
        }

        public void SaveAndPublishVariableSet(Guid id, int version, string data, string publisher)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.Build())
            {
                if (variableManager.IsVariableSetVersionPublished(version) || !variableManager.DoesVariableSetExist(id))
                {
                    id = CombGuid.Instance.Generate();
                    version += 1;
                    variableManager.SaveVariableSet(id, version, data);
                }
                else
                {
                    variableManager.UpdateVariableSet(id, version, data);
                }

                variableManager.InsertPublishedVariableSet(CombGuid.Instance.Generate(), id, Guid.Parse(PublishStatusEnumDataModel.IN_PROGRESS), DateTime.Now, publisher);
                uow.Complete();
            }
        }


        public System.Collections.Generic.IEnumerable<string> GetLatestVariableSetWords()
        {
            VariableSetDataModel dataModel = variableManager.GetLatestVariableSet();
            JObject jObject = JObject.Parse(dataModel.Data);
            IList<string> words = new List<string>();
            foreach (JObject groupItem in jObject["variables"]["groups"])
            {
                Flatten(groupItem, "Variables", ref words);
            }
            return words;
        }

        private void Flatten(JObject jObject, string prefix, ref IList<string> words)
        {

            string moduleName = jObject["name"].ToObject<string>();
            moduleName = string.IsNullOrEmpty(prefix) ? StringExtensions.PascalCase(moduleName) : string.Format("{0}::{1}", prefix, StringExtensions.CamelCase(moduleName));
            if (jObject["variables"] != null)
            {
                foreach (JObject enumItem in jObject["variables"])
                {
                    words.Add(string.Format("{0}.{1}", moduleName, StringExtensions.PascalCase(enumItem["name"].ToObject<string>())));
                }
            }
            if (jObject["groups"] != null)
            {
                foreach (JObject groupItem in jObject["groups"])
                {
                    Flatten(groupItem, moduleName, ref words);
                }
            }
        }
    }
}
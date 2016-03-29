using Newtonsoft.Json.Linq;
using SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib.Templates;
using System;
using System.IO;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib
{
    public class DecisionTreeQueryGenerator
    {
        private class VariableSetMap
        {
            public Guid VariableSetId { get; set; }

            public int? Version { get; set; }

            public string Data { get; set; }
        };

        public DecisionTreeQueryGenerator()
        {
        }

        public void GenerateTreeQueryObject(string outputPath, JArray treeVariablesJson, string defaultNameSpace, string treeName, int treeVersion)
        {
            var decisionTreeQueryObject = new DecisionTreeQueryObject(treeVariablesJson, defaultNameSpace, treeName, treeVersion,false);

            if (decisionTreeQueryObject != null)
            {
                string generatedData = decisionTreeQueryObject.TransformText();
                using (StreamWriter sw = new StreamWriter(outputPath + "\\" + decisionTreeQueryObject.DecisionTreeQueryClassName))
                {
                    sw.Write(generatedData);
                    sw.Flush();
                }
            }
        }

        // Maybe consider doing query handler and javascript client

        public void GenerateTreeQueryResultObject(string outputPath, JArray treeVariablesJson, string defaultNameSpace, string treeName, int treeVersion)
        {
            var decisionTreeQueryResultObject = new DecisionTreeQueryResultObject(treeVariablesJson, defaultNameSpace, treeName, treeVersion,false);

            if (decisionTreeQueryResultObject != null)
            {
                string generatedData = decisionTreeQueryResultObject.TransformText();
                using (StreamWriter sw = new StreamWriter(outputPath + "\\" + decisionTreeQueryResultObject.DecisionTreeQueryResultClassName))
                {
                    sw.Write(generatedData);
                    sw.Flush();
                }
            }
        }
    }
}
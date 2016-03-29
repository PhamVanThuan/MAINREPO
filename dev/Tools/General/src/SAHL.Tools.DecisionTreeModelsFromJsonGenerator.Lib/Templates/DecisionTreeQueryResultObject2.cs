using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib.Templates
{
    public partial class DecisionTreeQueryResultObject : GeneratorObject
    {
        private bool isLatestVersion;

        public string JsonText { get; private set; }

        public string DecisionTreeQueryResultClassName
        {
            get
            {
                if(isLatestVersion)
                    return string.Format("{0}_QueryResult", Utilities.StripSpacesAndUpperCaseFirst(TreeName));
                return string.Format("{0}_{1}QueryResult", Utilities.StripSpacesAndUpperCaseFirst(TreeName), TreeVersion);
            }
        }

        public string Namespace { get; protected set; }

        public string TreeName { get; protected set; }

        public int TreeVersion { get; protected set; }

        private JArray treeVariablesJson;

        public DecisionTreeQueryResultObject(JArray treeVariablesJson, string defaultNamespace, string treeName, int treeVersion,bool isLatestVersion)
        {
            this.TreeName = treeName;
            this.Namespace = defaultNamespace;
            this.TreeVersion = treeVersion;
            this.treeVariablesJson = treeVariablesJson;
            this.isLatestVersion = isLatestVersion;
        }

        private string BuildOutputProperties()
        {
            //var treeVariablesDefinition = JsonConvert.DeserializeObject<dynamic>(this.treeVariablesJson);
            string[] array = new string[] { "int", "float", "string", "bool", "double" };
            var outputPropertiesDeclarationSnipet = new StringBuilder();
            foreach (dynamic variable in treeVariablesJson)
            {
                string variableUsage = Convert.ToString(variable.usage);
                if (variableUsage.Equals("output", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (variable.type != null && variable.type.ToString().Trim() != "")
                    {
                        string name = Convert.ToString(variable.type);
                        if (name == "float")
                        {
                            name = "double";
                        }
                        if (array.Contains(name))
                        {
                            outputPropertiesDeclarationSnipet.AppendFormat("public {0} {1} {{ get; set; }}\n", name, Utilities.StripSpacesAndUpperCaseFirst(Convert.ToString(variable.name)));
                        }
                        else
                        {
                            outputPropertiesDeclarationSnipet.AppendFormat("public string {0} {{ get; set; }}\n", Utilities.StripSpacesAndUpperCaseFirst(Convert.ToString(variable.name)));
                        }
                    }

                }
            }
            return outputPropertiesDeclarationSnipet.ToString();
        }
    }
}
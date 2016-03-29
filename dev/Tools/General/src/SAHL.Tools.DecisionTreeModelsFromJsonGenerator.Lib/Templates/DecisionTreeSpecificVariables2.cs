using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib.Templates
{
    public partial class DecisionTreeSpecificVariables
    {
        public string DecisionTreeVariablesClassName
        {
            get
            {
                return string.Format("{0}_{1}Variables", Utilities.StripSpacesAndUpperCaseFirst(TreeName), TreeVersion);
            }
        }

        public string InputsClassName
        {
            get
            {
                return string.Format("{0}_{1}Inputs", Utilities.StripSpacesAndUpperCaseFirst(TreeName), TreeVersion);
            }
        }

        public string OutputsClassName
        {
            get
            {
                return string.Format("{0}_{1}Outputs", Utilities.StripSpacesAndUpperCaseFirst(TreeName), TreeVersion);
            }
        }

        public string Namespace { get; protected set; }

        public string TreeName { get; protected set; }

        public int TreeVersion { get; protected set; }

        public JArray treeVariablesJson { get; set; }

        private string[] simpleTypes = new string[] { "int", "float", "string", "bool", "double" };

        public DecisionTreeSpecificVariables(JArray treeVariablesJson, string defaultNamespace, string treeName, int treeVersion)
        {
            this.TreeName = treeName;
            this.Namespace = defaultNamespace;
            this.TreeVersion = treeVersion;
            this.treeVariablesJson = treeVariablesJson;
        }

        public string BuildCompleteTreeProperties(string usage)
        {
            var propertiesDeclarationSnipet = new StringBuilder();
            foreach (dynamic variable in this.treeVariablesJson)
            {
                string variableUsage = Convert.ToString(variable.usage);
                if (variableUsage.Equals(usage, StringComparison.InvariantCultureIgnoreCase))
                {
                    string outputVariableType = Convert.ToString(variable.type).Trim();
                    if (!string.IsNullOrEmpty(outputVariableType))
                    {
                        if (simpleTypes.Contains(outputVariableType))
                        {
                            if (outputVariableType == "float")
                            {
                                outputVariableType = "double";
                            }
                            propertiesDeclarationSnipet.AppendFormat("public {0} {1} {{ get; set; }}\n", outputVariableType, Utilities.StripSpacesAndUpperCaseFirst(Convert.ToString(variable.name)));
                        }
                        else
                        {
                            propertiesDeclarationSnipet.AppendFormat("public string {0} {{ get; set; }}\n", Utilities.StripSpacesAndUpperCaseFirst(Convert.ToString(variable.name)));
                        }
                    }
                }
            }
            return propertiesDeclarationSnipet.ToString();
        }

        public string InitializeOutputsWithUserSpecifiedDefaults()
        {
            var outputPropertiesInitializerSnipet = new StringBuilder();

            foreach (dynamic variable in this.treeVariablesJson)
            {
                string variableUsage = Convert.ToString(variable.usage);
                string outputVariableType = Convert.ToString(variable.type).Trim();
                if (variableUsage.Equals("output", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (variable.defaultValue == null)
                    {
                        continue;
                    }

                    string outputVariableDefaultValue = Convert.ToString(variable.defaultValue).Trim();
                    if (!string.IsNullOrEmpty(outputVariableDefaultValue))
                    {
                        if (simpleTypes.Contains(outputVariableType))
                        {
                            if (outputVariableType == "string")
                            {
                                outputPropertiesInitializerSnipet.AppendFormat("{0} = \"{1}\";\n", Utilities.StripSpacesAndUpperCaseFirst(Convert.ToString(variable.name)), Convert.ToString(variable.defaultValue));
                            }
                            else
                            {
                                string defaultValue = Convert.ToString(variable.defaultValue);
                                if (outputVariableType == "float")
                                {
                                    outputVariableType = "double";
                                }
                                if (outputVariableType == "bool")
                                {
                                    defaultValue = defaultValue.ToLower();
                                }
                                outputPropertiesInitializerSnipet.AppendFormat("{0} = ({1}){2};\n", Utilities.StripSpacesAndUpperCaseFirst(Convert.ToString(variable.name)), outputVariableType, defaultValue);
                            }
                        }

                        if (outputVariableType.Contains("::"))
                        {
                            outputPropertiesInitializerSnipet.AppendFormat("{0} = {1};\n", Utilities.StripSpacesAndUpperCaseFirst(Convert.ToString(variable.name)), Utilities.StripSpacesAndUpperCaseFirst(Convert.ToString(variable.defaultValue.value).Replace("::", ".")));
                        }
                    }
                    else if (outputVariableType.Equals("string"))
                    {
                        outputPropertiesInitializerSnipet.AppendFormat("{0} = string.Empty;\n", Utilities.StripSpacesAndUpperCaseFirst(Convert.ToString(variable.name)), Convert.ToString(variable.defaultValue));
                    }
                }
            }
            return outputPropertiesInitializerSnipet.ToString();
        }
    }
}
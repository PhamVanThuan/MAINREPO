using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib.Templates
{
    public partial class DecisionTreeQueryObject : GeneratorObject
    {
        private bool isLatestVersion;

        public string DecisionTreeQueryClassName
        {
            get
            {
                if(isLatestVersion)
                    return string.Format("{0}_Query", Utilities.StripSpacesAndUpperCaseFirst(TreeName));

                return string.Format("{0}_{1}Query", Utilities.StripSpacesAndUpperCaseFirst(TreeName), TreeVersion);
            }
        }

        public string Namespace { get; protected set; }

        public string TreeName { get; protected set; }

        public int TreeVersion { get; protected set; }

        private List<TreeVariableDescription> properties;

        public List<TreeVariableDescription> Properties
        {
            get
            {
                return properties;
            }
        }

        private JArray treeVariablesJson;

        internal MaxVersionMap VersionInfo
        {
            get;
            set;
        }

        public DecisionTreeQueryObject(JArray treeVariablesJson, string defaultNamespace, string treeName, int treeVersion, bool isLatestVersion)
        {
            this.TreeName = treeName;
            this.Namespace = defaultNamespace;
            this.TreeVersion = treeVersion;
            this.treeVariablesJson = treeVariablesJson;
            this.VersionInfo = MaxVersion.Mappings;
            this.isLatestVersion = isLatestVersion;
        }

        public string BuildInputProperties()
        {
            properties = new List<TreeVariableDescription>();
            var propertiesDeclarationSnipet = new StringBuilder();
            string[] array = new string[] { "int", "float", "string", "bool", "double" };
            foreach (dynamic variable in this.treeVariablesJson)
            {
                var property = new TreeVariableDescription(Convert.ToInt32(variable.id), Convert.ToString(variable.name), Convert.ToString(variable.type), Convert.ToString(variable.usage));
                properties.Add(property);
                if (property.Usage.Equals("input", StringComparison.InvariantCultureIgnoreCase))
                {
                    string name = property.TypeName;
                    if (name == "float")
                    {
                        name = "double";
                    }
                    if (array.Contains(name))
                    {
                        propertiesDeclarationSnipet.AppendFormat("public {0} {1} {{ get; protected set; }}\n", name, Utilities.StripSpacesAndUpperCaseFirst(property.PropertyName));
                    }
                    else
                    {
                        propertiesDeclarationSnipet.AppendFormat("public string {0} {{ get; protected set; }}\n", Utilities.StripSpacesAndUpperCaseFirst(property.PropertyName));
                    }
                }
            }
            return propertiesDeclarationSnipet.ToString();
        }

        public string BuildQueryConstructorArguments()
        {
            var queryConstractorArgumentsSnipet = new StringBuilder();
            string[] array = new string[] { "int", "float", "string", "bool", "double" };
            foreach (var variable in Properties)
            {
                if (variable.Usage.Equals("input", StringComparison.InvariantCultureIgnoreCase))
                {
                    string name = variable.TypeName;

                    if (name == "float")
                    {
                        name = "double";
                    } 

                    if (array.Contains(name))
                    {
                        queryConstractorArgumentsSnipet.AppendFormat(string.Format("{0} {1},", name, Utilities.ToCamelCase(Convert.ToString(variable.PropertyName))));
                    }
                    else
                    {
                        queryConstractorArgumentsSnipet.AppendFormat(string.Format("string {0},", Utilities.ToCamelCase(Convert.ToString(variable.PropertyName))));
                    }
                }
            }
            var result = queryConstractorArgumentsSnipet.ToString();
            return result;
        }

        public string BuildPropertyInitializers()
        {
            
            var inputPropertiesInitSnipet = new StringBuilder();
            foreach (var variable in Properties)
            {
                if (variable.Usage.Equals("input", StringComparison.InvariantCultureIgnoreCase))
                {
                    inputPropertiesInitSnipet.AppendFormat(string.Format("this.{0} = {1};\n", Utilities.StripSpacesAndUpperCaseFirst(variable.PropertyName), Utilities.ToCamelCase(variable.PropertyName)));
                }
            }
            return inputPropertiesInitSnipet.ToString();
        }
    }
}
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace SAHL.Services.DecisionTreeDesign.Templates
{
    public partial class VariableObject : GeneratorObject
    {
        public string Namespace { get; protected set; }
        public string JsonText { get; protected set; }
        public string EnumText { get; protected set; }
        public string Version { get; protected set; }

        JToken baseObject = null;
        JToken baseValues = null;
        JObject enums = null;
        public VariableObject(string rawJson, string defaultNameSpace, string version, string rawEnumJson, string rawEnumJsonVersion)
        {  
            this.Namespace = defaultNameSpace;
            this.JsonText = rawJson;
            this.EnumText = rawEnumJson;
            this.Version = version;
        }

        public void ParseJson()
        {
            baseObject = JObject.Parse(JsonText)["variables"];
            baseValues = baseObject["values"];
            if(!string.IsNullOrEmpty(this.EnumText))
            {
                enums = JObject.Parse(EnumText);
            }
        }

        public void RecurseJObject(JToken jObj,string tabs)
        {
            if (jObj != null)
            {
                var groups = jObj["groups"];
                var values = jObj["variables"];
                if (groups != null)
                {
                    foreach (var group in groups)
                    {
                        this.WriteLine("{0}public class {1}", tabs, ToPascalCase(group["name"].ToString()));
                        this.WriteLine("{0}{1}", tabs, "{");
                        RecurseJObject(group, tabs + "\t");
                        this.WriteLine("{0}{1}", tabs, "}");
                    }
                    this.WriteLine("");
                    foreach (var group in groups)
                    {
                        this.WriteLine("{0}public {1} {2} = new {1}();", tabs, ToPascalCase(group["name"].ToString()), toCamelCase(group["name"].ToString()));
                    }
                    if (values != null)
                    {
                        this.WriteLine("");
                    }
                }
                if (values != null)
                {
                    foreach (var value in values)
                    {
                        Variable variable = GetVariableFromToken(value);
                        this.WriteLine("{0}public {1} {2} {{ get {{ return {3}; }} }}", tabs, variable.Type, variable.Name, variable.Value);
                    }
                }
            }
        }

        public Variable GetVariableFromToken(JToken token)
        {
            string id = token["id"].ToString();
            string name = ToPascalCase(token["name"].ToString());
            string type = token["type"].ToString();
            string value = (baseValues.Single(v => v["variableId"].ToString() == id))["value"].ToString();
            bool isEnum = false;
            if (type == "string")
            {
                value = string.Format("\"{0}\"", value);
            }
            else if (type == "bool")
            {
                value = value.ToLower();
            }
            else if (type == "enumeration")
            {
                string[] markers = value.Split('_');
                type = "dynamic";
                value = string.Format("Enumerations.{0}.{1}", Capitilise(token["fullName"].ToString()), Sanitize(value));
                type = value.Substring(0, value.LastIndexOf('.'));
            }
            return new Variable(name, type, value,isEnum);
        }

        public string Capitilise(string input)
        {
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        public string Sanitize(string input)
        {
            input = Regex.Replace(input, @"[^A-Za-z0-9]+", "");
            return input;
        }

        public string ToPascalCase(string original)
        {
            return ObjectGenerator.ToPascalCase(original);
        }

        public string toCamelCase(string original)
        {
            return ObjectGenerator.toCamelCase(original);
        }
        
    }
}

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace SAHL.Tools.ObjectFromJsonGenerator.Lib.Templates
{
    public partial class EnumObject : GeneratorObject
    {
        public List<EnumGroupDescription> GroupDescriptions;

        public string Namespace { get; protected set; }

        public string JsonText { get; private set;  }

        public string Version { get; protected set; }

        public JObject BaseObject { get; protected set; }

        public EnumObject(string rawJson, string defaultNameSpace, string version)
        {  
            this.Namespace = defaultNameSpace;
            this.JsonText = rawJson;
            this.Version = version == null?"":"_"+version;
            GroupDescriptions = new List<EnumGroupDescription>();
        }

        public void ParseJson()
        {
            this.BaseObject = JObject.Parse(JsonText);
        }

        public string ToPascalCase(string original)
        {
            return ObjectGenerator.ToPascalCase(original);
        }

        public string toCamelCase(string original)
        {
            return ObjectGenerator.toCamelCase(original);
        }

        public string Sanitize(string input)
        {
            input = Regex.Replace(input, @"[^A-Za-z0-9]+", "");
            return input;
        }

        public void RecurseJObject(JToken jObj, string tabs,string currentNamespace)
        {
            if (jObj != null)
            {
                var groups = jObj["groups"];
                var enumerations = jObj["enumerations"];
                if (groups != null && groups.Count() > 0)
                {
                    foreach (var group in groups)
                    {
                        this.WriteLine("{0}public class {1}", tabs, ToPascalCase(group["name"].ToString().Trim()));
                        this.WriteLine("{0}{1}", tabs, "{");
                        RecurseJObject(group, tabs + "\t", string.Format("{0}.{1}", currentNamespace,toCamelCase(group["name"].ToString())));
                        this.WriteLine("{0}{1}", tabs, "}");
                    }
                    this.WriteLine("");
                    foreach (var group in groups)
                    {
                        this.WriteLine("{0}public {1} {2} = new {1}();", tabs, ToPascalCase(group["name"].ToString()), toCamelCase(group["name"].ToString()));
                    }

                }
                if (enumerations != null && (groups != null && groups.Count() > 0))
                {
                    this.WriteLine("");
                }
                if (enumerations != null)
                {
                    foreach (var value in enumerations)
                    {
                        string enumFormat = "{0}\t public string {1} {{ get {{ return \"{2}\"; }} }}";
                        var output = value["value"].Select(x => string.Format(enumFormat, tabs, Sanitize(x.ToString()), string.Format("{0}.{1}.{2}", currentNamespace, toCamelCase(value["name"].ToString()), Sanitize(x.ToString()))))
                            .Aggregate((c, n) => c + "\r\n" + n);
                        this.WriteLine("{0}public class {1}", tabs, Sanitize(value["name"].ToString()));
                        this.WriteLine("{0}{{", tabs);
                        this.WriteLine("{1}", tabs,output);
                        this.WriteLine("{0}}}", tabs);
                    }
                    foreach (var value in enumerations)
                    {
                        this.WriteLine("{0}public {1} {2} = new {1}();", tabs, Sanitize(value["name"].ToString()), toCamelCase(value["name"].ToString()));
                    }
                }
            }
        }
    }
}

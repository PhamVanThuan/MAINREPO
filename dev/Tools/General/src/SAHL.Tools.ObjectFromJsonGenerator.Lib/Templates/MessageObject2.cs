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
    public partial class MessageObject : GeneratorObject
    {

        public string Namespace { get; protected set; }

        public string JsonText { get; private set;  }

        public string Version { get; protected set; }

        public JObject BaseObject { get; protected set; }

        public MessageObject(string rawJson, string defaultNameSpace, string version)
        {  
            this.Namespace = defaultNameSpace;
            this.JsonText = rawJson;
            this.Version = version == null ? "" : "_" + version;
            
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

        public void RecurseJObject(JToken jObj, string tabs)
        {
            if (jObj != null)
            {
                var groups = jObj["groups"];
                var messages = jObj["messages"];
                if (groups != null && groups.Count() > 0)
                {
                    foreach (var group in groups)
                    {
                        this.WriteLine("{0}public class {1}", tabs, ToPascalCase(group["name"].ToString().Trim()));
                        this.WriteLine("{0}{1}", tabs, "{");
                        RecurseJObject(group, tabs + "\t");
                        this.WriteLine("{0}{1}", tabs, "}");
                    }
                    this.WriteLine("");
                    foreach (var group in groups)
                    {
                        this.WriteLine("{0}public {1} {2} = new {1}();", tabs, ToPascalCase(group["name"].ToString()), toCamelCase(group["name"].ToString()));
                    }
                    
                }
                if (messages != null && (groups != null && groups.Count() > 0))
                {
                    this.WriteLine("");
                }
                if (messages != null)
                {
                    foreach (var value in messages)
                    {
                        //make interpolation whatever with escaped double qoutes
                        this.WriteLine("{0}public string {1} {{ get {{ return \"\\\"{2}\\\"\"; }} }}", tabs, Sanitize(value["name"].ToString()), value["value"]);
                    }
                }
            }
        }
        
    }
}

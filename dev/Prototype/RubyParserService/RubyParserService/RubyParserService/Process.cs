using IronRuby;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace RubyParserService
{
    public class Process
    {
        private ScriptEngine rubyEngine;

        internal ScriptEngine RubyEngine
        {
            get
            {
                if (rubyEngine == null)
                {
                    rubyEngine = Ruby.CreateEngine();
                    rubyEngine.SetSearchPaths(new[] {
                        @"../../",
                        @"C:/Ruby200-x64/lib/ruby/gems/2.0.0",
                        @"C:/Users/neerajr/.gem/ruby/2.0.0"
                    });
                }
                return rubyEngine;
            }
        }

        private dynamic rubyRuntime;

        internal dynamic RubyRuntime
        {
            get
            {
                if (rubyRuntime == null)
                {
                    rubyRuntime = RubyEngine.Runtime.Globals;
                }
                return rubyRuntime;
            }
        }

        public string DebugDecisionTreeNodes(string treeInJson, string variablesSetData)
        {
            var processedVariables = string.Empty;
            // convert tree to ruby format
            // Use given variables to process tree nodes upto the debug marker
            // return variables collection containing the results of the processing
            return processedVariables;
        }

        public string ProcessDecisionTreeNodes(string treeJson, string variablesSetData)
        {
            StringBuilder sb = new StringBuilder();
            var treeObj = JObject.Parse(treeJson);
            var moduleName = ((string)treeObj["decision_tree"]["name"]).Replace(" ","");
            sb.AppendFormat("Module {0}\n @@", moduleName);
            //var dataSetObj = JObject.Parse(variablesSetData);
            //// Set the specified variables in tree to the given values in dataSet
            //foreach (var namedVar in dataSetObj["variables"])
            //{
            //    treeObj["decision_tree"]["tree"]["variables"][namedVar] = dataSetObj["variables"][namedVar]["value"];
            //}
            string decision = string.Empty;
            foreach (var node in treeObj["decision_tree"]["tree"]["nodes"])
            {
                // Break if any message is added to the message collection
                var rubyClassName = (string)node["name"];
                if (rubyClassName.Equals("start",System.StringComparison.InvariantCultureIgnoreCase))
                {
                    // Get the required variables and output variables
                }

                if (rubyClassName.Equals("process", System.StringComparison.InvariantCultureIgnoreCase))
                {

                }

                if (rubyClassName.Equals("decision", System.StringComparison.InvariantCultureIgnoreCase))
                {

                    // decision to determine the linked node to process
                }
            }
            // Foreach Node execute process, set output values and move to next node in decision path
            //dynamic treeInRuby = n
            // convert tree to ruby format
            // Use given variables to process tree nodes upto the debug marker
            // return variables collection containing the results of the processing
            return decision;//
        }

        public bool ValidateNodeScript(string codeAsJson)
        {
            var validRuby = false;
            try
            {
                dynamic nodeInIronRuby = RubyEngine.CreateScriptSourceFromString(codeAsJson);
                validRuby = nodeInIronRuby.Process();
            }
            catch (System.Exception)
            {
                // Add to messages collection
            }
            return validRuby;
        }

        public string GetRubyClassFromTree(string treeInJson)
        {
            StringBuilder sb = new StringBuilder();
            JObject treeObj = JObject.Parse(treeInJson);
            var moduleName = ((string)treeObj["decision_tree"]["name"]).Replace(" ", "");
            sb.AppendFormat("class {0}\n ", moduleName);

            IDictionary<string, JToken> dicTopLevelAttributes = (JObject)treeObj["decision_tree"]["tree"];
            //Dictionary<string, object> dicTreeVariableNameValue = dicTopLevelAttributes.ToDictionary(pair => pair.Key, pair => );

            foreach (var attrib in dicTopLevelAttributes.Keys)
            {
                JObject inner = dicTopLevelAttributes[attrib].Value<JObject>();
                
            }

            foreach (JProperty namedVar in treeObj["decision_tree"]["tree"])
            {
                System.Console.WriteLine(namedVar.Name);
                foreach (var subProp in namedVar)
                {
                   System.Console.WriteLine(subProp);
                }
            }
            sb.AppendLine("attr_accessor :id \n attr_accessor :variables \n attr_accessor :nodes \n attr_accessor :links");
            //var dataSetObj = JObject.Parse(variablesSetData);
            return sb.ToString();
        }

        public void ParseJsonTree(string treeInJson)
        {
            
            //var treeObj = JObject.Parse(treeInJson);
            JToken entireJson = JToken.Parse(treeInJson);
            JArray inner = entireJson["decision_tree"].Value<JArray>();
            string key = null;
            string value = null;

            foreach (var item in inner)
            {
                JProperty treeDetails = item.First.Value<JProperty>();

                var treeSchemaReference = treeDetails.Name;

                var propertyList = (JObject)item[treeSchemaReference];

                var dicTreeDetails = new Dictionary<string, object>();

                foreach (var property in propertyList)
                {
                    key = property.Key;
                    value = property.Value.ToString();
                }

                dicTreeDetails.Add(key, value);
            }
        }

        static IEnumerable<string> GetSubdirectoriesContainingOnlyFiles(string path)
        {
            return from subdirectory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories)                  
                   select subdirectory;
        }

        public string RubyParseJsonTree(string treeInJson)
        {
            var engine = IronRuby.Ruby.CreateEngine();


            List<string> paths = new List<string>();

            foreach (string path in GetSubdirectoriesContainingOnlyFiles("C:\\Ruby200-x64\\lib\\ruby\\gems\\2.0.0\\gems"))
            {
                paths.Add(path);
            }

            engine.SetSearchPaths(paths.ToArray());

            engine.Execute("require 'hashie'"); // without SetSearchPaths, you get a LoadError
            /*
            engine.Execute("require 'restclient'"); // install through igem, then check with igem list
            engine.Execute("puts RestClient.get('http://localhost/').body");
            */




//RubyEngine.Execute("require 'json'");            
            RubyEngine.Execute("require 'hashToObject'");
            RubyEngine.Execute("require 'rubygems'");
            RubyEngine.Execute("require 'json'");
            //var engine = Ruby.CreateEngine();
            //engine.Runtime.Globals.SetVariable("Mouse", new Holly("Test"));
            //engine.ExecuteFile("./test.rb");
            //Console.ReadLine();
            dynamic rubyParser = RubyRuntime.JsonToRubyObject.@new();
            var rubyObject = rubyParser.get_ruby_object(treeInJson);
            return rubyObject;
        }
    }
}
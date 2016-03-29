using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib
{
    public class PackageHelper
    {
        public IEnumerable<string> GetPackageManifest(string file)
        {
            if (File.Exists(file)) { 
                JObject jobject = JObject.Parse(File.ReadAllText(file));
                var trees = jobject["trees"];
                return trees.Select(x => x.ToString()).ToArray();
            }
            return null;
        }
    }
}

using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator.Model
{
    public class PageModel
    {
        public string Namespace { get; protected set; }

        public string Name { get; protected set; }

        public string FilePath { get; protected set; }

        public string CtrlName { get; protected set; }

        public string TypePage { get; protected set; }

        public Dictionary<string, string> Properties { get; protected set; }

        public PageModel(TypeDefinition definition)
        {
            string filePath = definition.Namespace.Split(".".ToCharArray()).Select(x => StringHelper.toCamelCase(x)).Aggregate((c, n) => c + "." + n);
            if(!filePath.Contains(".pages.") && filePath.Contains(".configuration.") && filePath.Contains("wizard")){
                string jsNameSpace = definition.FullName.Split(".".ToCharArray()).Select(x => StringHelper.toCamelCase(x)).Aggregate((c, n) => c + "." + n);
                this.Name = definition.Name;
                this.Namespace = jsNameSpace.Replace("configuration.", "views.wizardPages.");
                this.FilePath = filePath.Substring((filePath.IndexOf("configuration.") + 14)).Replace(".", "\\");
                this.CtrlName = definition.FullName.Substring((definition.FullName.IndexOf("Configuration.") + 14)).Replace(".", "_");
                this.TypePage = "wizardPages";
                this.Properties = definition.Properties.ToDictionary(k => k.Name, v => StringHelper.PropertyToWord(v.Name));
            }
            else {
                string jsNameSpace = definition.FullName.Split(".".ToCharArray()).Select(x => StringHelper.toCamelCase(x)).Aggregate((c, n) => c + "." + n);
                this.Name = definition.Name;
                this.Namespace = jsNameSpace.Replace("pages.", "views.pages.");
                this.FilePath = filePath.Substring((filePath.IndexOf("pages.") + 6)).Replace(".", "\\");
                this.CtrlName = definition.FullName.Substring((definition.FullName.IndexOf("Pages.") + 6)).Replace(".","_");
                this.TypePage = "pages";
            }
        }
    }
}

using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator.Model
{
    public class TileModel
    {
        private TypeDefinition definition;
        public Dictionary<string, string> Properties { get; protected set; }
        public string FilePath { get; protected set; }
        
        public TileModel(TypeDefinition typeDefinition)
        {
            definition = typeDefinition.Resolve();
            int index = typeDefinition.FullName.IndexOf("Models.")+7;
            this.FilePath = definition.FullName.Substring(index).Split('.').Select(x => x[0].ToString().ToLower()+x.Substring(1)).Aggregate((z,y)=> z + "\\"+y) + ".tpl.html";
            this.Properties = definition.Properties.ToDictionary(k => k.Name, v => StringHelper.PropertyToWord(v.Name));
        }

        public IEnumerable<AttributeModel> GetPropertyAnnotations(string name)
        {
            var property = definition.Properties.Where(v => StringHelper.PropertyToWord(v.Name) == name).Single();
            return property.CustomAttributes.Select(x => new AttributeModel(x)).Where(y=>y.HtmlAttribute!= null);
        }
    }
}

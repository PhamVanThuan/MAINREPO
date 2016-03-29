using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Shared
{
    public partial class FieldDefinitionTemplate : ITemplate, ITemplateForModel<FieldDefinition>
    {
        private TypeDefinition typeDefinition;
        internal ITemplateManager TemplateManager { get; set; }

        public FieldDefinition Model { get; protected set; }

        public TypeDefinition ModelType
        {
            get
            {
                if (this.typeDefinition == null && this.Model.FieldType.Name != "ISystemMessageCollection")
                {
                    typeDefinition = this.Model.FieldType.Resolve();
                }
                return typeDefinition;
            }
        }

        public FieldDefinitionTemplate(ITemplateManager templateManager)
        {
            this.TemplateManager = templateManager;
        }

        public string Process(dynamic model)
        {
            this.Model = model;
            
            return this.TransformText();
        }
        public IEnumerable<FieldDefinition> GetFields()
        {
            return this.ModelType.Fields;
        }

        public IEnumerable<PropertyDefinition> GetProperties()
        {
            
            return this.ModelType.Properties;
        }
    }
}

using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners
{
    public class ModelTypeScanner : ITypeScanner
    {
        private List<ModelModel> models;
        private readonly List<EnumerationModel> enumerations;

        public ModelTypeScanner(List<ModelModel> models, List<EnumerationModel> enumerations)
        {
            this.models = models;
            this.enumerations = enumerations;
        }

        public bool ProcessTypeDefinition(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.IsInterface == false && typeToProcess.Name.EndsWith("Model"))
            {
                ModelModel model = new ModelModel();
                model.Name = typeToProcess.Name;
                model.FullType = typeToProcess.FullName;
                foreach(var property in typeToProcess.Properties)
                {
                    Property prop = PropertyTypeFiller.FillProperty(property);
                    prop.IsSAHLModel = models.Select(x => x.Name).Contains(prop.Type);
                    prop.IsCollectionSAHLModel = models.Select(x => x.Name).Contains(prop.CollectionPropertyType);
                    var emumerator = enumerations.FirstOrDefault(x => x.Name == prop.Type);
                    if (emumerator != null)
                    {
                        prop.IsSAHLEnum = true;
                        emumerator.IsUsed = true;
                    }
                    model.Properties.Add(prop);
                }

                this.models.Add(model);
                return true;
            }

            return false;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners
{
    public class ModelPropertyTypeScanner : ITypeScanner
    {
         private List<ModelModel> models;
        private readonly List<EnumerationModel> enumerations;

        public ModelPropertyTypeScanner(List<ModelModel> models, List<EnumerationModel> enumerations)
        {
            this.models = models;
            this.enumerations = enumerations;
        }

        public bool ProcessTypeDefinition(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.IsInterface == false && typeToProcess.Name.EndsWith("Model"))
            {
                ModelModel model = models.FirstOrDefault(x => x.Name == typeToProcess.Name);
                if (model == null) {  return true; } 

                foreach (var property in model.Properties)
                {
                    property.IsSAHLModel = models.Select(x => x.Name).Contains(property.Type);
                    property.IsCollectionSAHLModel = models.Select(x => x.Name).Contains(property.CollectionPropertyType);
                    var emumerator = enumerations.FirstOrDefault(x => x.Name == property.Type);
                    if (emumerator != null)
                    {
                        property.IsSAHLEnum = true;
                        emumerator.IsUsed = true;
                    }
                }
                return true;
            }

            return false;
        }
    }
}
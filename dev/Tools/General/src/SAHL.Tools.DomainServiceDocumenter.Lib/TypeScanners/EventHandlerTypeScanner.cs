using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners
{
    public class EventTypeScanner : ITypeScanner
    {
        private List<EventModel> events;
        private List<ModelModel> models;
        private List<EnumerationModel> enumerations;
        private readonly List<CommandModel> commands;

        public EventTypeScanner(List<EventModel> events, List<ModelModel> models, List<EnumerationModel> enumerations)
        {
            this.events = events;
            this.models = models;
            this.enumerations = enumerations;
            this.commands = commands;
        }

        public bool ProcessTypeDefinition(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.IsInterface == false && typeToProcess.Name.EndsWith("Event"))
            {
                EventModel evt = new EventModel();
                evt.Name = typeToProcess.Name;
                evt.FullType = typeToProcess.FullName;

                foreach (var property in typeToProcess.Properties)
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
                    evt.Properties.Add(prop);
                }

                this.events.Add(evt);

                return true;
            }

            return false;
        }
    }
}
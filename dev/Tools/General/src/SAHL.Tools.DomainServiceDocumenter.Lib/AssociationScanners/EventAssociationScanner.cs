using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SAHL.Tools.DomainServiceDocumenter.Lib.Models;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.AssociationScanners
{
    public class EventAssociationScanner : IAssociationScanner
    {
        private readonly List<EventModel> events;
        private readonly List<CommandModel> commands;

        public EventAssociationScanner(List<EventModel> events, List<CommandModel> commands)
        {
            this.events = events;
            this.commands = commands;
        }

        public void ProcessAssociations()
        {
            foreach (var command in commands)
            {
                if (command.RaisedEvent != null)
                {
                    var raisedEvent = command.RaisedEvent;

                    var currentEvent = events.FirstOrDefault(x => x.Name.Equals(raisedEvent.Name));

                    if (currentEvent != null)
                    {
                        if (currentEvent.ParentCommands.FirstOrDefault(x => x.Name.Equals(command.Name)) == null)
                        {
                            currentEvent.ParentCommands.Add(new AssociateItemModel() { Name = command.Name });    
                        }
                    }
                }
            }
            
        }

    }
}
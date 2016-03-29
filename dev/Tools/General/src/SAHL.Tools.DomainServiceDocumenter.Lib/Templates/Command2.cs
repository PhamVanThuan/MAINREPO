using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Command
    {
        public Command(List<CommandModel> commands, CommandModel command, ServiceModel service)
        {
            this.CommandModels = commands;

            this.CommandModel = command;

            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }

        public CommandModel CommandModel { get; protected set; }

        public List<CommandModel> CommandModels { get; protected set; }
    }
}
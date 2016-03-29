using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Commands
    {
        public Commands(List<CommandModel> commands, ServiceModel service)
        {
            this.CommandModels = commands;
            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }

        public List<CommandModel> CommandModels { get; protected set; }
    }
}
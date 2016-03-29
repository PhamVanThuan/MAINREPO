using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Commands
{
    public class CreateLegacyEventFromTransitionCommand : ServiceCommand, ILegacyEventTriggerCommand
    {
        public CreateLegacyEventFromTransitionCommand(int stageTransitionKeyKey)
        {
            this.StageTransitionKeyKey = stageTransitionKeyKey;
        }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int StageTransitionKeyKey { get; protected set; }
    }
}
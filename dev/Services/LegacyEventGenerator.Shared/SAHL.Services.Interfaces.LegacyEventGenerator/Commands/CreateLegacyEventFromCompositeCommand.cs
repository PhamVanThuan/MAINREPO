using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Commands
{
    public class CreateLegacyEventFromCompositeCommand : ServiceCommand, ILegacyEventTriggerCommand
    {
        public CreateLegacyEventFromCompositeCommand(int stageTransitionCompositeKey)
        {
            this.StageTransitionCompositeKey = stageTransitionCompositeKey;
        }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int StageTransitionCompositeKey { get; protected set; }
    }
}
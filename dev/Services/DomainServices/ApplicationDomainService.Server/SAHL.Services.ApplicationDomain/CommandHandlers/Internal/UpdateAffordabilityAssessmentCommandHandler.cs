﻿using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;

namespace SAHL.Services.ApplicationDomain.CommandHandlers.Internal
{
    public class UpdateAffordabilityAssessmentCommandHandler : IServiceCommandHandler<UpdateAffordabilityAssessmentCommand>
    {
        private IADUserManager adUserManager;
        private IAffordabilityAssessmentManager affordabilityAssessmentManager;

        public UpdateAffordabilityAssessmentCommandHandler(IADUserManager adUserManager, IAffordabilityAssessmentManager affordabilityAssessmentManager)
        {
            this.adUserManager = adUserManager;
            this.affordabilityAssessmentManager = affordabilityAssessmentManager;
        }

        public ISystemMessageCollection HandleCommand(UpdateAffordabilityAssessmentCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessageCollection = SystemMessageCollection.Empty();

            int? _ADUserKey = adUserManager.GetAdUserKeyByUserName(metadata.UserName);
            if (_ADUserKey == null || _ADUserKey.Value <= 0)
            {
                systemMessageCollection.AddMessage(new SystemMessage("Failed to retrieve ADUserKey.", SystemMessageSeverityEnum.Error));
                return systemMessageCollection;
            }

            affordabilityAssessmentManager.UpdateAffordabilityAssessmentAndAffordabilityAssessmentItems(command.AffordabilityAssessment, _ADUserKey.Value);

            return systemMessageCollection;
        }
    }
}
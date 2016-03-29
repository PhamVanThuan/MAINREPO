﻿using SAHL.Core.Services;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Commands
{
    public class AmendAffordabilityAssessmentIncomeContributorsCommand : ServiceCommand, IApplicationDomainCommand
    {
        public AmendAffordabilityAssessmentIncomeContributorsCommand(AffordabilityAssessmentModel affordabilityAssessment)
        {
            this.AffordabilityAssessment = affordabilityAssessment;
        }

        [Required]
        public AffordabilityAssessmentModel AffordabilityAssessment { get; protected set; }
    }
}
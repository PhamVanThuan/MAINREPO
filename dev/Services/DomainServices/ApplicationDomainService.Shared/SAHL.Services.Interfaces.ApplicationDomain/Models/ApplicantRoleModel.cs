using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class ApplicantRoleModel : ValidatableModel
    {
        public ApplicantRoleModel(int applicationRoleKey)
        {
            this.ApplicationRoleKey = applicationRoleKey;
            Validate();
        }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int ApplicationRoleKey { get; protected set; }
    }
}
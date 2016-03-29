using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System.ComponentModel.DataAnnotations;
using System;
using SAHL.Services.Interfaces.Capitec.Models;

namespace SAHL.Services.Interfaces.Capitec.Queries
{
    [AuthorisedCommand(Roles = "User")]
    public class GetRolesFromUserQuery : ServiceQuery<GetRolesFromUserQueryResult>
    {
        [Required]
        public Guid UserId { get; protected set; }

        public GetRolesFromUserQuery(Guid userId)
        {
            this.UserId = userId;
        }
    }
}

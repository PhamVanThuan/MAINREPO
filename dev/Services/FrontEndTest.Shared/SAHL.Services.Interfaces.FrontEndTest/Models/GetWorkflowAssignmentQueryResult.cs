using System;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetWorkflowAssignmentQueryResult
    {
        public int InstanceID { get; set; }

        public int OfferRoleTypeOrganisationStructureMappingKey { get; set; }

        public int ADUserKey { get; set; }

        public GeneralStatus GeneralStatusKey { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
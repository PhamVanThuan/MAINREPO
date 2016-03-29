using System;

namespace SAHL.Services.Capitec.Managers.CapitecApplication.Models
{
    public class CapitecUserBranchMappingModel
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public string UserName { get; private set; }

        public string BranchName { get; private set; }

        public int? ApplicationNumber { get; private set; }

        public CapitecUserBranchMappingModel()
        {
        }

        public CapitecUserBranchMappingModel(Guid Id, Guid userId, string userName, string branchName, int? applicationNumber)
        {
            this.Id = Id;
            this.UserId = userId;
            this.UserName = userName;
            this.BranchName = branchName;
            this.ApplicationNumber = applicationNumber;
        }
    }
}
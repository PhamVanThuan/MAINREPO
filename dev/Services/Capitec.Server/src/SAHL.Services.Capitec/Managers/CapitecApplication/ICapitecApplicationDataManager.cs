using SAHL.Services.Capitec.Managers.CapitecApplication.Models;
using System;

namespace SAHL.Services.Capitec.Managers.CapitecApplication
{
    public interface ICapitecApplicationDataManager
    {
        CapitecUserBranchMappingModel GetCapitecUserBranchMappingForApplication(Guid applicationId);
    }
}
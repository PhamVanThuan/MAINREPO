using System.Collections.Generic;
namespace BuildingBlocks.Services.Contracts
{
    public interface IActiveDirectoryService
    {
        List<string> GetAllLockedTestUsers();
    }
}

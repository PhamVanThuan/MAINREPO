using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IContextMenuService
    {
        IEnumerable<Automation.DataModels.ContextMenu> GetContextMenuItemsByCBOKey(int cboKey);
    }
}
using System;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public class HaloTileActionEditBase<TEditTile> : HaloTileActionBase<TEditTile>, IHaloTileActionEdit 
        where TEditTile : IHaloTileConfiguration
    {
        protected HaloTileActionEditBase(string actionName, string iconName, string actionGroup = "", int sequence = 0) 
            : base (actionName, iconName, actionGroup, sequence)
        {
        }
    }
}

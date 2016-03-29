using System;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloTileConfiguration
    {
        string Name { get; }
        int Sequence { get; }
        string TileType { get; }
        bool IsTileBased { get; }
        string NonTilePageState { get; set; }

        Type GetTileModelType();
        bool IsDynamicTile();
        IEnumerable<string> GetAllRoleNames();

        bool IsInRole(string roleName);
        bool IsInCapability(string[] capabilities);
    }
}

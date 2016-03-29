using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public interface IHaloMenuItem
    {
        string Name { get; }
        int Sequence { get; }
        string ModuleName { get; }
        bool IsTileBased { get; }
        string NonTilePageState { get; }

        bool IsInRole(string roleName);
        bool IsInCapability(string[] capabilities);
    }
}

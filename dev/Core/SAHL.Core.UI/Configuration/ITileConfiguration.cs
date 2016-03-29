using System;

namespace SAHL.Core.UI.Configuration
{
    public interface ITileConfiguration : IElementWithUrlConfiguration, IElementWthOrderConfiguration, IRequiredFeatureAccess
    {
        Type TileType { get; }

        Type TileModelType { get; }

        void OverrideTileModelType(Type tileModelType);
    }
}
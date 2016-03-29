using SAHL.Core.UI.Enums;
using System;

namespace SAHL.Core.UI.Configuration.Tiles
{
    public abstract class AbstractTileConfiguration<T> : IRequiredFeatureAccess
    {
        public AbstractTileConfiguration(string requiredFeatureAccess, int sequence, string controllerName, string controllerAction, UrlAction urlAction)
        {
            this.Url = new UrlConfiguration(controllerName, controllerAction, urlAction);
            this.RequiredFeatureAccess = requiredFeatureAccess;
            this.Sequence = sequence;
            this.TileModelType = typeof(T);
        }

        public AbstractTileConfiguration(string requiredFeatureAccess, int sequence)
            : this(requiredFeatureAccess, sequence, UrlNames.TileController, UrlNames.DrillDownAndGetUsersTilesForContextAction, UrlAction.TileDrillDown)
        {
        }

        public string RequiredFeatureAccess { get; protected set; }

        public UrlConfiguration Url { get; protected set; }

        public Type TileType { get { return this.GetType(); } }

        public Type TileModelType { get; protected set; }

        public int Sequence { get; protected set; }

        public void OverrideTileModelType(Type tileModelType)
        {
            this.TileModelType = tileModelType;
        }
    }
}
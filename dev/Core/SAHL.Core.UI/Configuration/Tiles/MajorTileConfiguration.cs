using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Tiles;
using SAHL.Core.UI.Models;
using SAHL.Core.UI.Modules;

namespace SAHL.Core.UI.Configuration.Tiles
{
    public abstract class MajorTileConfiguration<T> : AbstractTileConfiguration<T>, IMajorTileConfiguration<T> where T : ITileModel
    {
        public MajorTileConfiguration(string requiredFeatureAccess, string contextDescription)
            : base(requiredFeatureAccess, 0)
        {
            this.ContextDescription = contextDescription;
        }

        public MajorTileElement CreateElement(BusinessContext businessContext)
        {
            return new MajorTileElement(new TileBusinessContext(businessContext, this.TileModelType, this.GetType()), this.Url.GenerateActionUrl());
        }

        public string ContextDescription { get; protected set; }
    }

    public class NoAccessConfiguration : MajorTileConfiguration<NoAccessTileModel>, IRootTileConfiguration<IApplicationModule>
    {
        public NoAccessConfiguration()
            : base("RootTileAccess", "Client")
        {
        }
    }

    public class NoAccessTileModel : ITileModel
    {
        public string NoAccess { get; set; }
    }
}
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Parts;
using SAHL.Core.UI.Enums;
using System;

namespace SAHL.Core.UI.Elements.Tiles
{
    public class AbstractTileElement : VisualElement, IUrlElement
    {
        public AbstractTileElement(TileBusinessContext businessContext, string url, UrlAction urlAction)
            : base(ElementNames.TileItemIdPrefix, businessContext)
        {
            this.Url = url;
            this.UrlAction = urlAction;
            this.TileModelType = businessContext.TileModelType;
            this.TileConfigurationType = businessContext.TileConfigurationType;
            this.Content = new TileContentPart();
            base.AddPart(this.Content);
        }

        public string Url { get; protected set; }

        public UrlAction UrlAction { get; protected set; }

        public Type TileModelType { get; protected set; }

        public Type TileConfigurationType { get; protected set; }

        public TileContentPart Content { get; protected set; }
    }
}
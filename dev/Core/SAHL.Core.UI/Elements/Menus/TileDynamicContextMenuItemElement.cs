using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Parts;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Menus
{
    public class TileDynamicContextMenuItemElement : AbstractContextMenuItemElement
    {
        public TileDynamicContextMenuItemElement(string menuText, TileBusinessContext tileBusinessContext, string url)
            : base(tileBusinessContext, url, UrlAction.TileChangeContext)
        {
            this.AddPart(new StaticTextPart(menuText));
            this.AddPart(new StaticTextPart(" / "));
        }

        public TileBusinessContext TileBusinessContext
        {
            get
            {
                return this.BusinessContext as TileBusinessContext;
            }
        }
    }
}